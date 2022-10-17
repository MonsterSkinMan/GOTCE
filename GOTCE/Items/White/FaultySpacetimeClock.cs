using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using UnityEngine.Networking;
using GOTCE.Components;
using R2API.Networking;
using R2API.Networking.Interfaces;
using System;

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

        private bool lastStageWasCrit = false;

        public EventHandler<StageCritEventArgs> OnStageCrit;

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.OnStageBeginEffect, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/faultyspacetimeclock.png");
        private System.Random rand = new();

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
            CharacterBody.onBodyStartGlobal += Crit;
            RoR2.Run.onRunStartGlobal += (run) =>
            {
                lastStageWasCrit = false;
            };
        }

        public void Crit(CharacterBody body)
        { // there is a 99% chance this code is horrible
            if (body.masterObject && !body.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                body.masterObject.AddComponent<GOTCE_StatsComponent>();
            }
            if (NetworkServer.active && body.isPlayerControlled && Stage.instance.entryTime.timeSince <= 3f && body.masterObject.GetComponent<GOTCE_StatsComponent>())
            {
                bool lastStageWasCritPrev = lastStageWasCrit;

                if (lastStageWasCrit)
                {
                    EventHandler<StageCritEventArgs> raiseEvent = OnStageCrit;

                    if (raiseEvent != null)
                    {
                        raiseEvent(this, new StageCritEventArgs());
                    }
                    lastStageWasCrit = false;
                }
                float totalChance = 0f;
                if (body.masterObject.GetComponent<GOTCE_StatsComponent>())
                {
                    GOTCE_StatsComponent vars = body.masterObject.GetComponent<GOTCE_StatsComponent>();
                    vars.DetermineStageCrit();
                    totalChance += vars.stageCritChance;
                };

                if (Util.CheckRoll(totalChance) && !lastStageWasCritPrev)
                {
                    lastStageWasCrit = true;
                    Run.instance.AdvanceStage(Run.instance.nextStageScene);
                }
            }
        }
    }

    public class StageCritEventArgs : EventArgs
    {
        public StageCritEventArgs()
        {
        }
    }
}