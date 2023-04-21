/*using System;
using UnityEngine;
using RoR2;
using RoR2.Skills;
using Unity;
using RoR2.Orbs;
using EntityStates.BrotherMonster;

namespace GOTCE.Enemies.Superbosses {
    public class Glassthrix : EnemyBase<Glassthrix> {
        public override string CloneName => "Glassthrix";
        public override string PathToClone => Utils.Paths.GameObject.BrotherBody;
        public override string PathToCloneMaster => Utils.Paths.GameObject.BrotherMaster;
        public override bool local => false;
        private CharacterBody body;
        private static int P1WavesCount = 10;
        private static float WavesInterval = 0.5f;
        private static int P3WavesCount = 50;
        public override void CreatePrefab()
        {
            base.CreatePrefab();
            SwapStats(prefab, 20, 0, 12, 2500, 0, 12, 25);
            body = prefab.GetComponent<CharacterBody>();
            body.baseNameToken = "GOTCE_GLASSTHRIX_NAME";
            body.subtitleNameToken = "GOTCE_GLASSTHRIX_SUBTITLE";
            body.gameObject.AddComponent<WaveController>();
        }
        public override void Modify()
        {
            base.Modify();
            SkillLocator locator = prefab.GetComponent<SkillLocator>();
            ReplaceSkill(locator.primary, Skills.Glassthrix.GlassthrixHammer1.Instance.SkillDef);
            ReplaceSkill(locator.secondary, Skills.Glassthrix.GlassthrixSlash1.Instance.SkillDef);
            ReplaceSkill(locator.utility, Skills.Glassthrix.GlassthrixDash1.Instance.SkillDef);

            ConditionalSkillOverride skillOverride = prefab.GetComponent<ConditionalSkillOverride>();
            GameObject.Destroy(skillOverride);

            DeathRewards deathRewards = prefab.GetComponent<DeathRewards>();
            deathRewards = prefab.AddComponent<DeathRewards>();
            deathRewards.characterBody = body;
            deathRewards.logUnlockableDef = ScriptableObject.CreateInstance<UnlockableDef>();
            deathRewards.logUnlockableDef.nameToken = "GOTCE_GLASSTHRIX_NAME";

            SwapMaterials(prefab, Utils.Paths.Material.maBrotherGlassOverlay.Load<Material>(), true, null);

            LanguageAPI.Add("GOTCE_GLASSTHRIX_NAME", "Glassthrix");
            LanguageAPI.Add("GOTCE_GLASSTHRIX_SUBTITLE", "Cracked King");
            
            GlobalEventManager.onServerDamageDealt += StealItem;
            On.EntityStates.BrotherMonster.ExitSkyLeap.FireRingAuthority += GameDesign;
        }

        public override void PostCreation()
        {
            base.PostCreation();
            RegisterEnemy(prefab, prefabMaster, null);
        }

        private void GameDesign(On.EntityStates.BrotherMonster.ExitSkyLeap.orig_FireRingAuthority orig, ExitSkyLeap self) {
            orig(self);
            WaveController controller = self.GetComponent<WaveController>();
            if (!controller || controller.hasStarted) {
                return;
            }

            self.duration += PhaseCounter.instance.phase == 3 ? P3WavesCount * WavesInterval : P1WavesCount * WavesInterval;
            controller.hasStarted = true;
            controller.count = PhaseCounter.instance.phase == 3 ? P3WavesCount : P1WavesCount;
            controller.exit = self;
            controller.TheFunny();
        }

        private class WaveController : MonoBehaviour {
            public bool hasStarted;
            public int count;
            public ExitSkyLeap exit;

            public void TheFunny() {
                if (count <= 0) {
                    hasStarted = false;
                    return;
                }

                exit.FireRingAuthority();
                count--;
                Invoke(nameof(TheFunny), WavesInterval);
            }
        }

        private void StealItem(DamageReport report) {
            if (NetworkServer.active && report.damageInfo.HasModdedDamageType(DamageTypes.StealItem)) {
                if (report.victimBody && report.victimBody.inventory) {
                    ItemIndex index;
                    try {
                        index = report.victimBody.inventory.itemAcquisitionOrder.First();
                    } catch {
                        return;
                    }
                    int c = report.victimBody.inventory.GetItemCount(index);
                    report.victimBody.inventory.RemoveItem(index, c);
                    ItemTransferOrb orb = new();
                    orb.inventoryToGrantTo = report.attackerBody.inventory;
                    orb.itemIndex = index;
                    orb.stack = c;
                    orb.origin = report.damageInfo.position;
                    orb.target = report.attackerBody.mainHurtBox;
                    OrbManager.instance.AddOrb(orb);
                }
            }
        }
    }
}*/