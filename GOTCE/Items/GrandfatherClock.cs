using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;
using UnityEngine.Networking;

namespace GOTCE.Items
{
    public class GrandfatherClock : ItemBase<GrandfatherClock>
    {
        public override string ConfigName => "Grandfather Clock";

        public override string ItemName => "Grandfather Clock";

        public override string ItemLangTokenName => "GOTCE_TheGameplayFunder";

        public override string ItemPickupDesc => "Small chance for an enemy to die when it damage you. Your health constantly drains. 'Critical Stage Transitions' kill you.";

        public override string ItemFullDescription => "5% <style=cStack>(+5% per stack)</style> chance on getting hit to kill the enemy that damaged you. Deal 133.33% <style=cStack>(+133.33% per stack of ALL items)</style> damage to yourself every second. On stage transition crit, <style=cDeath>die 1</style> <style=cStack>(+1 per stack)</style> time(s). Gain 5% stage transition crit chance.";

        public override string ItemLore => "Order: 12-Hour Decorative Clock\nTracking Number: 59******\nEstimated Delivery: 16/02/2061\nShipping Method: Delicate\nShipping Address: **** 8th Avenue, New York, Earth\nShipping Details:\n\n\"I know how much you valued old Pop. He was one tough son-of-a-bitch, and he admired that in you. You did know why he spent so much time in his lab, right? Why he spent so much time overseas?\nIf not, well... I have so much to tell you. Not here, but some day.\"";

        public override ItemTier Tier => ItemTier.Tier2;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Healing, ItemTag.OnStageBeginEffect, ItemTag.AIBlacklist };

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
            On.RoR2.GlobalEventManager.OnCrit += GlobalEventManager_OnCrit;
        }

        private void GlobalEventManager_OnCrit(On.RoR2.GlobalEventManager.orig_OnCrit orig, GlobalEventManager self, CharacterBody body, DamageInfo damageInfo, CharacterMaster master, float procCoefficient, ProcChainMask procChainMask)
        {
            orig(self, body, damageInfo, master, procCoefficient, procChainMask);
            if (body && procCoefficient > 0f && master && master.inventory)
            {
                Inventory inventory = master.inventory;
                int itemCount = inventory.GetItemCount(GrandfatherClock.Instance.ItemDef);
                if (itemCount > 0 && body.healthComponent)
                {
                    body.healthComponent.Suicide(body.gameObject, body.gameObject, DamageType.BypassArmor | DamageType.BypassBlock | DamageType.BypassOneShotProtection);
                }
            }
        }
    }
}
