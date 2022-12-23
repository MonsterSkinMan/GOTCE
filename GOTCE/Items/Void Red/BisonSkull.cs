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

        public override string ItemPickupDesc => "Gain barrier on kill. <style=cIsVoid>Corrupts all healing items</style>.";

        public override string ItemFullDescription => "On kill, gain a <style=cIsHealing>temporary barrier</style> for <style=cIsHealing>1</style> <style=cStack>(+1 per stack)</style> health. <style=cIsVoid>Corrupts all healing items</style>.";

        public override string ItemLore => "When Clayton set out for another hunt, he saw something he didn’t expect. Clayton was a simple hunter. He hunted whatever the next billionaire could pay him, which usually was endangered species. He didn't want to hunt them, but money was always tight for the poor boy. Whilst hunting, he saw something extremely odd: a man in red with a crystalline blade, rounding up all the Bighorn Bison. He shrugged and turned around, knowing this wasn't his problem and he'd just find another creature to poach.\n\nBut before he could run, the man appeared in front of him, and as quick as a lightning flash, Clayton's blood was spilled, and his gun fell to the ground.";

        public override ItemTier Tier => ItemTier.VoidTier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.Healing, ItemTag.OnKillEffect, GOTCETags.Bullshit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/BisonSkull.png");

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
            orig(self, damageReport);
        }

        private void powerUncreep(On.RoR2.Items.ContagiousItemManager.orig_Init orig)
        {
            List<ItemDef> healing = new();
            foreach (ItemDef def in ItemCatalog.allItemDefs)
            {
                if (def.ContainsTag(ItemTag.Healing) && def != ItemDef)
                {
                    healing.Add(def);
                }
            }

            ItemHelpers.RegisterCorruptions(ItemDef, healing);
            orig();
        }
    }
}