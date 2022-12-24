using RoR2;
using RoR2.Achievements;
using System;
using Unity;
using UnityEngine;

namespace GOTCE.Achievements.CrackedCommando
{
    [RegisterAchievement("OVERDOSE", "CrackedCommandoSurvivorUnlockable", null, typeof(Server))]
    public class SurvivorUnlockAchievement : BaseAchievement
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
            return BodyCatalog.FindBodyIndex("CommandoBody");
        }

        private class Server : BaseServerAchievement
        {
            public float required = 16f; // 1600%

            public override void OnInstall()
            {
                base.OnInstall();
                RoR2Application.onUpdate += Check;
            }

            public override void OnUninstall()
            {
                base.OnUninstall();
                RoR2Application.onUpdate -= Check;
            }

            public void Check()
            {
                CharacterBody current = serverAchievementTracker.networkUser.GetCurrentBody();
                if (current && current.attackSpeed >= required)
                {
                    Grant();
                }
            }
        }
    }

    public class SurvivorUnlock : AchievementBase<SurvivorUnlock>
    {
        public override string Name => "Commando: The Delirious";
        public override string Description => "As Commando, reach 1600% attack speed.";
        public override string UnlockName => "CrackedCommandoSurvivorUnlockable";
        public override string TokenName => "OVERDOSE";
        public override Sprite Icon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Survivors/CrackedCommando.png");
    }
}