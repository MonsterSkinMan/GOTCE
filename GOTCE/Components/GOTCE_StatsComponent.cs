using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using static GOTCE.Main;

namespace GOTCE.Components
{
    public class GOTCE_StatsComponent : MonoBehaviour
    {
        // this monobehavior is attached to every charactermaster when they spawn
        // store per-master variables here (like sprint crit chance etc)

        // generic player stuff
        public CharacterMaster master;

        public Inventory inventory;
        public CharacterBody body;

        // critical chances
        public float stageCritChance;

        public float sprintCritChance;
        public float fovCritChance;
        public float deathCritChance;

        // death chance
        public float deathChance;

        // other stats
        public int aoeEffect;

        // item: crown prince
        public int crownPrinceUses;

        public float crownPrinceTrueKillChance;

        // item: defibrillator
        public float reviveChance;

        // item: grandfather clock
        public int clockDeathCount = 0;

        private float deathTimer = 0f;
        private GameObject voidVFX;

        // item: gabe's shank
        public int game_count;

        // item: sigma grindset
        public int total_sprint_crits = 0;

        // item: unseasoned patty
        public List<GameObject> bubbles = new();

        public bool withinBubble = false;
        public bool lastWasCombatShrine = false;

        // item: peer reviewed source
        public int identifiedkillCount = 0;

        // item: gamepad
        public int inputs = 0;

        public float increase = 0f;

        // run stats
        public int deathCount;

        // recalc
        public float StageCritChanceAdd;

        public float SprintCritChanceAdd;
        public float FovCritChanceAdd;
        public float DeathChanceAdd;
        public int AOEAdd;
        public float reviveChanceAdd;
        public float DeathCritChanceAdd;

        // sprint crit
        public bool isCriticallySprinting = false;

        public WarCrime mostRecentlyCommitedWarCrime = WarCrime.None;

        // death crit
        private float stopwatchDeathCrit = 0f;
        private float safeTimerDeathCrit = 5f;
        private bool deathCritTimerOn = false;
        public bool isOnCritDeathCooldown = false;
        public Vector3 deathPos;

        private void Start()
        {
            // assign on-start vars and load void death vfx
            if (gameObject.GetComponent<CharacterMaster>())
            {
                master = gameObject.GetComponent<CharacterMaster>();
                body = master.GetBody();
                inventory = master.inventory;
            }
            RecalculateStatsAPI.GetStatCoefficients += UpdateChances;
            voidVFX = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/CritGlassesVoid/CritGlassesVoidExecuteEffect.prefab").WaitForCompletion();

            InvokeRepeating(nameof(RollInputs), 1f, 1f);
        }

        private void OnDestroy()
        {
            RecalculateStatsAPI.GetStatCoefficients -= UpdateChances;
        }

        private void UpdateChances(CharacterBody cbody, RecalculateStatsAPI.StatHookEventArgs args)
        {
            // critical chances
            if (cbody && cbody.master == master && cbody.inventory && cbody.masterObject.GetComponent<GOTCE_StatsComponent>())
            {
                inventory = cbody.inventory;
                body = cbody;
                AOEAdd = 0;
                SprintCritChanceAdd = 0;
                StageCritChanceAdd = 0;
                FovCritChanceAdd = 0;
                reviveChanceAdd = 0;
                DeathChanceAdd = 0;
                DeathCritChanceAdd = 0;
                

                StatsCompEvent.StatsCompRecalc?.Invoke(this, new(cbody.masterObject.GetComponent<GOTCE_StatsComponent>()));
                

                fovCritChance = FovCritChanceAdd;
                sprintCritChance = SprintCritChanceAdd + increase;
                stageCritChance = StageCritChanceAdd;
                reviveChance = reviveChanceAdd;
                aoeEffect = AOEAdd;
                deathChance = DeathChanceAdd;
                deathCritChance = DeathCritChanceAdd;
            }

            // grant attack speed if the player is within an ethereal bubble from seasoned patty
            if (withinBubble)
            {
                args.attackSpeedMultAdd += 0.03f;
                withinBubble = false;
            }

            // grandfather clock stage crit stuff

            if (clockDeathCount > 0 && master.GetBody() && master.GetBody().healthComponent && master.GetBody().healthComponent.health > 0)
            {
                clockDeathCount--;
                Invoke(nameof(Die), 3f);
            }
        }

