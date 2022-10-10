using BepInEx.Configuration;
using RoR2;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace GOTCE.Artifact
{
    internal class ArtifactOfBlindness : ArtifactBase<ArtifactOfBlindness>
    {
        public override string ArtifactName => "Artifact of Blindness";

        public override string ArtifactLangTokenName => "GOTCE_ArtifactOfBlindness";

        public override string ArtifactDescription => "The fog is coming";

        public override Sprite ArtifactEnabledIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/zanysoup.png");

        public override Sprite ArtifactDisabledIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/GrandfatherClock.png");

        public static RampFog fog;
        public static GameObject ppHolder;

        public override void Init(ConfigFile config)
        {
            ppHolder = new("PPInferno");
            Object.DontDestroyOnLoad(ppHolder);
            ppHolder.layer = LayerIndex.postProcess.intVal;
            ppHolder.AddComponent<GOTCE_ArtifactOfBlindnessPostProcessingController>();
            PostProcessVolume pp = ppHolder.AddComponent<PostProcessVolume>();
            Object.DontDestroyOnLoad(pp);
            pp.isGlobal = true;
            pp.weight = 0f;
            pp.priority = float.MaxValue - 1;
            PostProcessProfile ppProfile = ScriptableObject.CreateInstance<PostProcessProfile>();
            Object.DontDestroyOnLoad(ppProfile);
            ppProfile.name = "ppInferno";
            fog = ppProfile.AddSettings<RampFog>();
            fog.SetAllOverridesTo(true);
            fog.fogColorStart.value = new Color32(45, 45, 53, 165);
            fog.fogColorMid.value = new Color32(44, 44, 56, 255);
            fog.fogColorEnd.value = new Color32(44, 44, 56, 255);
            fog.skyboxStrength.value = 0.02f;
            fog.fogZero.value = -0.02f;
            fog.fogOne.value = 0.03f;

            pp.sharedProfile = ppProfile;
            CreateLang();
            CreateArtifact();
            Hooks();
        }

        public override void Hooks()
        {
            On.RoR2.SceneDirector.Start += SceneDirector_Start;
        }

        private void SceneDirector_Start(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            var ppVolume = ppHolder.GetComponent<PostProcessVolume>();
            if (Instance.ArtifactEnabled)
            {
                ppVolume.weight = 1f;
            }
            else
            {
                ppVolume.weight = 0f;
            }
            orig(self);
        }
    }

    public class GOTCE_ArtifactOfBlindnessPostProcessingController : MonoBehaviour
    {
        public PostProcessVolume volume;

        public void Start()
        {
            volume = GetComponent<PostProcessVolume>();
        }
    }
}