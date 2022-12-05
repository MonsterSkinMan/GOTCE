using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.White
{
    public class ChristmasGift : ItemBase<ChristmasGift>
    {
        public override string ConfigName => "Christmas Gift";

        public override string ItemName => "Christmas Gift";

        public override string ItemLangTokenName => "GOTCE_ChristmasGift";

        public override string ItemPickupDesc => "If it is December, you gain 3 random white items.";

        public override string ItemFullDescription => "If the current month is not December, this does nothing. If the current month is December, you gain 3 random white items.";

        public override string ItemLore => "TBA";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.CannotDuplicate, GOTCETags.TimeDependant };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/ChristmasGift.png");

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
            On.RoR2.Inventory.GiveItem_ItemIndex_int += Consumerism;
        }

        private void Consumerism(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            bool december = DateTime.Now.Month == 12;
            if (NetworkServer.active && itemIndex == Instance.ItemDef.itemIndex && december)
            {
                self.GiveRandomItems(3, false, false);
            }
        }
    }
}
