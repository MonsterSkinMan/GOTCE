using BepInEx.Configuration;
using R2API;
using RoR2;
using System.Data;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace GOTCE.Items.White
{
    public class PluripotentSteak : ItemBase<PluripotentSteak>
    {
        public override string ConfigName => "Pluripotent Bison Steak";

        public override string ItemName => "Pluripotent Bison Steak";

        public override string ItemLangTokenName => "GOTCE_PluripotentSteak";

        public override string ItemPickupDesc => "Gain increased health, taking damage randomizes your inventory.";

        public override string ItemFullDescription => "Gain +25% (+30% per stack) max health. Upon taking damage, randomize your inventory.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.VoidTier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility };

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
            On.RoR2.HealthComponent.TakeDamage += Bison;
            RecalculateStatsAPI.GetStatCoefficients += Hp;
        }

        public void Bison(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo info) {
            if (NetworkServer.active) {
                if (self.body && self.body.inventory) {
                    CharacterBody body = self.body;
                    Inventory inv = self.body.inventory;
                    int count = inv.GetItemCount(ItemDef);
                    if (count > 0) {
                        int total = 0;
                        foreach (ItemIndex item in inv.itemAcquisitionOrder) {
                            if (ItemCatalog.GetItemDef(item).tier != ItemTier.NoTier && ItemCatalog.GetItemDef(item).deprecatedTier != ItemTier.NoTier) {
                                total += inv.GetItemCount(item);
                            }
                        }
                        for (int i = 0; i < inv.itemAcquisitionOrder.Count; i++) {
                            ItemIndex index = inv.itemAcquisitionOrder[i];
                            if (index != ItemDef.itemIndex && ItemCatalog.GetItemDef(index).tier != ItemTier.NoTier && ItemCatalog.GetItemDef(index).deprecatedTier != ItemTier.NoTier) {
                                inv.RemoveItem(index, inv.GetItemCount(index));
                            }
                        }
                        inv.GiveRandomItems(total, true, true);
                    }
                }
            }
            orig(self, info);
        }

        public void Hp(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args) {
            if (body.inventory) {
                float bonus = 1.25f + (body.inventory.GetItemCount(ItemDef)-1)*0.30f;
                args.healthMultAdd += bonus;
            }
        }
    }
}