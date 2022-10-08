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

        public override string ItemFullDescription => "Gain a <style=cIsUtility>10%</style> <style=cStack>(+10% per stack)</style> chance to <style=cIsUtility>critically stage transition</style>, skipping the next stage.";

        public override string ItemLore => "";
        // private bool shouldCrit = true;

        private bool lastStageWasCrit = false;

        public EventHandler<StageCritEventArgs> OnStageCrit;

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.OnStageBeginEffect, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;
        private System.Random rand = new();

        public override void Init(ConfigFile config)
        {
            base.Init(config);
            // NetworkingAPI.RegisterMessageType<StageCrit>();
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
            body.gameObject.AddComponent<GOTCE_StatsComponent>();
            if (NetworkServer.active && Stage.instance.entryTime.timeSince <= 3f && body.isPlayerControlled)
            {
                bool lastStageWasCritPrev = lastStageWasCrit;

                if (lastStageWasCrit)
                {
                    // NetMessageExtensions.Send(new StageCrit(), NetworkDestination.Clients);
                    EventHandler<StageCritEventArgs> raiseEvent = OnStageCrit;

                    if (raiseEvent != null)
                    {
                        raiseEvent(this, new StageCritEventArgs());
                    }
                    lastStageWasCrit = false;
                }
                // var instances = PlayerCharacterMasterController.instances;
                float totalChance = 0f;
                // Main.ModLogger.LogDebug(Stage.instance.entryTime.timeSince);
                if (body.gameObject.GetComponent<GOTCE_StatsComponent>())
                {
                    GOTCE_StatsComponent vars = body.gameObject.GetComponent<GOTCE_StatsComponent>();
                    vars.DetermineStageCrit();
                    // Main.ModLogger.LogDebug(vars.stageCritChance + " stagecritplayer");
                    totalChance += vars.stageCritChance;
                };

                if (Util.CheckRoll(totalChance) && !lastStageWasCritPrev)
                {
                    lastStageWasCrit = true;
                    Run.instance.AdvanceStage(Run.instance.nextStageScene);
                    // NetMessageExtensions.Send(new StageCrit(), NetworkDestination.Server);
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

    /* public class StageCrit : INetMessage, ISerializableObject
    {
        public void Serialize(NetworkWriter writer)
        {
        }
        public void Deserialize(NetworkReader reader)
        {
        }
        public void OnReceived()
        {
            // do things on stage crit here
            // this will be called on first stage load but the player has no items there so it shouldnt matter
            if (NetworkServer.active) { // server stuff
                CharacterBody body = PlayerCharacterMasterController.instances[0].master.GetBody();
                if (body.inventory && body.inventory.GetItemCount(Items.Green.GrandfatherClock.Instance.ItemDef) > 0) {
                    if (body.gameObject.GetComponent<GOTCE_StatsComponent>()) {
                        body.gameObject.GetComponent<GOTCE_StatsComponent>().clockDeathCount += body.inventory.GetItemCount(Items.Green.GrandfatherClock.Instance.ItemDef);
                    }
                }
                Main.ModLogger.LogDebug("server received stagecrit");
            }
            if (!NetworkServer.active) { // client stuff
                CharacterBody body = PlayerCharacterMasterController.instances[0].master.GetBody();
                if (body.inventory && body.inventory.GetItemCount(Items.Green.GrandfatherClock.Instance.ItemDef) > 0) {
                    if (body.gameObject.GetComponent<GOTCE_StatsComponent>()) {
                        body.gameObject.GetComponent<GOTCE_StatsComponent>().clockDeathCount += body.inventory.GetItemCount(Items.Green.GrandfatherClock.Instance.ItemDef);
                    }
                }
                Main.ModLogger.LogDebug("client received stagecrit");
            }
        }
    } */
}