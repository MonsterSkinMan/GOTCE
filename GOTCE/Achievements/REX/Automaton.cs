using RoR2;
using RoR2.Achievements;
using System;
using Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GOTCE.Achievements.REX
{
    [RegisterAchievement("AUTOMATON", "AutomatonUnlockable", null, typeof(Server))]
    public class Automaton : BaseAchievement
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
            return BodyCatalog.FindBodyIndex("TreebotBody");
        }

        private class Server : BaseServerAchievement
        {
            public bool active = false;

            public override void OnInstall()
            {
                base.OnInstall();
                SceneManager.activeSceneChanged += SceneChanged;
                GlobalEventManager.onServerDamageDealt += TakenDamage;
                TeleporterInteraction.onTeleporterChargedGlobal += TeleporterCharged;
            }

            public override void OnUninstall()
            {
                base.OnUninstall();
                SceneManager.activeSceneChanged -= SceneChanged;
                GlobalEventManager.onServerDamageDealt -= TakenDamage;
                TeleporterInteraction.onTeleporterChargedGlobal -= TeleporterCharged;
            }

            private void SceneChanged(Scene oldScene, Scene newScene) {

                if (SceneManager.GetActiveScene().name == "skymeadow") {
                    active = true;
                }
                else {
                    active = false;
                }
            }

            private void TakenDamage(DamageReport report) {
                if (report.victimMaster) {
                    CharacterMaster current = base.serverAchievementTracker.networkUser.master;
                    if (current && current == report.victimMaster) {
                        active = false;
                    }
                }
            }

            private void TeleporterCharged(TeleporterInteraction interaction) {
                if (active) {
                    base.Grant();
                }
            }
        }
    }

    public class AutomatonUnlock : AchievementBase<AutomatonUnlock>
    {
        public override string Name => "REX: Freshly Grown";
        public override string Description => "As REX, complete Sky Meadow without taking damage.";
        public override string UnlockName => "AutomatonUnlockable";
        public override string TokenName => "AUTOMATON";
        public override Sprite Icon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/NEA.png");
    }
}