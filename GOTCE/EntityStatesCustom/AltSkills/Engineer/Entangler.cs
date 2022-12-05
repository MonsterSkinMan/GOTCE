using RoR2;
using EntityStates;
using Unity;
using UnityEngine;
using RoR2.CharacterAI;

namespace GOTCE.EntityStatesCustom.AltSkills.Engineer {
    public class Entangler : BaseSkillState {
        private IEnumerable<CharacterMaster> minions;
        private float delay = 0.05f;
        private float stopwatch = 0f;

        public override void OnEnter()
        {
            base.OnEnter();
            CharacterMaster owner = base.characterBody.master;
            minions = CharacterMaster.readOnlyInstancesList.Where(x => x.minionOwnership && x.minionOwnership.ownerMaster == owner && x.GetBody() && (x.GetBody().bodyFlags.HasFlag(CharacterBody.BodyFlags.Mechanical)));

            if (!gameObject.GetComponent<EntanglerControllerLeader>()) {
                gameObject.AddComponent<EntanglerControllerLeader>();
            }

            EntanglerControllerLeader leader = gameObject.GetComponent<EntanglerControllerLeader>();
            leader.isControlling = true;


            foreach (CharacterMaster minion in minions) {
                if (minion.GetBody() && NetworkServer.active) {
                    minion.GetBody().AddBuff(Buffs.ValveBalance.instance.BuffDef);
                    if (!minion.GetComponent<EntanglerController>()) {
                        EntanglerController controller = minion.gameObject.AddComponent<EntanglerController>();
                        controller.leaderController = leader;
                    }
                }
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!inputBank.skill1.down && base.isAuthority) {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            gameObject.GetComponent<EntanglerControllerLeader>().isControlling = false;

            if (NetworkServer.active) {
                foreach (CharacterMaster minion in minions) {
                    if (minion.GetBody()) {
                        minion.GetBody().RemoveBuff(Buffs.ValveBalance.instance.BuffDef);
                        minion.GetBody().AddTimedBuff(Buffs.ValveBalance.instance.BuffDef, 3f);
                    }
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}