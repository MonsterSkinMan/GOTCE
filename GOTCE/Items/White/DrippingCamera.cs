using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using System;

namespace GOTCE.Items.White
{
    public class DrippingCamera : ItemBase<DrippingCamera>
    {
        public override string ConfigName => "Dripping Camera";

        public override string ItemName => "Dripping Camera";

        public override string ItemLangTokenName => "GOTCE_DrippingCamera";

        public override string ItemPickupDesc => "Gain a chance to <style=cIsUtility>critically camera rotate</style>, spinning your camera to the right.";

        public override string ItemFullDescription => "Every second, you have a <style=cIsUtility>10%</style> <style=cStack>(+10% chance per stack)</style> to <style=cIsUtility>'Camera Rotation Crit'</style>, <style=cIsDamage>rapidly spinning your vision</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.AIBlacklist, GOTCETags.Crit };

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
            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory)
                    {
                        args.Stats.RotationCritChanceAdd += GetCount(args.Stats.body) * 10;
                    }
                }
            };
        }
    }
}