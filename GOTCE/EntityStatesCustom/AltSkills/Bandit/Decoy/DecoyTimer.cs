using RoR2;
using EntityStates;
using System;
using RoR2.Projectile;
using Unity;
using UnityEngine;
using RoR2.CharacterAI;

namespace GOTCE.EntityStatesCustom.AltSkills.Bandit.Decoy {
    public class DecoyTimer : BaseState {
        public float duration = 5f;
        public float stopwatch = 0f;
        public float delay = 0.5f;

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration) {
                if (NetworkServer.active) {
                    base.characterBody.healthComponent.Suicide();
                }
                outer.SetNextStateToMain();
            }
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= delay) {
                stopwatch = 0f;
                if (NetworkServer.active) {
                    List<HurtBox> buffer = new();
                    SphereSearch search = new();
                    search.radius = 200f;
                    search.origin = base.characterBody.corePosition;
                    search.mask = LayerIndex.entityPrecise.mask;
                    search.RefreshCandidates();
                    search.FilterCandidatesByHurtBoxTeam(TeamMask.AllExcept(TeamIndex.Player));
                    search.FilterCandidatesByDistinctHurtBoxEntities();
                    search.OrderCandidatesByDistance();
                    search.GetHurtBoxes(buffer);
                    search.ClearCandidates();

                    foreach (HurtBox box in buffer) {
                        if (box.healthComponent && box.healthComponent.body && box.healthComponent.body.master) {
                            foreach (BaseAI ai in box.healthComponent.body.master.aiComponents) {
                                ai.currentEnemy.gameObject = base.gameObject;
                            }
                        }
                    }
                }
            }
        }
    }
}