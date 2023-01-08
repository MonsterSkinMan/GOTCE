using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items.Yellow
{
    public class RightRingFingerOfProvidence : ItemBase<RightRingFingerOfProvidence>
    {
        public override string ConfigName => "Right Ring Finger of Providence";

        public override string ItemName => "Right Ring Finger of Providence";

        public override string ItemLangTokenName => "GOTCE_ProvidenceFinger";

        public override string ItemPickupDesc => "Double ALL of your stats.";

        public override string ItemFullDescription => "Increase your <style=cIsDamage>base damage</style>, <style=cIsDamage>attack speed</style>, <style=cIsUtility>jump height</style>, <style=cIsUtility>movement speed</style>, <style=cIsHealing>maximum health</style>, <style=cIsUtility>luck</style>, <style=cIsDamage>crit chances</style>, <style=cIsDamage>AOE effect</style>, <style=cIsUtility>cooldowns</style>, <style=cIsHealing>maximum shield</style>, <style=cIsHealing>regen</style>, and size by <style=cIsDamage>100%</style> <style=cStack>(+100% per stack)</style>.";

        public override string ItemLore => "As Cracked Emoji wandered the desolate wasteland, looking for something to do as he spread corruption in his wake, he stumbled upon some rubble and ruins. Intrigued, he began looking around. Suddenly, his eyes were caught by a shiny black object sticking out of the rocks. He picked it up and began to examine it. It seemed to be the right ring finger of some unfortunate soul.\n\"<color=#e64b13>Wait holy shit this must be what remains of Providence.</color>\" he realized. However, he was beginning to get bored, and not wanting to linger too long, he shrugged his shoulders and spit a glitchy liquid on the finger before tossing it aside. As he left, a crack in the air appeared above the finger and sucked it in.\nMeanwhile, in the past, Providence was sitting around, eating a hot dog, made out of soul or something because Providence doesn't really eat I don't think.\n\"Man, I'm bored. I wish something would happen.\" he sighed. Out of nowhere, the same rift appeared above him and sucked him in.";

        public override ItemTier Tier => ItemTier.Boss;

        public override Enum[] ItemTags => new Enum[] { ItemTag.BrotherBlacklist, ItemTag.Damage, ItemTag.Utility, ItemTag.Healing, GOTCETags.Crit, GOTCETags.Cracked };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Items/RightRingFingerOfProvidence.png");

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
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.attackSpeedMultAdd += Mathf.Pow(2f, stack) - 1f;
                    args.critDamageMultAdd += Mathf.Pow(2f, stack) - 1f;
                    args.jumpPowerMultAdd += Mathf.Pow(2f, stack) - 1f;
                    args.damageMultAdd += Mathf.Pow(2f, stack) - 1f;
                    args.healthMultAdd += Mathf.Pow(2f, stack) - 1f;
                    args.moveSpeedMultAdd += Mathf.Pow(2f, stack) - 1f;
                    args.regenMultAdd += sender.regen * stack;
                    args.cooldownMultAdd += Mathf.Pow(2f, stack) - 1f;
                    args.shieldMultAdd += Mathf.Pow(2f, stack) - 1f;
                }
            }
        }
    }
}