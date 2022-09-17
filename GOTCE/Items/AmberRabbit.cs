using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items
{
    public class AmberRabbit : ItemBase<AmberRabbit>
    {
        public override string ConfigName => "Amber Rabbit";

        public override string ItemName => "Amber Rabbit";

        public override string ItemLangTokenName => "GOTCE_DoubledItemsOnStageCrit";

        public override string ItemPickupDesc => "'Critical Stage Transitions' double your item count.";

        public override string ItemFullDescription => "On stage transition crit, gain 1 <style=cStack>(+1 per stack)</style> additional stack(s) of all of your items.";

        public override string ItemLore => "The old clock was just the beginning. Now that I've seen this little blip, this little tear in the fabric of reality, everything looks like a tool to widen it. My newest discovery is a small figurine of a rabbit that appears to be covered in some odd form of amber. On a subatomic scale, something incredibly odd appears to be happening. When I shine a light on it, not only is some absorbed, but an equal amount is emitted by the amber itself. Perfectly equal. So far, it's just light, but I believe this can go further. Much further.";

        public override ItemTier Tier => ItemTier.Tier2;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility, ItemTag.OnStageBeginEffect, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/AmberRabbit.png");

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
