/* using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using RoR2.Skills;

namespace GOTCE.Equipment.EliteEquipment
{
    class BackupMag : EliteEquipmentBase<BackupMag>
    {
        public override string EliteEquipmentName => "Secret Compartment";

        public override string EliteAffixToken => "GOTCE_BackupElite";

        public override string EliteEquipmentPickupDesc => "Become an aspect of backup.";

        public override string EliteEquipmentFullDescription => "";

        public override string EliteEquipmentLore => "";

        public override string EliteModifier => "Compartmentalized";

        public override GameObject EliteEquipmentModel => new GameObject();

        public override Sprite EliteEquipmentIcon => null;

        public override Sprite EliteBuffIcon => null;

        public override void Init(ConfigFile config)
        {
            Backuped.Awake();
            CreateConfig(config);
            CreateLang();
            CreateEquipment();
            CreateEliteTiers();
            CreateElite();
            Hooks();
        }

        private void CreateConfig(ConfigFile config)
        {

        }

        private void CreateEliteTiers()
        {
        }


        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += Backup;
            GlobalEventManager.onServerDamageDealt += OnDamage;
        }

        //If you want an on use effect, implement it here as you would with a normal equipment.
        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            return false;
        }

        public void OnDamage(DamageReport report) {
            if (report.attacker && report.attackerBody && report.victim && report.victimBody) {
                if (report.attackerBody.equipmentSlot) {
                    CharacterBody body = report.attackerBody;
                    CharacterBody victim = report.victimBody;
                    if (body.equipmentSlot.equipmentIndex == EliteEquipmentDef.equipmentIndex) {
                        float duration = 5.0f * report.damageInfo.procCoefficient;
                        victim.AddTimedBuff(Backuped.buff, duration, 1);
                    }
                }
            }
        }

        public void Backup(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args) {
            if (body.equipmentSlot) {
                if (body.equipmentSlot.equipmentIndex == EliteEquipmentDef.equipmentIndex) {
                    args.secondaryCooldownMultAdd -= 0.5f;
                    body.skillLocator.secondary.AddOneStock();
                    body.skillLocator.secondary.AddOneStock();
                    body.skillLocator.secondary.AddOneStock();
                }
            }

            if (body.HasBuff(Backuped.buff)) {
                SkillDef lockedDef = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Captain/CaptainSkillDisconnected.asset").WaitForCompletion();
                body.skillLocator.secondary.SetSkillOverride(body, lockedDef, GenericSkill.SkillOverridePriority.Replacement);
            }
        }
    }

    public class Backuped {
        public static BuffDef buff;

        public static void Awake() {
            buff = ScriptableObject.CreateInstance<BuffDef>();
            buff.canStack = false;
            buff.isDebuff = true;
            buff.name = "Backuped";

            R2API.ContentAddition.AddBuffDef(buff);
        }
    }
} */