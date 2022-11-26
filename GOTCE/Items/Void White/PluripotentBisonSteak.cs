using BepInEx.Configuration;
using R2API;
using RoR2;
using System.Data;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using HarmonyLib;

namespace GOTCE.Items.White
{
    public class PluripotentBisonSteak : ItemBase<PluripotentBisonSteak>
    {
        public override string ConfigName => "Pluripotent Bison Steak";

        public override string ItemName => "Pluripotent Bison Steak";

        public override string ItemLangTokenName => "GOTCE_PluripotentBisonSteak";

        public override string ItemPickupDesc => "Gain increased health, taking damage randomizes your inventory. <style=cIsVoid>Corrupts all Bison Steaks</style>.";

        public override string ItemFullDescription => "Gain <style=cIsHealing>+25%</style> <style=cStack>(+30% per stack)</style> <style=cIsHealing>max health</style>. Upon taking damage, <style=cIsUtility>randomize</style> your inventory. <style=cIsVoid>Corrupts all Bison Steaks</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.VoidTier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, GOTCETags.Unstable, GOTCETags.NonLunarLunar };

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
            On.RoR2.Items.ContagiousItemManager.Init += WoolieDimension;
        }

        public void Bison(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo info)
        {
            if (NetworkServer.active)
            {
                if (self.body && self.body.inventory)
                {
                    CharacterBody body = self.body;
                    Inventory inv = self.body.inventory;
                    int count = inv.GetItemCount(ItemDef);
                    if (count > 0)
                    {
                        int total = 0;
                        foreach (ItemIndex item in inv.itemAcquisitionOrder)
                        {
                            if (ItemCatalog.GetItemDef(item).tier != ItemTier.NoTier && ItemCatalog.GetItemDef(item).deprecatedTier != ItemTier.NoTier)
                            {
                                total += inv.GetItemCount(item);
                            }
                        }
                        for (int i = 0; i < inv.itemAcquisitionOrder.Count; i++)
                        {
                            ItemIndex index = inv.itemAcquisitionOrder[i];
                            if (index != ItemDef.itemIndex && ItemCatalog.GetItemDef(index).tier != ItemTier.NoTier && ItemCatalog.GetItemDef(index).deprecatedTier != ItemTier.NoTier)
                            {
                                inv.RemoveItem(index, inv.GetItemCount(index));
                            }
                        }
                        inv.GiveRandomItems(total, true, true);
                    }
                }
            }
            orig(self, info);
        }

        public void Hp(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body.inventory && GetCount(body) > 0)
            {
                float bonus = 1.25f + (body.inventory.GetItemCount(ItemDef) - 1) * 0.30f;
                args.healthMultAdd += bonus;
            }
        }

        private void WoolieDimension(On.RoR2.Items.ContagiousItemManager.orig_Init orig)
        {
            ItemDef.Pair transformation = new()
            {
                itemDef1 = RoR2Content.Items.FlatHealth,
                itemDef2 = this.ItemDef
            };
            ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem] = ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem].AddToArray(transformation);
            orig();
        }
    }
}