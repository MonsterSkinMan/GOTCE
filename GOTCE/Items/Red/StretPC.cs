using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using BepInEx.Configuration;
using UnityEngine.Profiling.Memory.Experimental;

namespace GOTCE.Items.Red
{
    public class StretPC : ItemBase<StretPC>
    {
        public override string ItemName => "Stret PC";

        public override string ConfigName => ItemName;

        public override string ItemLangTokenName => "GOTCE_StretPC";

        public override string ItemPickupDesc => "Double your common items.";

        public override string ItemFullDescription => "Increases your <style=cSub>common item</style> amount by <style=cIsUtility>100%</style> <style=cStack>(+100% per stack)</style>. Generates NRE's every frame.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility, ItemTag.AIBlacklist };

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
            On.RoR2.Inventory.GiveItem_ItemIndex_int += Increase;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
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