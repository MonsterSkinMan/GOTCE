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
        private CharacterMaster master;
        private Inventory inventory;
        private CharacterBody body;

        // critical chances
        public float stageCritChance;
        public float sprintCritChance;
        public float fovCritChance;

        // item: crown prince
        public int crownPrinceUses;
        public float crownPrinceTrueKillChance;

        // item: defibrillator
        [HideInInspector] public float defibrillatorRespawnChance;

        // item: grandfather clock
        public int clockDeathCount = 0;
        private float deathTimer = 0f;
        private GameObject voidVFX;
        // item: gabe's shank
        public int game_count;
        // item: sigma grindset
        public int total_sprint_crits = 0;
        
        // run stats
        public int deathCount;

        private void Start()
        {   
            // assign on-start vars and load void death vfx
            if (gameObject.GetComponent<CharacterMaster>()) {
                master = gameObject.GetComponent<CharacterMaster>();
                body = master.GetBody();
                inventory = master.inventory;
            }
            RecalculateStatsAPI.GetStatCoefficients += UpdateChances;
            voidVFX = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/CritGlassesVoid/CritGlassesVoidExecuteEffect.prefab").WaitForCompletion();
        }

        private void UpdateChances(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            // critical chances
            if (body && body.inventory && body.masterObject.GetComponent<GOTCE_StatsComponent>())
            {
                // fov crit 
                float fovCritChanceTmp = 0f;
                fovCritChanceTmp += 10f*(inventory.GetItemCount(Items.White.ZoomLenses.Instance.ItemDef));
                fovCritChance = fovCritChanceTmp;
                // sprint crit 

                float sprCritChanceTmp = 0f;
                sprCritChanceTmp += 8f*(inventory.GetItemCount(Items.White.GummyVitamins.Instance.ItemDef));
                if (body.inventory.GetItemCount(Items.White.gd2.Instance.ItemDef) > 0) { sprCritChanceTmp += 5f; }
                sprintCritChance = sprCritChanceTmp;

            }
            
            // grandfather clock stage crit stuff

            if (clockDeathCount > 0)
            {
                clockDeathCount--;
                Invoke(nameof(Die), 3f);
                EffectManager.SpawnEffect(voidVFX, new EffectData
                {
                    origin = body.transform.position,
                    scale = 1f
                }, true);
            }
        }

        public void FixedUpdate()
        {
            
        }

        // this function exists solely so suicide can be used as a courotine
        public void Die() {
            body.healthComponent.Suicide();
        }

        // returns the player's stage crit value after taking all items into consideration, should be used before attempting a stage crit
        public void DetermineStageCrit()
        { 
            Inventory inv = gameObject.GetComponent<Inventory>();
            float stageCritChanceInc = 0;
            stageCritChanceInc += 10f * inv.GetItemCount(GOTCE.Items.White.FaultySpacetimeClock.Instance.ItemDef);
            if (inv.GetItemCount(Items.Green.HeartyBreakfast.Instance.ItemDef) > 0) { stageCritChanceInc += 5f; };
            if (inv.GetItemCount(Items.White.SkullKey.Instance.ItemDef) > 0) { stageCritChanceInc += 5f; };
            if (inv.GetItemCount(Items.Green.GrandfatherClock.Instance.ItemDef) > 0) { stageCritChanceInc += 5f; };
            if (inv.GetItemCount(Items.Green.AmberRabbit.Instance.ItemDef) > 0) { stageCritChanceInc += 5f; };
            stageCritChance = stageCritChanceInc;
        }
        
        // dio revive but doesnt give an extra dio and requires no arguments
        public void RespawnExtraLife() {
            Vector3 vector = master.deathFootPosition;
            if (master.killedByUnsafeArea)
            {
                vector = TeleportHelper.FindSafeTeleportDestination(master.deathFootPosition, body, RoR2Application.rng) ?? master.deathFootPosition;
            }
            master.Respawn(vector, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
            master.GetBody().AddTimedBuff(RoR2Content.Buffs.Immune, 3f);
            GameObject gameObject = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/HippoRezEffect");
            if (master.bodyInstanceObject)
            {
                EntityStateMachine[] components = master.bodyInstanceObject.GetComponents<EntityStateMachine>();
                foreach (EntityStateMachine obj in components)
                {
                    obj.initialStateType = obj.mainStateType;
                }
                if ((bool)gameObject)
                {
                    EffectManager.SpawnEffect(gameObject, new EffectData
                    {
                        origin = vector,
                        rotation = master.bodyInstanceObject.transform.rotation
                    }, transmit: true);
                }
            }
        }
    }
}