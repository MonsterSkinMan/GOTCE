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

namespace GOTCE.Based {
    public class SuppressiveNader {
        public static void Hook() {
            On.RoR2.GlobalEventManager.OnHitEnemy += Nader;

            GameObject commandoBodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoBody.prefab").WaitForCompletion();

            SkillLocator skillLocator = commandoBodyPrefab.GetComponent<SkillLocator>();
            SkillFamily skillFamily = skillLocator.special.skillFamily;

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.SuppressiveNader.Instance.SkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(Skills.SuppressiveNader.Instance.SkillDef.skillNameToken, false, null)
            };

            LanguageAPI.Add(Skills.SuppressiveNader.Instance.SkillDef.skillNameToken, "Suppressive Nader");
            LanguageAPI.Add(Skills.SuppressiveNader.Instance.SkillDef.skillDescriptionToken, "Fires 6 stunning nades (scales with attack speed) (stacks ruin) (applies exposed) (they're impact too) (weakens) (refunds cooldown on kill) (shoots phase round lightning) (summons a beetle guard) (spawns a squid turret) (spawns a blue orb) (spawns a gold orb) (applies bleed) (applies malachite) (freezes) (immobilizes) (roots) (spawns a celestial orb) (applies hemorrhage) (applies poison) (applies blight) (applies burn) (cripples) (applies fruiting) (has +2 aoe effect) (applies stronger burn) (applies permanent armor reduction) (spawns a purple orb) (applies void fog) (applies collapse) (spawns a mending bomb) (spawns a friendly void infestor) (spawns a friendly void reaver) (spawns a friendly void jailer) (spawns a friendly void devastator) (spawns a friendly void barnacle) (eradicates crowdfunder from the universe) (slayer) (gives you a desperado token on kill) (heavy) (corrupts all corruptible items) (agile) (cleanses all debuffs) (gives you the hunter's harpoon buff) (regenerative) (Enters you into a frenzy)");
        }

        public static void Nader(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo info, GameObject victim) {
            if (info.HasModdedDamageType(Main.nader)) {
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

                    TeleporterInteraction.instance.shouldAttemptToSpawnGoldshoresPortal = true;
                    TeleporterInteraction.instance.shouldAttemptToSpawnShopPortal = true;
                    TeleporterInteraction.instance.shouldAttemptToSpawnMSPortal = true;

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
                    
                    Util.CleanseBody(body2, true, false, true, true, true, false);

                    using ReadOnlyArray<ContagiousItemManager.TransformationInfo>.Enumerator enumerator = ContagiousItemManager.transformationInfos.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        ContagiousItemManager.TryForceReplacement(originalItemIndex: enumerator.Current.originalItem, inventory: body2.inventory);
                    }
                }
            }
        }
    }
}