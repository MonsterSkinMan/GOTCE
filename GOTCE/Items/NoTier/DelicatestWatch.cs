using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using UnityEngine.Networking;

namespace GOTCE.Items.NoTier
{
    public class DelicatestWatch : ItemBase<DelicatestWatch>
    {
        public override string ConfigName => "Delicatest Watch";

        public override string ItemName => "Delicatest Watch";

        public override string ItemLangTokenName => "GOTCE_DelicatestWatch";

        public override string ItemPickupDesc => "Skill issue";

        public override string ItemFullDescription => "Skill issue";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.NoTier;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.AIBlacklist };

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
        }
    }
}