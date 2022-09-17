using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using R2API;
using UnityEngine;
using BepInEx.Configuration;
using System.Linq;
using static RoR2.CombatDirector;
using RoR2.CharacterAI;
using GOTCE.Utils;
using GOTCE.Equipment;
using GOTCE.Utils.Components;
using R2API.Networking.Interfaces;
using UnityEngine.Networking;
using R2API.Networking;
using static GOTCE.Main;
using static GOTCE.Utils.MathHelpers;
using static GOTCE.Utils.ItemHelpers;
using static GOTCE.Utils.MiscUtils;

namespace GOTCE.Equipment.EliteEquipment
{
    public class AffixRushing : EliteEquipmentBase<AffixRushing>
    {
        public static ConfigOption<float> SpeedMultiplierOfElite;

        public override string EliteEquipmentName => "Blessing of Woolie";

        public override string EliteAffixToken => "AFFIX_RUSHING";

        public override string EliteEquipmentPickupDesc => "Become an aspect of speed.";

        public override string EliteEquipmentFullDescription => "Increased movement speed by 70% and gives you a chance to steal items on hit. The items have a 95% chance to be returned on death.";

        public override string EliteEquipmentLore => "";

        public override string EliteModifier => "Rushing";

        public override GameObject EliteEquipmentModel => null;

        public override Material EliteMaterial => null;

        public override Sprite EliteEquipmentIcon => null;

        public override Sprite EliteBuffIcon => null;

        public override Color EliteBuffColor => new Color(255, 255, 0, 255);

        public override float HealthMultiplier => 4f;

        public override float DamageMultiplier => 2f;


        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateEquipment();
            CreateEliteTiers();
            CreateElite();
            Hooks();
        }

        private void CreateConfig(ConfigFile config)
        {
            SpeedMultiplierOfElite = config.ActiveBind<float>("Elite Equipment: " + EliteEquipmentName, "Speed Multiplier of Elite", 1f, "How hard should Rushing Elites Woolie Rush?");
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += new RecalculateStatsAPI.StatHookEventHandler(AffixRushing.MovementSpeedIncrease);
            On.RoR2.GlobalEventManager.OnHitEnemy += RootOnHit;
        }

        private void CreateEliteTiers()
        {
            CanAppearInEliteTiers = new CombatDirector.EliteTierDef[]
            {
                new CombatDirector.EliteTierDef()
                {
                    costMultiplier = CombatDirector.baseEliteCostMultiplier,
                    eliteTypes = Array.Empty<EliteDef>(),
                    isAvailable = SetAvailability
                }
            };
        }

        private bool SetAvailability(SpawnCard.EliteRules arg)
        {
            return Run.instance.ambientLevel >= 1 && arg == SpawnCard.EliteRules.Default;
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            return false;
        }

        public static void MovementSpeedIncrease(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body.inventory.currentEquipmentIndex == Instance.EliteEquipmentDef.equipmentIndex)
            {
                args.moveSpeedMultAdd += 0.7f * SpeedMultiplierOfElite;
            }
        }

        private void RootOnHit(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            if (damageInfo.attacker && !damageInfo.rejected)
            {
                var body = damageInfo.attacker.GetComponent<CharacterBody>();
                if (body && body.inventory && body.inventory.currentEquipmentIndex == Instance.EliteEquipmentDef.equipmentIndex)
                {
                    CharacterBody characterBody = victim ? victim.GetComponent<CharacterBody>() : null;
                    characterBody.AddTimedBuff(RoR2Content.Buffs.Entangle, 5f * damageInfo.procCoefficient * SpeedMultiplierOfElite);
                }
                orig(self, damageInfo, victim);
                if (damageInfo == null || damageInfo.rejected || !damageInfo.attacker)
                {
                    return;
                }
            }
        }
    }
}
