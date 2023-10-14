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

        public override string ItemFullDescription => "Literally just Regenerating Scrap but as a <style=cSub>common item</style>.";

        public override string ItemLore => "<color=#e64b13>there is no lore this is literally just regen scrap but as a common item I don't know what you were expecting.</color>";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.PriorityScrap, ItemTag.CannotDuplicate, ItemTag.OnStageBeginEffect, ItemTag.AIBlacklist, GOTCETags.Consumable };

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
            On.RoR2.Inventory.RemoveItem_ItemIndex_int += Inventory_RemoveItem_ItemIndex_int;
        }

        [RunMethod(RunAfter.Items)]
        private static void RegisterFragile()
        {
            Fragile.AddFragileItem(HealingScrap.Instance.ItemDef, new Fragile.FragileInfo
            {
                broken = Items.NoTier.HealingScrapConsumed.Instance.ItemDef,
                shouldGiveBroken = true,
                fraction = 25f
            });
        }

        private void Stage_Start(On.RoR2.Stage.orig_Start orig, Stage self)
        {
            orig(self);
            if (CharacterMaster.instancesList != null)
            {
                foreach (CharacterMaster cm in CharacterMaster.instancesList)
                {
                    if (cm.inventory)
                    {
                        var stack = cm.inventory.GetItemCount(NoTier.HealingScrapConsumed.Instance.ItemDef);
                        if (stack > 0)
                        {
                            cm.inventory.RemoveItem(NoTier.HealingScrapConsumed.Instance.ItemDef, stack);
                            cm.inventory.GiveItem(Instance.ItemDef, stack);
                            CharacterMasterNotificationQueue.SendTransformNotification(cm, NoTier.HealingScrapConsumed.Instance.ItemDef.itemIndex, Instance.ItemDef.itemIndex, CharacterMasterNotificationQueue.TransformationType.RegeneratingScrapRegen);
                        }
                    }
                }
            }
        }

        private void Inventory_RemoveItem_ItemIndex_int(On.RoR2.Inventory.orig_RemoveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            if (itemIndex == Instance.ItemDef.itemIndex)
            {
                self.GiveItem(NoTier.HealingScrapConsumed.Instance.ItemDef, count);
                var master = self.GetComponent<CharacterMaster>();
                if (master)
                {
                    CharacterMasterNotificationQueue.PushItemTransformNotification(master, Instance.ItemDef.itemIndex, NoTier.HealingScrapConsumed.Instance.ItemDef, CharacterMasterNotificationQueue.TransformationType.RegeneratingScrapConsumed);
                }
            }
        }
    }
}