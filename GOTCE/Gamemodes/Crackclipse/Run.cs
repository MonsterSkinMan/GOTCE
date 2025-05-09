using RoR2;
using UnityEngine;
using RoR2.UI;

namespace GOTCE.Gamemodes.Crackclipse {
    public class CrackclipseRun : Run {
        private static string UnlockablePrefix = "Crackclipse.";
        private static int MaxCrackclipseLevels = 9;
        private static int MinCrackclipseLevels = 1;
        public static Dictionary<SurvivorDef, int> defToLevel;
        public override void Start()
        {
            SceneDef rallypoint = Utils.Paths.SceneDef.frozenwall.Load<SceneDef>();
            nextStageScene = rallypoint;
            base.Start();
            SetEventFlag("NoArtifactWorld");
        }

        public override void OverrideRuleChoices(RuleChoiceMask mustInclude, RuleChoiceMask mustExclude, ulong runSeed)
        {
            base.OverrideRuleChoices(mustInclude, mustExclude, runSeed);
            int highest = 1;
            foreach (NetworkUser user in NetworkUser.readOnlyInstancesList) {
                if (user.inputPlayer == null) continue;
                LocalUser lc = LocalUserManager.FindLocalUser(user.inputPlayer);
                if (lc == null) continue;
                int cLevel = HighestCrackclipseLevel(lc, lc.userProfile.survivorPreference);
                if (cLevel > highest) {
                    highest = cLevel;
                }
            }
            string dif = $"Difficulty.Crackclipse ({highest})";
            ForceChoice(mustInclude, mustExclude, dif);
            for (int i = 0; i < ArtifactCatalog.artifactCount; i++) {
                ArtifactDef def = ArtifactCatalog.GetArtifactDef((ArtifactIndex)i);
                RuleDef rd = RuleCatalog.FindRuleDef("Artifacts." + def.cachedName);
                ForceChoice(mustInclude, mustExclude, rd.FindChoice("Off"));
            }

            try {

            SurvivorIconController[] controllers = GameObject.Find("CharacterSelectUIMain(Clone)").transform.Find("SafeArea").Find("LeftHandPanel (Layer: Main)").Find("SurvivorChoiceGrid, Panel").Find("SurvivorGrid").GetComponentsInChildren<SurvivorIconController>();
            for (int i = 0; i < controllers.Length; i++) {
                if (controllers[i]._survivorDef != LocalUserManager.GetFirstLocalUser()._userProfile.survivorPreference) {
                    controllers[i].gameObject.SetActive(false);
                }
            }
            }
            catch (Exception err) {
                Debug.Log(err.ToString());
            }
        }

        public override void OnClientGameOver(RunReport runReport)
        {
            base.OnClientGameOver(runReport);
            if (runReport.gameEnding.isWin) {
                Debug.Log("is win");
                SurvivorIndex index = SurvivorCatalog.GetSurvivorIndexFromBodyIndex(runReport.FindFirstPlayerInfo().bodyIndex);
                SurvivorDef def = SurvivorCatalog.GetSurvivorDef(index);
                if (def) {
                    Debug.Log("unlocking next level");
                    UnlockCrackclipseLevel(runReport.FindFirstPlayerInfo().localUser, def);
                }
            }
        }

        public override void AdvanceStage(SceneDef nextScene)
        {
            if (stageClearCount >= 9) {
                base.AdvanceStage(Utils.Paths.SceneDef.moon.Load<SceneDef>());
                return;
            }

            SceneDef rallypoint = Utils.Paths.SceneDef.frozenwall.Load<SceneDef>();
            base.AdvanceStage(rallypoint);
        }

        // crackclipse level stuff
        public static int HighestCrackclipseLevel(LocalUser user, SurvivorDef def) {
            int highest = MinCrackclipseLevels;
            user.userProfile.GrantUnlockable(UnlockableCatalog.GetUnlockableDef(UnlockablePrefix + def.cachedName + "." + 1));
            for (int i = MinCrackclipseLevels; i < MaxCrackclipseLevels; i++) {
                string unlock = UnlockablePrefix + def.cachedName + "." + i.ToString();
                /*Debug.Log(user.userProfile.HasUnlockable(unlock));
                Debug.Log(unlock);
                Debug.Log(UnlockableCatalog.GetUnlockableDef(unlock)); */
                if (user.userProfile.HasUnlockable(unlock)) {
                    // Debug.Log("player has: " + unlock);
                    highest = i;
                    // Debug.Log("highest is now: " + highest);
                }
                else {
                    // Debug.Log("player does not have: " + unlock + " - breaking");
                    break;
                }
            }

            if (!defToLevel.ContainsKey(def)) {
                defToLevel.Add(def, highest);
            }
            else {
                defToLevel[def] = highest;
            }

            return highest;
        }

        public static void UnlockCrackclipseLevel(LocalUser user, SurvivorDef def) {
            int crack = HighestCrackclipseLevel(user, def);
            crack = Mathf.Min(crack + 1, 8);
            string unlock = UnlockablePrefix + def.cachedName + "." + crack.ToString();
            // Debug.Log("trying to grant: " + unlock);
            // Debug.Log(UnlockableCatalog.GetUnlockableDef(unlock));
            user.userProfile.GrantUnlockable(UnlockableCatalog.GetUnlockableDef(unlock));
        }
    }
}