﻿using R2API;
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

        public override string ItemLore => "Y'know, I think it's funny how the ol' lopper, one of the shittiest items in RoR1, is still better than old guillotine when brought back as a void counterpart to it. Just goes to show how much guillotine sucks, I guess.";

        public override ItemTier Tier => ItemTier.VoidTier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.Crit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/AncientAxe.png");

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
                if (damageInfo.attacker.GetComponent<HealthComponent>().body.inventory)
                {
                    var inv = damageInfo.attacker.GetComponent<HealthComponent>().body.inventory;
                    int stack = inv.GetItemCount(Instance.ItemDef);
                    if (self.health < (self.fullCombinedHealth * (0.09 * stack)))
                    {
                        damageInfo.crit = true;
                    }
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