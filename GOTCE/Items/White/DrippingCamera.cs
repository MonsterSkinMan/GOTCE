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

        public override string ItemLore => "Expedition log - M. Johnson\nDay 4\n\nApproaching the subject now appears possible, as long as it is not observed on the way there. Unfortunately it gets a bit tricky then, so I will describe what happens with my interactions.\n\nI look at the subject, I turn away, I turn back, not looking, look at the subject again, I turn away. I point the camera at it, the camera drops into the swamp water. I bend down to pick it up, gazing over the subject, and turn away. I turn back around facing down and retrieve the camera. I close my eyes and look up, I immediately turn away and drop my camera. At least it's waterproof. I glance at the subject reflected off my camera’s screen, and immediately turn around, walk a few steps, and drop my camera again.\n\nI still have no idea what this thing even looks like.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.AIBlacklist, GOTCETags.Crit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/DrippingCamera.png");

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