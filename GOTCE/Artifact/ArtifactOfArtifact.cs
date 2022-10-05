using BepInEx.Configuration;
using UnityEngine;
using RoR2;
using R2API;

namespace GOTCE.Artifact
{
    public class ArtifactOfArtifact : ArtifactBase<ArtifactOfArtifact>
    {
        public override string ArtifactName => "Artifact Of Artifact";

        public override string ArtifactLangTokenName => "GOTCE_ArtifactOfLodr";

        public override string ArtifactDescription => "All players and monsters move at a quarter of their regular speed.";

        public override Sprite ArtifactEnabledIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Artifact/artifactofartifact.png");

        public override Sprite ArtifactDisabledIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Artifact/artifactofartifactDisabled.png");

        public override void Init(ConfigFile config)
        {
            CreateLang();
            CreateArtifact();
            Hooks();
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += SplashPotionOfSlowness;
        }

        public static void SplashPotionOfSlowness(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (Instance.ArtifactEnabled && sender)
            {
                args.moveSpeedReductionMultAdd += 3f;
                // 3f is 25% move speed, or 75% actual reduction using the game's stupid formula
            }
        }
    }
}
