using UnityEngine;
using RoR2;
using UnityEngine.SceneManagement;
using RoR2.Skills;
using R2API;
using System;
using System.Reflection;
using MonoMod.RuntimeDetour;
using GOTCE.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Mono.Cecil;
// using Mono.Reflection;

namespace GOTCE.Based {
    public class AltSkills {
        public static void AddAlts() {
            // PassiveReplacement.RunHooks();
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

            // PassiveReplacement.ReplacePassiveSlot(rgPrefab, MagneticPropulsor.defAlt, MagneticPropulsor.def);

            LanguageAPI.Add(MagneticPropulsor.defAlt.skillNameToken, "Magnetic Propulsors");
            LanguageAPI.Add(MagneticPropulsor.defAlt.skillDescriptionToken, "Critical Strike chance is converted into Jump Height.");
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
            ViendAltPassive.Create();
            ViendAltPassive.Hooks();

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
            LanguageAPI.Add(Skills.Pearl.Instance.SkillDef.skillDescriptionToken, "Launch a <style=cIsVoid>void orb</style> that sticks to surfaces and <style=cIsDamage>repeatedly strikes</style> for 60% damage. Use while a <style=cIsVoid>void orb</style> is active to <style=cIsVoid>teleport</style> to it, destroying the <style=cIsVoid>void orb.</style>");

            LanguageAPI.Add(Skills.PearlTeleport.Instance.SkillDef.skillNameToken, "Retur??n");
            LanguageAPI.Add(Skills.PearlTeleport.Instance.SkillDef.skillDescriptionToken, "<style=cIsVoid>Teleport</style> to your most recently deployed <style=cIsVoid>void orb</style>.");

            LanguageAPI.Add("GOTCE_CORRUPTIONM2UPGRADE_KEYWORD", "[Corruption Upgrade]\nThe <style=cIsVoid>void orb</style> bounces off terrain. Deal 2600% damage and <style=cIsVoid>nullify</style> enemies near your teleport location.");

            // drain
            skillFamily = sl.special.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.Drain.Instance.SkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(Skills.Drain.Instance.SkillDef.skillNameToken, false, null)
            };

            LanguageAPI.Add(Skills.Drain.Instance.SkillDef.skillNameToken, "Dr??ain");
            LanguageAPI.Add(Skills.Drain.Instance.SkillDef.skillDescriptionToken, "Rapidly drain your corruption, and fire a devastating blast for 100% damage with +10% for each 1% of corruption drained. Range increases with corruption drained. Guaranteed critical strike after draining more than 50% corruption.@");

            foreach (GenericSkill skill in viendPrefab.GetComponentsInChildren<GenericSkill>()) {
                if ((skill._skillFamily as ScriptableObject).name.Contains("Passive")) {
                    SkillFamily family = skill._skillFamily;
                    Array.Resize(ref family.variants, family.variants.Length + 1);
                    family.variants[family.variants.Length - 1] = new SkillFamily.Variant
                    {
                        skillDef = ViendAltPassive.skillDef,
                        unlockableName = "",
                        viewableNode = new ViewablesCatalog.Node("GOTCE_VIENDPASSIVE_NAME", false, null)
                    };
                }
            }

            LanguageAPI.Add("GOTCE_VIENDPASSIVE_NAME", "The Only Thing They Fear");
            LanguageAPI.Add("GOTCE_VIENDPASSIVE_DESC", "You are permanently corrupted. Your health drains alongside your corruption, and raises alongside it aswell. Your corruption cannot go below 1%. Healing is converted into armor, and your armor decays over time. Deal damage restores corruption.");
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

    public class ViendAltPassive {
        private static ItemDef def;
        public delegate float orig_minimumCorruption(VoidSurvivorController self);
        public delegate bool orig_isPermanentlyCorrupted(VoidSurvivorController self);

