using RoR2;
using EntityStates;
using R2API;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using Rewired.ComponentControls.Effects;
using EntityStates.Commando.CommandoWeapon;

namespace GOTCE.EntityStatesCustom.CrackedMando
{
    public class DoubleDoubleTap : BaseSkillState
    {
        public float delay = 5;
        public float bulletsFired;
        public int timer = 0;

        public float damageCoeff = 1f;
        public int hits = 96;
        public float procCoeff = 1f;
        private Transform rotateObject => base.characterBody.gameObject.transform.Find("RotateObject");

        public override void OnEnter()
        {
            base.OnEnter();
            PlayAnimation("Gesture, Override", "Fire", "Fire.playbackRate", 1f);
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
            timer++;

            if (timer >= delay && bulletsFired >= 8) {
                outer.SetNextStateToMain();
            }
            
            if (base.isAuthority)
            {
                List<HurtBox> mandobuffer = new();
                SphereSearch mandosearch = new()
                {
                    radius = 25f,
                    origin = base.characterBody.corePosition,
                    mask = LayerIndex.entityPrecise.mask
                };
                mandosearch.RefreshCandidates();
                mandosearch.FilterCandidatesByHurtBoxTeam(TeamMask.GetUnprotectedTeams(base.teamComponent.teamIndex));
                mandosearch.FilterCandidatesByDistinctHurtBoxEntities();
                mandosearch.OrderCandidatesByDistance();
                mandosearch.GetHurtBoxes(mandobuffer);
                mandosearch.ClearCandidates();

                GameObject guh = this.gameObject;
                foreach (HurtBox box in mandobuffer)
                {
                    guh.transform.LookAt(box.healthComponent.gameObject.transform);
                    BulletAttack bulletAttack = new()
                    {
                        owner = base.gameObject,
                        weapon = base.gameObject,
                        origin = GetModelChildLocator().FindChild(GetRandomMuzzle()).position,
                        aimVector = guh.transform.forward,
                        minSpread = 0f,
                        maxSpread = 0f,
                        bulletCount = 1u,
                        damage = base.damageStat,
                        force = 0,
                        tracerEffectPrefab = FireBarrage.tracerEffectPrefab,
                        hitEffectPrefab = FireBarrage.hitEffectPrefab,
                        isCrit = base.RollCrit(),
                        radius = 0.001f,
                        smartCollision = false,
                        falloffModel = BulletAttack.FalloffModel.None,
                        procCoefficient = 1f,
                        maxDistance = 25f,
                        muzzleName = GetRandomMuzzle()
                    };

                    if (bulletsFired == 3 || bulletsFired == 6) { // fire an extra time every 3rd or 6th bullet to reach 96
                        bulletAttack.Fire();
                    }
                    bulletAttack.Fire();
                }

                BulletAttack bulletAttack2 = new()
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = GetModelChildLocator().FindChild(GetRandomMuzzle()).position,
                    aimVector = base.GetAimRay().direction,
                    minSpread = 0f,
                    maxSpread = 360f,
                    bulletCount = 1u,
                    damage = 0,
                    force = 0,
                    tracerEffectPrefab = FireBarrage.tracerEffectPrefab,
                    hitEffectPrefab = FireBarrage.hitEffectPrefab,
                    isCrit = false,
                    radius = 0.001f,
                    smartCollision = false,
                    falloffModel = BulletAttack.FalloffModel.None,
                    procCoefficient = 0f,
                    maxDistance = 25f,
                    muzzleName = GetRandomMuzzle()
                };

                bulletAttack2.Fire();

                bulletsFired += 1;
                rotateObject.Rotate(new Vector3(0, 12, 0));
                base.characterDirection.forward = rotateObject.forward;
            }

        }

        private string GetRandomMuzzle() {
            List<string> muzzles = new() {
                "Muzzle0",
                "Muzzle1",
                "Muzzle2",
                "Muzzle3",
                "Muzzle4",
                "Muzzle5",
                "Muzzle6",
                "Muzzle7",
                "Muzzle8",
                "Muzzle9",
                "Muzzle10"
            };

            return muzzles[Run.instance.runRNG.RangeInt(0, muzzles.Count - 1)];
         }
    }
}