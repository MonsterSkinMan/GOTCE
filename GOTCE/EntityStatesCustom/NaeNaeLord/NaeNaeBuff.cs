using RoR2;
using EntityStates;
using R2API;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using RoR2.Projectile;

namespace GOTCE.EntityStatesCustom.NaeNaeLord {
    public class NaeNaeBuff : BaseState {

        public float duration = 3f;
        public override void OnEnter()
        {
            base.OnEnter();
            if (NetworkServer.active) {
                List<HurtBox> naenaebuffer = new();
                SphereSearch naenaesearch = new();
                naenaesearch.radius = 50f;
                naenaesearch.origin = base.characterBody.corePosition;
                naenaesearch.mask = LayerIndex.entityPrecise.mask;
                naenaesearch.RefreshCandidates();
                naenaesearch.FilterCandidatesByHurtBoxTeam(TeamMask.AllExcept(TeamIndex.Player));
                naenaesearch.FilterCandidatesByDistinctHurtBoxEntities();
                naenaesearch.OrderCandidatesByDistance();
                naenaesearch.GetHurtBoxes(naenaebuffer);
                naenaesearch.ClearCandidates();

                foreach(HurtBox box in naenaebuffer) {
                    if (box && box.healthComponent && box.healthComponent.body) {
                        box.healthComponent.body.AddTimedBuff(RoR2Content.Buffs.CloakSpeed, 5f);
                    }
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