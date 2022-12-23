using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.NoTier
{
    public class DilutedFlask : ItemBase<DilutedFlask>
    {
        public override string ConfigName => "Diluted Flask";

        public override string ItemName => "Diluted Flask";

        public override string ItemLangTokenName => "GOTCE_DilutedFlask";

        public override string ItemPickupDesc => "Skill issue";

        public override string ItemFullDescription => "Skill issue";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.NoTier;

        public override Enum[] ItemTags => new Enum[] { ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/DilutedFlask.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
        }
    }
}