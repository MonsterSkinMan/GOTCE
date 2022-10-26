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

            
        }

        private static void SceneDirector_Start(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            orig(self);
            if (SceneManager.GetActiveScene().name == "bazaar")
            {
                GameObject.Find("HOLDER: Store").transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                // disables lunar slab
            }
        }
    }
}