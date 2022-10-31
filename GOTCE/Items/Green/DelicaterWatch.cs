using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class DelicaterWatch : ItemBase<DelicaterWatch>
    {
        public override string ConfigName => "Delicater Watch";

        public override string ItemName => "Delicater Watch";

        public override string ItemLangTokenName => "GOTCE_DelicaterWatch";

        public override string ItemPickupDesc => "Deal increased damage. Breaks upon activating the teleporter.";

        public override string ItemFullDescription => "Gain <style=cIsDamage>+100%</style> <style=cStack>(+100% per stack)</style> <style=cIsDamage>damage</style>. Activating the teleporter <style=cIsUtility>breaks</style> this item.";

        public override string ItemLore => "Now, it is time for us to discuss the Delicater Watch, a marvelous piece of machinery that- oh shit it's already broken. Guess I shouldn't have even mentioned its existence.";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/DelicaterWatch.png");

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
            On.RoR2.TeleporterInteraction.OnInteractionBegin += BreakStacks;
            RecalculateStatsAPI.GetStatCoefficients += Damage;
        }

        public void BreakStacks(On.RoR2.TeleporterInteraction.orig_OnInteractionBegin orig, TeleporterInteraction self, Interactor activator)
        {
            var instances = PlayerCharacterMasterController.instances;
            foreach (PlayerCharacterMasterController playerCharacterMaster in instances)
            {
                if (playerCharacterMaster && playerCharacterMaster.master && playerCharacterMaster.master.inventory && playerCharacterMaster.master.inventory.GetItemCount(ItemDef) > 0)
                {
                    var inventory = playerCharacterMaster.master.inventory;
                    inventory.GiveItem(GOTCE.Items.NoTier.DelicatestWatch.Instance.ItemDef, playerCharacterMaster.master.GetBody().inventory.GetItemCount(ItemDef));
                    inventory.RemoveItem(ItemDef, playerCharacterMaster.master.GetBody().inventory.GetItemCount(ItemDef));
                    CharacterMasterNotificationQueue.SendTransformNotification(playerCharacterMaster.master, ItemDef.itemIndex, GOTCE.Items.NoTier.DelicatestWatch.Instance.ItemDef.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);
                }
            }
            orig(self, activator);
        }

        public void Damage(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.inventory)
            {
                if (body.inventory.GetItemCount(ItemDef) > 0)
                {
                    args.damageMultAdd += 1f + 1f * (body.inventory.GetItemCount(ItemDef) - 1);
                }
            }
        }
    }
}