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
    public class ExplorersTorch : ItemBase<ExplorersTorch>
    {
        public override string ConfigName => "Explorers Torch";

        public override string ItemName => "Explorer's Torch";

        public override string ItemLangTokenName => "GOTCE_ExplorersTorch";

        public override string ItemPickupDesc => "Chance to ignite on hit. <style=cIsVoid>Corrupts all Ignition Tanks</style>.";

        public override string ItemFullDescription => "Gain a <style=cIsDamage>25%</style> chance on hit to <style=cIsDamage>ignite 1</style> <style=cStack>(+1 per stack)</style> times. <style=cIsVoid>Corrupts all Ignition Tanks</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.VoidTier2;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/ExplorersTorch.png");

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
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.Items.ContagiousItemManager.Init += Sussy;
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            if (damageInfo.attacker && damageInfo.attacker.GetComponent<CharacterBody>().inventory)
            {
                var inv = damageInfo.attacker.GetComponent<CharacterBody>().inventory;
                var stack = inv.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    if (Util.CheckRoll(25f * damageInfo.procCoefficient, damageInfo.attacker.GetComponent<CharacterBody>().master.luck))
                    {
                        InflictDotInfo blaze = new()
                        {
                            attackerObject = damageInfo.attacker,
                            victimObject = victim,
                            dotIndex = DotController.DotIndex.Burn,
                            damageMultiplier = 1f,
                            totalDamage = damageInfo.attacker.GetComponent<CharacterBody>().damage * 0.5f
                        };
                        for (int i = 0; i < stack; i++)
                        {
                            DotController.InflictDot(ref blaze);
                        }
                    }
                }
            }
            orig(self, damageInfo, victim);
        }

        private void Sussy(On.RoR2.Items.ContagiousItemManager.orig_Init orig)
        {
            ItemDef.Pair transformation = new()
            {
                itemDef1 = DLC1Content.Items.StrengthenBurn,
                itemDef2 = this.ItemDef
            };
            ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem] = ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem].AddToArray(transformation);
            orig();
        }
    }
}