using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items.Void_Lunar
{
    public class ExpressionOfTheImmolated : ItemBase<ExpressionOfTheImmolated>
    {
        public override string ConfigName => "Expression of the Immolated";

        public override string ItemName => "Expression of the Immolated";

        public override string ItemLangTokenName => "GOTCE_ExpressionOfTheImmolated";

        public override string ItemPickupDesc => "Halve your cooldowns... <color=#FF7F7F>BUT your skills automatically activate</color>. <style=cIsVoid>Corrupts all Gestures of the Drowned</style>.";

        public override string ItemFullDescription => "Skill cooldowns are reduced by 50% (+50% per stack). Your skills are activated automatically off cooldown.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.VoidTier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

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
                    args.cooldownMultAdd += -(0.5f * (float)Math.Pow(stack, 0.5));
                }
            }
        }

        private bool GenericSkill_IsReady(On.RoR2.GenericSkill.orig_IsReady orig, GenericSkill self)
        {
            var result = orig(self);
            CharacterBody body = PlayerCharacterMasterController.instances[0].master.GetBody();
            if (result && body.inventory && body && body.inventory.GetItemCount(ItemDef) > 0)
            {
                self.ExecuteIfReady();
            }
            return result;
        }
    }
}