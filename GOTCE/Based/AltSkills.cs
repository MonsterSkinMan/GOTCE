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
            RGAlts();
            HuntressAlts();
            ViendAlts();
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

        private static void RGAlts() {
            MagneticPropulsor.Create();
            GameObject rgPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/RailgunnerBody.prefab").WaitForCompletion();

            SkillLocator sl = rgPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily;
            // dumb rounds
            skillFamily = sl.primary.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.DumbRounds.Instance.SkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(Skills.SigmaShotgun.Instance.SkillDef.skillNameToken, false, null)
            };

            LanguageAPI.Add(Skills.DumbRounds.Instance.SkillDef.skillNameToken, "CJI Stupid Rounds");
            LanguageAPI.Add(Skills.DumbRounds.Instance.SkillDef.skillDescriptionToken, "Fire a highly inaccurate spread of 6 rounds per second for 100% each.");

            // magnetic propulsor
            // guh 
            /* skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
            skillFamily.variants = new SkillFamily.Variant[2];
            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = MagneticPropulsor.def,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(Skills.SigmaShotgun.Instance.SkillDef.skillNameToken, false, null)
            };

            skillFamily.variants[1] = new SkillFamily.Variant
            {
                skillDef = MagneticPropulsor.defAlt,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(Skills.SigmaShotgun.Instance.SkillDef.skillNameToken, false, null)
            };

            LanguageAPI.Add(Skills.DumbRounds.Instance.SkillDef.skillNameToken, "Magnetic Propulsor");
            LanguageAPI.Add(Skills.DumbRounds.Instance.SkillDef.skillDescriptionToken, "Critical chance is converted into jump height.");

            ContentAddition.AddSkillFamily(skillFamily);
            ContentAddition.AddSkillDef(MagneticPropulsor.def);
            ContentAddition.AddSkillDef(MagneticPropulsor.defAlt);
            GenericSkill primary = sl.primary;
            GenericSkill secondary = sl.secondary;
            GenericSkill utility = sl.utility;
            GenericSkill special = sl.special;
            SkillFamily rgPrimary = sl.primary.skillFamily;
            SkillFamily rgSecondary = sl.secondary.skillFamily;
            SkillFamily rgUtility = sl.utility.skillFamily;
            SkillFamily rgSpecial = sl.special.skillFamily;
            GenericSkill genericSkill = sl.primary;
            SkillFamily familyP = primary.skillFamily;
            SkillFamily familyS = secondary.skillFamily;
            SkillFamily familyU = utility.skillFamily;
            SkillFamily familySp = special.skillFamily;
            primary = secondary;
            secondary = utility;
            utility = special;
            special = rgPrefab.AddComponent<GenericSkill>();
            genericSkill._skillFamily = skillFamily;
            primary._skillFamily = rgPrimary;
            secondary._skillFamily = rgSecondary;
            utility._skillFamily = rgUtility;
            special._skillFamily = rgSpecial; */
        }

        private static void HuntressAlts() {
            GameObject huntressPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Huntress/HuntressBody.prefab").WaitForCompletion();

            SkillLocator sl = huntressPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily;
            // shark saw
            skillFamily = sl.secondary.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.Sawblade.Instance.SkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(Skills.Sawblade.Instance.SkillDef.skillNameToken, false, null)
            };

            LanguageAPI.Add(Skills.Sawblade.Instance.SkillDef.skillNameToken, "Shark Saw");
            LanguageAPI.Add(Skills.Sawblade.Instance.SkillDef.skillDescriptionToken, "Throw a fast piercing sawblade that moves along surfaces and rapidly strikes enemies for 30% per tick. Bleeds.");

            /* On.RoR2.Projectile.ProjectileStickOnImpact.UpdateSticking += (orig, self) => {
                if (self.stuckTransform == null && !self.gameObject.GetComponent<EntityStatesCustom.AltSkills.Huntress.MoveForward>()) {
                    orig(self);
                }
                else if (self.stuckTransform && self.gameObject.GetComponent<EntityStatesCustom.AltSkills.Huntress.MoveForward>()) {
                    self.gameObject.GetComponent<EntityStatesCustom.AltSkills.Huntress.MoveForward>().Move();
                }
                else {
                    orig(self);
                }
            }; */
        }

        private static void ViendAlts() {
            GameObject viendPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBody.prefab").WaitForCompletion();

            SkillLocator sl = viendPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily;
            // pearl
            skillFamily = sl.secondary.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.Pearl.Instance.SkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(Skills.Pearl.Instance.SkillDef.skillNameToken, false, null)
            };

            LanguageAPI.Add(Skills.Pearl.Instance.SkillDef.skillNameToken, "War??p");
            LanguageAPI.Add(Skills.Pearl.Instance.SkillDef.skillDescriptionToken, "Launch a void orb that sticks to surfaces and repeatedly strikes for 60% damage. Nullifies. Use while a void orb is active to teleport to it, destroying the void orb.");

            LanguageAPI.Add(Skills.PearlTeleport.Instance.SkillDef.skillNameToken, "Retur??n");
            LanguageAPI.Add(Skills.PearlTeleport.Instance.SkillDef.skillDescriptionToken, "Teleport to your most recently deployed void orb.");
        }
    }

    public class MagneticPropulsor {
        public static PassiveItemSkillDef defAlt;
        public static PassiveItemSkillDef def;
        public static void Create() {
            GameObject rgPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/RailgunnerBody.prefab").WaitForCompletion();

            SkillLocator sl = rgPrefab.GetComponent<SkillLocator>();

            def = ScriptableObject.CreateInstance<PassiveItemSkillDef>();
            def.passiveItem = DLC1Content.Items.ConvertCritChanceToCritDamage;
            def.icon = sl.passiveSkill.icon;
            def.skillNameToken = sl.passiveSkill.skillNameToken;
            def.skillDescriptionToken = sl.passiveSkill.skillDescriptionToken;
            def.activationStateMachineName = "Weapon";
            def.activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.Idle));

            defAlt = ScriptableObject.CreateInstance<PassiveItemSkillDef>();
            defAlt.skillNameToken = "GOTCE_PROPULSOR_NAME";
            defAlt.skillDescriptionToken = "GOTCE_PROPULSOR_DESC";
            defAlt.passiveItem = Items.NoTier.MagneticPropulsor.Instance.ItemDef;
            defAlt.icon = Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/NEA.png");
            defAlt.activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.Idle));
            defAlt.activationStateMachineName = "Weapon";
        }
    }
}