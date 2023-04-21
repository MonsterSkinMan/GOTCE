using System;
using UnityEngine;

namespace GOTCE.EntityStatesCustom.Cracktain {
    public class Laser : BaseSkillState {
        public GameObject tracerPrefab => Utils.Paths.GameObject.TracerCaptainDefenseMatrix.Load<GameObject>();
        public float damageCoefficient = 2f;
        public float duration = 1f;
        public float procCoefficient = 1f;

        public override void OnEnter()
        {
            base.OnEnter();

            duration /= base.attackSpeedStat;

            BulletAttack attack = new();
            attack.tracerEffectPrefab = tracerPrefab;
            attack.damage = base.damageStat * damageCoefficient;
            attack.procCoefficient = procCoefficient;
            attack.falloffModel = BulletAttack.FalloffModel.None;
            attack.damageType = Util.CheckRoll(50, base.characterBody.master) ? DamageType.IgniteOnHit : DamageType.Generic;
            attack.isCrit = base.RollCrit();
            attack.muzzleName = "MuzzleL";
            attack.aimVector = base.inputBank.GetAimRay().direction;
            attack.origin = base.transform.position;
            attack.radius = 1;
            attack.owner = base.gameObject;
            attack.weapon = base.gameObject;

            attack.Fire();
            attack.muzzleName = "MuzzleR";
            attack.Fire();
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
            return InterruptPriority.PrioritySkill;
        }
    }
}