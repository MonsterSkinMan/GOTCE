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

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.VoidTier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Healing, ItemTag.Utility, ItemTag.LowHealth, ItemTag.AIBlacklist };

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

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