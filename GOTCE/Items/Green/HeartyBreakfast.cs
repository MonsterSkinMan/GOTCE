using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using UnityEngine.Networking;
using GOTCE.Items.White;

namespace GOTCE.Items.Green
{
    public class HeartyBreakfast : ItemBase<HeartyBreakfast>
    {
        public override string ConfigName => "Hearty Breakfast";

        public override string ItemName => "Hearty Breakfast";

        public override string ItemLangTokenName => "GOTCE_HeartyBreakfast";

        public override string ItemPickupDesc => "On Stage Transition Crit, gain a temporary barrier. Consumed on use.";

        public override string ItemFullDescription => "On '<style=cIsUtility>Stage Transition Crit</style>', gain <style=cIsHealing>50%</style> of your <style=cIsHealing>maximum health</style> as <style=cIsHealing>temporary barrier</style>, <style=cIsUtility>consuming</style> this item.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier2;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Healing, ItemTag.AIBlacklist };

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
            FaultySpacetimeClock.Instance.OnStageCrit += ILoveAegis;
        }

        public void ILoveAegis(object sender, StageCritEventArgs args)
        {
            CharacterBody body = PlayerCharacterMasterController.instances[0].master.GetBody();
            if (body.inventory && body.inventory.GetItemCount(ItemDef) > 0)
            {
                body.healthComponent.AddBarrier(body.healthComponent.fullHealth * 0.5f);
                body.inventory.RemoveItem(ItemDef, 1);
                body.inventory.GiveItem(Items.NoTier.HeartlessBreakfast.Instance.ItemDef, 1);
                CharacterMasterNotificationQueue.SendTransformNotification(body.master, ItemDef.itemIndex, GOTCE.Items.NoTier.HeartlessBreakfast.Instance.ItemDef.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);
            }
        }
    }
}