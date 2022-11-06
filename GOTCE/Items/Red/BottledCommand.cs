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

        public override string ItemFullDescription => "Gain <style=cIsUtility>2</style> <style=cStack>(+2 per stack)</style> stacks of every vanilla S Tier <style=cIsHealing>green item</style>.";

        public override string ItemLore => "Artifacts 2.0 is in it's own tier not because it's bad, but because of what it did to game discussion 1. it was so bad, the night it released. flowers were blooming, subarus were dying, but on days like those updates such as that were so hype, you were so excited for the content, everyone in gd1 kept spamming the same fucking gif for like 3 hours going like AAAAAAA OOOO A A AJRJJRRR yknow like the hype train it was like CHOO CHOO bitch gd1 was like fucking amazing the mods were like /shrug and just let the fucking thing go ham. But on that day, that fucking DAY <insertdateofartifactsreleased> everything changed.\nHEY GUYS, CHECK OUT MY EPIC COMMAND BUILD IT HAS CLOVER, IT HAS ATG, IT AS UKULELE NEVER SEEN BEFORE\nHEY GUYS CHECK OUT MY EPIC COMMAND BUILD, ITS SO OP THIS GAME IS SO FUN\nno game discussion, only crying, only suffering in the constant ocean of endless command play by play screenshots, I still have scars on my forehead after screaming in utter dismay and pain from the amount it was nothing but command it was insanity, it was my lack of understanding\nhow the fuck is this fun?\nquickplay\nquickplay don't even get me started on quickplay\nEVERY SINGLE LOBBY\ndrizzle\ncommand\nSacrifice\nSwarms\nit was like the ultimate trio, just end me dude just end me please aaAAAAAAAAAAAA";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.Healing, ItemTag.Utility, ItemTag.AIBlacklist };

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
            if (NetworkServer.active && itemIndex == Instance.ItemDef.itemIndex)
            {
                List<ItemDef> items = new()
                {
                    RoR2Content.Items.Missile, RoR2Content.Items.Bandolier, RoR2Content.Items.Feather, RoR2Content.Items.FireRing, RoR2Content.Items.Thorns,
                    RoR2Content.Items.SprintArmor, RoR2Content.Items.IceRing, RoR2Content.Items.ChainLightning, RoR2Content.Items.JumpBoost
                };

                var stack = self.GetItemCount(itemIndex);
                foreach (ItemDef itemDef in RoR2.ContentManagement.ContentManager._itemDefs)
                {
                    if (items.Contains(itemDef))
                    {
                        self.RemoveItem(itemDef, 2 * stack);
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
                    RoR2Content.Items.Missile, RoR2Content.Items.Bandolier, RoR2Content.Items.Feather, RoR2Content.Items.FireRing, RoR2Content.Items.Thorns,
                    RoR2Content.Items.SprintArmor, RoR2Content.Items.IceRing, RoR2Content.Items.ChainLightning, RoR2Content.Items.JumpBoost
                };
                var stack = self.GetItemCount(itemIndex);
                foreach (ItemDef itemDef in RoR2.ContentManagement.ContentManager._itemDefs)
                {
                    if (items.Contains(itemDef))
                    {
                        self.GiveItem(itemDef, 2 * stack);
                    }
                }
            }
        }
    }
}