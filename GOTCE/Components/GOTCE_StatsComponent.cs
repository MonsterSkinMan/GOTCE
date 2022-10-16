using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using static GOTCE.Main;
using UnityEngine.AddressableAssets;

namespace GOTCE.Components
{
    public class GOTCE_StatsComponent : MonoBehaviour
    {
        // this monobehavior is attached to every characterbody when they spawn
        // store per-body variables here (like sprint crit chance etc)

        public float stageCritChance;
        public float sprintCritChance;
        public float fovCritChance;
        public float respawnChance;
        public int crownPrinceUses;
        public float crownPrinceTrueKillChance;
        [HideInInspector] public float defibrillatorRespawnChance;
        public int clockDeathCount = 0;

        private float deathTimer = 0f;
        private GameObject voidVFX;
        private CharacterMaster master;
        private Inventory inventory;
        private CharacterBody body;

        // add more of these for every respawn chance item
        public int deathCount;

        private void Start()
        {
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
            if (body && body.inventory && body.gameObject.GetComponent<GOTCE_StatsComponent>())
            {
                // fov crit (unimplemented)

                // sprint crit (unimplemented)

                respawnChance = defibrillatorRespawnChance;
                // crownPrinceTrueKillChance = body.inventory.GetItemCount();
            }

            if (clockDeathCount > 0 && deathTimer >= 3f)
            {
                // Main.ModLogger.LogDebug("clock death pre: " + clockDeathCount);
                clockDeathCount--;
                // Main.ModLogger.LogDebug("clock death post: " + clockDeathCount);
                EffectManager.SpawnEffect(voidVFX, new EffectData
                {
                    origin = body.transform.position,
                    scale = 1f
                }, true);
                body.healthComponent.Suicide(null, null, DamageType.BypassOneShotProtection | DamageType.VoidDeath);
                deathTimer = 0f;
            }
        }

        public void FixedUpdate()
        {
            deathTimer += Time.fixedDeltaTime;
        }

        public void DetermineStageCrit()
        { // stage crit goes here so i can make sure it's been determined BEFORE the characterbody tries to get it
            if (gameObject.GetComponent<CharacterBody>())
            {
                CharacterBody body = gameObject.GetComponent<CharacterBody>();
                if (body.isPlayerControlled)
                {
                    // Main.ModLogger.LogDebug("this ran");
                    float stageCritChanceInc = 0;
                    stageCritChanceInc += 10f * body.inventory.GetItemCount(GOTCE.Items.White.FaultySpacetimeClock.Instance.ItemDef);
                    if (body.inventory.GetItemCount(Items.Green.HeartyBreakfast.Instance.ItemDef) > 0) { stageCritChanceInc += 5f; };
                    if (body.inventory.GetItemCount(Items.White.SkullKey.Instance.ItemDef) > 0) { stageCritChanceInc += 5f; };
                    if (body.inventory.GetItemCount(Items.Green.GrandfatherClock.Instance.ItemDef) > 0) { stageCritChanceInc += 5f; };
                    stageCritChance = stageCritChanceInc;
                }
            };
        }

        public void RespawnExtraLife() {
            // inventory.GiveItem(RoR2Content.Items.ExtraLifeConsumed);
            // CharacterMasterNotificationQueue.SendTransformNotification(this, RoR2Content.Items.ExtraLife.itemIndex, RoR2Content.Items.ExtraLifeConsumed.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);
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