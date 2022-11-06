using UnityEngine;
using R2API;
using BepInEx;
using RoR2.Projectile;
using RoR2;
using System.Collections.ObjectModel;
using System;
using RoR2.Items;
using System.Collections.Generic;
using HG;
using RoR2.Skills;
using RoR2.Orbs;

namespace GOTCE.Based {
    public class SuppressiveNader {
        public static void Hook() {
            On.RoR2.GlobalEventManager.OnHitEnemy += Nader;

            /* GameObject commandoBodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoBody.prefab").WaitForCompletion();

            SkillLocator skillLocator = commandoBodyPrefab.GetComponent<SkillLocator>();
            SkillFamily skillFamily = skillLocator.primary.skillFamily;

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.DoubleDoubleTap.Instance.SkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(Skills.DoubleDoubleTap.Instance.SkillDef.skillNameToken, false, null)
            };

            skillFamily = skillLocator.secondary.skillFamily;

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.PhaseRounder.Instance.SkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(Skills.PhaseRounder.Instance.SkillDef.skillNameToken, false, null)
            };

            skillFamily = skillLocator.special.skillFamily;

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.SuppressiveNader.Instance.SkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(Skills.SuppressiveNader.Instance.SkillDef.skillNameToken, false, null)
            }; */



            LanguageAPI.Add(Skills.VeryTactical.Instance.SkillDef.skillNameToken, "Very Tactical Diving Slide");
            LanguageAPI.Add(Skills.VeryTactical.Instance.SkillDef.skillDescriptionToken, "Literally just gives you 5 seconds of complete flight.");
            LanguageAPI.Add(Skills.DoubleDoubleTap.Instance.SkillDef.skillNameToken, "Double Double Double Double Double Tap");
            LanguageAPI.Add(Skills.DoubleDoubleTap.Instance.SkillDef.skillDescriptionToken, "Fire bullets all around you for 96x100% with a 1.0 proc coeff. Cannot miss.");
            LanguageAPI.Add(Skills.SuppressiveNader.Instance.SkillDef.skillNameToken, "Suppressive Nader");
            LanguageAPI.Add(Skills.SuppressiveNader.Instance.SkillDef.skillDescriptionToken, "Fires 8 stunning nades that stick to enemies, spawn multiple allies on hit, apply almost every debuff, spawn every portal, eradicate crowdfunder from the universe, give the regenerative buff, cleanses debuffs and corrupts all items on hit.");
            LanguageAPI.Add(Skills.PhaseRounder.Instance.SkillDef.skillNameToken, "Phase Blaster Round");
            LanguageAPI.Add(Skills.PhaseRounder.Instance.SkillDef.skillDescriptionToken, "Fire a spread of Phase Rounds for 8x360% that split into chain lightning on hit");
            LanguageAPI.Add(Skills.SuppressiveBarrage.Instance.SkillDef.skillNameToken, "Suppressive Barrage");
            LanguageAPI.Add(Skills.SuppressiveBarrage.Instance.SkillDef.skillDescriptionToken, "Unleash a rapid-fire barrage of 128 stunning bullets for 100% damage.");
        }

