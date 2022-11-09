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

        public override string ItemPickupDesc => "On 'Stage Transition Crit', gain a temporary barrier. Consumed on use.";

        public override string ItemFullDescription => "On '<style=cIsUtility>Stage Transition Crit</style>', gain <style=cIsHealing>50%</style> of your <style=cIsHealing>maximum health</style> as <style=cIsHealing>temporary barrier</style>, <style=cIsUtility>consuming</style> this item.";

        public override string ItemLore => "\"It's breakfast time!\"\n\"Oh boy! I'm so hungry, I could eat an Octorok! What's for breakfast?\"\n\"A can of beans.\"\n\"Wh-what the fuck? Who the fuck eats a can of beans for breakfast?!\"\n\"Me.\"\n\"The fuck is wrong with you?\"\n\"Everything.\"\n\"Yeah, that checks out.\"";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Healing, ItemTag.Utility, ItemTag.AIBlacklist, GOTCETags.BarrierRelated, ItemTag.OnStageBeginEffect, GOTCETags.Crit, GOTCETags.Consumable };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/heartybreakfast.png");

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
            CriticalTypes.OnStageCrit += ILoveAegis;
        }

        public void ILoveAegis(object sender, StageCritEventArgs args)
        {
            if (NetworkServer.active)
            {
                var instances = PlayerCharacterMasterController.instances;
                foreach (PlayerCharacterMasterController playerCharacterMaster in instances)
                {
                    if (playerCharacterMaster.master.inventory.GetItemCount(ItemDef) > 0)
                    {
                        CharacterBody body = playerCharacterMaster.master.GetBody();
                        body.healthComponent.AddBarrier(body.healthComponent.fullHealth * 0.5f);
                        body.inventory.RemoveItem(ItemDef, 1);
                        body.inventory.GiveItem(Items.NoTier.HeartlessBreakfast.Instance.ItemDef, 1);
                        CharacterMasterNotificationQueue.SendTransformNotification(body.master, ItemDef.itemIndex, GOTCE.Items.NoTier.HeartlessBreakfast.Instance.ItemDef.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);
                    }
                }
            }
        }
    }
}