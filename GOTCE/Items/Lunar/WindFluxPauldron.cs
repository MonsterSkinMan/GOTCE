using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items.Lunar
{
    public class WindFluxPauldron : ItemBase<WindFluxPauldron>
    {
        public override string ConfigName => "Wind Flux Pauldron";

        public override string ItemName => "Wind Flux Pauldron";

        public override string ItemLangTokenName => "GOTCE_WindFluxPauldron";

        public override string ItemPickupDesc => "Double your speed... BUT halve your health";

        public override string ItemFullDescription => "Increase movement speed by 100% (+100% per stack). Reduce max health by 50% (+50% per stack)";

        public override string ItemLore => "TBA";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility };

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
                    args.healthMultAdd -= (Mathf.Pow(2f, stack) - 1f) / Mathf.Pow(2f, stack);
                    args.moveSpeedMultAdd += Mathf.Pow(2f, stack) - 1f;
                }
            }
        }
    }
}
