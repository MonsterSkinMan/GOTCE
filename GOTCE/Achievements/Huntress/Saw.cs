using RoR2;
using RoR2.Achievements;
using System;
using Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GOTCE.Achievements.Huntress
{
    [RegisterAchievement("SAW", "SawUnlockable", null, 10, typeof(Server))]
    public class Saw : BaseAchievement
    {
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

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("HuntressBody");
        }

        private class Server : BaseServerAchievement
        {
            private GameObject mostRecentSawmerang = null;
            private int totalKills = 0;
            private int requiredKills = 3;

            public override void OnInstall()
            {
                base.OnInstall();
                GlobalEventManager.onCharacterDeathGlobal += OnKill;
            }

            public override void OnUninstall()
            {
                base.OnUninstall();
                GlobalEventManager.onCharacterDeathGlobal -= OnKill;
            }

            private void OnKill(DamageReport report) {
                if (report.attackerMaster != base.serverAchievementTracker.networkUser.master) {
                    return;
                }

                if (report.damageInfo.inflictor && report.damageInfo.inflictor.name.ToLower().Contains("saw")) {
                    if (mostRecentSawmerang && mostRecentSawmerang == report.damageInfo.inflictor) {
                        totalKills++;

                        if (totalKills >= requiredKills) {
                            base.Grant();
                        }
                    }
                    else {
                        mostRecentSawmerang = report.damageInfo.inflictor;
                        totalKills = 0;
                    }
                }
            }
        }
    }

    public class SawUnlock : AchievementBase<SawUnlock>
    {
        public override string Name => "Huntress: Mincemeat";
        public override string Description => "As Huntress, kill 3 enemies with a single use of Sawmerang";
        public override string UnlockName => "SawUnlockable";
        public override string TokenName => "SAW";
        public override Sprite Icon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Skills/SharkSaw.png");
    }
}