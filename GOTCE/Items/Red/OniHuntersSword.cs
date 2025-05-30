using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Red
{
    public class OniHuntersSword : ItemBase<OniHuntersSword>
    {
        public override string ConfigName => "Oni Hunters Sword";

        public override string ItemName => "Oni Hunter's Sword";

        public override string ItemLangTokenName => "GOTCE_OniHuntersSword";

        public override string ItemPickupDesc => "Gain a chance to convert a lunar item into a red item on pickup.";

        public override string ItemFullDescription => "Gain a <style=cIsUtility>20%</style> <style=cStack>(+10% per stack)</style> chance to <style=cIsUtility>convert</style> a <style=cIsLunar>lunar</style> item into a random <style=cIsHealth>red</style> item.";

        public override string ItemLore => "Order: Suspicious sword\nTracking Number: 44*********\nEstimated Delivery: February 15, 2067\nShipping Method: High Priority\nShipping Address: 2016, Anomalous Oddities Headquarters, Oregon\nShipping Details:\n\nZen was the hunter. He took me under his wing after we stopped a ghost together. I looked up to him. A lot. So it pained me a lot to find him dead with what looked to be a burned slash through his throat.\n\nI thought you all could help me find his killer. I know you all over there love to research some anomalies, and I'm telling you, nobody should be able to kill Zen with a quick slash like this. Not anything human at least.";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/OniHuntersSword.png");

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
        }

        private void Inventory_GiveItem_ItemIndex_int(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            var master = self.GetComponent<CharacterMaster>();
            var stack = GetCount(master);
            if (NetworkServer.active && stack > 0)
            {
                var itemDef = ItemCatalog.GetItemDef(itemIndex);
                if (itemDef.tier == ItemTier.Lunar || itemDef.deprecatedTier == ItemTier.Lunar)
                {
                    if (Util.CheckRoll(20f + 10f * (stack - 1), master))
                    {
                        self.RemoveItem(itemIndex);

                        WeightedSelection<List<PickupIndex>> weightedSelection = new(8);
                        weightedSelection.AddChoice(Run.instance.availableTier3DropList, 100f);

                        List<PickupIndex> list = weightedSelection.Evaluate(UnityEngine.Random.value);
                        PickupDef pickupDef = PickupCatalog.GetPickupDef(list[UnityEngine.Random.Range(0, list.Count)]);
                        self.GiveItem((pickupDef != null) ? pickupDef.itemIndex : ItemIndex.None, 1);
                    }
                }
            }
        }
    }
}