using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items.Lunar
{
    public class DarkFluxPauldron : ItemBase<DarkFluxPauldron>
    {
        public override string ConfigName => "Dark Flux Pauldron";

        public override string ItemName => "Dark Flux Pauldron";

        public override string ItemLangTokenName => "GOTCE_DarkFluxPauldron";

        public override string ItemPickupDesc => "Double your attack speed... BUT double your cooldowns.";

        public override string ItemFullDescription => "Increase attack speed by 100% (+100% per stack). Increase skill cooldowns by 100% (+100% per stack).";

        public override string ItemLore => "TBA";

        public override ItemTier Tier => ItemTier.Lunar;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

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
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.attackSpeedMultAdd += Mathf.Pow(2f, stack) - 1f;
                    args.cooldownMultAdd += Mathf.Pow(2f, stack) - 1f;
                }
            }
        }
    }
}
