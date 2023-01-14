using System;
using UnityEngine;
using RoR2;
using RoR2.Skills;
using Unity;
using RoR2.Orbs;

namespace GOTCE.Enemies.Superbosses {
    public class Glassthrix : EnemyBase<Glassthrix> {
        public override string CloneName => "Glassthrix";
        public override string PathToClone => Utils.Paths.GameObject.BrotherBody;
        public override string PathToCloneMaster => Utils.Paths.GameObject.BrotherMaster;
        public override bool local => false;
        private CharacterBody body;
        public override void CreatePrefab()
        {
            base.CreatePrefab();
            SwapStats(prefab, 20, 0, 12, 2500, 0, 12, 25);
            body = prefab.GetComponent<CharacterBody>();
            body.baseNameToken = "GOTCE_GLASSTHRIX_NAME";
            body.subtitleNameToken = "GOTCE_GLASSTHRIX_SUBTITLE";
        }
        public override void Modify()
        {
            base.Modify();
            SkillLocator locator = prefab.GetComponent<SkillLocator>();
            ReplaceSkill(locator.primary, Skills.Glassthrix.GlassthrixHammer1.Instance.SkillDef);
            ReplaceSkill(locator.secondary, Skills.Glassthrix.GlassthrixSlash1.Instance.SkillDef);

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
        }

        public override void PostCreation()
        {
            base.PostCreation();
            RegisterEnemy(prefab, prefabMaster, null);
        }

        private void StealItem(DamageReport report) {
            if (NetworkServer.active && report.damageInfo.HasModdedDamageType(DamageTypes.StealItem)) {
                if (report.victimBody && report.victimBody.inventory) {
                    ItemIndex index = report.victimBody.inventory.itemAcquisitionOrder.First(x => ItemCatalog.GetItemDef(x).hidden == false);
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
}