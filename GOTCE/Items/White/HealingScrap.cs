using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;
using UnityEngine.Networking;

namespace GOTCE.Items.White
{
    public class HealingScrap : ItemBase<HealingScrap>
    {
        public override string ConfigName => "Healing Scrap";

        public override string ItemName => "Healing Scrap";

        public override string ItemLangTokenName => "GOTCE_HealingScrap";

        public override string ItemPickupDesc => "Prioritized when used with Common 3D Printers. Usable once per stage.";

        public override string ItemFullDescription => "Literally just Regenerating Scrap but as a Common item.";

        public override string ItemLore => "there is no lore this is literally just regen scrap but as a common item I don't know what you were expecting";

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility, ItemTag.PriorityScrap, ItemTag.CannotDuplicate, ItemTag.OnStageBeginEffect, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/HealingScrap.png");

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
            On.RoR2.Inventory.RemoveItem_ItemDef_int += Inventory_RemoveItem_ItemDef_int;
        }

        private void Inventory_RemoveItem_ItemDef_int(On.RoR2.Inventory.orig_RemoveItem_ItemDef_int orig, Inventory self, ItemDef itemDef, int count)
        {
            if (self.gameObject.GetComponent<CharacterBody>()) {
                CharacterBody body = self.gameObject.GetComponent<CharacterBody>();
                if (itemDef == ItemDef) {
                    self.GiveItem(GOTCE.Items.NoTier.HealingScrapConsumed.Instance.ItemDef, count);
                    CharacterMasterNotificationQueue.SendTransformNotification(body.master, itemDef.itemIndex, NoTier.HealingScrapConsumed.Instance.ItemDef.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);
                }
            }
            orig(self, itemDef, count);
        }
    }
}
