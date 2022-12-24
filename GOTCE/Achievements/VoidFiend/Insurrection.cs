using RoR2;
using RoR2.Achievements;
using System;
using Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GOTCE.Achievements.VoidFiend
{
    [RegisterAchievement("INSURRECTIOn", "InsurrectionUnlockable", null, typeof(Server))]
    public class Insurrection : BaseAchievement
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

            public override void OnInstall()
            {
                base.OnInstall();
                On.RoR2.BlastAttack.Fire += MarkExplosion;
                GlobalEventManager.onCharacterDeathGlobal += Killed;
            }

            public override void OnUninstall()
            {
                base.OnUninstall();
                On.RoR2.BlastAttack.Fire -= MarkExplosion;
                GlobalEventManager.onCharacterDeathGlobal -= Killed;
            }

            private BlastAttack.Result MarkExplosion(On.RoR2.BlastAttack.orig_Fire orig, BlastAttack self) {
                self.AddModdedDamageType(DamageTypes.Explosion);
                return orig(self);
            }

            private void Killed(DamageReport report) {
                if (report.attackerMaster) {
                    CharacterMaster current = base.serverAchievementTracker.networkUser.master;
                    if (current && report.attackerMaster == current) {
                        if (report.victimBody && report.victimBody.healthComponent && report.victimBody.baseNameToken == "VOIDRAIDCRAB_BODY_NAME") {
                            float fullHealth = report.victimBody.healthComponent.fullCombinedHealth;
                            if (report.damageDealt >= fullHealth) {
                                if (report.damageInfo.HasModdedDamageType(DamageTypes.Explosion)) {
                                    base.Grant();
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class InsurrectionUnlock : AchievementBase<InsurrectionUnlock>
    {
        public override string Name => "Void Fiend: Insurrection";
        public override string Description => "As Void Fiend, slay the descendant of the Void in a single explosion.";
        public override string UnlockName => "InsurrectionUnlockable";
        public override string TokenName => "INSURRECTION";
        public override Sprite Icon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Skills/Drain.png");
    }
}