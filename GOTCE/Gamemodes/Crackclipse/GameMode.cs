using RoR2;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using RoR2.ContentManagement;

namespace GOTCE.Gamemodes.Crackclipse {
    public class GameMode {
        public static GameObject CrackclipsePrefab;
        public static PostProcessProfile PP;
        
        public static void Create() {
            CrackclipsePrefab = PrefabAPI.InstantiateClone(new("CrackclipseRun"), "CrackclipseRun");
            GameObject classic = Utils.Paths.GameObject.ClassicRun.Load<GameObject>();
            Run cRun = classic.GetComponent<Run>();
            CrackclipseRun run = CrackclipsePrefab.AddComponent<CrackclipseRun>();
            GameObject lobbyPrefab = PrefabAPI.InstantiateClone(cRun.lobbyBackgroundPrefab, "CrackclipseLobbyPrefab");
            GameObject ppHolder = new("ppHolder");
            ppHolder.transform.parent = lobbyPrefab.transform;

            PostProcessVolume pp = ppHolder.AddComponent<PostProcessVolume>();
            pp.priority = 3;
            pp.isGlobal = true;
            pp.weight = 1;
            ppHolder.layer = LayerIndex.postProcess.intVal;

            PP = ScriptableObject.CreateInstance<PostProcessProfile>();
            
            /*Vignette v = PP.AddSettings<Vignette>();
            v.color.value = new Color(230, 75, 19, 255);
            v.center.value = new Vector2(0.5f, 0.5f);
            v.enabled.value = true;
            v.active = true;
            v.mode.value = VignetteMode.Classic;
            v.rounded.value = false;
            v.intensity.value = 30;
            v.SetAllOverridesTo(true);

            Bloom b = PP.AddSettings<Bloom>();
            b.color.value = new Color(230, 75, 19, 255);
            b.active = true;
            b.enabled.value = true;
            b.intensity.value = 5f;
            b.threshold.value = 0f;
            b.softKnee.value = 0.2f;
            b.SetAllOverridesTo(true);

            pp.sharedProfile = PP;
            pp.profile = PP; */
            
            run.userPickable = true;
            run.nameToken = "GOTCE_CRACKRUN_NAME";
            run.lobbyBackgroundPrefab = lobbyPrefab;
            run.uiPrefab = cRun.uiPrefab;
            run.gameOverPrefab = cRun.gameOverPrefab;
            CrackclipsePrefab.AddComponent<NetworkRuleBook>();
            CrackclipsePrefab.AddComponent<TeamManager>();
            CrackclipsePrefab.AddComponent<RunCameraManager>();
            LanguageAPI.Add("GOTCE_CRACKRUN_NAME", "Crackclipse");

            ContentAddition.AddGameMode(CrackclipsePrefab);

            CrackclipseUI.Initialize();

            CrackclipseRun.defToLevel = new();

            Difficulty.Create();
        }
        /*[SystemInitializer(typeof(SurvivorCatalog))]
        public static void GenerateUnlockableDefsForSurvivors() {
            List<UnlockableDef> defs = ContentManager._unlockableDefs.ToList();
            foreach (SurvivorDef def in SurvivorCatalog.survivorDefs) {
                int min = 1;
                int max = 9;
                string prefix = "Crackclipse.";
                for (int i = min; i < max; i++) {
                    // Debug.Log($"Adding crackclipse level {i} for survivor {def.cachedName}");
                    string unlock = prefix + def.cachedName + "." + i.ToString();
                    Debug.Log(unlock);
                    UnlockableDef udef = ScriptableObject.CreateInstance<UnlockableDef>();
                    udef.hidden = false;
                    udef.cachedName = unlock;

                    ContentAddition.AddUnlockableDef(udef);
                    defs.Add(udef);
                }
            }
            ContentManager._unlockableDefs = defs.ToArray();
            UnlockableCatalog.SetUnlockableDefs(ContentManager.unlockableDefs);
        }*/
    }
}