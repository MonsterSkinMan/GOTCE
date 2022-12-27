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
using EntityStates;
using RoR2.Projectile;
using GOTCE.EntityStatesCustom.AltSkills.Huntress;
// using Mono.Reflection;

namespace GOTCE.Based
{
    public class AltSkills
    {
        public static GameObject huntressSawPrefab;
        public static GameObject railgunnerDumbPrefab;
        public static GameObject suppressiveNaderPrefab;
        public static void AddAlts()
        {
            // PassiveReplacement.RunHooks();
            CommandoAlts();
            RexAlts();
            RGAlts();
            HuntressAlts();
            ViendAlts();
            BanditAlts();
            CaptainAlts();
            EngineerAlts();

            CreateProjectiles();
        }

        private static void CreateProjectiles() {
            // huntress saw
            GameObject prefab = Addressables.LoadAssetAsync<GameObject>(Utils.Paths.GameObject.Sawmerang).WaitForCompletion().InstantiateClone("huntresssaw");
            GameObject prefabGhost = Addressables.LoadAssetAsync<GameObject>(Utils.Paths.GameObject.SawmerangGhost).WaitForCompletion().InstantiateClone("huntresssawghost");

            prefab.AddComponent<ProjectileStickOnImpact>();
            prefab.AddComponent<ProjectileTargetComponent>();
            ProjectileSphereTargetFinder finder = prefab.AddComponent<ProjectileSphereTargetFinder>();
            finder.onlySearchIfNoTarget = true;
            finder.targetSearchInterval = 0.1f;
            finder.lookRange = 9f;
            ProjectileDotZone zone = prefab.GetComponent<ProjectileDotZone>();
            zone.damageCoefficient = 0.6f;
            zone.resetFrequency = 1f;
            GameObject.Destroy(prefab.GetComponent<BoomerangProjectile>());
            prefab.AddComponent<SawBehavior>();
            prefab.AddComponent<ProjectileSimple>();
            ProjectileSimple simple = prefab.GetComponent<ProjectileSimple>();
            simple.desiredForwardSpeed = 70f;
            prefab.transform.localScale = new(5f, 5f, 5f);
            prefabGhost.transform.localScale = new(5f, 5f, 5f);
            prefab.GetComponent<ProjectileController>().ghostPrefab = prefabGhost;

            huntressSawPrefab = prefab;
            ContentAddition.AddProjectile(huntressSawPrefab);

            prefab = Addressables.LoadAssetAsync<GameObject>(Utils.Paths.GameObject.RailgunnerPistolProjectile).WaitForCompletion().InstantiateClone("dumbrounds");
            GameObject.DestroyImmediate(prefab.GetComponent<ProjectileSteerTowardTarget>());
            GameObject.DestroyImmediate(prefab.GetComponent<ProjectileTargetComponent>());

            railgunnerDumbPrefab = prefab;
            ContentAddition.AddProjectile(railgunnerDumbPrefab);

            prefab = Addressables.LoadAssetAsync<GameObject>(Utils.Paths.GameObject.CommandoGrenadeProjectile).WaitForCompletion().InstantiateClone("nader");
            ProjectileImpactExplosion impact = prefab.AddComponent<ProjectileImpactExplosion>();
            impact.destroyOnEnemy = true;
            impact.destroyOnWorld = true;
            impact.explosionEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXQuick");

            suppressiveNaderPrefab = prefab;
            ContentAddition.AddProjectile(prefab);
        }

        private static void CommandoAlts()
        {
            GameObject commandoPrefab = Addressables.LoadAssetAsync<GameObject>(Utils.Paths.GameObject.CommandoBody).WaitForCompletion();

            SkillLocator s1 = commandoPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily;
            skillFamily = s1.primary.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.SuperShotgun.Instance.SkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(Skills.SuperShotgun.Instance.SkillDef.skillNameToken, false, null)
            };

            LanguageAPI.Add(Skills.SuperShotgun.Instance.SkillDef.skillNameToken, "Doom Blast");
            LanguageAPI.Add(Skills.SuperShotgun.Instance.SkillDef.skillDescriptionToken, "Fire a slow, but powerful blast of <style=cIsDamage>20</style> bullets for <style=cIsDamage>20x100% damage</style>.");
        }

