using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;

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

        public override Sprite ItemIcon => null; /* Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/Bottled_Enigma.png"); */ // replace with bottled command icon

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
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
                    if (ItemDef.tier != ItemTier.NoTier && itemDef.deprecatedTier != ItemTier.NoTier && ItemDef.tier != ItemTier.VoidTier1 && itemDef.tier != ItemTier.VoidTier2 && ItemDef.tier != ItemTier.VoidTier3 && ItemDef.deprecatedTier != ItemTier.VoidTier1 && ItemDef.deprecatedTier != ItemTier.VoidTier2 && ItemDef.deprecatedTier != ItemTier.VoidTier3 && itemDef != Instance.ItemDef)
                    {
                        Debug.Log("item def is " + itemDef);
                        self.GiveItem(itemDef);
                    }
                }
            }
        }
    }
}
