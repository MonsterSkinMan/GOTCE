using System.Collections.Generic;
using System.Data;
using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace GOTCE.Items.White
{
    public class AmberRabbit : ItemBase<AmberRabbit>
    {
        public override string ConfigName => "Amber Rabbit";

        public override string ItemFullDescription => "On 'Critical Stage Transition', double your item count exponentially";

        public override Sprite ItemIcon => null;

        public override string ItemLangTokenName => "GOTCE_AmberRabbit";

        public override string ItemLore => "";

        public override GameObject ItemModel => null;

        public override string ItemName => "Amber Rabbit";

        public override string ItemPickupDesc => "'Critical Stage Transisitons' double your item count.";

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            FaultySpacetimeClock.Instance.OnStageCrit += the;
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public void the(object sender, StageCritEventArgs args)
        {
            if (NetworkServer.active)
            {
                var instances = PlayerCharacterMasterController.instances;
                foreach (PlayerCharacterMasterController controller in instances)
                {
                    if (controller.master && controller.master.GetBody())
                    {
                        Inventory inv = controller.master.GetBody().inventory;
                        List<ItemIndex> items = inv.itemAcquisitionOrder;
                        foreach (ItemIndex itemIndex in items)
                        {
                            int toIncrease = inv.GetItemCount(itemIndex) ^ inv.GetItemCount(ItemDef);
                            inv.GiveItem(itemIndex, toIncrease);
                        }
                    }
                }
            }
        }

    }
}