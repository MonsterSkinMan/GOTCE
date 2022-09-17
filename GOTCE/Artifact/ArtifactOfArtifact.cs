/*using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using R2API;
using R2API.Utils;
using GOTCE.Artifact;
using MonoMod.Cil;

namespace GOTCE.Artifact
{
    public class ArtifactOfArtifact : ArtifactBase<ArtifactOfArtifact>
    {
        public override string ArtifactName => "Artifact Of Artifact";

        public override string ArtifactLangTokenName => "GOTCE_ArtifactOfLodr";

        public override string ArtifactDescription => "All players and monsters move at a quarter of their regular speed.";

        public override Sprite ArtifactEnabledIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Artifact/artifactofartifact.png");

        public override Sprite ArtifactDisabledIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Artifact/artifactofartifactDisabled.png");

        public static ArtifactDef artifact;

        public override void Init(ConfigFile config)
        {
            CreateLang();
            CreateArtifact();
            Hooks();
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += new RecalculateStatsAPI.StatHookEventHandler(ArtifactOfArtifact.SplashPotionOfSlowness);
        }

        public static void SplashPotionOfSlowness(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (RunArtifactManager.instance.IsArtifactEnabled(artifact) && body)
            {
                args.moveSpeedMultAdd += -0.75f;
            }
        }
    }
}*/
