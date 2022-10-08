using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class GrandfatherClock : ItemBase<GrandfatherClock>
    {
        public override string ConfigName => "Grandfather Clock";

        public override string ItemName => "Grandfather Clock";

        public override string ItemLangTokenName => "GOTCE_GrandfatherClock";

        public override string ItemPickupDesc => "On 'Critical Strike', die. (stage crits aren't implemented yet so we're just going with normal crits for now)";

        public override string ItemFullDescription => "On <style=cIsDamage>critical strike</style>, literally fucking <style=cIsHealth>die</style>. Stage crits aren't implemented yet so we're just going with normal crits for now.";

        public override string ItemLore => "Order: 12-Hour Decorative Clock\nTracking Number: 59******\nEstimated Delivery: 16/02/2061\nShipping Method: Delicate\nShipping Address: **** 8th Avenue, New York, Earth\nShipping Details:\n\n\"I know how much you valued old Pop. He was one tough son-of-a-bitch, and he admired that in you. You did know why he spent so much time in his lab, right? Why he spent so much time overseas?\nIf not, well... I have so much to tell you. Not here, but some day.\"";

        public override ItemTier Tier => ItemTier.Tier2;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Healing, ItemTag.OnStageBeginEffect, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/GrandfatherClock.png");

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
                int itemCount = master.inventory.GetItemCount(Instance.ItemDef);
                if (itemCount > 0 && body.healthComponent)
                {
                    body.healthComponent.Suicide(body.gameObject, body.gameObject, DamageType.BypassArmor | DamageType.BypassBlock | DamageType.BypassOneShotProtection);
                }
            }
        }
    }
}