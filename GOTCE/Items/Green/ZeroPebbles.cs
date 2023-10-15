using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class ZeroPebbles : ItemBase<ZeroPebbles>
    {
        public override string ConfigName => "0 Pebbles";

        public override string ItemName => "0 Pebbles";

        public override string ItemLangTokenName => "GOTCE_ZeroPebbles";

        public override string ItemPickupDesc => "Gain 0 Pebbles.";

        public override string ItemFullDescription => "Gain 0 Pebbles";

        public override string ItemLore => "(there's Nothing There...)";

        public override ItemTier Tier => ItemTier.Tier2;

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/0Pebbles.png");

        public override Enum[] ItemTags => new Enum[] { ItemTag.PriorityScrap };

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }
    }
}