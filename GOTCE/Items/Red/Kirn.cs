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

        public override string ItemPickupDesc => "Even if frags did 2000% with no falloff I'd still use suppressive because I value survivability and consistency more than anything.";

        public override string ItemFullDescription => "<style=cIsUtility>Upgrades</style> all of your skills with <style=cIsDamage>Suppressive Fire</style>. Increase <style=cIsDamage>attack speed</style> by <style=cIsDamage>10%</style> <style=cStack>(+10% per stack)</style>.";

        public override string ItemLore => "\"Even if frags did 2000% with no falloff I'd still use suppressive because I value survivability and consistency more than anything\" \"Suppressive fire is a guaranteed stun. All of your damage is m1. Suppressive is also perfectly accurate (Resetting bloom on commando needs you to shoot 4 times a second which is pretty lole). Suppressive also scales more with attack speed\"\n-a fucking insane person";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.NonLunarLunar };

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
                    // var consistency = Skills.SuppressiveNader.Instance.SkillDef;
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