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

        public override string ItemPickupDesc => "On 'Stage Transition Crit', die.";

        public override string ItemFullDescription => "Gain <style=cIsUtility>5% stage transition crit chance</style>. On '<style=cIsUtility>Stage Transition Crit</style>', literally fucking <style=cIsHealth>die</style> <style=cIsDamage>1</style> <style=cStack>(+1 per stack)</style> times. ";

        public override string ItemLore => "Order: 12-Hour Decorative Clock\nTracking Number: 59******\nEstimated Delivery: 16/02/2061\nShipping Method: Delicate\nShipping Address: **** 8th Avenue, New York, Earth\nShipping Details:\n\n\"I know how much you valued old Pop. He was one tough son-of-a-bitch, and he admired that in you. You did know why he spent so much time in his lab, right? Why he spent so much time overseas?\nIf not, well... I have so much to tell you. Not here, but some day.\"";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Healing, ItemTag.OnStageBeginEffect, ItemTag.AIBlacklist, GOTCETags.Crit, ItemTag.OnStageBeginEffect, GOTCETags.NonLunarLunar };

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
            CriticalTypes.OnStageCrit += Kill;
            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory)
                    {
                        args.Stats.StageCritChanceAdd += GetCount(args.Stats.body) > 0 ? 5 : 0;
                    }
                }
            };
        }

        public void Kill(object sender, StageCritEventArgs args)
        {
            if (NetworkServer.active)
            {
                var instances = PlayerCharacterMasterController.instances;
                foreach (PlayerCharacterMasterController playerCharacterMaster in instances)
                {
                    if (playerCharacterMaster.master.inventory.GetItemCount(ItemDef) > 0)
                    {
                        if (playerCharacterMaster.master.gameObject.GetComponent<GOTCE_StatsComponent>()) {
                            playerCharacterMaster.master.gameObject.GetComponent<GOTCE_StatsComponent>().clockDeathCount += (1 * playerCharacterMaster.master.inventory.GetItemCount(ItemDef));
                        }
                    }
                }
            }
        }
    }
}