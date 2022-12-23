using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using UnityEngine.Networking;

namespace GOTCE.Items.NoTier
{
    public class HeartlessBreakfast : ItemBase<HeartlessBreakfast>
    {
        public override string ConfigName => "Heartless Breakfast";

        public override string ItemName => "Heartless Breakfast";

        public override string ItemLangTokenName => "GOTCE_HeartlessBreakfast";

        public override string ItemPickupDesc => "An empty can.";

        public override string ItemFullDescription => "An empty can.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.NoTier;

        public override Enum[] ItemTags => new Enum[] { ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/HeartlessBreakfast.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
        }
    }
}