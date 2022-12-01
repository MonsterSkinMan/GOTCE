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
        public Color crackedOrange = Color.red;
        public LocalUser user;
        public void StartCrackclipse()
        {
            RoR2.Console.instance.SubmitCmd(null, "transition_command \"gamemode CracklipseRun; host 0;\"");
        }

        public void Setup() {
            title.token = "GOTCE_CRACKCLIPSE_TITLE";
            title.GetComponent<HGTextMeshProUGUI>().color = crackedOrange;
            description.token = "GOTCE_CRACKCLIPSE_DESCRIPTION";
            description.GetComponent<HGTextMeshProUGUI>().color = crackedOrange;
            button.GetComponent<LanguageTextMeshController>().token = "GOTCE_CRACKCLIPSE_START_NAME";
            button.GetComponent<Image>().color = crackedOrange;

            foreach (Image image in difficultyBadges.GetComponentsInChildren<Image>()) {
                GameObject.DestroyImmediate(image.gameObject.GetComponent<EclipseDifficultyMedalDisplay>());
                image.gameObject.AddComponent<CrackclipseDifficultyMedalDisplay>();
                image.gameObject.GetComponent<CrackclipseDifficultyMedalDisplay>().crack = gameObject.GetComponent<CrackclipseMenuController>();
                image.gameObject.GetComponent<CrackclipseDifficultyMedalDisplay>().Refresh(UserProfile.defaultProfile);
            }
                
            button.GetComponent<HGButton>().onClick.AddListener(delegate {
                StartCrackclipse();
            });

            UserProfile profile = UserProfile.defaultProfile;

            if (profile.survivorPreference) {
                SurvivorDef def = profile.survivorPreference;
                survivorName.token = def.displayNameToken;

                eclipseLevelTitle.token = $"GOTCE_CRACKCLIPSE_{GetLocalUserSurvivorCompletedCrackclipseLevel(profile, def)}_TITLE";
                eclipseDescription.token = $"GOTCE_CRACKCLIPSE_{GetLocalUserSurvivorCompletedCrackclipseLevel(profile, def)}_DESC";
            }

            UserProfile.onSurvivorPreferenceChangedGlobal += UpdateSurvivors;
        }

        public void UpdateSurvivors(UserProfile profile) {
            if (profile.survivorPreference) {
                SurvivorDef def = profile.survivorPreference;
                survivorName.token = def.displayNameToken;

                eclipseLevelTitle.token = $"GOTCE_CRACKCLIPSE_{GetLocalUserSurvivorCompletedCrackclipseLevel(profile, def)}_TITLE";
                eclipseDescription.token = $"GOTCE_CRACKCLIPSE_{GetLocalUserSurvivorCompletedCrackclipseLevel(profile, def)}_DESC";
            }
        }

        private List<UnlockableDef> GetCrackclipseUnlockables(SurvivorDef survivor) {
            if (!survivorsToCrackclipse.TryGetValue(survivor, out var value)) {
                value = new();
                survivorsToCrackclipse[survivor] = value;
                if (BodyCatalog.GetBodyName(BodyCatalog.FindBodyIndex(survivor.bodyPrefab)) != null) {
                    int min = 1;
                    StringBuilder builder = HG.StringBuilderPool.RentStringBuilder();
                        
                    while (true) {
                        builder.Clear();
                        builder.Append("Crackclipse.").Append(survivor.cachedName).Append(".").AppendInt(min);
                        UnlockableDef def = UnlockableCatalog.GetUnlockableDef(builder.ToString());
                        if (!def) {
                            break;
                        }
                        value.Add(def);
                        min++;
                    }
                    HG.StringBuilderPool.ReturnStringBuilder(builder);
                }
            }
            return value;
        }

        public int GetLocalUserSurvivorCompletedCrackclipseLevel(UserProfile userProfile, SurvivorDef survivorDef)
        {
            List<UnlockableDef> crackclipseLevelUnlockablesForSurvivor = GetCrackclipseUnlockables(survivorDef);
            int num = 1;
            for (int i = 0; i < crackclipseLevelUnlockablesForSurvivor.Count && userProfile.HasUnlockable(crackclipseLevelUnlockablesForSurvivor[i]); i++)
            {
                num = 1 + i;
            }
            return Mathf.Clamp(num - 1, 1, 8);
        }
    }
    
    public class Crackclipse {

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
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_MENU_TITLE", "Crackclipse");
            LanguageAPI.Add("GOTCE_CRACKCLIPSE_MENU_HOVER", "Lol, lmao.");

            SceneManager.activeSceneChanged += OnSceneChanged;
        }
    }

    public class CrackclipseDifficultyMedalDisplay : MonoBehaviour
    {
        private int crackclipseLevel = 1;
        private Image iconImage => gameObject.GetComponent<Image>();
        private Sprite unearnedSprite = Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Prefabs/Survivors/Crackmando/SkillFamilies/rounder.png");
        private Sprite incompleteSprite = Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Prefabs/Survivors/Crackmando/SkillFamilies/nader.png");
        private Sprite completeSprite = Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Prefabs/Survivors/Crackmando/SkillFamilies/double.png");
        public CrackclipseMenuController crack;

        public void Setup()
        {
            UserProfile.onSurvivorPreferenceChangedGlobal += OnSurvivorPreferenceChangedGlobal;
            Refresh(UserProfile.defaultProfile);
        }

        private void OnDisable()
        {
            UserProfile.onSurvivorPreferenceChangedGlobal -= OnSurvivorPreferenceChangedGlobal;
        }

        private void OnSurvivorPreferenceChangedGlobal(UserProfile userProfile)
        {
            Refresh(userProfile);
        }

        public void Refresh(UserProfile profile)
        {
            SurvivorDef survivorDef = profile.GetSurvivorPreference();
            if (!survivorDef)
            {
                return;
            }
            int localUserSurvivorCompletedEclipseLevel = crack.GetLocalUserSurvivorCompletedCrackclipseLevel(profile, survivorDef);
            if (crackclipseLevel <= localUserSurvivorCompletedEclipseLevel)
            {
                bool flag = true;
                foreach (SurvivorDef orderedSurvivorDef in SurvivorCatalog.orderedSurvivorDefs)
                {
                    if (ShouldDisplaySurvivor(orderedSurvivorDef, profile))
                    {
                        localUserSurvivorCompletedEclipseLevel = crack.GetLocalUserSurvivorCompletedCrackclipseLevel(profile, orderedSurvivorDef);
                        if (localUserSurvivorCompletedEclipseLevel < crackclipseLevel)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    iconImage.sprite = completeSprite;
                }
                else
                {
                    iconImage.sprite = incompleteSprite;
                }
            }
            else
            {
                iconImage.sprite = unearnedSprite;
            }
        }

        private bool ShouldDisplaySurvivor(SurvivorDef survivorDef, UserProfile profile)
        {
            if ((bool)survivorDef && !survivorDef.hidden)
            {
                return survivorDef.CheckUserHasRequiredEntitlement(LocalUserManager.GetFirstLocalUser());
            }
            return false;
        }
    }
}