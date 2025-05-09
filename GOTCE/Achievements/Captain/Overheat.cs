using RoR2;
using RoR2.Achievements;
using System;
using Unity;
using UnityEngine;

namespace GOTCE.Achievements.Captain
{
    [RegisterAchievement("OVERHEAT", "OverheatUnlockable", null, 10, typeof(Server))]
    public class Overheat : BaseAchievement
    {
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("CaptainBody");
        }

        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            SetServerTracked(true);
        }

        public override void OnBodyRequirementBroken()
        {
            base.OnBodyRequirementBroken();
            SetServerTracked(false);
        }
        
        private class Server : BaseServerAchievement {
            private WarCrime crime => this.serverAchievementTracker.networkUser.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime;
            public override void OnInstall()
            {
                base.OnInstall();
                RoR2.Run.onClientGameOverGlobal += GameOver;
            }

            public override void OnUninstall()
            {
                base.OnUninstall();
                RoR2.Run.onClientGameOverGlobal -= GameOver;
            }

            private void GameOver(Run run, RunReport report) {
                if (report.gameEnding.isWin && crime == WarCrime.Incendiary) {
                    Grant();
                }
            }
        }
    }

    public class OverheatUnlock : AchievementBase<OverheatUnlock>
    {
        public override string Name => "Captain: Streetcleaning";
        public override string Description => "As Captain, beat the game with your most recent war crime being Incendiary Weaponry.";
        public override string UnlockName => "OverheatUnlockable";
        public override string TokenName => "OVERHEAT";
        public override Sprite Icon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Skills/HephaestusShotgun.png");
    }
} 