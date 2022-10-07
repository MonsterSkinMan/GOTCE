using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using R2API;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class SpafnarsFries : ItemBase<SpafnarsFries>
    {
        public override string ConfigName => "Spafnars Fries";

        public override string ItemName => "Spafnar's Fries";

        public override string ItemLangTokenName => "GOTCE_SpafnarsFries";

        public override string ItemPickupDesc => "Gain +50% max health. #JusticeForSpafnar";

        public override string ItemFullDescription => "Increases <style=cIsHealing>maximum health</style> by <style=cIsHealing>50%</style> <style=cStack>(+50% per stack)</style>";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier2;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Healing, ItemTag.Utility };

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
            RecalculateStatsAPI.GetStatCoefficients += new RecalculateStatsAPI.StatHookEventHandler(HealthIncrease);
        }

        public static void HealthIncrease(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.inventory)
            {
                var stack = body.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.healthMultAdd += 0.5f * (stack);
                }
            }
        }
    }
}
