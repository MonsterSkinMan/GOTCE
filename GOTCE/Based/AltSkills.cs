using UnityEngine;
using RoR2;
using UnityEngine.SceneManagement;
using RoR2.Skills;
using R2API;
using System;

namespace GOTCE.Based {
    public class AltSkills {
        public static void AddAlts() {
            RexAlts();
        }

        private static void RexAlts() {
            GameObject treebotPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Treebot/TreebotBody.prefab").WaitForCompletion();

            SkillLocator sl = treebotPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily;
            // stigmata shotgun
            skillFamily = sl.primary.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.SigmaShotgun.Instance.SkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(Skills.SigmaShotgun.Instance.SkillDef.skillNameToken, false, null)
            };

            LanguageAPI.Add(Skills.SigmaShotgun.Instance.SkillDef.skillNameToken, "Stigmata Shotgun");
            LanguageAPI.Add(Skills.SigmaShotgun.Instance.SkillDef.skillDescriptionToken, "Weakens. Fires 9 pollen pellets for 9x50% damage. 5% HP");
        }
    }
}