        public static void Nader(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo info, GameObject victim) {
            if (info.HasModdedDamageType(DamageTypes.NaderEffect)) {
                info.damageType |= DamageType.Stun1s | DamageType.FruitOnHit | DamageType.BypassArmor | DamageType.SlowOnHit | DamageType.WeakPointHit | DamageType.IgniteOnHit | DamageType.Freeze2s | DamageType.BleedOnHit | DamageType.ClayGoo | DamageType.PoisonOnHit | DamageType.PercentIgniteOnHit | DamageType.Nullify | DamageType.Shock5s | DamageType.ResetCooldownsOnKill | DamageType.ApplyMercExpose | DamageType.SuperBleedOnCrit;
                if (victim && victim.GetComponent<CharacterBody>()) {
                    CharacterBody body = victim.GetComponent<CharacterBody>();
                    CharacterBody body2 = info.attacker.GetComponent<CharacterBody>();
                    BuffDef[] buffs = {
                        RoR2Content.Buffs.PermanentCurse,
                        RoR2Content.Buffs.Fruiting,
                        RoR2Content.Buffs.BeetleJuice,
                        RoR2Content.Buffs.Bleeding,
                        RoR2Content.Buffs.Blight,
                        RoR2Content.Buffs.OnFire,
                        RoR2Content.Buffs.Poisoned,
                        RoR2Content.Buffs.Slow60,
                        DLC1Content.Buffs.PermanentDebuff,
                        DLC1Content.Buffs.StrongerBurn,
                        DLC1Content.Buffs.KillMoveSpeed,
                        RoR2Content.Buffs.LunarDetonationCharge,
                        RoR2Content.Buffs.Weak,
                        RoR2Content.Buffs.MercExpose,
                        RoR2Content.Buffs.SuperBleed,
                        RoR2Content.Buffs.Cripple,
                    };
                    for (int i = 0; i < buffs.Length; i++) {
                        body.AddTimedBuff(buffs[i], 10f);
                    }

                    body2.AddTimedBuff(RoR2Content.Buffs.CrocoRegen, 10f);

                    DotController.DotIndex[] indexes = {
                        DotController.DotIndex.Bleed,
                        DotController.DotIndex.Burn,
                        DotController.DotIndex.Blight,
                        DotController.DotIndex.Poison,
                        DotController.DotIndex.PercentBurn,
                        DotController.DotIndex.StrongerBurn,
                        DotController.DotIndex.Helfire,
                        DotController.DotIndex.SuperBleed,
                    };

                    foreach (DotController.DotIndex index in indexes) {
                        DotController.InflictDot(victim, info.attacker, index, 10f);
                    }

                    Run.instance.availableEquipment.Remove(RoR2Content.Equipment.GoldGat.equipmentIndex);

                    try {
                        CharacterSpawnCard vrailer = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/DLC1/VoidJailer/cscVoidJailerAlly.asset").WaitForCompletion();
                        CharacterSpawnCard vreaver = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/Base/Nullifier/cscNullifierAlly.asset").WaitForCompletion();
                        CharacterSpawnCard vrevastator = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/DLC1/VoidMegaCrab/cscVoidMegaCrabAlly.asset").WaitForCompletion();
                        CharacterSpawnCard beeble = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/Base/BeetleGland/cscBeetleGuardAlly.asset").WaitForCompletion();
                        CharacterSpawnCard vranacle = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/DLC1/VoidBarnacle/cscVoidBarnacleAlly.asset").WaitForCompletion();
                        CharacterSpawnCard squrret = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/Base/Squid/cscSquidTurret.asset").WaitForCompletion();
                        CharacterSpawnCard[] cards = {
                            vrailer,
                            vreaver,
                            vrevastator,
                            vranacle,
                            beeble,
                            squrret
                        };

                        DirectorPlacementRule placementRule = new DirectorPlacementRule
                        {
                            placementMode = DirectorPlacementRule.PlacementMode.Approximate,
                            minDistance = 3f,
                            maxDistance = 40f,
                            spawnOnTarget = info.attacker.transform
                        };
                        foreach (CharacterSpawnCard card in cards) {
                            DirectorSpawnRequest directorSpawnRequest = new DirectorSpawnRequest(card, placementRule, Run.instance.stageRng);
                            directorSpawnRequest.summonerBodyObject = info.attacker;
                            DirectorCore.instance.TrySpawnObject(directorSpawnRequest);
                        }
                    } catch (NullReferenceException err) {
                        Main.ModLogger.LogError("Suppressive Nader could not spawn an ally");
                        Main.ModLogger.LogError("Error: " + err);
                    }

                    TeleporterInteraction.instance.shouldAttemptToSpawnGoldshoresPortal = true;
                    TeleporterInteraction.instance.shouldAttemptToSpawnShopPortal = true;
                    TeleporterInteraction.instance.shouldAttemptToSpawnMSPortal = true;

                    try {
                        MasterSummon masterSummon2 = new MasterSummon();
                        masterSummon2.position = info.attacker.transform.position;
                        masterSummon2.ignoreTeamMemberLimit = true;
                        masterSummon2.masterPrefab = GlobalEventManager.CommonAssets.eliteEarthHealerMaster;
                        masterSummon2.summonerBodyObject = info.attacker;
                        masterSummon2.rotation = Quaternion.LookRotation(info.attacker.transform.forward);
                        masterSummon2.Perform();
                        
                        masterSummon2 = new MasterSummon();
                        masterSummon2.position = info.attacker.transform.position;
                        masterSummon2.ignoreTeamMemberLimit = true;
                        masterSummon2.masterPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/EliteVoid/VoidInfestorMaster.prefab").WaitForCompletion();
                        masterSummon2.summonerBodyObject = info.attacker;
                        masterSummon2.rotation = Quaternion.LookRotation(info.attacker.transform.forward);
                        masterSummon2.Perform();
                    } catch (NullReferenceException err) {
                        Main.ModLogger.LogError("Suppressive Nader could not perform a master summon");
                        Main.ModLogger.LogError("Error: " + err);
                    }
                    
                    Util.CleanseBody(body2, true, false, true, true, true, false);

                    using ReadOnlyArray<ContagiousItemManager.TransformationInfo>.Enumerator enumerator = ContagiousItemManager.transformationInfos.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        ContagiousItemManager.TryForceReplacement(originalItemIndex: enumerator.Current.originalItem, inventory: body2.inventory);
                    }
                }
            }

            if (info.HasModdedDamageType(DamageTypes.FullChainLightning) && info.attacker) {
                CharacterBody body = info.attacker.GetComponent<CharacterBody>();
                LightningOrb lightningOrb = new LightningOrb
                {
                    origin = body.corePosition,
                    damageValue = body.damage * 2f,
                    isCrit = true,
                    bouncesRemaining = 3,
                    teamIndex = body.teamComponent.teamIndex,
                    attacker = info.attacker,
                    procCoefficient = 1f,
                    lightningType = LightningOrb.LightningType.Tesla,
                    damageColorIndex = DamageColorIndex.Item,
                    range = 35f,
                    damageCoefficientPerBounce = 1f,
                    bouncedObjects = new List<HealthComponent> { victim.GetComponent<HealthComponent>() },
                };
                lightningOrb.damageValue = body.damage;
                HurtBox hurtbox = lightningOrb.PickNextTarget(body.corePosition);
                if (hurtbox) {
                    lightningOrb.target = hurtbox;
                    OrbManager.instance.AddOrb(lightningOrb);
                }
            }
            orig(self, info, victim);
        }
    }
}