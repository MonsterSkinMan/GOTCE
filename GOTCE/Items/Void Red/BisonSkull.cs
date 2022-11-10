using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using GOTCE.Components;
using BepInEx.Configuration;
using HarmonyLib;

namespace GOTCE.Items.Void_Red
{
    public class BisonSkull : ItemBase<BisonSkull>
    {
        public override string ConfigName => "Bison Skull";

        public override string ItemName => "Bison Skull";

        public override string ItemLangTokenName => "GOTCE_BisonSkull";

        public override string ItemPickupDesc => "Gain barrier on kill. Corrupts all healing items.";

        public override string ItemFullDescription => "On killing an enemy, gain a temporary barrier for 1 (+1 per stack) health. Corrupts all healing items.";

        public override string ItemLore => "TBA";

        public override ItemTier Tier => ItemTier.VoidTier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.Healing, ItemTag.OnKillEffect, GOTCETags.Bullshit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

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
            On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
            On.RoR2.Items.ContagiousItemManager.Init += powerUncreep;
        }

        private void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, RoR2.GlobalEventManager self, RoR2.DamageReport damageReport)
        {
            if (damageReport.attackerBody && damageReport.attackerBody.inventory)
            {
                var stack = damageReport.attackerBody.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    if (damageReport.attackerBody.healthComponent && NetworkServer.active)
                    {
                        damageReport.attackerBody.healthComponent.AddBarrier(1f * stack);
                    }
                }
            }
        }

        private void powerUncreep(On.RoR2.Items.ContagiousItemManager.orig_Init orig)
        {
            foreach (ItemDef def in ItemCatalog.allItemDefs) {
                if (def.ContainsTag(ItemTag.Healing) && def != ItemDef) {
                    ItemDef.Pair transformation = new ItemDef.Pair() {
                        itemDef1 = def,
                        itemDef2 = ItemDef
                    };
                    ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem] = ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem].AddToArray(transformation);
                }
            }
            orig();
        }
    }
}
