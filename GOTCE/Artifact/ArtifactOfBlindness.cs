using BepInEx.Configuration;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

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
        private static readonly string[] blacklistedScenes = { "artifactworld", "crystalworld", "eclipseworld", "infinitetowerworld", "intro", "loadingbasic", "lobby", "logbook", "mysteryspace", "outro", "PromoRailGunner", "PromoVoidSurvivor", "splash", "title", "voidoutro" };

        public override void Init(ConfigFile config)
        {
            ppHolder = new("GOTCE_ArtifactOfBlindnessPP");
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
            ppProfile.name = "GOTCE_ArtifactOfBlindness";
            fog = ppProfile.AddSettings<RampFog>();
            fog.SetAllOverridesTo(true);
            fog.fogColorStart.value = new Color32(45, 45, 53, 165);
            fog.fogColorMid.value = new Color32(44, 44, 56, 255);
            fog.fogColorEnd.value = new Color32(44, 44, 56, 255);
            fog.skyboxStrength.value = 0.02f;
            fog.fogPower.value = 0.35f;
            fog.fogIntensity.value = 0.994f;
            fog.fogZero.value = 0f;
            fog.fogOne.value = 0.05f;

            pp.sharedProfile = ppProfile;
            CreateLang();
            CreateArtifact();
            Hooks();
        }

        public override void Hooks()
        {
            On.RoR2.SceneDirector.Start += SceneDirector_Start;
            Run.onRunDestroyGlobal += Run_onRunDestroyGlobal;
        }

        private void Run_onRunDestroyGlobal(Run obj)
        {
            var ppVolume = ppHolder.GetComponent<PostProcessVolume>();
            var sceneName = SceneManager.GetActiveScene().name;
            if (!blacklistedScenes.Contains(sceneName))
            {
                ppVolume.weight = 0f;
            }
        }

        private void SceneDirector_Start(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            var ppVolume = ppHolder.GetComponent<PostProcessVolume>();

            var sceneName = SceneManager.GetActiveScene().name;
            if (Instance.ArtifactEnabled && !blacklistedScenes.Contains(sceneName))
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