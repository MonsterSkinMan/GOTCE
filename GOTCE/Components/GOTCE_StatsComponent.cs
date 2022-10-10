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
        [HideInInspector] public float defibrillatorRespawnChance;
        public int clockDeathCount = 0;

        private float deathTimer = 0f;
        private GameObject voidVFX;

        // add more of these for every respawn chance item
        public int deathCount;

        private void Start()
        {
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
    }
}