using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using BepInEx.Configuration;

namespace GOTCE.Items.Red
{
    public class TubOfLard : ItemBase<TubOfLard>
    {
        public override string ItemName => "Tub Of Lard";

        public override string ConfigName => ItemName;

        public override string ItemLangTokenName => "GOTCE_TubOfLard";

        public override string ItemPickupDesc => "Gain a lot of maximum health and armor.";

        public override string ItemFullDescription => "Gain <style=cIsHealing>400 maximum health</style> and <style=cIsHealing>10</style> <style=cStack>(+15 per stack)</style> <style=cIsHealing>armor</style>.";

        public override string ItemLore => "I don't know why you keep ordering entire tubs of lard from me. It's getting concerning. This really can't be healthy. I thought you were trying to lose weight? Are you trying to get diabetes? Eh, who am I to judge? I'm just a lowly exporter. And I suppose you are paying good money for this but... look, stay safe out there, okay?";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.Healing };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/tuboflard.png");

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
                    args.baseHealthAdd += 400f;
                    args.armorAdd += 10f + 15f * (stack - 1);
                }
            }
        }
    }
}