using RoR2;
using R2API;
using UnityEngine;
using System.Text;
using RoR2.UI;
using System.Linq;
using System.Reflection;
using System.Xml.Schema;

namespace GOTCE.Gamemodes.Crackclipse {
    public class Difficulty {
        public static DifficultyIndex c1;
        public static DifficultyIndex c2;
        public static DifficultyIndex c3;
        public static DifficultyIndex c4;
        public static DifficultyIndex c5;
        public static DifficultyIndex c6;
        public static DifficultyIndex c7;
        public static DifficultyIndex c8;

        public static void Create() {
            Sprite tmp = Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Misc/smirk_cat.png");
            CreateDifficulty(tmp, ref c1, "CRACKCLIPSE_1_NAME", 1, "CRACKCLIPSE_1_DESC");
            CreateDifficulty(tmp, ref c2, "CRACKCLIPSE_2_NAME", 2, "CRACKCLIPSE_2_DESC");
            CreateDifficulty(tmp, ref c3, "CRACKCLIPSE_3_NAME", 3, "CRACKCLIPSE_3_DESC");
            CreateDifficulty(tmp, ref c4, "CRACKCLIPSE_4_NAME", 4, "CRACKCLIPSE_4_DESC");
            CreateDifficulty(tmp, ref c5, "CRACKCLIPSE_5_NAME", 5, "CRACKCLIPSE_5_DESC");
            CreateDifficulty(tmp, ref c6, "CRACKCLIPSE_6_NAME", 6, "CRACKCLIPSE_6_DESC");
            CreateDifficulty(tmp, ref c7, "CRACKCLIPSE_7_NAME", 7, "CRACKCLIPSE_7_DESC");
            CreateDifficulty(tmp, ref c8, "CRACKCLIPSE_8_NAME", 8, "CRACKCLIPSE_8_DESC");
            LanguageAPI.Add("CRACKCLIPSE_1_NAME", "Crackclipse (1)");
            LanguageAPI.Add("CRACKCLIPSE_2_NAME", "Crackclipse (2)");
            LanguageAPI.Add("CRACKCLIPSE_3_NAME", "Crackclipse (3)");
            LanguageAPI.Add("CRACKCLIPSE_4_NAME", "Crackclipse (4)");
            LanguageAPI.Add("CRACKCLIPSE_5_NAME", "Crackclipse (5)");
            LanguageAPI.Add("CRACKCLIPSE_6_NAME", "Crackclipse (6)");
            LanguageAPI.Add("CRACKCLIPSE_7_NAME", "Crackclipse (7)");
            LanguageAPI.Add("CRACKCLIPSE_8_NAME", "Crackclipse (8)");
            Debug.Log("c1: " + (int)c1);
            Debug.Log("c2: " + (int)c2);
            Debug.Log("c3: " + (int)c3);
            Debug.Log("c4: " + (int)c4);
            Debug.Log("c5: " + (int)c5);
            Debug.Log("c6: " + (int)c6);
            Debug.Log("c7: " + (int)c7);
            Debug.Log("c8: " + (int)c8);

            StringBuilder sb = new();
            sb.Append("- You dream of quiet snowfall...");
            LanguageAPI.Add("CRACKCLIPSE_1_DESC", sb.ToString());
            sb.Append("\n- Bad sax overwhelms you...");
            LanguageAPI.Add("CRACKCLIPSE_2_DESC", sb.ToString());
            sb.Append("\n- Enemy spawns are <style=cDeath>completely random</style>");
            LanguageAPI.Add("CRACKCLIPSE_3_DESC", sb.ToString());
            sb.Append("\n- The voices are <style=cDeath>getting louder</style>...");
            LanguageAPI.Add("CRACKCLIPSE_4_DESC", sb.ToString());
            sb.Append("\n- The fog is coming.");
            LanguageAPI.Add("CRACKCLIPSE_5_DESC", sb.ToString());
            sb.Append("\n- Enemies <style=cDeath>steal your items</style>.");
            LanguageAPI.Add("CRACKCLIPSE_6_DESC", sb.ToString());
            sb.Append("\n- The fate of the universe rests on a <style=cDeath>coin flip</style>.");
            LanguageAPI.Add("CRACKCLIPSE_7_DESC", sb.ToString());
            sb.Append("\n- The skeleton appears.");
            LanguageAPI.Add("CRACKCLIPSE_8_DESC", sb.ToString());

            On.RoR2.MusicController.PickCurrentTrack += BadSax;
            On.RoR2.Run.OverrideRuleChoices += DisableIcons;
            On.RoR2.CombatDirector.AttemptSpawnOnTarget += TrueDisso;
            On.RoR2.CombatDirector.OnEnable += BadSax2;
            On.RoR2.CharacterBody.Start += TheFog;
            On.RoR2.GlobalEventManager.OnHitEnemy += StealItem;
            On.RoR2.PlayerCharacterMasterController.Update += TheVoices;
            On.RoR2.CharacterBody.Start += SDPGetsAC;
        }

        private class SpareDronePartsYouWantYourDronesToTankNotDealDamage : MonoBehaviour {
            private float stopwatch = 0f;
            public bool shouldInvert = false;

