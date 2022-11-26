using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using UnityEngine.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;

namespace GOTCE.Items.NoTier
{
    public class ViendAltPassive : ItemBase<ViendAltPassive>
    {
        public override string ConfigName => "ViendAltPassive";

        public override string ItemName => "ViendAltPassive";

        public override string ItemLangTokenName => "GOTCE_ViendAltPassive";

        public override string ItemPickupDesc => "bruh";

        public override string ItemFullDescription => "Wake up\r\nGet out\r\nCan you feel the fire rush\r\nWeakened foundation\r\nLosing sensation\r\nOur homes destroyed\r\nThere's nowhere left to go\r\nWe took our love and we made it all destructive\r\nSo the flame burns on\r\nSwallowing all of our shrines and doubts\r\nDeath knows it won't be long\r\nI lived in fear until I found\r\nLove and pride\r\nMove aside\r\nI'm burning too many bridges to prove that I'm alive\r\nPick a side to defy\r\nI'm burning too many bridges to prove that I'm alive\r\nSo we stand alone in the darkness\r\nWith the doubt buried deep inside of us\r\nPray tell this is not enough\r\nMy mind's in the deepest rut\r\nWake up get out\r\nCan you feel it rush\r\nI pray for only bliss\r\nWeakened foundation losing sensation\r\nCan you feel the rush\r\nI no longer feel the light\r\nFinds me when I close my eyes\r\nSo alone in dire times\r\nI need you here to look inside\r\nLove and pride\r\nMove aside\r\nI'm burning too many bridges to prove that I'm alive\r\nPick a side to defy\r\nI'm burning too many bridges to prove that I'm alive\r\nLet me drown\r\nLet me suffer on my terms\r\nTake me alive\r\nStuck in my head\r\nStuck in my head\r\nStuck in my head\r\nHelp me find my way\r\nI want my questions answered\r\nI want to thrive\r\nI want to thrive\r\nI want to use my anger\r\nI want to know what this is worth\r\nI want my questions answered\r\nI want to thrive\r\nI want to thrive\r\nI want to use my anger\r\nI want to know what this is worth\r\nLove and pride\r\nMove aside\r\nI'm burning too many bridges to prove that I'm alive\r\nPick a side to defy\r\nI'm burning too many bridges to prove that I'm alive\r\nLove and pride\r\nMove aside\r\nI'm burning too many bridges to prove that I'm alive\r\nPick a side to defy\r\nI'm burning too many bridges to prove that I'm alive\r\nSo we come to the roundabout\r\nRunning in circles\r\nI will break";
        public override bool Hidden => true;
        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.NoTier;

        public override Enum[] ItemTags => new Enum[] { ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

        public static float extraDifficultyTime = 0f;

        public static bool calculatingDifficultyCoefficient = false;

        public static int totalItemCount = 0;

        public static float onlineSyncTimer = 0f;

        public static float onlineSyncDuration = 60f;

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
            On.RoR2.Run.Start += Run_Start;
            On.RoR2.Run.RecalculateDifficultyCoefficentInternal += Run_RecalculateDifficultyCoefficentInternal;
            On.RoR2.Run.GetRunStopwatch += Run_GetRunStopwatch;
            On.RoR2.Run.FixedUpdate += Run_FixedUpdate;
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
        }

        private void Run_Start(On.RoR2.Run.orig_Start orig, Run self)
        {
            orig(self);
            extraDifficultyTime = 0f;
        }

        private void Run_RecalculateDifficultyCoefficentInternal(On.RoR2.Run.orig_RecalculateDifficultyCoefficentInternal orig, Run self)
        {
            calculatingDifficultyCoefficient = true;
            orig(self);
            calculatingDifficultyCoefficient = false;
        }

        private float Run_GetRunStopwatch(On.RoR2.Run.orig_GetRunStopwatch orig, Run self)
        {
            return orig(self) + (calculatingDifficultyCoefficient ? extraDifficultyTime : 0);
        }

        private void Run_FixedUpdate(On.RoR2.Run.orig_FixedUpdate orig, Run self)
        {
            orig(self);
            if (totalItemCount > 0)
            {
                if (!self.isRunStopwatchPaused)
                    extraDifficultyTime += Time.fixedDeltaTime * 0.5f;

                if (NetworkServer.active)
                {
                    onlineSyncTimer -= Time.fixedDeltaTime;
                    if (onlineSyncTimer <= 0f)
                    {
                        onlineSyncTimer = onlineSyncDuration;
                        new SyncTimer(extraDifficultyTime).Send(NetworkDestination.Clients);
                    }
                }
            }
        }

        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);

            int newTotalItemCount = 0;
            for (int team = 0; team < (int)TeamIndex.Count; team++)
            {
                newTotalItemCount += Util.GetItemCountForTeam((TeamIndex)team, this.ItemDef.itemIndex, true);
            }
            if (totalItemCount > 0 && newTotalItemCount == 0 && NetworkServer.active)
            {
                new SyncTimer(extraDifficultyTime).Send(NetworkDestination.Clients);
            }
            totalItemCount = newTotalItemCount;
        }

        public class SyncTimer : INetMessage
        {
            private float time;

            public SyncTimer()
            {
            }

            public SyncTimer(float time)
            {
                this.time = time;
            }

            public void Deserialize(NetworkReader reader)
            {
                time = reader.ReadSingle();
            }

            public void OnReceived()
            {
                if (NetworkServer.active) return;
                extraDifficultyTime = time;
            }

            public void Serialize(NetworkWriter writer)
            {
                writer.Write(time);
            }
        }
    }
}