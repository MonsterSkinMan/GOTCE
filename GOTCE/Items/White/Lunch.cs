﻿using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using UnityEngine;

namespace GOTCE.Items.White
{
    public class Lunch : ItemBase<Lunch>
    {
        public override string ConfigName => "Lunch";

        public override string ItemName => "Lunch";

        public override string ItemLangTokenName => "GOTCE_Lunch";

        public override string ItemPickupDesc => "If it's lunch time, gain 10% max health.";

        public override string ItemFullDescription => "Gain <style=cIsHealing>10%</style> <style=cStack>(+7% per stack)</style> <style=cIsHealing>maximum health</style> if the time is between 12 PM and 4 PM.";

        public override string ItemLore => "\"God, I never want to fucking eat beans again. What are we having for lunch?\"\n\"Pizza I guess.\"\n\"Oh shit really?\"\n\"Yeah, here you go.\"\n\"Oh wow thanks man.\"\n\"Make sure you eat it quickly though it's getting late.\"\n\"Yeah yeah whatever man I'll take as long as I- wait why the fuck is it disappearing?\"\n\"It's not lunch time anymore.\"\n\"Well, you know what it is? It's fucking stupid, that's what.\"\n\"Hey, I don't make the rules around here.\"\n\"Whatever, man.\"";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Healing, ItemTag.Utility, GOTCETags.TimeDependant };

        public override GameObject ItemModel => Main.GOTCEModels.LoadAsset<GameObject>("Assets/GOTCE/Lunch.prefab");

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/Lunch.png");

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
            RecalculateStatsAPI.GetStatCoefficients += new RecalculateStatsAPI.StatHookEventHandler(HealthIncrease);
        }

        public static void HealthIncrease(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.inventory)
            {
                bool lunchTime = DateTime.Now.Hour >= 12 && DateTime.Now.Hour <= 16;
                var stack = body.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0 && lunchTime)
                {
                    args.healthMultAdd += 0.1f + 0.07f * (stack - 1);
                }
            }
        }
    }
}

// seems to not update, borrow groove's code later