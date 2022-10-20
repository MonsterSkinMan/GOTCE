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

<<<<<<< HEAD
=======
        public override string ItemLore => "The old clock was just the beginning. Now that I've seen this little blip, this little tear in the fabric of reality, everything looks like a tool to widen it. My newest discovery is a small figurine of a rabbit that appears to be covered in some odd form of amber. On a subatomic scale, something incredibly odd appears to be happening. When I shine a light on it, not only is some absorbed, but an equal amount is emitted by the amber itself. Perfectly equal. So far, it's just light, but I believe this can go further. Much further.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

        public override GameObject ItemModel => null;

>>>>>>> 1fa07417373c0c8f53a6c9055f497d83cd6112c0
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