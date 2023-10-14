using UnityEngine;

namespace GOTCE.Items.NoTier
{
    public class HealingScrapConsumed : ItemBase<HealingScrapConsumed>
    {
        public override string ConfigName => "Healing Scrap";

        public override string ItemName => "Healing Scrap (Consumed)";

        public override string ItemLangTokenName => "GOTCE_HealingScrapConsumed";

        public override string ItemPickupDesc => "At the start of each stage, transforms into Healing Scrap.";

        public override string ItemFullDescription => "At the start of each stage, transforms into Healing Scrap.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.NoTier;

        public override Enum[] ItemTags => new Enum[] { ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/HealingScrapConsumed.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
        }
    }
}