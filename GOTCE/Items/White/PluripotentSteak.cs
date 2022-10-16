/* using BepInEx.Configuration;
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

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

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
                            total += inv.GetItemCount(item);
                        }
                        for (int i = 0; i < inv.itemAcquisitionOrder.Count; i++) {
                            ItemIndex index = inv.itemAcquisitionOrder[i];
                            if (index != ItemDef.itemIndex) inv.RemoveItem(index, inv.GetItemCount(index));
                        }
                        inv.GiveRandomItems(total, true, true);
                    }

                    // TODO: items arent getting cleared before randomization
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
} */