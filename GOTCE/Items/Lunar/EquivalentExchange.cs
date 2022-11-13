/*using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items.Lunar
{
    public class EquivalentExchange : ItemBase<EquivalentExchange>
    {
        public override string ConfigName => "Equivalent Exchange";

        public override string ItemName => "Equivalent Exchange";

        public override string ItemLangTokenName => "GOTCE_EquivalentExchange";

        public override string ItemPickupDesc => "On death, kill the closest enemy... BUT on killing an enemy through any other method, die.";

        public override string ItemFullDescription => "On death, the closest 1 (+1 per stack) monster(s) are killed. If you kill a monster in any other way, you die 1 (+1 per stack) time(s).";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.AIBlacklist, ItemTag.OnKillEffect, GOTCETags.OnDeathEffect };

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
            On.RoR2.GlobalEventManager.OnCharacterDeath += DieOnKill;
            On.RoR2.GlobalEventManager.OnCharacterDeath += KillOnDie;
        }

        private void DieOnKill(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
        {
            var attacker = damageReport.attackerBody;
            var victim = damageReport.victim;
            if (attacker && victim && attacker.inventory)
            {
                var stack = attacker.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0 && attacker.healthComponent && NetworkServer.active)
                {
                    attacker.healthComponent.Suicide(attacker.gameObject, attacker.gameObject, DamageType.Generic);
                }
            }
            orig(self, damageReport);
        }

        private void KillOnDie(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport report)
        {
            if (report.victim && report.victimBody)
            {
                CharacterBody body = report.victimBody;
                if (body.inventory)
                {
                    int count = body.inventory.GetItemCount(ItemDef);
                    if (count > 0)
                    {
                        if (NetworkServer.active)
                        {
                            body.healthComponent.AddBarrier((body.healthComponent.fullHealth * 0.05f) * count); //WIP placeholder idk how to make it kill a random enemy
                        }
                    }
                }
                orig(self, report);
            }
        }
    }
} */
