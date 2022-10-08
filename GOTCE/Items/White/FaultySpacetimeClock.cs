using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using UnityEngine.Networking;
using GOTCE.Components;
using R2API.Networking;
using R2API.Networking.Interfaces;

namespace GOTCE.Items.White
{
    public class FaultySpacetimeClock : ItemBase<FaultySpacetimeClock>
    {
        public override string ConfigName => "Faulty Spacetime Clock";

        public override string ItemName => "Faulty Spacetime Clock";

        public override string ItemLangTokenName => "GOTCE_FaultySpacetimeClock";

        public override string ItemPickupDesc => "Gain a chance to critically Stage Transition, skipping the next stage and unlocking powerful synergies...";

        public override string ItemFullDescription => "Gain a 10% (+10% per stack) chance to critically stage transition, skipping the next stage.";

        public override string ItemLore => "";
        private bool shouldCrit = true;

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.OnStageBeginEffect, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;
        private System.Random rand = new System.Random();

        public override void Init(ConfigFile config)
        {
            base.Init(config);
            NetworkingAPI.RegisterMessageType<StageCrit>();
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            CharacterBody.onBodyStartGlobal += Crit;
        }

        public void Crit(CharacterBody body)
        { // there is a 99% chance this code is horrible
            body.gameObject.AddComponent<GOTCE_StatsComponent>();
            if (NetworkServer.active && Stage.instance.entryTime.timeSince <= 300f && body.isPlayerControlled)
            {
                shouldCrit = !shouldCrit;
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
                /* foreach (PlayerCharacterMasterController playerCharacterMaster in instances)
                {
                    if (playerCharacterMaster.master.GetBody() && playerCharacterMaster.master.GetBody().gameObject.GetComponent<GOTCE_StatsComponent>()) {
                        GOTCE_StatsComponent vars = playerCharacterMaster.master.GetBody().gameObject.GetComponent<GOTCE_StatsComponent>();
                        Main.ModLogger.LogDebug(vars.stageCritChance+ " stagecritplayer");
                        totalChance += vars.stageCritChance;
                    }
                } */

                // Main.ModLogger.LogDebug(totalChance);

                if (rand.Next(0, 101) < totalChance && shouldCrit)
                {
                    Run.instance.AdvanceStage(Run.instance.nextStageScene);
                    NetMessageExtensions.Send(new StageCrit(), NetworkDestination.Clients);
                    // NetMessageExtensions.Send(new StageCrit(), NetworkDestination.Server);
                }
            }
        }
    }

    public class StageCrit : INetMessage, ISerializableObject
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
        }
    }
}