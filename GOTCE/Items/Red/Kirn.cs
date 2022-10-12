using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using HarmonyLib;

namespace GOTCE.Items.Red
{
    public class Kirn : ItemBase<Kirn>
    {
        public override string ItemName => "Kirn: The Item";

        public override string ConfigName => ItemName;

        public override string ItemLangTokenName => "GOTCE_Kirn";

        public override string ItemPickupDesc => "Even if frags did 2000% with no falloff...";

        public override string ItemFullDescription => "Upgrades all of your skills with <style=cIsDamage>suppressive fire</style>. Increases attack speed by 10% (+10% per stack).";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Healing };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/kirn.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.CharacterBody.OnInventoryChanged += GainConsistency;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        public void GainConsistency(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            if (self.inventory && NetworkServer.active)
            {
                int count = self.inventory.GetItemCount(ItemDef);
                if (count > 0)
                {
                    var consistency = Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>("RoR2/Base/Commando/CommandoBodyBarrage.asset").WaitForCompletion();
                    self.skillLocator.primary.SetSkillOverride(self.masterObject, consistency, GenericSkill.SkillOverridePriority.Upgrade);
                    self.skillLocator.secondary.SetSkillOverride(self.masterObject, consistency, GenericSkill.SkillOverridePriority.Upgrade);
                    self.skillLocator.utility.SetSkillOverride(self.masterObject, consistency, GenericSkill.SkillOverridePriority.Upgrade);
                    self.skillLocator.special.SetSkillOverride(self.masterObject, consistency, GenericSkill.SkillOverridePriority.Upgrade);
                }
            }
            orig(self);
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.damageMultAdd -= (Mathf.Pow(2f, stack) - 1) / Mathf.Pow(2f, stack);
                    args.attackSpeedMultAdd += 0.1f * stack;
                }
            }
        }
    }
}