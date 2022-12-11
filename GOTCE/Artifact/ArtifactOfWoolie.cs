using BepInEx.Configuration;
using UnityEngine;
using RoR2;

namespace GOTCE.Artifact
{
    public class ArtifactOfWoolie : ArtifactBase<ArtifactOfWoolie>
    {
        public override string ArtifactName => "Artifact of Woolie";

        public override string ArtifactLangTokenName => "GOTCE_ArtifactOfWoolie";

        public override string ArtifactDescription => "If you take more than 5 minutes to complete a stage, you instantly fucking die. Should've rushed harder, dumbass.";

        public override Sprite ArtifactEnabledIcon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Artifacts/artifactofwoolieon.png");

        public override Sprite ArtifactDisabledIcon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Artifacts/artifactofwoolieoff.png");
        // public override ArtifactDef artifact;

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
                if (currentTime - prevTime > 300)
                {
                    for (int i = 0; i < CharacterMaster.readOnlyInstancesList.Count; i++)
                    {
                        //CharacterMaster.readOnlyInstancesList[i] is the player.
                        if (ArtifactOfWoolie.Instance.ArtifactEnabled)
                        {
                            CharacterMaster.readOnlyInstancesList[i].TrueKill();
                            prevTime = 0;
                            RoR2Application.onNextUpdate += () =>
                            {
                                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "You got outscaled, idiot." } });
                            };
                        }
                    }
                }
            }
        }
    }
}