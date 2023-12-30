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

        public override string ItemLore => "Hot damn! This is a weird-ass prism! Holding it is somehow making me reminisce on the old days, even though I literally cast off my capability of doing that by making Purity. Actually, this strange prism is strange in another way: it seems to be damaging the fabric of reality. I think that I have noticed myself warping around in time at certain intervals, although I feel a lot less focused whenever that happens for a bit.";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.OnStageBeginEffect, GOTCETags.Crit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/ReminiscencePrism.png");

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
