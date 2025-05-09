using RoR2;
using RoR2.Achievements;
using System;
using Unity;
using UnityEngine;

namespace GOTCE.Achievements.Railgunner
{
    [RegisterAchievement("STUPID", "StupidRoundsUnlockable", null, 10, typeof(Server))]
    public class StupidRounds : BaseAchievement
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
            return BodyCatalog.FindBodyIndex("RailgunnerBody");
        }

        private class Server : BaseServerAchievement
        {
            public int required = 678;
            public int current = 0;

            public override void OnInstall()
            {
                base.OnInstall();
                On.RoR2.CharacterBody.OnSkillActivated += Check;
                current = 0;
            }

            public override void OnUninstall()
            {
                base.OnUninstall();
                On.RoR2.CharacterBody.OnSkillActivated -= Check;
                current = 0;
            }

            public void Check(On.RoR2.CharacterBody.orig_OnSkillActivated orig, CharacterBody self, GenericSkill skill)
            {
                orig(self, skill);
                if ((skill.skillFamily as ScriptableObject).name.ToLower().Contains("primary"))
                {
                    if (NetworkServer.active && self == serverAchievementTracker.networkUser.GetCurrentBody())
                    {
                        current += 1;

                        if (current >= required)
                        {
                            Grant();
                        }
                    }
                }
            }
        }
    }

    public class StupidRoundsUnlock : AchievementBase<StupidRoundsUnlock>
    {
        public override string Name => "Railgunner: Balance";
        public override string Description => "As Railgunner, fire $13,567.5 worth of XQR Smart Rounds ammo in a single run.";
        public override string UnlockName => "StupidRoundsUnlockable";
        public override string TokenName => "STUPID";
        public override Sprite Icon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Skills/CJI.png");
    }
}