using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using UnityEngine.Networking;
using GOTCE.Items.White;
using HarmonyLib;
using GOTCE.Items.NoTier;

namespace GOTCE.Items.VoidWhite
{
    public class FortifiedFlask : ItemBase<FortifiedFlask>
    {
        public override string ConfigName => "Fortified Flask";

        public override string ItemName => "Fortified Flask";

        public override string ItemLangTokenName => "GOTCE_FortifiedFlask";

        public override string ItemPickupDesc => "Receive instant barrier at low health. Consumed on use. <style=cIsVoid>Corrupts all Power Elixirs</style>.";

        public override string ItemFullDescription => "Taking damage to below <style=cIsHealth>25% health</style> <style=cIsUtility>consumes</style> this item, giving you <style=cIsHealing>barrier</style> for <style=cIsHealing>100%</style> of your <style=cIsHealing>maximum health</style>. <style=cIsVoid>Corrupts all Power Elixirs</style>.";

        public override string ItemLore => "So, you're aware of the fact that RoR2 has some unused items, right? Most of them still have some fragments left over in the code, and some of them can be obtained through cheats, though they aren't functional for the most part. However, one of these items actually only remains through a mention in the language file. It's called Gold Flask. According to its description, the item gives you a barrier worth 100% max health when you reach 30% health, and is then consumed on use. Some people, myslf included, think that this item eventually became power elixir. So it obviously made sense to bring that item back as a void elixir, especially to fit in with the barrier-related items GOTCE adds. The only difference between Fortified Flask and Gold Flask is that f flask activates at 25% health instead of 30%, since 25% is the standard low health threshold these days.";

        public override ItemTier Tier => ItemTier.VoidTier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Healing, ItemTag.Utility, ItemTag.LowHealth, ItemTag.AIBlacklist };

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/FortifiedFlask.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.HealthComponent.UpdateLastHitTime += HealthComponent_UpdateLastHitTime;
            On.RoR2.Items.ContagiousItemManager.Init += WoolieDimension;
        }

        private void HealthComponent_UpdateLastHitTime(On.RoR2.HealthComponent.orig_UpdateLastHitTime orig, HealthComponent self, float damageValue, Vector3 damagePosition, bool damageIsSilent, GameObject attacker)
        {
            if (NetworkServer.active && self.body && damageValue > 0)
            {
                var body = self.body;
                if (body.inventory)
                {
                    var stack = body.inventory.GetItemCount(Instance.ItemDef);
                    if (stack > 0 && self.isHealthLow)
                    {
                        body.inventory.RemoveItem(Instance.ItemDef);
                        body.inventory.GiveItem(DilutedFlask.Instance.ItemDef);
                        CharacterMasterNotificationQueue.SendTransformNotification(body.master, Instance.ItemDef.itemIndex, DilutedFlask.Instance.ItemDef.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);
                        self.AddBarrier(self.fullCombinedHealth);
                    }
                }
            }
            orig(self, damageValue, damagePosition, damageIsSilent, attacker);
        }

        private void WoolieDimension(On.RoR2.Items.ContagiousItemManager.orig_Init orig)
        {
            ItemDef.Pair transformation = new()
            {
                itemDef1 = DLC1Content.Items.HealingPotion,
                itemDef2 = this.ItemDef
            };
            ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem] = ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem].AddToArray(transformation);
            orig();
        }
    }
}