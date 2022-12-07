using System;
using BepInEx.Configuration;
using GOTCE.Components;
using R2API;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace GOTCE.Items.White
{
    public class FaultySpacetimeClock : ItemBase<FaultySpacetimeClock>
    {
        public override string ConfigName => "Faulty Spacetime Clock";

        public override string ItemName => "Faulty Spacetime Clock";

        public override string ItemLangTokenName => "GOTCE_FaultySpacetimeClock";

        public override string ItemPickupDesc => "Gain a chance to critically Stage Transition, skipping the next stage and unlocking powerful synergies...";

        public override string ItemFullDescription => "Gain a <style=cIsUtility>10%</style> <style=cStack>(+10% per stack)</style> chance to '<style=cIsUtility>Stage Transition Crit</style>', skipping the next stage.";

        public override string ItemLore => "Order: [ERR 04: DATABASE CORRUPTION]\nTracking Number: 95******\nEstimated Delivery: 15/02/2051\nShipping Method: Delicate\nShipping Address: **** Espl. des Particules, Switzerland, Earth\nShipping Details:\n\n...think I'm on the verge of a breakthrough. This broken clock I found seems to have incredibly anomalous properties, dramatically affecting the flow of local time and space. It's easily the weirdest thing I've ever found. I genuinely believe it could revolutionize everything, to the the point where I...";

        private System.Random rand = new();

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/faultyspacetimeclock.png");

        private bool lastStageWasCrit = true;
        public override GameObject ItemModel => null;

        public override Enum[] ItemTags => new Enum[] { ItemTag.OnStageBeginEffect, ItemTag.AIBlacklist, GOTCETags.Crit };

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }


        public override void Hooks()
        {

            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory)
                    {
                        args.Stats.StageCritChanceAdd += GetCount(args.Stats.body) * 10;
                    }
                }
            };
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }
    }
}