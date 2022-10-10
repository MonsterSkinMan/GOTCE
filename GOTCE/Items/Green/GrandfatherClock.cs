using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using GOTCE.Items.White;
using System;
using GOTCE.Components;

namespace GOTCE.Items.Green
{
    public class GrandfatherClock : ItemBase<GrandfatherClock>
    {
        public override string ConfigName => "Grandfather Clock";

        public override string ItemName => "Grandfather Clock";

        public override string ItemLangTokenName => "GOTCE_GrandfatherClock";

        public override string ItemPickupDesc => "On Stage Transition Crit, die.";

        public override string ItemFullDescription => "On '<style=cIsUtility>Stage Transition Crit</style>', literally fucking <style=cIsHealth>die</style>. ";

        public override string ItemLore => "Order: 12-Hour Decorative Clock\nTracking Number: 59******\nEstimated Delivery: 16/02/2061\nShipping Method: Delicate\nShipping Address: **** 8th Avenue, New York, Earth\nShipping Details:\n\n\"I know how much you valued old Pop. He was one tough son-of-a-bitch, and he admired that in you. You did know why he spent so much time in his lab, right? Why he spent so much time overseas?\nIf not, well... I have so much to tell you. Not here, but some day.\"";

        public override ItemTier Tier => ItemTier.Tier2;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Healing, ItemTag.OnStageBeginEffect, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/GrandfatherClock.png");

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
            FaultySpacetimeClock.Instance.OnStageCrit += Kill;
        }

        public void Kill(object sender, StageCritEventArgs args)
        {
            CharacterBody body = PlayerCharacterMasterController.instances[0].master.GetBody();
            if (body.inventory && body.inventory.GetItemCount(Items.Green.GrandfatherClock.Instance.ItemDef) > 0)
            {
                if (body.gameObject.GetComponent<GOTCE_StatsComponent>())
                {
                    body.gameObject.GetComponent<GOTCE_StatsComponent>().clockDeathCount += body.inventory.GetItemCount(Items.Green.GrandfatherClock.Instance.ItemDef);
                }
            }
        }
    }
}