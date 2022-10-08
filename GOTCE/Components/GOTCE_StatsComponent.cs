using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using static GOTCE.Main;

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

        // add more of these for every respawn chance item
        public int deathCount;

        private void Start()
        {
            RecalculateStatsAPI.GetStatCoefficients += UpdateChances;
        }

        private void UpdateChances(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.inventory && body.gameObject.GetComponent<GOTCE_StatsComponent>())
            {
                // fov crit (unimplemented)

                // sprint crit (unimplemented)

                respawnChance = defibrillatorRespawnChance;
            }
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
                    stageCritChanceInc += 10f * body.inventory.GetItemCount(Items.White.FaultySpacetimeClock.Instance.ItemDef);
                    stageCritChance = stageCritChanceInc;
                }
            }
        }
    }
}