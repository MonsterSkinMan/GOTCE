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

        public override string ItemPickupDesc => "Double your attack speed... <color=#FF7F7F>BUT double your cooldowns.</color>";

        public override string ItemFullDescription => "Increase <style=cIsDamage>attack speed</style> by <style=cIsDamage>100%</style> <style=cStack>(+100% per stack)</style>. Increase <style=cIsUtility>skill cooldowns</style> by <style=cIsUtility>100%</style> <style=cStack>(+100% per stack)</style>.";

        public override string ItemLore => "TBA";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, GOTCETags.Masochist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/DarkFluxPauldron.png");

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
                    args.attackSpeedMultAdd += Mathf.Pow(2f, stack) - 1;
                    args.cooldownMultAdd += Mathf.Pow(2f, stack) - 1;
                }
            }
        }
    }
}