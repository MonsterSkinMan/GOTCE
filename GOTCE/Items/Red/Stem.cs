using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using HarmonyLib;

namespace GOTCE.Items.Red
{
    public class Stem : ItemBase<Stem>
    {
        public override string ItemName => "The Stem";

        public override string ConfigName => ItemName;

        public override string ItemLangTokenName => "GOTCE_Stem";

        public override string ItemPickupDesc => "Stun Grenade (4)";

        public override string ItemFullDescription => "<style=cIsUtility>Upgrades</style> all of your skills with <style=cIsDamage>Suppressive Fire</style>. Increase <style=cIsDamage>attack speed</style> by <style=cIsDamage>10%</style> <style=cStack>(+10% per stack)</style>.";

        public override string ItemLore => 
        """
        I've come to make an announcement: Ayin's a bitch-ass motherfucker. He ripped out my fucking spinal cord. That's right. He took his eggman-ass claw machine and ripped out my FUCKING spine, and he then built a fucking sex robot, and benjamin was like "ayin what the actual fuck". So I'm making a callout post from within the light. Ayin, you got a small dick. It's the size of this phillip's self-esteem except WAY smaller. And guess what? Here's what my dong looks like. That's right, baby. Tall points, no quills, no pillows, look at that, it looks like two balls and a bong. He ripped out my spine, so guess what, I'm gonna fuck the earth. That's right, this is what you get! My GASLIGHTING! Except I'm not gonna gaslight the earth. I'm gonna go higher. I'm turning that random bitch over there into a cow! How do you like that, A CORP? I TURNED THAT BITCH INTO A COW, YOU IDIOT! You have twenty-three hours before the the fucking cow obliterates half of district 12, now get out of my fucking sight before I gaslight you too!
        """;

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.NonLunarLunar };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.SecondaryAssets.LoadAsset<Sprite>("stem.png");

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