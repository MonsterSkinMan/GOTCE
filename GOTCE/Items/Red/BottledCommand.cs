using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using BepInEx.Configuration;
using System.Linq;

namespace GOTCE.Items.Red
{
    public class BottledCommand : ItemBase<BottledCommand>
    {
        public override string ConfigName => "Bottled Command";

        public override string ItemName => "Bottled Command";

        public override string ItemLangTokenName => "GOTCE_BottledCommand";

        public override string ItemPickupDesc => "Gain 1 stack of every <style=cIsVoid>non-void</style> item. Have fun.";

        public override string ItemFullDescription => "Gain <style=cIsUtility>1</style> <style=cStack>(+1 per stack)</style> stack of every non-void item.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage, ItemTag.Healing, ItemTag.Utility };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/bottledcommand.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        private static readonly List<ItemTier> woolieTierlist = new();
        // private static readonly List<ItemTag> rndTierlist = new();

        public override void Init(ConfigFile config)
        {
            base.Init(config);
            woolieTierlist.AddRange(new List<ItemTier> { ItemTier.VoidTier1, ItemTier.VoidTier2, ItemTier.VoidTier3, ItemTier.VoidBoss, ItemTier.NoTier });
            // rndTierlist.AddRange(new List<ItemTag> { ItemTag.WorldUnique });
        }

        public override void Hooks()
        {
            On.RoR2.Inventory.GiveItem_ItemIndex_int += Inventory_GiveItem_ItemIndex_int;
        }

        private void Inventory_GiveItem_ItemIndex_int(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            if (NetworkServer.active && itemIndex == Instance.ItemDef.itemIndex)
            {
                foreach (ItemDef itemDef in RoR2.ContentManagement.ContentManager._itemDefs)
                {
                    if (!woolieTierlist.Contains(ItemDef.tier) && !woolieTierlist.Contains(ItemDef.deprecatedTier) && itemDef != Instance.ItemDef && itemDef != BottledEnigma.Instance.ItemDef)
                    {
                        Debug.Log("item def is " + itemDef);
                        self.GiveItem(itemDef);
                    }
                }
            }
        }
    }
}