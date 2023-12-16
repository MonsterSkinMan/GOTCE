using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using R2API;
using GOTCE.Buffs;

namespace GOTCE.Items.Lunar
{
    public class ReminiscencePrism : ItemBase<ReminiscencePrism>
    {
        public override string ConfigName => "Reminiscence Prism";

        public override string ItemName => "Reminiscence Prism";

        public override string ItemLangTokenName => "GOTCE_ReminiscencePrism";

        public override string ItemPickupDesc => "Gain 20% stage transition crit chance.... BUT your critical strike chance is set to 0 for the stage upon stage transition crit.";

        public override string ItemFullDescription => "Gain 20% (+20% per stack) stage transition crit chance. Critical strikes are disabled for the stage upon stage transition crit.";

        public override string ItemLore => "TBA";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.OnStageBeginEffect, GOTCETags.Crit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override void Hooks()
        {
            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory)
                    {
                        args.Stats.StageCritChanceAdd += GetCount(args.Stats.body) * 20;
                    }
                }
            };
            CriticalTypes.OnStageCrit += NoCritHit;
        }

        public void NoCritHit(object sender, StageCritEventArgs args)
        {
            if (NetworkServer.active)
            {
                var instances = PlayerCharacterMasterController.instances;
                foreach (PlayerCharacterMasterController playerCharacterMaster in instances)
                {
                    if (playerCharacterMaster.master.inventory.GetItemCount(ItemDef) > 0)
                    {
                        if (playerCharacterMaster.master.gameObject.GetComponent<GOTCE_StatsComponent>())
                        {
                            playerCharacterMaster.body.AddBuff(NoCrit.def);
                        }
                    }
                }
            }
        }
    }
}
