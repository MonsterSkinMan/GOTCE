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
        public Color crackedOrange = new Color32(230, 75, 19, 255);
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

            EclipseDifficultyMedalDisplay[] displays = GameObject.FindObjectsOfType<EclipseDifficultyMedalDisplay>();
            for (int i = 0; i < displays.Length; i++) {
                int c = displays[i].eclipseLevel;
                GameObject the = displays[i].gameObject;
                GameObject.Destroy(displays[i]);
                CrackclipseMedalController controller = the.AddComponent<CrackclipseMedalController>();
                controller.level = c;
            }
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
            StringBuilder sb = new();
            sb.Append("(1) - You dream of quiet snowfall...");
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_TITLE", "Crackclipse");
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_DESCRIPTION", "Climb through another 8 stacking challenges! This time even more stupid.");
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_START_NAME", "Suffer");
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_1_TITLE", "Crackclipse (1)");
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_1_DESC", sb.ToString());
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_2_TITLE", "Crackclipse (2)");
            sb.Append("\n(2) - Bad sax overwhelms you...");
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_2_DESC", sb.ToString());
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_3_TITLE", "Crackclipse (3)");
            sb.Append("\n(3) - Enemy spawns are <style=cDeath>completely random</style>...");
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_3_DESC", sb.ToString());
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_MENU_TITLE", "Crackclipse");
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_MENU_HOVER", "Endure 8 stacking difficulty modifiers, each more bullshit than the last!.");

            SceneManager.activeSceneChanged += OnSceneChanged;
        }
    }
    
    public class CrackclipseMedalController : MonoBehaviour {
        public int level = 0;
        public Image image => GetComponent<Image>();
        public Sprite sprite => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Misc/smirk_cat.png");
        bool hasRefreshedAgain = false;
        public void OnEnable() {
            image.sprite = sprite;
            UserProfile.onSurvivorPreferenceChangedGlobal += Refresh;
            image.color = new Color32(9, 9, 9, 255);
            LocalUser user = LocalUserManager.GetFirstLocalUser();
            Refresh(user.userProfile);
        }

        public void FixedUpdate() {
            if (level != 0 && !hasRefreshedAgain) {
                LocalUser user = LocalUserManager.GetFirstLocalUser();
                Refresh(user.userProfile);
                hasRefreshedAgain = true;
            }
        }

        public void OnDisable() {
            UserProfile.onSurvivorPreferenceChangedGlobal -= Refresh;
        }
        public void Refresh(UserProfile profile) {
            LocalUser user = LocalUserManager.GetFirstLocalUser();
            int highest = CrackclipseRun.HighestCrackclipseLevel(user, user.userProfile.survivorPreference);
            if (highest >= level) {
                image.color = Color.yellow;
            }
            else {
                image.color = new Color32(9, 9, 9, 255);
            }
        }
    }
}