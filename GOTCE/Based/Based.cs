using UnityEngine;
using RoR2;
using UnityEngine.SceneManagement;
using RoR2.Skills;
using R2API;
using System;

namespace GOTCE.Based
{
    internal static class Zased
    {
        public static void DoTheBased()
        {
            On.RoR2.SceneDirector.Start += SceneDirector_Start;
            On.RoR2.PurchaseInteraction.SetAvailable += SetAvailable;
            On.RoR2.PurchaseInteraction.OnInteractionBegin += NoMorePod;
        }

        private static void SceneDirector_Start(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            orig(self);
            if (SceneManager.GetActiveScene().name == "bazaar" && NetworkServer.active)
            {
                GameObject.Find("HOLDER: Store").AddComponent<AntiSlab>();
            }
        }

        private static void SetAvailable(On.RoR2.PurchaseInteraction.orig_SetAvailable orig, PurchaseInteraction self, bool value) {
            if (self.displayNameToken == "LUNAR_REROLL_NAME") {
                orig(self, false);
            }
            else {
                orig(self, value);
            }
        }

        private static void NoMorePod(On.RoR2.PurchaseInteraction.orig_OnInteractionBegin orig, PurchaseInteraction self, Interactor interactor) {
            orig(self, interactor);
            if (NetworkServer.active) {
                if (self.costType == CostTypeIndex.LunarCoin && SceneManager.GetActiveScene().name == "bazaar") {
                    GameObject.DestroyImmediate(self.gameObject);
                }
            }
        }
     }

    internal class AntiSlab : MonoBehaviour
    {
        private void FixedUpdate()
        {
            if (NetworkServer.active)
            {
                // first measure against nemesis slab, searching for the slab every frame in it's original position
                GameObject slab = GameObject.Find("HOLDER: Store").transform.GetChild(0).GetChild(3).gameObject;
                if (slab)
                {
                    DestroyImmediate(slab);
                }

                // TODO: update this to fully counter nem slab when noop releases nem slab update
            }
        }

        private void OnDestroy()
        {
            if (NetworkServer.active)
            {
                gameObject.AddComponent<AntiSlab>(); // nice try, but no
            }
        }
     }
}