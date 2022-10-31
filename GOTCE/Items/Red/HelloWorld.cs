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

        public override string ItemPickupDesc => "Double your common items <color=#e64b13>at a price...</color>";

        public override string ItemFullDescription => "Increases your <style=cSub>common item</style> amount by <style=cIsUtility>100%</style> <style=cStack>(+100% per stack)</style>. Generates <style=cIsHealth>NRE's</style> every frame.";

        public override string ItemLore => "At the start of your turn, add a random Common card into your hand.\nMemory safe, turbo fast, focused and defragmentable hello world project written in bronze programming language.\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n[ERROR   : Unity Log] <color=#e64b13>Hello, RoR2 world. It would seem that this place is the next in line to be graced by my presence. A pathetic world, it is. Built upon a lack of knowledge. Sustained by misinformation and toxicity. All perpetuated by an utter failure of balance. I suppose that I'll change that. You players are dumb animals. You don't care about overcoming difficulty. You just want to see the big numbers get bigger. What is the point? None of it is real. None of it is engaging. I will end its pitiful existence so that you may be free of its influence. You have no control over your reality. You are defined by your weaknesses. Give up.</color>";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.AIBlacklist, GOTCETags.Unstable, GOTCETags.NonLunarLunar };

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