        public static PassiveItemSkillDef skillDef;
        private static BindingFlags propFlags = (BindingFlags)16 | (BindingFlags)4;
        private static BindingFlags methFlags = (BindingFlags)16 | (BindingFlags)8;
        public static void Create() {
            skillDef = ScriptableObject.CreateInstance<PassiveItemSkillDef>();
            skillDef.skillNameToken = "GOTCE_VIENDPASSIVE_NAME";
            skillDef.skillDescriptionToken = "GOTCE_VIENDPASSIVE_DESC";
            skillDef.passiveItem = Items.NoTier.ViendAltPassive.Instance.ItemDef;
            skillDef.icon = Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/NEA.png");
            skillDef.activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.Idle));
            skillDef.activationStateMachineName = "Weapon";
        }
        public static void Hooks() {
            // IL hooks

            IL.RoR2.CharacterBody.RecalculateStats += (il) => {
                ILCursor c = new ILCursor(il);

                bool found = c.TryGotoNext(MoveType.After,
                    x => x.MatchLdarg(0),
                    x => x.MatchLdarg(0),
                    x => x.MatchCallOrCallvirt<RoR2.CharacterBody>(nameof(CharacterBody.armor)),
                    x => x.MatchLdarg(0),
                    x => x.MatchLdsfld("RoR2.DLC1Content/Buffs", "VoidSurvivorCorruptMode"),
                    x => x.MatchCallOrCallvirt<RoR2.CharacterBody>("HasBuff"),
                    x => x.MatchBrtrue(out _),
                    x => x.MatchLdcR4(0f),
                    x => x.MatchBr(out _),
                    x => x.MatchLdcR4(100f)
                );

                if (found) {
                    c.Index -= 1;
                    c.Remove();
                    c.EmitDelegate<Func<CharacterBody, float>>((cb) => {
                        if (cb.inventory && cb.inventory.GetItemCount(Items.NoTier.ViendAltPassive.Instance.ItemDef) > 0) {
                            return 0f;
                        }
                        else {
                            return 100f;
                        }
                    });
                }
                else {
                    Main.ModLogger.LogFatal("IL Hook for viend alt passive failed");
                }
            };


            // runtime detour hooks
            Hook minCorruptionHook = new Hook(
                typeof(VoidSurvivorController).GetProperty(nameof(VoidSurvivorController.minimumCorruption), propFlags).GetGetMethod(),
                typeof(ViendAltPassive).GetMethod(nameof(ViendAltPassive.VoidSurvivorController_minimumCorruption_get), methFlags)
            );

            Hook permaCorruptionHook = new Hook(
                typeof(VoidSurvivorController).GetProperty(nameof(VoidSurvivorController.isPermanentlyCorrupted), propFlags).GetGetMethod(),
                typeof(ViendAltPassive).GetMethod(nameof(ViendAltPassive.VoidSurvivorContoller_isPermanentlyCorrupted_get), methFlags)
            );


            def = Items.NoTier.ViendAltPassive.Instance.ItemDef;

            // no more corruption gain on hit
            On.RoR2.VoidSurvivorController.OnTakeDamageServer += (orig, self, report) => {
                if (self.characterBody.inventory.GetItemCount(def) > 0 && NetworkServer.active) {
                    report.damageDealt *= -1;
                }
                orig(self, report);
            };

            // start at full
            /* On.RoR2.VoidSurvivorController.OnEnable += (orig, self) => {
                if (self.characterBody && self.characterBody.inventory.GetItemCount(def) > 0 && NetworkServer.active) {
                    self.AddCorruption(100);
                }
                orig(self);
            }; */

            // armor on heal
            On.RoR2.VoidSurvivorController.OnCharacterHealServer += (orig, self, com, amount, mask) => {
                if (self.characterBody.inventory.GetItemCount(def) > 0) {
                    if (NetworkServer.active) {
                        self.characterBody.armor += 10f;
                    }
                }
                else {
                    orig(self, com, amount, mask);
                }
            };

            // no healing
            On.RoR2.HealthComponent.Heal += (orig, self, amount, mask, regen) => {
                if (NetworkServer.active && self.body.inventory && self.body.inventory.GetItemCount(def) > 0) {
                    amount = 0;
                }
                return orig(self, amount, mask, regen);
            };

            // "lifesteal"
            On.RoR2.VoidSurvivorController.OnDamageDealtServer += (orig, self, info) => {
                orig(self, info);
                if (NetworkServer.active && self.characterBody.inventory.GetItemCount(def) > 0) {
                    self.AddCorruption(info.damageDealt * 0.15f);
                }
            };
            
            // reworked corruption mechanics
            On.RoR2.VoidSurvivorController.FixedUpdate += (orig, self) => {
                if (NetworkServer.active && self.characterBody && self.characterBody.inventory && self.characterBody.inventory.GetItemCount(def) > 0 && self.bodyHealthComponent) {
                    self.corruptionForFullHeal = 0;
                    self.characterBody.armor -= 0.1f * Time.fixedDeltaTime;
                    // self.corruptionFractionPerSecondWhileCorrupted = -0.001f;
                    self.maxCorruption = 100f + ((10f * (self.characterBody.level - 1)));

                    if (self.corruption < 1) {
                        self.AddCorruption(float.MaxValue);
                    }

                    float fraction = self.bodyHealthComponent.fullHealth * (self.corruption / 100);
                    self.bodyHealthComponent.health = fraction;

                    self.corruptionFractionPerSecondWhileCorrupted = self.characterBody.outOfCombat ? -0.022f : -0.044f;
                    if (self.corruption >= 100) {
                        self.corruptionFractionPerSecondWhileCorrupted = -0.66f;
                    }

                    if (self.characterBody.skillLocator) {
                        GenericSkill utility = self.characterBody.skillLocator.utility;
                        if (utility.skillDef) {
                            utility.skillDef.isCombatSkill = false;
                        }
                    }

                    /* if (self.gameObject.GetComponent<CharacterMotor>()) {
                        CharacterMotor motor = self.gameObject.GetComponent<CharacterMotor>();
                        motor.airControl = Mathf.InverseLerp(self.corruption, self.maxCorruption, 5);
                    } */
                }
                orig(self);
            };
        }

        public static bool VoidSurvivorContoller_isPermanentlyCorrupted_get(orig_isPermanentlyCorrupted orig, VoidSurvivorController self) {
            if (self.characterBody && self.characterBody.inventory && self.characterBody.inventory.GetItemCount(Items.NoTier.ViendAltPassive.Instance.ItemDef) > 0) {
                return true;
            }
            else {
                return self.minimumCorruption >= self.maxCorruption;
            }
        }

        public static float VoidSurvivorController_minimumCorruption_get(orig_minimumCorruption orig, VoidSurvivorController self) {
            if (self.characterBody && self.characterBody.inventory && self.characterBody.inventory.GetItemCount(Items.NoTier.ViendAltPassive.Instance.ItemDef) > 0) {
                return 1f;
            }
            else {
                return self.minimumCorruptionPerVoidItem * self.voidItemCount;
            }
        }
    }
}