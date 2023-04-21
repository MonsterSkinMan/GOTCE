using System;
using UnityEngine;

namespace GOTCE.EntityStatesCustom.Jerone {
    public class Shotgun : BaseSkillState {
        public GameObject tracerPrefab => Utils.Paths.GameObject.TracerCaptainDefenseMatrix.Load<GameObject>();
        public float damageCoefficient = 2f;
        public float duration = 0.4f;
        public float procCoefficient = 0.7f;

        public override void OnEnter()
        {
            base.OnEnter();

            duration /= base.attackSpeedStat;

            BulletAttack attack = new();
            attack.tracerEffectPrefab = tracerPrefab;
            attack.damage = base.damageStat * damageCoefficient;
            attack.procCoefficient = procCoefficient;
            attack.isCrit = base.RollCrit();
            attack.muzzleName = "Muzzle";
            attack.aimVector = base.inputBank.GetAimRay().direction;
            attack.origin = base.transform.position;
            attack.radius = 1;
            attack.owner = base.gameObject;
            attack.weapon = base.gameObject;
            attack.bulletCount = 6;
            attack.minSpread = -2;
            attack.maxSpread = 2;

            attack.Fire();

            AkSoundEngine.PostEvent(Events.Play_captain_drone_zap, base.gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration) {
                outer.SetNextStateToMain();
            }
        }
    }
}