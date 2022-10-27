using RoR2;
using EntityStates;
using R2API;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using EntityStates.Commando.CommandoWeapon;

namespace GOTCE.EntityStatesCustom.CrackedMando {
    public class DoubleDoubleTap : BaseSkillState {

        public float duration = 1f;
        public float force = 5f;
        public float stopwatch = 0f;
        public int bulletsFired;

        public float damageCoeff = 1f;
        public int hits = 96;
        public float procCoeff = 1f;
        public override void OnEnter() {
            base.OnEnter();
        }
        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if (bulletsFired >= 96 && base.fixedAge >= duration) {
                outer.SetNextStateToMain();
            }
            BulletAttack bulletAttack = new()
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = base.gameObject.transform.position,
                    aimVector = base.GetAimRay().direction,
                    minSpread = 20f,
                    maxSpread = 360f,
                    bulletCount = 1u,
                    damage = 0f,
                    force = 300,
                    tracerEffectPrefab = FireBarrage.tracerEffectPrefab,
                    hitEffectPrefab = FireBarrage.hitEffectPrefab,
                    isCrit = false,
                    radius = 5,
                    smartCollision = true,
                    damageType = DamageType.BypassArmor,
                    falloffModel = BulletAttack.FalloffModel.None,
                    procCoefficient = 0f,
                    maxDistance = 100000f
                };

                bulletAttack.Fire();
            if (base.isAuthority && stopwatch >= 0.3f) {
                List<HurtBox> mandobuffer = new();
                SphereSearch mandosearch = new();
                mandosearch.radius = 25f;
                mandosearch.origin = base.characterBody.corePosition;
                mandosearch.mask = LayerIndex.entityPrecise.mask;
                mandosearch.RefreshCandidates();
                mandosearch.FilterCandidatesByHurtBoxTeam(TeamMask.GetUnprotectedTeams(base.teamComponent.teamIndex));
                mandosearch.FilterCandidatesByDistinctHurtBoxEntities();
                mandosearch.OrderCandidatesByDistance();
                mandosearch.GetHurtBoxes(mandobuffer);
                mandosearch.ClearCandidates();

                foreach(HurtBox box in mandobuffer) {
                    for (int i = 0; i < (96/3); i++) {
                        box.healthComponent.TakeDamage(new DamageInfo {
                            attacker = base.gameObject,
                            damage = base.damageStat * damageCoeff,
                            procCoefficient = procCoeff,
                            procChainMask = default,
                            damageType = DamageType.Generic,
                        });
                    }
                }
                bulletsFired += (96/3);
                stopwatch = 0f;
            }
        }
    }
}