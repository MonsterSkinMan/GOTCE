using BepInEx.Configuration;
using UnityEngine;
using RoR2;
using R2API;

namespace GOTCE.Artifact
{
    public class ArtifactOfCrunder : ArtifactBase<ArtifactOfCrunder>
    {
        public override string ArtifactName => "Artifact Of Crowdfunder";

        public override string ArtifactLangTokenName => "GOTCE_ArtifactOfCrunder";

        public override string ArtifactDescription => "Your gold passively drains.";

        public override Sprite ArtifactEnabledIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Artifact/artifactofblindnesson.png");

        public override Sprite ArtifactDisabledIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Artifact/artifactofblindnesson.png");

        public override void Init(ConfigFile config)
        {
            CreateLang();
            CreateArtifact();
            Hooks();
        }

        public override void Hooks()
        {
            On.RoR2.CharacterMaster.Start += (orig, self) => {
                orig(self);
                if (RunArtifactManager.instance.IsArtifactEnabled(ArtifactDef) && NetworkServer.active && self.playerCharacterMasterController) {
                    ExtremelyLow com = self.gameObject.GetComponent<ExtremelyLow>();
                    if (!com) {
                        self.gameObject.AddComponent<ExtremelyLow>();
                    }
                }
            };
        }

    }

    public class ExtremelyLow : MonoBehaviour {
        float baseCost = 1;
        float delay = 0.16f;
        float stopwatch = 0f;
        CharacterMaster master;
        public void Start() {
            master = gameObject.GetComponent<CharacterMaster>();
        }
        public void FixedUpdate() {
            if (NetworkServer.active) {
                stopwatch += Time.fixedDeltaTime;
                if (stopwatch >= delay) {
                    stopwatch = 0f;
                    float cost = baseCost * (TeamManager.instance.GetTeamLevel(TeamIndex.Player) * 0.25f);
                    master.money = (uint)Mathf.Max(0f, master.money - cost);
                }
            }
        }
    }
}