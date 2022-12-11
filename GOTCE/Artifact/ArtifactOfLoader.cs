using BepInEx.Configuration;
using UnityEngine;
using RoR2;
using R2API;

namespace GOTCE.Artifact
{
    public class ArtifactOfLoader : ArtifactBase<ArtifactOfLoader>
    {
        public override string ArtifactName => "Artifact Of Loader";

        public override string ArtifactLangTokenName => "GOTCE_ArtifactOfLoader";

        public override string ArtifactDescription => "All players and monsters move at a quarter of their regular speed.";

        public override Sprite ArtifactEnabledIcon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Artifacts/artifactofloaderon.png");

        public override Sprite ArtifactDisabledIcon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Artifacts/artifactofloaderoff.png");

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