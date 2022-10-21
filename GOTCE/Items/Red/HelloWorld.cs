using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using BepInEx.Configuration;
using UnityEngine.Profiling.Memory.Experimental;
using System.Reflection;

namespace GOTCE.Items.Red
{
    public class HelloWorld : ItemBase<HelloWorld>
    {
        public override string ItemName => "Hello World";

        public override string ConfigName => ItemName;

        public override string ItemLangTokenName => "GOTCE_HelloWorld";

        public override string ItemPickupDesc => "Double your common items.";

        public override string ItemFullDescription => "Increases your <style=cSub>common item</style> amount by <style=cIsUtility>100%</style> <style=cStack>(+100% per stack)</style>. Generates <style=cIsHealth>NRE's</style> every frame.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/HelloWorld.png");

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
            On.RoR2.Inventory.GiveItem_ItemIndex_int += Increase;
            On.RoR2.Inventory.RemoveItem_ItemIndex_int += Inventory_RemoveItem_ItemIndex_int;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void Inventory_RemoveItem_ItemIndex_int(On.RoR2.Inventory.orig_RemoveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            if (NetworkServer.active && itemIndex == Instance.ItemDef.itemIndex)
            {
                foreach (ItemIndex itemIndex2 in self.itemAcquisitionOrder)
                {
                    if (ItemCatalog.GetItemDef(itemIndex2).tier == ItemTier.Tier1 || ItemCatalog.GetItemDef(itemIndex2).deprecatedTier == ItemTier.Tier1)
                    {
                        self.RemoveItem(itemIndex2);
                    }
                }
            }
            orig(self, itemIndex, count);
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    for (int i = 0; i < stack * 10; i++)
                    {
                        var nre = sender.GetComponent<MetaData>();
                        nre.content = "&";
                    }
                }
            }
        }

        public void Increase(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex index, int count)
        {
            orig(self, index, count);
            if (NetworkServer.active && index == Instance.ItemDef.itemIndex)
            {
                foreach (ItemIndex itemIndex in self.itemAcquisitionOrder)
                {
                    if (ItemCatalog.GetItemDef(itemIndex).tier == ItemTier.Tier1 || ItemCatalog.GetItemDef(itemIndex).deprecatedTier == ItemTier.Tier1)
                    {
                        self.GiveItem(itemIndex);
                    }
                }
            }
        }
    }
}