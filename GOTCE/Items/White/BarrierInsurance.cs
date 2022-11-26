using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using GOTCE.Components;

namespace GOTCE.Items.White
{
    public class BarrierInsurance : ItemBase<BarrierInsurance>
    {
        public override bool CanRemove => true;
        public override string ConfigName => ItemName;
        public override string ItemFullDescription => "On stage entry, gain Barrier equal to the amount (+70% of the amount per stack) of BarrierRelated category items you have. ";
        public override Sprite ItemIcon => null;
        public override string ItemLangTokenName => "GOTCE_BarrierInsurance";
        public override string ItemLore => "";
        public override GameObject ItemModel => null;
        public override string ItemName => "Barrier Insurance";
        public override string ItemPickupDesc => "Gain an amount of barrier at the start of each stage based on how many barrier items you have.";
        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.Healing, GOTCETags.BarrierRelated, ItemTag.OnStageBeginEffect };
        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.CharacterBody.Start += Barrier;
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public void Barrier(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self)
        {
            if (NetworkServer.active)
            {
                if (GetCount(self) > 0)
                {
                    if (self.inventory)
                    {
                        int total = 0;
                        foreach (ItemIndex index in self.inventory.itemAcquisitionOrder)
                        {
                            if (ContainsTag(ItemCatalog.GetItemDef(index), GOTCETags.BarrierRelated))
                            {
                                total += self.inventory.GetItemCount(index);
                            }
                        }
                        float scale = 70f * GetCount(self);
                        self.healthComponent.AddBarrier(total * scale);
                    }
                }
            }
            orig(self);
        }
    }
}