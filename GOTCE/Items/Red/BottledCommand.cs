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

        public override string ItemPickupDesc => "Gain 2 stacks of every vanilla S Tier green item. Have fun.";

        public override string ItemFullDescription => "Gain <style=cIsUtility>2</style> <style=cStack>(+2 per stack)</style> stacks of every vanilla <style=cIsHealth>S Tier</style> <style=cIsHealing>green item</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage, ItemTag.Healing, ItemTag.Utility, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/bottledcommand.png");

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
            On.RoR2.Inventory.GiveItem_ItemIndex_int += Inventory_GiveItem_ItemIndex_int;
            On.RoR2.Inventory.RemoveItem_ItemIndex_int += Inventory_RemoveItem_ItemIndex_int;
        }

        private void Inventory_RemoveItem_ItemIndex_int(On.RoR2.Inventory.orig_RemoveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            if (NetworkServer.active && itemIndex == Instance.ItemDef.itemIndex)
            {
                List<ItemDef> items = new()
                {
                    RoR2Content.Items.Missile, RoR2Content.Items.Bandolier, RoR2Content.Items.Feather, RoR2Content.Items.FireRing, RoR2Content.Items.Thorns,
                    RoR2Content.Items.SprintArmor, RoR2Content.Items.IceRing, RoR2Content.Items.ChainLightning, RoR2Content.Items.JumpBoost
                };

                foreach (ItemDef itemDef in RoR2.ContentManagement.ContentManager._itemDefs)
                {
                    if (items.Contains(itemDef))
                    {
                        self.RemoveItem(itemDef);
                    }
                }
            }
        }

        private void Inventory_GiveItem_ItemIndex_int(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            if (NetworkServer.active && itemIndex == Instance.ItemDef.itemIndex)
            {

                List<ItemDef> items = new()
                {
                    RoR2Content.Items.Missile, RoR2Content.Items.Bandolier, RoR2Content.Items.Feather, RoR2Content.Items.FireRing, RoR2Content.Items.Thorns,
                    RoR2Content.Items.SprintArmor, RoR2Content.Items.IceRing, RoR2Content.Items.ChainLightning, RoR2Content.Items.JumpBoost
                };

                foreach (ItemDef itemDef in RoR2.ContentManagement.ContentManager._itemDefs)
                {
                    if (items.Contains(itemDef))
                    {
                        self.GiveItem(itemDef);
                    }
                }
            }
        }
    }
}