        private void RollInputs()
        {
            if (!body || !body.inventory)
            {
                return;
            }
            float amount = 2f * body.inventory.GetItemCount(Items.Red.Gamepad.Instance.ItemDef);
            increase = amount * inputs;
            inputs = 0;
            if (increase > 0 && NetworkServer.active && body.inventory.GetItemCount(Items.Red.Gamepad.Instance.ItemDef) > 0)
            {
                body.RecalculateStats();
            }
        }

        public void CriticallyDie() {
            deathCritTimerOn = true;
            Invoke(nameof(RespawnExtraLifeNoImmune), 2.5f);
            Invoke(nameof(Die), 3f);
        }

        public void FixedUpdate()
        {
            if (deathCritTimerOn) {
                master.preventGameOver = true;
                stopwatchDeathCrit += Time.fixedDeltaTime;
                if (stopwatchDeathCrit >= safeTimerDeathCrit) {
                    stopwatchDeathCrit = 0f;
                    deathCritTimerOn = false;
                }

                isOnCritDeathCooldown = true;
            }
            else {
                isOnCritDeathCooldown = false;
            }
        }

        // this function exists solely so suicide can be used as a courotine
        public void Die()
        {
            if (!body || !body.healthComponent) {
                return;
            }
            body.healthComponent.Suicide();
            EffectManager.SpawnEffect(voidVFX, new EffectData
            {
                origin = body.transform.position,
                scale = 1f
            }, true);
        }

        // returns the player's stage crit value after taking all items into consideration, should be used before attempting a stage crit
        public void DetermineStageCrit()
        {
            if (master)
            {
                StageCritChanceAdd = 0;
                EventHandler<StatsCompRecalcArgs> raiseEvent = StatsCompEvent.StatsCompRecalc;
                if (raiseEvent != null)
                {
                    raiseEvent(this, new StatsCompRecalcArgs(gameObject.GetComponent<GOTCE_StatsComponent>()));
                }
                stageCritChance = StageCritChanceAdd;

                /* Inventory inv = master.inventory;
                float stageCritChanceInc = 0;
                stageCritChanceInc += 10f * inv.GetItemCount(GOTCE.Items.White.FaultySpacetimeClock.Instance.ItemDef);
                if (inv.GetItemCount(Items.Green.HeartyBreakfast.Instance.ItemDef) > 0) { stageCritChanceInc += 5f; };
                if (inv.GetItemCount(Items.White.SkullKey.Instance.ItemDef) > 0) { stageCritChanceInc += 5f; };
                if (inv.GetItemCount(Items.Green.GrandfatherClock.Instance.ItemDef) > 0) { stageCritChanceInc += 5f; };
                if (inv.GetItemCount(Items.Green.AmberRabbit.Instance.ItemDef) > 0) { stageCritChanceInc += 5f; };
                stageCritChanceInc += 2f*(inventory.GetItemCount(Items.White.EmpathyC4.Instance.ItemDef));
                stageCritChance = stageCritChanceInc; */
            }
        }

        // dio revive but doesnt give an extra dio and requires no arguments
        public void RespawnExtraLife()
        {
            Vector3 vector = master.deathFootPosition;
            if (master.killedByUnsafeArea)
            {
                vector = TeleportHelper.FindSafeTeleportDestination(master.deathFootPosition, body, RoR2Application.rng) ?? master.deathFootPosition;
            }
            master.Respawn(vector, Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f));
            master.GetBody().AddTimedBuff(RoR2Content.Buffs.Immune, 3f);
            GameObject gameObject = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/HippoRezEffect");
            if (master.bodyInstanceObject)
            {
                EntityStateMachine[] components = master.bodyInstanceObject.GetComponents<EntityStateMachine>();
                foreach (EntityStateMachine obj in components)
                {
                    obj.initialStateType = obj.mainStateType;
                }
            }
        }
        // dio revive but doesnt give dio and doesnt give iframes and doesnt play vfx
        public void RespawnExtraLifeNoImmune()
        {
            Vector3 vector = deathPos;
            if (master.killedByUnsafeArea)
            {
                vector = TeleportHelper.FindSafeTeleportDestination(deathPos, body, RoR2Application.rng) ?? deathPos;
            }
            master.Respawn(vector, Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f));

            if (master.bodyInstanceObject)
            {
                EntityStateMachine[] components = master.bodyInstanceObject.GetComponents<EntityStateMachine>();
                foreach (EntityStateMachine obj in components)
                {
                    obj.initialStateType = obj.mainStateType;
                }
            }
        }
    }
}