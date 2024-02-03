using BepInEx.Configuration;
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
    class Rushing : EliteEquipmentBase<Rushing>
    {
        public override string EliteEquipmentName => "Blessing of Woolie";

        public override string EliteAffixToken => "RUSH";

        public override string EliteEquipmentPickupDesc => "Become an aspect of speed.";

        public override string EliteEquipmentFullDescription => "";

        public override string EliteEquipmentLore => "";
        public override Texture2D EliteRampTexture => Main.SecondaryAssets.LoadAsset<Texture2D>("Assets/Prefabs/EliteOverlays/Backup.png"); //Change this to rushing elite ramp at some point (should be like a yellow color or smth)

        public override string EliteModifier => "Rushing";

        public override GameObject EliteEquipmentModel => Main.SecondaryAssets.LoadAsset<GameObject>("Assets/Prefabs/Equipment/Backup/BackupAspect.prefab"); //Again, change to rushing when real

        public override Sprite EliteEquipmentIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Equipment/RushingEliteAffix.png");
        public override Sprite EliteBuffIcon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Buffs/BackupAffixIcon.png"); //gd1
        public override float DamageMultiplier => 2f;
        public override float HealthMultiplier => 4f;
        public override CombatDirector.EliteTierDef[] CanAppearInEliteTiers => EliteAPI.GetCombatDirectorEliteTiers().Where(x => x.eliteTypes.Contains(Addressables.LoadAssetAsync<EliteDef>("RoR2/Base/EliteFire/edFire.asset").WaitForCompletion())).ToArray();

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
            RecalculateStatsAPI.GetStatCoefficients += Inconsistent;
            GlobalEventManager.onServerDamageDealt += OnDamage;
        }

        //If you want an on use effect, implement it here as you would with a normal equipment.
        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            return false;
        }

        public void OnDamage(DamageReport report)
        {
            if (report.attacker && report.attackerBody && report.victim && report.victimBody)
            {
                if (report.attackerBody.equipmentSlot)
                {
                    CharacterBody body = report.attackerBody;
                    CharacterBody victim = report.victimBody;
                    if (body.HasBuff(EliteBuffDef))
                    {
                        float duration = 5.0f * report.damageInfo.procCoefficient;
                        victim.AddTimedBuff(RoR2Content.Buffs.Entangle, duration, 1);
                    }
                }
            }
        }

        public void Inconsistent(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body.HasBuff(EliteBuffDef))
            {
                args.moveSpeedMultAdd += 0.7f;
            }
        }
    }
}
