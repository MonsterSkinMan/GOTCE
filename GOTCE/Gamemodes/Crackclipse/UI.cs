using Unity;
using RoR2;
using RoR2.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using TMPro;
using R2API;
using System.Text;

namespace GOTCE.Gamemodes.Crackclipse {
    [RequireComponent(typeof(MPEventSystemLocator))]
    public class CrackclipseMenuController : MonoBehaviour {
        public LanguageTextMeshController survivorName;
        public LanguageTextMeshController title;
        public LanguageTextMeshController description;
        public LanguageTextMeshController eclipseLevelTitle;
        public LanguageTextMeshController eclipseDescription;
        public Dictionary<SurvivorDef, List<UnlockableDef>> survivorsToCrackclipse = new();
        public GameObject button;
        public GameObject difficultyBadges;
        public Color crackedOrange = new Color(230, 75, 19, 10);
        public LocalUser user;
        public void StartCrackclipse()
        {
            Debug.Log("starting CrackclipseRun");
            RoR2.Console.instance.SubmitCmd(null, "transition_command \"gamemode CrackclipseRun; host 0;\"");
        }

        public void Setup() {
            title.token = "GOTCE_CRACKCLIPSE_TITLE";
            title.GetComponent<HGTextMeshProUGUI>().color = crackedOrange;
            description.token = "GOTCE_CRACKCLIPSE_DESCRIPTION";
            description.GetComponent<HGTextMeshProUGUI>().color = crackedOrange;
            button.GetComponent<LanguageTextMeshController>().token = "GOTCE_CRACKCLIPSE_START_NAME";
            button.GetComponent<Image>().color = crackedOrange;
                
            button.GetComponent<HGButton>().onClick.AddListener(delegate {
                StartCrackclipse();
            });

            LocalUser user = LocalUserManager.GetFirstLocalUser();
            UserProfile profile = user.userProfile;

            if (profile.survivorPreference) {
                SurvivorDef def = profile.survivorPreference;
                survivorName.token = def.displayNameToken;

                eclipseLevelTitle.token = $"GOTCE_CRACKCLIPSE_{CrackclipseRun.HighestCrackclipseLevel(user, def)}_TITLE";
                eclipseDescription.token = $"GOTCE_CRACKCLIPSE_{CrackclipseRun.HighestCrackclipseLevel(user, def)}_DESC";
            }

            user.userProfile.onSurvivorPreferenceChanged += UpdateSurvivors;
        }

        private void UpdateSurvivors() {
            LocalUser user = LocalUserManager.GetFirstLocalUser();
            UserProfile profile = user.userProfile;

            if (profile.survivorPreference) {
                SurvivorDef def = profile.survivorPreference;
                survivorName.token = def.displayNameToken;

                eclipseLevelTitle.token = $"GOTCE_CRACKCLIPSE_{CrackclipseRun.HighestCrackclipseLevel(user, def)}_TITLE";
                eclipseDescription.token = $"GOTCE_CRACKCLIPSE_{CrackclipseRun.HighestCrackclipseLevel(user, def)}_DESC";
            }
        }
    }
    
    public class CrackclipseUI {
        private static bool shouldReplace = false;

        private static void OnSceneChanged(Scene prev, Scene next) {
            if (shouldReplace && next.name == "eclipseworld") {
                GameObject[] objects = next.GetRootGameObjects();
                foreach (GameObject gameObject in objects) {
                    EclipseRunScreenController comp = gameObject.GetComponentInChildren<EclipseRunScreenController>();
                    if (comp) {
                        SetupMenu(comp);
                    }
                }
            }

            if (next.name == "title") {
                GameObject gameObject = GameObject.Find("MainMenu");
                Transform transform = gameObject.transform.Find("MENU: Extra Game Mode/ExtraGameModeMenu/Main Panel/GenericMenuButtonPanel/JuicePanel/GenericMenuButton (Eclipse)");
                if (transform)
                {
                    GameObject button = UnityEngine.Object.Instantiate(transform.gameObject, transform.parent);
                    button.GetComponent<LanguageTextMeshController>().token = "GOTCE_CRACKCLIPSE_MENU_TITLE";
                    ConsoleFunctions consoleFunctions = button.GetComponent<ConsoleFunctions>();
                    HGButton component = button.GetComponent<HGButton>();
                    component.hoverToken = "GOTCE_CRACKCLIPSE_MENU_HOVER";
                    component.onClick.AddListener(delegate
                    {
                        consoleFunctions.SubmitCmd("transition_command \"set_scene eclipseworld\"");
                        SetOverride(true);
                    });
                    HGButton component2 = transform.parent.Find("GenericMenuButton (Eclipse)").GetComponent<HGButton>();
                    component2.onClick.AddListener(delegate
                    {
                        SetOverride(false);
                    });
                }
                else {
                    Debug.Log("Could not find transform.");
                }
            }
        }

        public static void SetOverride(bool val) {
            shouldReplace = val;
        }

        private static void SetupMenu(EclipseRunScreenController eclipseRunScreenController) {
            CrackclipseMenuController crackMenu = eclipseRunScreenController.gameObject.AddComponent<CrackclipseMenuController>();
            crackMenu.enabled = false;
            crackMenu.survivorName = eclipseRunScreenController.survivorName;
            crackMenu.eclipseLevelTitle = eclipseRunScreenController.eclipseDifficultyName;
            crackMenu.eclipseDescription = eclipseRunScreenController.eclipseDifficultyDescription;
            crackMenu.title = GameObject.Find("MENU").transform.Find("EclipseRunMenu").Find("Main Panel").Find("Title").GetComponent<LanguageTextMeshController>();
            crackMenu.description = GameObject.Find("MENU").transform.Find("EclipseRunMenu").Find("Main Panel").Find("GenericMenuButtonPanel").Find("Description").GetComponent<LanguageTextMeshController>();
            crackMenu.button = GameObject.Find("MENU").transform.Find("EclipseRunMenu").Find("Main Panel").Find("GenericMenuButtonPanel").Find("JuicePanel").Find("GenericMenuButton (Start)").gameObject;
            crackMenu.difficultyBadges = GameObject.Find("MENU").transform.Find("EclipseRunMenu").Find("Main Panel").Find("RightPanel").Find("MedalPanel").Find("HorizontalLayout").gameObject;

            GameObject.DestroyImmediate(eclipseRunScreenController);
            crackMenu.enabled = true;
            crackMenu.Setup();
        }

        public static void Initialize() {
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_TITLE", "Crackclipse");
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_DESCRIPTION", "Climb through another 8 stacking challenges! This time even more stupid.");
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_START_NAME", "Suffer");
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_1_TITLE", "Crackclipse (1)");
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_1_DESC", "(1) - You dream of quiet snowfall...");
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_2_TITLE", "Crackclipse (2)");
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_2_DESC", "(1) - You dream of quiet snowfall...\n(2) - Bad sax overwhelms you...");
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_MENU_TITLE", "Crackclipse");
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_MENU_HOVER", "Lol, lmao.");

            SceneManager.activeSceneChanged += OnSceneChanged;
        }
    }

}