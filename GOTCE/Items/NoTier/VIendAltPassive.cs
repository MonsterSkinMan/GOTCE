using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using UnityEngine.Networking;

namespace GOTCE.Items.NoTier
{
    public class ViendAltPassive : ItemBase<ViendAltPassive>
    {
        public override string ConfigName => "ViendAltPassive";

        public override string ItemName => "ViendAltPassive";

        public override string ItemLangTokenName => "GOTCE_ViendAltPassive";

        public override string ItemPickupDesc => "";

        public override string ItemFullDescription => "";
        public override bool Hidden => true;
        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.NoTier;

        public override Enum[] ItemTags => new Enum[] { ItemTag.AIBlacklist };

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
    }
}