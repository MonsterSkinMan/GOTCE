using BepInEx.Configuration;
using GOTCE.Components;
using GOTCE.Items.Lunar;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace GOTCE.Items.White
{
    public class Defibrillator : ItemBase<Defibrillator>
    {
        public override string ConfigName => "Defibrillator";

        public override string ItemName => "Defibrillator";

        public override string ItemLangTokenName => "GOTCE_Defibrillator";

        public override string ItemPickupDesc => "Gain a small chance to cheat death. Increase attack speed upon dying.";

        public override string ItemFullDescription => "Gain an <style=cIsHealing>8%</style> <style=cStack>(+8% per stack)</style> chance to <style=cIsHealing>revive</style>. Each death increases your <style=cIsDamage>attack speed</style> by <style=cIsDamage>40%</style> <style=cStack>(+30% per stack)</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.Damage, ItemTag.AIBlacklist };
        // ai blacklist cause too much effort to make it work for mobs for now lol
        // hook CharacterMaster.IsDeadAndOutOfLivesServer for that

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
            On.RoR2.Inventory.GiveItem_ItemIndex_int += Inventory_GiveItem_ItemIndex_int;
            On.RoR2.Inventory.RemoveItem_ItemIndex_int += Inventory_RemoveItem_ItemIndex_int;
            On.RoR2.CharacterMaster.OnBodyDeath += CharacterMaster_OnBodyDeath;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void Inventory_RemoveItem_ItemIndex_int(On.RoR2.Inventory.orig_RemoveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            if (NetworkServer.active && itemIndex == Instance.ItemDef.itemIndex)
            {
                if (self.GetComponent<GOTCE_StatsComponent>())
                {
                    // var stack = self.GetItemCount(Instance.ItemDef);
                    var stats = self.GetComponent<GOTCE_StatsComponent>();
                    stats.reviveChanceAdd -= 8f * count;
                }
            }
            orig(self, itemIndex, count);
        }

        private void Inventory_GiveItem_ItemIndex_int(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            if (NetworkServer.active && itemIndex == Instance.ItemDef.itemIndex)
            {
                if (self.GetComponent<GOTCE_StatsComponent>())
                {
                    var stack = self.GetItemCount(Instance.ItemDef);
                    var stats = self.GetComponent<GOTCE_StatsComponent>();
                    stats.reviveChanceAdd += 8f * stack;
                }
            }
        }

        private void CharacterMaster_OnBodyDeath(On.RoR2.CharacterMaster.orig_OnBodyDeath orig, CharacterMaster self, CharacterBody body)
        {
            orig(self, body);
            if (NetworkServer.active)
            {
                if (self.GetComponent<GOTCE_StatsComponent>())
                {
                    var stats = self.GetComponent<GOTCE_StatsComponent>();
                    var stack = body.inventory.GetItemCount(Instance.ItemDef);
                    if (stack > 0 && Util.CheckRoll(stats.reviveChance))
                    {
                        self.preventGameOver = true;
                        stats.Invoke(nameof(stats.RespawnExtraLife), 1f);
                        stats.deathCount++;
                    }
                }
            }
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                var stats = sender.gameObject.GetComponent<GOTCE_StatsComponent>();
                if (stack > 0 && stats)
                {
                    args.baseAttackSpeedAdd += (0.4f + 0.3f * (stack - 1)) * stats.deathCount;
                }
            }
        }
    }
}