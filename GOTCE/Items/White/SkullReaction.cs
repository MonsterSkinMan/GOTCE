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

        public override string ItemLore => "3:00 \"Hey Dave, you're our star worker and we do need you to work the night shift. Are you available?\"\n3:30 \"Dave?\"\n3:30 \"Come the fuck on Dave, it's just office work.\"\n3:30 \"You only worked 90 hours this week Dave. That's less than last week.\"\n3:31 \"Dave I swear to fuck I'll kill your elf friend if I have to.\"\n5:00 \"Holy shit you're dead? <sprite name=\"Skull\"> <sprite name=\"Skull\"> <sprite name=\"Skull\">\"\n7:00 \"Disgusting piece of shit. You made our profits go down by dying on us <sprite name=\"Skull\">\"\n12:00 \"<sprite name=\"Skull\"> <sprite name=\"Skull\"> <sprite name=\"Skull\">\"";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.AIBlacklist, GOTCETags.OnDeathEffect};

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/SkullReaction.png");

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