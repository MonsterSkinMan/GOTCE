using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items.Lunar
{
    public class VesselOfHeresy : ItemBase<VesselOfHeresy>
    {
        public override string ConfigName => "Vessel Of Heresy";

        public override string ItemName => "Vessel Of Heresy";

        public override string ItemLangTokenName => "GOTCE_VesselOfHeresy";

        public override string ItemPickupDesc => "Become Heretic.";

        public override string ItemFullDescription => "On pickup, gain 1 stack each of Visions of Heresy, Hooks of Heresy, Strides of Heresy, and Essence of Heresy";

        public override string ItemLore => "it's just heretic she already has a log what more do you want of me";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.Utility, ItemTag.AIBlacklist, ItemTag.CannotSteal };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

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
            On.RoR2.Inventory.GiveItem_ItemIndex_int += Inventory_GiveItem_ItemIndex_int;
            On.RoR2.Inventory.RemoveItem_ItemIndex_int += Inventory_RemoveItem_ItemIndex_int;
        }

        private void Inventory_RemoveItem_ItemIndex_int(On.RoR2.Inventory.orig_RemoveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            if (NetworkServer.active && itemIndex == Instance.ItemDef.itemIndex)
            {
                List<ItemDef> items = new()
                {
                    RoR2Content.Items.LunarPrimaryReplacement,
                    RoR2Content.Items.LunarSecondaryReplacement,
                    RoR2Content.Items.LunarUtilityReplacement,
                    RoR2Content.Items.LunarSpecialReplacement
                };

                var stack = self.GetItemCount(itemIndex);
                foreach (ItemDef itemDef in RoR2.ContentManagement.ContentManager._itemDefs)
                {
                    if (items.Contains(itemDef))
                    {
                        self.RemoveItem(itemDef, 1 * stack);
                    }
                }
            }
            orig(self, itemIndex, count);
        }

        private void Inventory_GiveItem_ItemIndex_int(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            if (NetworkServer.active && itemIndex == Instance.ItemDef.itemIndex)
            {
                List<ItemDef> items = new()
                {
                    RoR2Content.Items.LunarPrimaryReplacement,
                    RoR2Content.Items.LunarSecondaryReplacement,
                    RoR2Content.Items.LunarUtilityReplacement,
                    RoR2Content.Items.LunarSpecialReplacement
                };
                var stack = self.GetItemCount(itemIndex);
                foreach (ItemDef itemDef in RoR2.ContentManagement.ContentManager._itemDefs)
                {
                    if (items.Contains(itemDef))
                    {
                        self.GiveItem(itemDef, 1 * stack);
                    }
                }
            }
        }
    }
}
