using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items.White
{
    public class SkullKey : ItemBase<SkullKey>
    {
        public override string ConfigName => "Skull Key";

        public override string ItemName => ":Skull: Key";

        public override string ItemLangTokenName => "GOTCE_SkullKey";

        public override string ItemPickupDesc => "'Critical Stage Transitions' give you 5 powerful items. Not consumed on use.";

        public override string ItemFullDescription => "Gain 5% stage crit chance. On stage crit, gain 5 (+5 per stack) items that can be either red or yellow rarity.";

        public override string ItemLore => "Perhaps the crack itself has treasures to be found. So far, its purpose has been as a slingshot along the borders of our reality, but this new artifact is a direct wedge into another dimension, uprooting its treasures and bringing them here. A parallel space carrying boundless treasures- imagine that! Everything I've pulled from within the crack has been incredibly useful; I've gotten rocket launchers, detritivore desk plants, and even a spaceship part. I can't wait to see what all I can gather.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/skullkey.png");

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
            FaultySpacetimeClock.Instance.OnStageCrit += MeWhenWithor;
        }

        public void MeWhenWithor(object sender, StageCritEventArgs args)
        {
            CharacterBody body = PlayerCharacterMasterController.instances[0].master.GetBody();
            if (body.inventory && body.inventory.GetItemCount(ItemDef) > 0)
            {
                //ok I don't feel like finishing this rn
            }
        }
    }
}
