using GOTCE;
using UnityEngine;
using System;
using RoR2;
using EntityStates;
using RoR2.CharacterAI;

namespace GOTCE.EntityStatesCustom.SolusScanner {
    public enum TargetType {
        Friendly,
        Enemy
    }
    public abstract class BeamBase : BaseState {
        public virtual TargetType targetType { get; }
        private GameObject laserPrefab => Utils.Paths.GameObject.LaserEngiTurret.Load<GameObject>();
        public virtual float damageCoefficient { get; } = 2f;
        private int ticks = 3;
        private float coeffPerTick => damageCoefficient / ticks;
        private float delay => 1f / ticks;
        private float stopwatch = 0f;
        private Transform endTransform;
        private GameObject laserInstance;

        public override void OnEnter() {
            base.OnEnter();
            laserInstance = GameObject.Instantiate(laserPrefab, base.transform);
            endTransform = laserInstance.GetComponent<ChildLocator>().FindChild("LaserEnd");
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            if (laserInstance && endTransform) {
                bool hasTarget = base.characterBody?.master?.GetComponent<BaseAI>()?.currentEnemy.gameObject != null;
                if (hasTarget) {
                    // endTransform.position = base.characterBody.master.GetComponent<BaseAI>().currentEnemy.gameObject.transform.position;
                    endTransform.position = base.GetAimRay().GetPoint(35);
                }
                else {
                    endTransform.position = base.GetAimRay().GetPoint(35);
                }
            }

            base.rigidbodyDirection.aimDirection = base.GetAimRay().direction;

            if (!(inputBank.skill1.down || inputBank.skill2.down)) {
                outer.SetNextStateToMain();
            }

            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= delay) {
                stopwatch = 0f;
                switch (targetType) {
                    case TargetType.Friendly:
                        base.characterBody.master.GetComponent<BaseAI>().currentEnemy.characterBody.AddTimedBuff(RoR2Content.Buffs.CloakSpeed, 3f);
                        break;
                    case TargetType.Enemy:
                        BulletAttack attack = new();
                        attack.damage = coeffPerTick * base.damageStat;
                        attack.aimVector = base.GetAimRay().direction;
                        attack.maxDistance = 35f;
                        attack.owner = base.gameObject;
                        attack.weapon = base.gameObject;
                        attack.origin = base.characterBody.corePosition;
                        attack.procCoefficient = 0.3f;
                        attack.Fire();
                        base.characterBody.master.GetComponent<BaseAI>().currentEnemy.characterBody.AddTimedBuff(Buffs.Magnetized.def, 3f);
                        break;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (laserInstance) {
                Destroy(laserInstance);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}