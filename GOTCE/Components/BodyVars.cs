using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using static GOTCE.Main;

namespace GOTCE.Components {

    public class BodyVars : MonoBehaviour
        {
        // this monobehavior is attached to every characterbody when they spawn
        // store per-body variables here (like sprint crit chance etc)

        public float stageCritChance;
        public float sprintCritChance;
        public float fovCritChance;

        public void Start() {
                RecalculateStatsAPI.GetStatCoefficients += UpdateChances;
        }

        private void UpdateChances(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args) {
                if (body && body.inventory && body.gameObject.GetComponent<BodyVars>()) {
                    // fov crit (unimplemented)

                    // sprint crit (unimplemented)
                }
        }

        public void DetermineStageCrit() { // stage crit goes here so i can make sure it's been determined BEFORE the characterbody tries to get it
            if (gameObject.GetComponent<CharacterBody>()) {
                    CharacterBody body = gameObject.GetComponent<CharacterBody>(); 
                    if (body.isPlayerControlled) {
                        // Main.ModLogger.LogDebug("this ran");
                        float stageCritChanceInc = 0;
                        stageCritChanceInc += 10f*body.inventory.GetItemCount(GOTCE.Items.White.FaultySpacetimeClock.Instance.ItemDef);
                        stageCritChance = stageCritChanceInc;
                    }
                };
        }
    }
}