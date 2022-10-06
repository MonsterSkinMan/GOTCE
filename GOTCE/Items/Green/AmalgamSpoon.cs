using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using UnityEngine.Networking;

namespace GOTCE.Items.Green
{
    public class AmalgamSpoon : ItemBase<AmalgamSpoon>
    {
        public override string ConfigName => "Amalgam Spoon";

        public override string ItemName => "Amalgam Spoon";

        public override string ItemLangTokenName => "GOTCE_AmalgamSpoon";

        public override string ItemPickupDesc => "Killing an elite enemy with a 'Critical Strike' grants a temporary barrier based on secondary charges.";

        public override string ItemFullDescription => "Gain <style=cIsDamage>5% critical strike chance</style>. Gain a <style=cIsHealing>temporary barrier</style> on killing an elite enemy with a <style=cIsDamage>critical strike</style> for <style=cIsHealing>100</style> <style=cStack>(+50 per stack)</style> health, multiplied by the amount of your <style=cIsUtility>secondary charges</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier2;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Healing, ItemTag.Utility };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null; /* Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/drill.png"); */ // replace with amalgam spoon icon

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
            RecalculateStatsAPI.GetStatCoefficients += new RecalculateStatsAPI.StatHookEventHandler(CritIncrease);
            On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        private void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
        {
            var attacker = damageReport.attackerBody;
            var victim = damageReport.victim;
            if (attacker && victim && attacker.inventory)
            {
                var stack = attacker.inventory.GetItemCount(Instance.ItemDef);
                if (damageReport.victimIsElite && damageReport.damageInfo.procCoefficient > 0f && damageReport.damageInfo.crit)
                {
                    if (attacker.healthComponent && NetworkServer.active && attacker.skillLocator && attacker.skillLocator.secondary)
                    {
                        attacker.healthComponent.AddBarrier((100f + 50f * (stack - 1)) * damageReport.damageInfo.procCoefficient * attacker.skillLocator.secondary.maxStock);
                    }
                }
            }
            orig(self, damageReport);
        }

        public static void CritIncrease(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.inventory)
            {
                var stack = body.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.critAdd += 5f;
                }
            }
        }
    }
}
