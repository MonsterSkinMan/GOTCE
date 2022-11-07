using RoR2;
using EntityStates;
using System;
using UnityEngine;
using Unity;

namespace GOTCE.EntityStatesCustom.The {
    public class Kick : BaseSkillState {
        public float duration = 0.2f;
        public float damageCoefficient = 0.3f;
        public float procCoefficient = 0.05f;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = duration / attackSpeedStat;
            if (NetworkServer.active) {
                OverlapAttack attack = new OverlapAttack();
                attack.damage = damageCoefficient * damageStat;
                attack.procCoefficient = procCoefficient;
                attack.damageType = DamageType.Generic;
                attack.attacker = gameObject;
                attack.hitBoxGroup = base.modelLocator.modelBaseTransform.GetComponent<HitBoxGroup>();
                attack.Fire();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration) {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}