using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace GOTCE.Items.Red
{
    public class ZanySoup : ItemBase<ZanySoup>
    {

        public override string ItemName => "Zany Soup";

        public override string ConfigName => ItemName;

        public override string ItemLangTokenName => "GOTCE_ZanySoup";

        public override string ItemPickupDesc => "Quadruple the amount of food-related items you have.";

        public override string ItemFullDescription => "Gain +3 (+1 per stack) of every food item you have.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null; 

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.Inventory.GiveItem_ItemIndex_int += Increase;
        }

        public void Increase(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex index, int count) {
            orig(self, index, count);
            if (NetworkServer.active && index == ItemDef.itemIndex) {
                // Main.ModLogger.LogDebug("network server is active");
            
                int toIncrease = self.GetItemCount(index) > 1 ? 1 : 3;
                List<ItemIndex> foodRelated = new List<ItemIndex>() {
                    RoR2.RoR2Content.Items.FlatHealth.itemIndex, RoR2.RoR2Content.Items.ParentEgg.itemIndex, GOTCE.Items.White.MoldySteak.Instance.ItemDef.itemIndex, RoR2.RoR2Content.Items.Mushroom.itemIndex, RoR2.DLC1Content.Items.MushroomVoid.itemIndex, RoR2.DLC1Content.Items.AttackSpeedAndMoveSpeed.itemIndex, RoR2.RoR2Content.Items.SprintBonus.itemIndex, RoR2.RoR2Content.Items.Plant.itemIndex, GOTCE.Items.Green.SpafnarsFries.Instance.ItemDef.itemIndex
                };

                foreach (ItemIndex item in self.itemAcquisitionOrder) {
                    if (foodRelated.Contains(item)) {
                        // Main.ModLogger.LogDebug("Food item found: " + item);
                        self.GiveItem(item, toIncrease);
                    }
                }
            }
        }
    }

        
}
