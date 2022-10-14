using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;
using HarmonyLib;

namespace GOTCE.Items.VoidGreen
{
    public class AncientAxe : ItemBase<AncientAxe>
    {
        public override string ConfigName => "Ancient Axe";

        public override string ItemName => "Ancient Axe";

        public override string ItemLangTokenName => "GOTCE_AncientAxe";

        public override string ItemPickupDesc => "Guaranteed 'Critical Strikes' against low health enemies. <style=cIsVoid>Corrupts all Old Guillotines</style>.";

        public override string ItemFullDescription => "Attacks against enemies below <style=cIsHealing>9% max health</style> <style=cStack>(+9% per stack)</style> are always <style=cIsDamage>critical</style>. <style=cIsVoid>Corrupts all Old Guillotines</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.VoidTier2;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };

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
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.Items.ContagiousItemManager.Init += Sussy;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (damageInfo == null || damageInfo.rejected || !damageInfo.attacker || damageInfo.attacker == self.gameObject)
            {
                orig(self, damageInfo);
                return;
            }
            if (damageInfo.attacker.GetComponent<CharacterBody>())
            {
                var inv = damageInfo.attacker.GetComponent<HealthComponent>().body.inventory;
                int stack = inv.GetItemCount(Instance.ItemDef);
                if (inv && self.health < (self.fullCombinedHealth * (0.09 * stack)))
                {
                    damageInfo.crit = true;
                }
            }
            orig(self, damageInfo);
        }

        private void Sussy(On.RoR2.Items.ContagiousItemManager.orig_Init orig)
        {
            ItemDef.Pair transformation = new ItemDef.Pair()
            {
                itemDef1 = RoR2Content.Items.ExecuteLowHealthElite,
                itemDef2 = this.ItemDef
            };
            ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem] = ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem].AddToArray(transformation);
            orig();
        }
    }
}