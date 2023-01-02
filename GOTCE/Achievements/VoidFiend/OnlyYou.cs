using RoR2;
using RoR2.Achievements;
using System;
using Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GOTCE.Achievements.VoidFiend
{
    [RegisterAchievement("ONLYYOU", "OnlyYouUnlockable", null, typeof(Server))]
    public class OnlyYou : BaseAchievement
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
            return BodyCatalog.FindBodyIndex("VoidSurvivorBody");
        }

        private class Server : BaseServerAchievement
        {
            private int requiredKills = 75;
            private int totalKills = 0;
            public override void OnInstall()
            {
                base.OnInstall();
                GlobalEventManager.onCharacterDeathGlobal += Killed;
                SceneManager.activeSceneChanged += SceneChange;
            }

            public override void OnUninstall()
            {
                base.OnUninstall();
                GlobalEventManager.onCharacterDeathGlobal -= Killed;
                SceneManager.activeSceneChanged -= SceneChange;
            }

            private void Killed(DamageReport report) {
                totalKills++;
                int totalMonsterCount = TeamComponent.GetTeamMembers(TeamIndex.Monster).Count + TeamComponent.GetTeamMembers(TeamIndex.Void).Count;
                bool areNotAlive = totalMonsterCount < 2;
                Debug.Log(totalMonsterCount);
                if (totalKills > requiredKills && areNotAlive) {
                    Chat.AddMessage("<style=cIsVoid>?...??..?..</style>");
                    base.Grant();
                    CombatDirector[] the = {};
                    CombatDirector.instancesList.CopyTo(the);
                    foreach (CombatDirector director in the) {
                        director.enabled = false;
                    };
                }
            }

            private void SceneChange(Scene s1, Scene s2) {
                totalKills = 0;
            }
        }
    }

    public class OnlyYouUnlock : AchievementBase<OnlyYouUnlock>
    {
        public override string Name => "Void Fiend: <style=cIsVoid>...only you</style>";
        public override string ConfigName { get => "Void Fiend: only you"; set => base.ConfigName = value; }
        public override string Description => "Against all the evil that Hell can conjure, all the wickedness that mankind can produce, we will send unto them... only you. Rip and tear, until it is done.";
        public override string UnlockName => "OnlyYouUnlockable";
        public override string TokenName => "ONLYYOU";
        public override Sprite Icon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Skills/Drain.png");
    }
}