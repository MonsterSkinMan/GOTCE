/* using BepInEx.Configuration;
using GOTCE.Components;
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

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility, ItemTag.Damage, ItemTag.AIBlacklist };
        // ai blacklist cause too much effort to make it work for mobs for now lol
        // hook CharacterMaster.IsDeadAndOutOfLivesServer for that

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null; Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/BangSnap.png"); // replace with defibrillator icon

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
            orig(self, itemIndex, count);
            if (NetworkServer.active && itemIndex == Instance.ItemDef.itemIndex)
            {
                if (self.gameObject.GetComponent<GOTCE_StatsComponent> != null)
                {
                    var stack = self.GetItemCount(Instance.ItemDef);
                    self.gameObject.GetComponent<CharacterMaster>().GetBody().GetComponent<GOTCE_StatsComponent>().defibrillatorRespawnChance = 8f * stack;
                }
            }
        }

        private void CharacterMaster_OnBodyDeath(On.RoR2.CharacterMaster.orig_OnBodyDeath orig, CharacterMaster self, CharacterBody body)
        {
            var cachedDestroy = self.destroyOnBodyDeath;
            var respawnChance = body.GetComponent<GOTCE_StatsComponent>().respawnChance;
            var cachedCondition = Util.CheckRoll(respawnChance);
            if (NetworkServer.active)
            {
                if (body.inventory)
                {
                    if (cachedCondition)
                    {
                        self.destroyOnBodyDeath = false;
                        self.Invoke("RespawnExtraLife", 2f);
                        self.Invoke("PlayExtraLifeSFX", 1f);
                    }
                }
            }
            orig(self, body);
            self.destroyOnBodyDeath = cachedDestroy;
            if (cachedCondition) self.preventGameOver = true;
            // issues: respawn chance doesnt get lowered when scrapping/removing the item
            // i can only respawn once for some reason
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0 && sender.gameObject.GetComponent<GOTCE_StatsComponent>())
                {
                    args.baseAttackSpeedAdd += (0.4f + 0.3f * (stack - 1)) * sender.gameObject.GetComponent<GOTCE_StatsComponent>().deathCount;
                }
            }
        }

        private void Inventory_GiveItem_ItemIndex_int(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            if (NetworkServer.active && itemIndex == Instance.ItemDef.itemIndex)
            {
                if (self.gameObject.GetComponent<GOTCE_StatsComponent> != null)
                {
                    var stack = self.GetItemCount(Instance.ItemDef);
                    self.gameObject.GetComponent<CharacterMaster>().GetBody().GetComponent<GOTCE_StatsComponent>().defibrillatorRespawnChance = 8f * stack;
                }
            }
        }
    }
} */
