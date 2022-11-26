using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items.NoTier
{
    public class HealingScrapConsumed : ItemBase<HealingScrapConsumed>
    {
        public override string ConfigName => "Healing Scrap (Consumed)";

        public override string ItemName => "Healing Scrap (Consumed)";

        public override string ItemLangTokenName => "GOTCE_HealingScrapConsumed";

        public override string ItemPickupDesc => "At the start of each stage, it transforms into Healing Scrap.";

        public override string ItemFullDescription => "At the start of each stage, it transforms into Healing Scrap.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.NoTier;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.OnStageBeginEffect, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/HealingScrapConsumed.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.CharacterMaster.OnServerStageBegin += CharacterMaster_OnServerStageBegin;
        }

        private void CharacterMaster_OnServerStageBegin(On.RoR2.CharacterMaster.orig_OnServerStageBegin orig, CharacterMaster self, Stage stage)
        {
            orig(self, stage);
            int itemCount = self.inventory.GetItemCount(Instance.ItemDef.itemIndex);
            if (itemCount > 0)
            {
                self.inventory.RemoveItem(Instance.ItemDef.itemIndex, itemCount);
                self.inventory.GiveItem(White.HealingScrap.Instance.ItemDef.itemIndex, itemCount);
                CharacterMasterNotificationQueue.SendTransformNotification(self, Instance.ItemDef.itemIndex, White.HealingScrap.Instance.ItemDef.itemIndex, CharacterMasterNotificationQueue.TransformationType.RegeneratingScrapRegen);
            }
        }
    }
}