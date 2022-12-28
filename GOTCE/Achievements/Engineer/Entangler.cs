using RoR2;
using RoR2.Achievements;
using System;
using Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GOTCE.Achievements.Engineer
{
    [RegisterAchievement("ENTANGLER", "EntanglerUnlockable", null, typeof(Server))]
    public class Entangler : BaseAchievement
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
            return BodyCatalog.FindBodyIndex("EngiBody");
        }

        private class Server : BaseServerAchievement
        {
            private bool active = false;
            public override void OnInstall()
            {
                base.OnInstall();
                On.RoR2.MinionOwnership.SetOwner += MinionAcquired;
                TeleporterInteraction.onTeleporterChargedGlobal += Charged;
                Stage.onServerStageBegin += SceneChanged;
            }

            public override void OnUninstall()
            {
                base.OnUninstall();
                On.RoR2.MinionOwnership.SetOwner -= MinionAcquired;
                TeleporterInteraction.onTeleporterChargedGlobal -= Charged;
                Stage.onServerStageBegin -= SceneChanged;
            }

            private void SceneChanged(Stage stage) {
                if (stage.sceneDef.sceneType == SceneType.Stage) {
                    active = true;
                }
                else {
                    active = false;
                }
            }

            private void MinionAcquired(On.RoR2.MinionOwnership.orig_SetOwner orig, MinionOwnership self, CharacterMaster owner) {
                orig(self, owner);
                if (owner == base.serverAchievementTracker.networkUser.master) {
                    if (self.gameObject.GetComponent<CharacterMaster>()) {
                        CharacterMaster master = self.gameObject.GetComponent<CharacterMaster>();
                        if (master.GetBody() && master.GetBody().bodyFlags.HasFlag(CharacterBody.BodyFlags.Mechanical)) {
                            active = false;
                        }
                    }
                }
            }

            private void Charged(TeleporterInteraction self) {
                if (active) {
                    base.Grant();
                }
            }
        }
    }

    public class EntanglerUnlock : AchievementBase<EntanglerUnlock>
    {
        public override string Name => "Engineer: Entangler";
        public override string Description => "As Engineer, complete a stage without acquiring any mechanical allies.";
        public override string UnlockName => "EntanglerUnlockable";
        public override string TokenName => "ENTANGLER";
        public override Sprite Icon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Skills/SharkSaw.png");
    }
}