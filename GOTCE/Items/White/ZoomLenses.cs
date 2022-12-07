using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using System;

namespace GOTCE.Items.White
{
    public class ZoomLenses : ItemBase<ZoomLenses>
    {
        public override string ConfigName => "Zoom Lenses";

        public override string ItemName => "Zoom Lenses";

        public override string ItemLangTokenName => "GOTCE_ZoomLenses";

        public override string ItemPickupDesc => "Gain a chance to periodically 'FOV Crit', zooming in your vision.";

        public override string ItemFullDescription => "Every second, you have a <style=cIsUtility>10%</style> <style=cStack>(+10% chance per stack)</style> to <style=cIsUtility>'FOV Crit'</style>, zooming in your vision for <style=cIsUtility>1</style> second.";

        public override string ItemLore => "Jesus fucking christ what drugs were we on when we came up with FOV crits god this is such a dogshit mechanic I hate this.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.AIBlacklist, GOTCETags.Crit, GOTCETags.FovRelated };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/ZoomLenses.png");

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
            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory)
                    {
                        args.Stats.FovCritChanceAdd += GetCount(args.Stats.body) * 10;
                    }
                }
            };
        }
    }
}