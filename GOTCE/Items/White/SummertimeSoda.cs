using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.White
{
    public class SummertimeSoda : ItemBase<SummertimeSoda>
    {
        public override string ConfigName => "Summertime Soda";

        public override string ItemName => "Summertime Soda";

        public override string ItemLangTokenName => "GOTCE_SummertimeSoda";

        public override string ItemPickupDesc => "Gain extra shield, damage reduction, and max HP if the current season is summer.";

        public override string ItemFullDescription => "If the current month is either June, July or August, gain 4% (+4% per stack) shield, 4 (+4 per stack) armor and 10 (+10 per stack) flat health.";

        public override string ItemLore => "TBA";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.Damage, ItemTag.Healing, GOTCETags.Shield, GOTCETags.TimeDependant, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += Soda;
        }

        public void Soda(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args) {
            int c = GetCount(body);
            if (c > 0) {
                int min = 5;
                int max = 9;

                if (DateTime.Now.Month < max && DateTime.Now.Month > min) {
                    args.baseShieldAdd = body.healthComponent.fullHealth * (0.04f * c);
                    args.baseHealthAdd = 10 * c;
                    args.armorAdd = 4 * c;
                }
            }
        }
    }
}