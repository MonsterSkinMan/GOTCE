using UnityEngine;
using RoR2;
using BepInEx.Configuration;

namespace GOTCE.Artifact
{
    public class ArtifactOfWoolie : ArtifactBase<ArtifactOfWoolie>
    {
        public override string ArtifactName => "Artifact of Woolie";

        public override string ArtifactLangTokenName => "GOTCE_ArtifactOfWoolie";

        public override string ArtifactDescription => "If you take more than 5 minutes to complete a stage, you instantly fucking die. Should've rushed harder, dumbass.";

        public override Sprite ArtifactEnabledIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Artifact/woolifact.png");

        public override Sprite ArtifactDisabledIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Artifact/woolifact.png");

        public override void Init(ConfigFile config)
        {
            CreateLang();
            CreateArtifact();
            Hooks();
        }

        public override void Hooks()
        {
            On.RoR2.CharacterMaster.OnServerStageBegin += RushOrDie.Hook_OnServerStageBegin;
            On.RoR2.Run.FixedUpdate += RushOrDie.Hook_FixedUpdate;
        }

        public class RushOrDie : MonoBehaviour
        {
            public static int prevTime;
            public static void Hook_OnServerStageBegin(On.RoR2.CharacterMaster.orig_OnServerStageBegin orig, CharacterMaster self, Stage stage)
            {
                prevTime = (int)stage.entryTime.t;
                // GOTCE.Main.ModLogger.LogDebug(stage.name);
                orig(self, stage);

            }
            public static void Hook_FixedUpdate(On.RoR2.Run.orig_FixedUpdate orig, Run self)
            {
                orig(self);
                int currentTime = (int)self.time;
                switch (currentTime - prevTime)
                {
                    case int n when n == 60:
                        RoR2Application.onNextUpdate += () =>
                        {
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "One Minute has passed." } });
                        };
                        break;

                    case int n when n == 120:
                        RoR2Application.onNextUpdate += () =>
                        {
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "Two Minutes have passed." } });
                        };
                        break;

                    case int n when n == 180:
                        RoR2Application.onNextUpdate += () =>
                        {
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "Three Minutes have passed." } });
                        };
                        break;

                    case int n when n == 240:
                        RoR2Application.onNextUpdate += () =>
                        {
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "Four Minutes have passed." } });
                        };
                        break;

                    case int n when n == 270:
                        RoR2Application.onNextUpdate += () =>
                        {
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "You have 30 seconds." } });
                        };
                        break;

                    case int n when n == 290:
                        RoR2Application.onNextUpdate += () =>
                        {
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "10 seconds left." } });
                        };
                        break;

                    case int n when n == 295:
                        RoR2Application.onNextUpdate += () =>
                        {
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "5." } });
                        };
                        break;

                    case int n when n == 296:
                        RoR2Application.onNextUpdate += () =>
                        {
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "4." } });
                        };
                        break;

                    case int n when n == 297:
                        RoR2Application.onNextUpdate += () =>
                        {
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "3." } });
                        };
                        break;

                    case int n when n == 298:
                        RoR2Application.onNextUpdate += () =>
                        {
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "2." } });
                        };
                        break;

                    case int n when n == 299:
                        RoR2Application.onNextUpdate += () =>
                        {
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "1." } });
                        };
                        break;

                    case int n when n > 300:
                        for (int i = 0; i < CharacterMaster.readOnlyInstancesList.Count; i++)
                        {
                            //CharacterMaster.readOnlyInstancesList[i] is the player.
                            if (Instance.ArtifactEnabled)
                            {
                                CharacterMaster.readOnlyInstancesList[i].TrueKill();
                                prevTime = 0;
                                RoR2Application.onNextUpdate += () =>
                                {
                                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "You got outscaled, idiot." } });
                                };
                            }
                        }
                        break;
                }
            }
        }
    }
}