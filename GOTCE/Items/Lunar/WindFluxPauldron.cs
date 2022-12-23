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

        public override string ItemPickupDesc => "Double your speed... <color=#FF7F7F>BUT halve your health.</color>";

        public override string ItemFullDescription => "Increase <style=cIsUtility>movement speed</style> by <style=cIsUtility>100%</style> <style=cStack>(+100% per stack)</style>. Reduce <style=cIsHealing>maximum health</style> by <style=cIsHealing>50%</style> <style=cStack>(+50% per stack)</style>.";

        public override string ItemLore => "\"Much like the wind, you must be nimble and flexible. Completely and utterly untouchable, in both mind and body. Fear not the bullet of your enemy, visualize it whiffing past your skin. Focus, and make it true. Fear not the tumbling avalanche, visualize yourself running past it unscathed. But heed, and do not flee without reason. Those who remain flighty will quickly find themselves surrounded.\"\n\n-Nemesis Will of Combat, Nemesis First Excerpt";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/WindFluxPauldron.png");

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