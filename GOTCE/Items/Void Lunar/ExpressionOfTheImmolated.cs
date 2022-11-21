using BepInEx.Configuration;
using HarmonyLib;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items.VoidLunar
{
    public class ExpressionOfTheImmolated : ItemBase<ExpressionOfTheImmolated>
    {
        public override string ConfigName => "Expression of the Immolated";

        public override string ItemName => "Expression of the Immolated";

        public override string ItemLangTokenName => "GOTCE_ExpressionOfTheImmolated";

        public override string ItemPickupDesc => "Dramatically reduce Skill cooldown... <color=#FF7F7F>BUT they automatically activate.</color> <style=cIsVoid>Corrupts all Gestures of the Drowned</style>.";

        public override string ItemFullDescription => "<style=cIsUtility>Reduce Skill cooldown</style> by <style=cIsUtility>50%</style> <style=cStack>(+15% per stack)</style>. Forces all players' Skills to <style=cIsUtility>activate</style> whenever they are off <style=cIsUtility>cooldown</style>. <style=cIsVoid>Corrupts all Gestures of the Drowned</style>.";

        public override string ItemLore => "This is a stupid item. At least it's more interesting than gesture.";

        public override ItemTier Tier => Tiers.LunarVoid.Instance.TierEnum;
        public override ItemTierDef OverrideTierDef => Tiers.LunarVoid.Instance.tier;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/ExpressionOfTheImmolated.png");

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
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.GenericSkill.IsReady += GenericSkill_IsReady;
            On.RoR2.Items.ContagiousItemManager.Init += JesterOfTheClowned;
        }

        public static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.cooldownMultAdd -= (Mathf.Pow(2f, stack) - 1) / Mathf.Pow(2f, stack);
                }
            }
        }

        private bool GenericSkill_IsReady(On.RoR2.GenericSkill.orig_IsReady orig, GenericSkill self)
        {
            bool res = orig(self);
            if (self.characterBody && GetCount(self.characterBody) > 0) {
                self.ExecuteIfReady();
            }
            return res;
        }

        private void JesterOfTheClowned(On.RoR2.Items.ContagiousItemManager.orig_Init orig)
        {
            ItemDef.Pair transformation = new ItemDef.Pair()
            {
                itemDef1 = RoR2Content.Items.AutoCastEquipment,
                itemDef2 = this.ItemDef
            };
            ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem] = ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem].AddToArray(transformation);
            orig();
        }
    }
}