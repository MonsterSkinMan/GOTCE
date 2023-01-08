using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.White
{
    public class SkullReaction : ItemBase<SkullReaction>
    {
        public override string ConfigName => "Skull Reaction";

        public override string ItemName => "Skull Reaction";

        public override string ItemLangTokenName => "GOTCE_SkullReaction";

        public override string ItemPickupDesc => "Gain a chance to <style=cIsUtility>critically die</style>, causing you to <style=cDeath>die again</style>.";

        public override string ItemFullDescription => "Gain a <style=cIsUtility>10%</style> <style=cStack>(+10% per stack)</style> chance upon death to <style=cIsUtility>critically die</style>, causing you to <style=cDeath>die a second time</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.NoTier;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.AIBlacklist};

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

        public override bool Hidden => true;

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
            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) => {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory)
                    {
                        args.Stats.DeathCritChanceAdd += GetCount(args.Stats.body) * 10;
                    }
                }
            };
        }
    }
}