        private static void RexAlts()
        {
            GameObject treebotPrefab = Addressables.LoadAssetAsync<GameObject>(Utils.Paths.GameObject.TreebotBody).WaitForCompletion();

            SkillLocator sl = treebotPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily;
            // stigmata shotgun
            skillFamily = sl.primary.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.SigmaShotgun.Instance.SkillDef,
                unlockableDef = Achievements.REX.AutomatonUnlock.Instance.enabled ? Achievements.REX.AutomatonUnlock.Instance.def : null,
                viewableNode = new ViewablesCatalog.Node(Skills.SigmaShotgun.Instance.SkillDef.skillNameToken, false, null)
            };

            LanguageAPI.Add(Skills.SigmaShotgun.Instance.SkillDef.skillNameToken, "Stigmata Shotgun");
            LanguageAPI.Add(Skills.SigmaShotgun.Instance.SkillDef.skillDescriptionToken, "<style=cIsDamage>Weakens</style>. Fire <style=cIsDamage>9</style> pollen pellets for <style=cIsDamage9x50% damage</style>. 5% HP");
        }

        private static void RGAlts()
        {
            MagneticPropulsor.Create();
            GameObject rgPrefab = Addressables.LoadAssetAsync<GameObject>(Utils.Paths.GameObject.RailgunnerBody).WaitForCompletion();

            SkillLocator sl = rgPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily;
            // dumb rounds
            skillFamily = sl.primary.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.DumbRounds.Instance.SkillDef,
                unlockableDef = Achievements.Railgunner.StupidRoundsUnlock.Instance.enabled ? Achievements.Railgunner.StupidRoundsUnlock.Instance.def : null,
                viewableNode = new ViewablesCatalog.Node(Skills.SigmaShotgun.Instance.SkillDef.skillNameToken, false, null)
            };

            LanguageAPI.Add(Skills.DumbRounds.Instance.SkillDef.skillNameToken, "CJI Stupid Rounds");
            LanguageAPI.Add(Skills.DumbRounds.Instance.SkillDef.skillDescriptionToken, "Fire a highly inaccurate spread of 30 rounds per second for 100% each. Spread scales with field of view.");

            // PassiveReplacement.ReplacePassiveSlot(rgPrefab, MagneticPropulsor.defAlt, MagneticPropulsor.def);

            LanguageAPI.Add(MagneticPropulsor.defAlt.skillNameToken, "Magnetic Propulsors");
            LanguageAPI.Add(MagneticPropulsor.defAlt.skillDescriptionToken, "Critical Strike chance is converted into Jump Height.");

            foreach (GenericSkill skill in rgPrefab.GetComponentsInChildren<GenericSkill>())
            {
                if ((skill._skillFamily as ScriptableObject).name.Contains("Passive"))
                {
                    SkillFamily family = skill._skillFamily;
                    Array.Resize(ref family.variants, family.variants.Length + 1);
                    family.variants[family.variants.Length - 1] = new SkillFamily.Variant
                    {
                        skillDef = MagneticPropulsor.defAlt,
                        unlockableName = "",
                        viewableNode = new ViewablesCatalog.Node(MagneticPropulsor.defAlt.skillNameToken, false, null)
                    };
                }
            }
            LanguageAPI.Add(MagneticPropulsor.defAlt.skillDescriptionToken, "All <style=cIsDamage>Critical Strike Chance</style> is converted into <style=cIsUtility>Jump Height</style>.");
        }

        private static void HuntressAlts()
        {
            GameObject huntressPrefab = Addressables.LoadAssetAsync<GameObject>(Utils.Paths.GameObject.HuntressBody).WaitForCompletion();

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
            LanguageAPI.Add(Skills.Sawblade.Instance.SkillDef.skillDescriptionToken, "Throw a piercing <style=cIsDamage>sawblade</style> that dashes onto a nearby target, dealing <style=cIsDamage>150%</style> damage and sticking to surfaces, dealing <style=cIsDamage>60%</style> <style=cDeath>bleed</style> damage.");

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

        private static void BanditAlts()
        {
            GameObject banditPrefab = Addressables.LoadAssetAsync<GameObject>(Utils.Paths.GameObject.Bandit2Body).WaitForCompletion();

            SkillLocator sl = banditPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily;
            // decoy
            skillFamily = sl.utility.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.Decoy.Instance.SkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(Skills.Decoy.Instance.SkillDef.skillNameToken, false, null)
            };

            LanguageAPI.Add(Skills.Decoy.Instance.SkillDef.skillNameToken, "Explosive Decoy");
            LanguageAPI.Add(Skills.Decoy.Instance.SkillDef.skillDescriptionToken, "Deploy a decoy that <style=cIsUtility>draws enemy attention</style> for <style=cIsUtility>5</style> seconds before exploding in a damaging blast for <style=cIsDamage>200% damage</style>. Explodes early if killed.");

            LanguageAPI.Add("GOTCE_EXPLOSIVEDECOY_NAME", "Explosive Decoy");
        }

        private static void CaptainAlts()
        {
            GameObject captainPrefab = Addressables.LoadAssetAsync<GameObject>(Utils.Paths.GameObject.CaptainBody).WaitForCompletion();

            SkillLocator sl = captainPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily;
            // hephastaeus shotgun
            skillFamily = sl.primary.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.Overheat.Instance.SkillDef,
                unlockableDef = Achievements.Captain.OverheatUnlock.Instance.enabled ? Achievements.Captain.OverheatUnlock.Instance.def : null,
                viewableNode = new ViewablesCatalog.Node(Skills.Overheat.Instance.SkillDef.skillNameToken, false, null)
            };

            LanguageAPI.Add(Skills.Overheat.Instance.SkillDef.skillNameToken, "Hephaestus Shotgun");
            LanguageAPI.Add(Skills.Overheat.Instance.SkillDef.skillDescriptionToken, "Charge up a blast of rapid fire <style=cIsDamage>incendiary</style> rounds for <style=cIsDamage>60% damage each</style>. The amount of bullets fired increases with charge time, but so does spread.");
        }

        private static void EngineerAlts()
        {
            GameObject engiPrefab = Addressables.LoadAssetAsync<GameObject>(Utils.Paths.GameObject.EngiBody).WaitForCompletion();

            SkillLocator sl = engiPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily;

            EntanglerHooks.Hook();
            // entangler
            skillFamily = sl.primary.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.Entangler.Instance.SkillDef,
                unlockableDef = null,
                viewableNode = new ViewablesCatalog.Node(Skills.Overheat.Instance.SkillDef.skillNameToken, false, null)
            };

            LanguageAPI.Add(Skills.Entangler.Instance.SkillDef.skillNameToken, "Entangler");
            LanguageAPI.Add(Skills.Entangler.Instance.SkillDef.skillDescriptionToken, "Take manual control of your mechanical allies, giving you and them <style=cIsUtility>+40% movement speed</style>. <style=cIsHealth>Reduce your armor by -50</style>. Entangled allies will be <style=cIsHealth>disabled</style> for <style=cIsHealth>5</style> seconds after exiting Entangler.");
        }

        private static void ViendAlts()
        {
            GameObject viendPrefab = Addressables.LoadAssetAsync<GameObject>(Utils.Paths.GameObject.VoidSurvivorBody).WaitForCompletion();

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

            LanguageAPI.Add(Skills.Pearl.Instance.SkillDef.skillNameToken, "「War??p』");
            LanguageAPI.Add(Skills.Pearl.Instance.SkillDef.skillDescriptionToken, "Launch a void orb that sticks to surfaces and repeatedly strikes for <style=cIsDamage>60% damage</style>. Use while a void orb is active to <style=cIsUtility>teleport</style> to it, destroying it.");

            LanguageAPI.Add(Skills.PearlTeleport.Instance.SkillDef.skillNameToken, "【Retur??n」");
            LanguageAPI.Add(Skills.PearlTeleport.Instance.SkillDef.skillDescriptionToken, "<style=cIsUtility>Teleport</style> to your most recently deployed void orb.");

            LanguageAPI.Add("GOTCE_CORRUPTIONM2UPGRADE_KEYWORD", "[ 【Corruption Upgrade】 ]\nLaunch a powerful void spear that teleports you on impact, releasing a devastating explosion for 2600% damage.");
            LanguageAPI.Add("GOTCE_CORRUPTIONSPECIALUPGRADE_KEYWORD", "[ 【Corruption Upgrade】 ]\nRoot yourself, gaining corruption while rooted... Release a devastating <style=cIsVoid>void implosion</style> upon unrooting");

            // drain
            skillFamily = sl.special.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.Drain.Instance.SkillDef,
                unlockableDef = Achievements.VoidFiend.InsurrectionUnlock.Instance.GetUnlockableDef(),
                viewableNode = new ViewablesCatalog.Node(Skills.Drain.Instance.SkillDef.skillNameToken, false, null)
            };

            LanguageAPI.Add(Skills.Drain.Instance.SkillDef.skillNameToken, "『Dr??ain】");
            LanguageAPI.Add(Skills.Drain.Instance.SkillDef.skillDescriptionToken, "Rapidly drain your <style=cIsVoid>Corruption</style>, and fire a devastating blast for <style=cIsDamage>100% damage</style> that massively scales with <style=cIsVoid>Corruption</style> consumed.");

            foreach (GenericSkill skill in viendPrefab.GetComponentsInChildren<GenericSkill>())
            {
                if ((skill._skillFamily as ScriptableObject).name.Contains("Passive"))
                {
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

            // drain corrupt

            LanguageAPI.Add(Skills.DrainUpgrade.Instance.SkillDef.skillNameToken, "【Dr??ain』");
            LanguageAPI.Add(Skills.DrainUpgrade.Instance.SkillDef.skillDescriptionToken, "Root yourself temporarily, gaining a large amount of <style=cIsVoid>Corruption</style>. Release a devastating laser upon breaking free.");

            LanguageAPI.Add("GOTCE_VIENDPASSIVE_NAME", "『The Only Thing They Fear】");
            LanguageAPI.Add("GOTCE_VIENDPASSIVE_DESC", "You are permanently <style=cIsVoid>Corrupted</style>. Gain <style=cIsVoid>Corruption</style> on hit. Your current health depends on your <style=cIsVoid>Corruption</style> at all times.");

            LanguageAPI.Add("GOTCE_PEARLUPGRADE_NAME", "『War??p」");
            LanguageAPI.Add("GOTCE_PEARLUPGRADE_DESC", "Launch a powerful void spike, <style=cIsUtility>teleporting</style> you to it and releasing a devastating void explosion for <style=cIsDamage>700% damage</style>.");

            // alt skills hooks

            On.EntityStates.VoidSurvivor.CorruptMode.CorruptMode.OnEnter += (orig, self) =>
            {
                bool hasSecondaryAlt = false;
                bool hasPrimaryAlt = false;
                bool hasUtilityAlt = false;
                bool hasSpecialAlt = false;
                bool exists = self.skillLocator && self.skillLocator.secondary && self.skillLocator.primary && self.skillLocator.utility && self.skillLocator.special;
                if (self.isAuthority && exists)
                {
                    hasSecondaryAlt = self.skillLocator.secondary.skillNameToken == Skills.Pearl.Instance.SkillDef.skillNameToken || self.skillLocator.secondary.skillNameToken == Skills.PearlTeleport.Instance.SkillDef.skillNameToken;
                    hasSpecialAlt = self.skillLocator.special.skillNameToken == Skills.Drain.Instance.SkillDef.skillNameToken;
                }
                orig(self);
                if (self.isAuthority && exists)
                {
                    if (hasSecondaryAlt)
                    {
                        self.skillLocator.secondary.UnsetSkillOverride(self.gameObject, self.secondaryOverrideSkillDef, GenericSkill.SkillOverridePriority.Upgrade);
                        self.skillLocator.secondary.SetSkillOverride(self.gameObject, Skills.PearlUpgrade.Instance.SkillDef, GenericSkill.SkillOverridePriority.Upgrade);
                    }

                    if (hasSpecialAlt)
                    {
                        self.skillLocator.special.UnsetSkillOverride(self.gameObject, self.specialOverrideSkillDef, GenericSkill.SkillOverridePriority.Upgrade);
                        self.skillLocator.special.SetSkillOverride(self.gameObject, Skills.DrainUpgrade.Instance.SkillDef, GenericSkill.SkillOverridePriority.Upgrade);
                    }
                }
            };

            On.EntityStates.VoidSurvivor.CorruptMode.CorruptMode.OnExit += (orig, self) =>
            {
                bool hasSecondaryAlt = false;
                bool hasPrimaryAlt = false;
                bool hasUtilityAlt = false;
                bool hasSpecialAlt = false;
                if (self.isAuthority)
                {
                    hasSecondaryAlt = self.skillLocator.secondary.skillNameToken == Skills.PearlUpgrade.Instance.SkillDef.skillNameToken;
                    hasSpecialAlt = self.skillLocator.special.skillNameToken == Skills.DrainUpgrade.Instance.SkillDef.skillNameToken;
                }
                orig(self);
                if (self.isAuthority)
                {
                    if (hasSecondaryAlt)
                    {
                        self.skillLocator.secondary.UnsetSkillOverride(self.gameObject, Skills.PearlUpgrade.Instance.SkillDef, GenericSkill.SkillOverridePriority.Upgrade);
                    }

                    if (hasSpecialAlt)
                    {
                        self.skillLocator.special.UnsetSkillOverride(self.gameObject, Skills.DrainUpgrade.Instance.SkillDef, GenericSkill.SkillOverridePriority.Upgrade);
                    }
                }
            };
        }
    }

    public class MagneticPropulsor
    {
        public static PassiveItemSkillDef defAlt;
        public static PassiveItemSkillDef def;

        public static void Create()
        {
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
            defAlt.baseRechargeInterval = 0f;
        }
    }

    public class ViendAltPassive
    {
        private static ItemDef def;

        public delegate float orig_minimumCorruption(VoidSurvivorController self);

        public delegate bool orig_isPermanentlyCorrupted(VoidSurvivorController self);

        public static PassiveItemSkillDef skillDef;
        private static readonly BindingFlags propFlags = (BindingFlags)16 | (BindingFlags)4;
        private static readonly BindingFlags methFlags = (BindingFlags)16 | (BindingFlags)8;

        public static void Create()
        {
            skillDef = ScriptableObject.CreateInstance<PassiveItemSkillDef>();
            skillDef.skillNameToken = "GOTCE_VIENDPASSIVE_NAME";
            skillDef.skillDescriptionToken = "GOTCE_VIENDPASSIVE_DESC";
            skillDef.passiveItem = Items.NoTier.ViendAltPassive.Instance.ItemDef;
            skillDef.icon = Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/NEA.png");
            skillDef.activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.Idle));
            skillDef.activationStateMachineName = "Weapon";
        }

        public static void Hooks()
        {
            // IL hooks

            /* IL.RoR2.CharacterBody.RecalculateStats += (il) => {
                ILCursor c = new ILCursor(il);

                bool found = c.TryGotoNext(MoveType.After,
                    x => x.MatchLdsfld(typeof(DLC1Content.Buffs), nameof(DLC1Content.Buffs.VoidSurvivorCorruptMode)),
                    x => x.MatchCallOrCallvirt<CharacterBody>(nameof(CharacterBody.HasBuff)),
                    x => x.MatchBrtrue(out _),
                    x => x.MatchLdcR4(0f),
                    x => x.MatchBr(out _),
                    x => x.MatchLdcR4(100f)
                );

                if (found) {
                    c.Index--;
                    c.Next.Operand = 0f;
                    c.Emit(OpCodes.Ldarg_0);
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
            }; */

            // runtime detour hooks
            Hook minCorruptionHook = new(
                typeof(VoidSurvivorController).GetProperty(nameof(VoidSurvivorController.minimumCorruption), propFlags).GetGetMethod(),
                typeof(ViendAltPassive).GetMethod(nameof(ViendAltPassive.VoidSurvivorController_minimumCorruption_get), methFlags)
            );

            Hook permaCorruptionHook = new(
                typeof(VoidSurvivorController).GetProperty(nameof(VoidSurvivorController.isPermanentlyCorrupted), propFlags).GetGetMethod(),
                typeof(ViendAltPassive).GetMethod(nameof(ViendAltPassive.VoidSurvivorContoller_isPermanentlyCorrupted_get), methFlags)
            );

            def = Items.NoTier.ViendAltPassive.Instance.ItemDef;

            // no more corruption gain on hit
            On.RoR2.VoidSurvivorController.OnTakeDamageServer += (orig, self, report) =>
            {
                if (self.characterBody.inventory.GetItemCount(def) > 0 && NetworkServer.active)
                {
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
            
            On.RoR2.VoidSurvivorController.OnCharacterHealServer += (orig, self, com, amount, mask) =>
            {
                if (self.characterBody.inventory.GetItemCount(def) > 0)
                {
                    if (NetworkServer.active)
                    {
                        self.characterBody.armor += 25f;
                        self.characterBody.baseArmor += 25f;
                    }
                }
                else
                {
                    orig(self, com, amount, mask);
                }
            };
            

            // no healing
            On.RoR2.HealthComponent.Heal += (orig, self, amount, mask, regen) =>
            {
                if (NetworkServer.active && self.body.inventory && self.body.inventory.GetItemCount(def) > 0)
                {
                    amount = 0;
                }
                return orig(self, amount, mask, regen);
            };

            // "lifesteal"
            On.RoR2.VoidSurvivorController.OnDamageDealtServer += (orig, self, info) =>
            {
                orig(self, info);
                if (NetworkServer.active && self.characterBody.inventory.GetItemCount(def) > 0)
                {
                    self.AddCorruption(Mathf.Max(1f, Mathf.Sqrt(info.damageDealt) / 2.4f));
                    // previously info.damageDealt * 0.1
                }
            };

            On.EntityStates.VoidSurvivor.Weapon.FireCorruptHandBeam.FixedUpdate += (orig, self) =>
            {
                if (self.isAuthority && self.characterBody.inventory.GetItemCount(def) > 0)
                {
                    // self.characterBody.AddTimedBuff(Buffs.ViendNoArmor.instance.BuffDef, 1.5f);
                    self.characterBody.baseArmor -= 3f * Time.fixedDeltaTime;
                    self.characterBody.armor -= 3f * Time.fixedDeltaTime;
                }
                orig(self);
            };

            // reworked corruption mechanics
            On.RoR2.VoidSurvivorController.FixedUpdate += (orig, self) =>
            {
                if (self.characterBody.hasAuthority && self.characterBody && self.characterBody.inventory && self.characterBody.inventory.GetItemCount(def) > 0 && self.bodyHealthComponent)
                {
                    self.corruptionForFullHeal = 0;
                    self.maxCorruption = 100f/* + ((10f * (self.characterBody.level - 1)))*/;

                    if (self.corruption < 1)
                    {
                        self.AddCorruption(float.MaxValue);
                    }

                    float fraction = self.bodyHealthComponent.fullHealth * (self.corruption / 100);
                    self.bodyHealthComponent.health = fraction;

                    self.corruptionFractionPerSecondWhileCorrupted = self.characterBody.outOfCombat ? -0.022f : -0.04f;
                    // -0.044 => -0.04
                    if (self.corruption >= 100)
                    {
                        self.corruptionFractionPerSecondWhileCorrupted = -0.066f;
                    }

                    if (self.characterBody.skillLocator)
                    {
                        GenericSkill utility = self.characterBody.skillLocator.utility;
                        if (utility.skillDef)
                        {
                            utility.skillDef.isCombatSkill = false;
                        }
                    }

                    /* if (self.gameObject.GetComponent<CharacterMotor>()) {
                        CharacterMotor motor = self.gameObject.GetComponent<CharacterMotor>();
                        motor.airControl = Mathf.InverseLerp(self.corruption, self.maxCorruption, 5);
                    } */

                    if (self.characterBody.armor < 0) {
                        self.characterBody.baseArmor += 1.5f * Time.fixedDeltaTime;
                        self.characterBody.armor += 1.5f * Time.fixedDeltaTime;
                    }
                    else if (self.characterBody.armor > 0.5) {
                        self.characterBody.baseArmor -= 0.1f * Time.fixedDeltaTime;
                        self.characterBody.armor -= 0.1f * Time.fixedDeltaTime;
                    }
                }
                orig(self);
            };
        }

        public static bool VoidSurvivorContoller_isPermanentlyCorrupted_get(orig_isPermanentlyCorrupted orig, VoidSurvivorController self)
        {
            if (self.characterBody && self.characterBody.inventory && self.characterBody.inventory.GetItemCount(Items.NoTier.ViendAltPassive.Instance.ItemDef) > 0)
            {
                return true;
            }
            else
            {
                return self.minimumCorruption >= self.maxCorruption;
            }
        }

        public static float VoidSurvivorController_minimumCorruption_get(orig_minimumCorruption orig, VoidSurvivorController self)
        {
            if (self.characterBody && self.characterBody.inventory && self.characterBody.inventory.GetItemCount(Items.NoTier.ViendAltPassive.Instance.ItemDef) > 0)
            {
                return 1f;
            }
            else
            {
                return self.minimumCorruptionPerVoidItem * self.voidItemCount;
            }
        }
    }
}