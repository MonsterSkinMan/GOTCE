using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items
{
    public class SkeletonKey : ItemBase<SkeletonKey>
    {
        public override string ConfigName => "Skeleton Key";

        public override string ItemName => "Skeleton Key";

        public override string ItemLangTokenName => "GOTCE_CheatOnStageCrit";

        public override string ItemPickupDesc => "'Critical Stage Transitions' spawn 5 powerful lockboxes that contain highly valuable items. Not consumed on use.";

        public override string ItemFullDescription => "On stage transition crit, spawn 5 <style=cStack>(+5 per stack)</style> powerful lockboxes that contain either <style=cIsHealth>red</style> or <style=cIsTierBoss>yellow</style> Items. This item is not consumed on use. Gain 5% stage transition crit chance.";

        public override string ItemLore => "Perhaps the crack itself has treasures to be found. So far, its purpose has been as a slingshot along the borders of our reality, but this new artifact is a direct wedge into another dimension, uprooting its treasures and bringing them here. A parallel space carrying boundless treasures- imagine that! Everything I've pulled from within the crack has been incredibly useful; I've gotten rocket launchers, detritivore desk plants, and even a spaceship part. I can't wait to see what all I can gather.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility, ItemTag.OnStageBeginEffect, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/skeletonkey.png");

        public override bool Hidden => true;

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
