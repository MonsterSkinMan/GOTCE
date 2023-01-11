using RoR2;
using R2API;
using UnityEngine;
using System.Text;

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
            Sprite tmp = Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/Aegiscentrism.png");
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
            sb.Append("- You dream of quiet snowfall.");
            LanguageAPI.Add("CRACKCLIPSE_1_DESC", sb.ToString());
            sb.Append("\n- Bad sax overwhelms you");
            LanguageAPI.Add("CRACKCLIPSE_2_DESC", sb.ToString());

            On.RoR2.MusicController.PickCurrentTrack += BadSax;
        }

        private static void CreateDifficulty(Sprite icon, ref DifficultyIndex _index, string token, int count, string tokenDesc) {
            DifficultyDef def = new(3f, token, "the", tokenDesc, new Color(230, 75, 19, 10), $"c{count}", true);
            def.iconSprite = icon;
            def.foundIconSprite = icon;
            _index = DifficultyAPI.AddDifficulty(def);
        }

        public static bool IsCurrentDifHigherOrEqual(DifficultyIndex index, Run run) {
            return run.selectedDifficulty <= index;
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
    }
}