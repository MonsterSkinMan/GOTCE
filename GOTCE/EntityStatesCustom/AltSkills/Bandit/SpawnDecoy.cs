using RoR2;
using EntityStates;
using System;
using RoR2.Projectile;
using Unity;
using UnityEngine;

namespace GOTCE.EntityStatesCustom.AltSkills.Bandit {
    public class SpawnDecoy : BaseSkillState {
        public float duration = 0.8f;

        public override void OnEnter()
        {
            base.OnEnter();
            if (NetworkServer.active) {
                try {
                    MasterSummon masterSummon2 = new MasterSummon();
                    masterSummon2.position = base.characterBody.corePosition;
                    masterSummon2.ignoreTeamMemberLimit = true;
                    masterSummon2.masterPrefab = Enemies.Standard.ExplodingDecoy.Instance.prefabMaster;
                    masterSummon2.summonerBodyObject = base.characterBody.gameObject;
                    masterSummon2.rotation = Quaternion.LookRotation(base.characterBody.transform.forward);
                    masterSummon2.Perform();
                }
                catch {

                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration) {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}