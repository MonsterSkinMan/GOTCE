/*using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using R2API;

namespace GOTCE.Artifact
{
    public class ArtifactOfWoolie : ArtifactBase<ArtifactOfWoolie>
    {
        public override string ArtifactName => "Artifact of Woolie";

        public override string ArtifactLangTokenName => "GOTCE_ArtifactOfWoolie";

        public override string ArtifactDescription => "If you take more than 5 minutes to complete a stage, you instantly fucking die. Should've rushed harder, dumbass.";

        public override Sprite ArtifactEnabledIcon => null;

        public override Sprite ArtifactDisabledIcon => null;

        public override void Hooks()
        {
            throw new NotImplementedException();
        }

        public override void Init(ConfigFile config)
        {
            CreateLang();
            CreateArtifact();
            Hooks();
        }

        public override void Hooks()
        {
            CharacterMaster.OnServerStageBegin  
        }

        public class RushOrDie : MonoBehaviour
        {

        }
    }
}*/
