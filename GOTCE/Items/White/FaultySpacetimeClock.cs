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

        public void Crit(CharacterBody body)
        { // there is a 99% chance this code is horrible
            if (body.masterObject && !body.masterObject.GetComponent<GOTCE_StatsComponent>())
            {
                body.masterObject.AddComponent<GOTCE_StatsComponent>();
            }
            if (NetworkServer.active && body.isPlayerControlled && Stage.instance.entryTime.timeSince <= 3f && body.masterObject.GetComponent<GOTCE_StatsComponent>())
            {
                bool lastStageWasCritPrev = lastStageWasCrit;

                if (lastStageWasCrit)
                {
                    EventHandler<StageCritEventArgs> raiseEvent = CriticalTypes.OnStageCrit;

                    if (raiseEvent != null)
                    {
                        raiseEvent(this, new StageCritEventArgs());
                    }
                    lastStageWasCrit = false;
                    // Debug.Log("Laststagewascrit");
                }
                float totalChance = 0f;
                /* if (body.masterObject.GetComponent<GOTCE_StatsComponent>())
                {
                    GOTCE_StatsComponent vars = body.masterObject.GetComponent<GOTCE_StatsComponent>();
                    vars.DetermineStageCrit();
                    totalChance += vars.stageCritChance;
                    Debug.Log("Chance: " + vars.stageCritChance);
                }; */

                foreach (PlayerCharacterMasterController masterController in PlayerCharacterMasterController.instances) {
                    CharacterMaster master = masterController.master;
                    if (master.gameObject.GetComponent<GOTCE_StatsComponent>()) {
                        GOTCE_StatsComponent vars = master.gameObject.GetComponent<GOTCE_StatsComponent>();
                        vars.DetermineStageCrit();
                        totalChance += vars.stageCritChance;
                    }
                }

                if (Util.CheckRoll(totalChance) && !lastStageWasCritPrev)
                {
                    lastStageWasCrit = true;
                    Run.instance.AdvanceStage(Run.instance.nextStageScene);
                }
            }
        }

        public override void Hooks()
        {
            CharacterBody.onBodyStartGlobal += Crit;
            RoR2.Run.onRunStartGlobal += (run) =>
            {
                lastStageWasCrit = false;
            };

            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) => {
                if (args.Stats && NetworkServer.active) {
                    if (args.Stats.inventory) {
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