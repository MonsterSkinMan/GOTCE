using EntityStates;
using UnityEngine;
using System;
using RoR2;
using RoR2.Projectile;

namespace GOTCE.EntityStatesCustom.SolusPylon {
    public class PylonMainState : BaseState {
        private SphereZone zone;
        private List<ProjectileSimple> modified = new();
        private float stopwatch;
        private float delay = 0.15f;

        public override void OnEnter()
        {
            base.OnEnter();
            zone = GetModelChildLocator().FindChild("SphereZone").GetComponent<SphereZone>();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= delay) {
                stopwatch = 0f;
                List<TeamComponent> teamComponents = TeamComponent.GetTeamMembers(GetTeam()).ToList();
                List<ProjectileSimple> simples = GameObject.FindObjectsOfType<ProjectileSimple>().ToList();
                foreach (TeamComponent com in teamComponents) {
                    if (com.body && zone.IsInBounds(com.body.corePosition)) {
                        com.body.AddTimedBuff(RoR2Content.Buffs.CloakSpeed, 3f);
                        com.body.AddTimedBuff(RoR2Content.Buffs.Cloak, 3f);
                    }
                }

                foreach (ProjectileSimple simple in simples) {
                    if (!modified.Contains(simple)) {
                        if (zone.IsInBounds(simple.transform.position)) {
                            modified.Add(simple);
                            simple.desiredForwardSpeed *= 0.25f;
                        }
                    }
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}