using RoR2;
using R2API;
using UnityEngine;
using BepInEx.Configuration;
using HarmonyLib;
using GOTCE.Items.Red;

namespace GOTCE.Items.VoidRed
{
    public class TubOfBart : ItemBase<TubOfBart>
    {
        public override string ItemName => "Tub Of Bart";

        public override string ConfigName => ItemName;

        public override string ItemLangTokenName => "GOTCE_TubOfBart";

        public override string ItemPickupDesc => "Increases agility. <style=cIsVoid>Corrupts all Tubs of Lard</style>.";

        public override string ItemFullDescription => "Increases <style=cIsUtility>acceleration, air control and move speed</style> by <style=cIsUtility>50%</style> <style=cStack>(+20% per stack)</style>. <style=cIsVoid>Corrupts all Tubs of Lard</style>.";

        // implement bhop l8r
        // either bubbet's bunny foot item or mint tea mod
        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.VoidTier3;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/tubofbart.png");

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
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.Items.ContagiousItemManager.Init += ContagiousItemManager_Init;
        }

        private void ContagiousItemManager_Init(On.RoR2.Items.ContagiousItemManager.orig_Init orig)
        {
            ItemDef.Pair transformation = new()
            {
                itemDef1 = TubOfLard.Instance.ItemDef,
                itemDef2 = this.ItemDef
            };
            ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem] = ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem].AddToArray(transformation);
            orig();
        }

        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);
            self.AddItemBehavior<GOTCE_AccelerationAirControlComponent>(self.inventory.GetItemCount(Instance.ItemDef));
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.moveSpeedMultAdd += 0.5f + 0.2f * (stack - 1);
                }
            }
        }
    }

    public class GOTCE_AccelerationAirControlComponent : CharacterBody.ItemBehavior
    {
        public CharacterMotor motor;
        public Inventory inv;

        public void Start()
        {
            motor = body.gameObject.GetComponent<CharacterMotor>();
            inv = body.master.GetComponent<Inventory>();
            if (motor != null)
            {
                motor.body.acceleration += motor.body.acceleration / 2f + motor.body.acceleration / 5f * (stack - 1);
                motor.airControl += motor.airControl / 2f + motor.airControl / 5f * (stack - 1);
            }
        }
    }
}