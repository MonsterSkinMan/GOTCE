using BepInEx.Configuration;
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

        public override string ItemFullDescription => "<style=cIsUtility>Reduce Skill cooldown</style> by <style=cIsUtility>50%</style> <style=cStack>(+15% per stack)</style>. Forces your Skills to <style=cIsUtility>activate</style> whenever they are off <style=cIsUtility>cooldown</style>. <style=cIsVoid>Corrupts all Gestures of the Drowned</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.VoidTier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

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
            var result = orig(self);
            CharacterBody body = PlayerCharacterMasterController.instances[0].master.GetBody();
            if (body && body.inventory && body.inventory.GetItemCount(ItemDef) > 0)
            {
                self.ExecuteIfReady();
            }
            return result;
        }
    }
}