            public void Start() {
                Util.PlaySound("Play_woolieList", base.gameObject);
            }

            public void FixedUpdate() {
                stopwatch += Time.fixedDeltaTime;
                if (stopwatch >= 20f) {
                    stopwatch = 0f;
                    shouldInvert = !shouldInvert;
                    Camera.main.aspect *= -1f;
                }
            }
        }

        private static void SDPGetsAC(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self)
        {
            orig(self);

            if (Run.instance && IsCurrentDifHigherOrEqual(c4, Run.instance)) {
                self.gameObject.AddComponent<SpareDronePartsYouWantYourDronesToTankNotDealDamage>();
            }
        }

        private static void TheVoices(On.RoR2.PlayerCharacterMasterController.orig_Update orig, PlayerCharacterMasterController self)
        {
            orig(self);

            if (self.body && Run.instance && IsCurrentDifHigherOrEqual(c4, Run.instance)) {
                SpareDronePartsYouWantYourDronesToTankNotDealDamage sdp = self.body.GetComponent<SpareDronePartsYouWantYourDronesToTankNotDealDamage>();

                if (sdp.shouldInvert) {
                    self.bodyInputs.moveVector *= -1f;
                }
            }
        }

        private static void StealItem(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            if (IsCurrentDifHigherOrEqual(c6, Run.instance)) {
                if (!damageInfo.attacker || damageInfo.attacker.GetComponent<CharacterBody>().isPlayerControlled) {
                    orig(self, damageInfo, victim);
                    return;
                }
                damageInfo.AddModdedDamageType(DamageTypes.StealItem);
            }

            orig(self, damageInfo, victim);
        }

        private static void CreateDifficulty(Sprite icon, ref DifficultyIndex _index, string token, int count, string tokenDesc) {
            DifficultyDef def = new(3f, token, "the", tokenDesc, new Color32(230, 75, 19, 255), $"c{count}", true);
            def.iconSprite = icon;
            def.foundIconSprite = icon;
            _index = DifficultyAPI.AddDifficulty(def);
        }

        private static void DisableIcons(On.RoR2.Run.orig_OverrideRuleChoices orig, Run self, RuleChoiceMask inc, RuleChoiceMask ex, ulong seed) {
            orig(self, inc, ex, seed);
            if ((self as CrackclipseRun) == null) {
                RuleChoiceController[] controllers = GameObject.FindObjectsOfType<RuleChoiceController>();
                for (int i = 0; i < controllers.Length; i++) {
                    if (controllers[i].gameObject.name.Contains("Crackclipse")) {
                        controllers[i].gameObject.SetActive(false);
                    }
                }
            }
        }

        public static bool IsCurrentDifHigherOrEqual(DifficultyIndex index, Run run) {
            return run.selectedDifficulty <= index;
        }

        private static void TheFog(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self) {
            orig(self);
            
            if (Run.instance && IsCurrentDifHigherOrEqual(c5, Run.instance) && self.isPlayerControlled) {
                self.baseVisionDistance = 20f;

                if (self.gameObject.name.Contains("VoidSurvivor") || self.gameObject.name.Contains("Railgunner")) {
                    self.baseRegen = -3f;
                    self.levelRegen = -0.3f;
                }

                self.statsDirty = true;
            }
        }

        private static void BadSax(On.RoR2.MusicController.orig_PickCurrentTrack orig, MusicController self, ref MusicTrackDef newTrack) {
            if (Run.instance && IsCurrentDifHigherOrEqual(c2, Run.instance)) {
                MusicTrackDef def = Utils.Paths.MusicTrackDef.muRaidfightDLC107.Load<MusicTrackDef>();
                self.currentTrack = def;
                newTrack = def;
            }
            else {
                orig(self, ref newTrack);
            }
        }

        private static void BadSax2(On.RoR2.CombatDirector.orig_OnEnable orig, CombatDirector self) {
            orig(self);
            if (Run.instance && IsCurrentDifHigherOrEqual(c2, Run.instance)) {
                CombatDirector.eliteTiers[0].availableDefs.Add(Utils.Paths.EliteDef.edVoid.Load<EliteDef>());
            }
            else {
                CombatDirector.eliteTiers[0].availableDefs.Remove(Utils.Paths.EliteDef.edVoid.Load<EliteDef>());
            }
        }

        private static bool TrueDisso(On.RoR2.CombatDirector.orig_AttemptSpawnOnTarget orig, CombatDirector self, Transform target, DirectorPlacementRule.PlacementMode mode) {
            if (Run.instance && IsCurrentDifHigherOrEqual(c3, Run.instance) && NetworkServer.active) {
                GameObject masterPrefab = MasterCatalog.masterPrefabs[Run.instance.spawnRng.RangeInt(0, MasterCatalog.masterPrefabs.Length)];
                if (self.currentMonsterCard != null && self.currentMonsterCard.spawnCard != null) {
                    self.currentMonsterCard.spawnCard.prefab = masterPrefab;
                }
                return orig(self, target, mode);
            }
            else {
                return orig(self, target, mode);
            }
        }
    }
}