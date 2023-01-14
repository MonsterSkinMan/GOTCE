using RoR2;
using EntityStates;
using System;
using UnityEngine;
using EntityStates.BrotherMonster;
using RoR2.Navigation;
using RoR2.Projectile;

namespace GOTCE.EntityStatesCustom.Glassthrix.P1 {
    public class Hammer : BaseState {
        private float duration = 1.5f;
        private float damageCoefficient = 9f;
        private float force = 40f;
        private float radius = 10f;
        private GameObject impactPrefab => Utils.Paths.GameObject.BrotherSlamImpact.Load<GameObject>();
        private GameObject orbPrefab => Utils.Paths.GameObject.BrotherSunderWaveEnergized.Load<GameObject>();
        private OverlapAttack attack;

        public override void OnEnter()
        {
            base.OnEnter();
            PlayCrossfade("FullBody Override", "WeaponSlam", "WeaponSlam.playbackRate", duration, 0.1f);
            AkSoundEngine.PostEvent(Events.Play_moonBrother_swing_vertical, base.gameObject);
            if (base.characterDirection) {
                base.characterDirection.moveVector = base.GetAimRay().direction;
            }

            if (base.isAuthority) {
                attack = new OverlapAttack() {
                    attacker = base.gameObject,
                    damage = damageCoefficient * base.damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    hitEffectPrefab = impactPrefab,
                    hitBoxGroup = base.FindHitBoxGroup("WeaponBig"),
                    inflictor = base.gameObject,
                    procChainMask = default(ProcChainMask),
                    pushAwayForce = force,
                    procCoefficient = 1f,
                    teamIndex = base.GetTeam()
                };

                for (int i = 0; i < 10; i++) {
                    float num = 360f / 10;
                    Vector3 vector = Vector3.ProjectOnPlane(base.inputBank.aimDirection, Vector3.up);
                    Vector3 footPosition = base.characterBody.footPosition;
                    FireProjectileInfo info = new();
                    Vector3 forward = Quaternion.AngleAxis(num * i, Vector3.up) * vector;
                    info.projectilePrefab = orbPrefab;
                    info.damage = base.damageStat * damageCoefficient;
                    info.position = footPosition;
                    info.rotation = Util.QuaternionSafeLookRotation(forward);
                    info.crit = base.RollCrit();
                    info.owner = base.gameObject;
                    ProjectileManager.instance.FireProjectile(info);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.GetModelAnimator()) {
                if (base.isAuthority && base.GetModelAnimator().GetFloat("weapon.hitboxActive") > 0.5f) {
                    attack.Fire();
                }
            }
            if (base.fixedAge >= duration && base.isAuthority) {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}