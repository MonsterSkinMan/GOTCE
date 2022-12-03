using RoR2;
using EntityStates;
using R2API;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using EntityStates.Commando.CommandoWeapon;

namespace GOTCE.EntityStatesCustom.CrackedMando
{
    public class SuppressiveBarrage : BaseSkillState
    {
        public float duration = 2f;
        public float totalBullets = 128f;
        public int bulletsFired = 0;
        public float delay = 0f;
        public float stopwatch = 0f;

        public override void OnEnter()
        {
            base.OnEnter();
            totalBullets = totalBullets * attackSpeedStat;
            delay = duration / totalBullets;
            PlayAnimation("Gesture, Override", "Fire", "Fire.playbackRate", 1f);
            base.characterDirection.forward = base.GetAimRay().direction;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= delay)
            {
                stopwatch = 0f;
                BulletAttack bulletAttack = new()
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = base.gameObject.transform.position,
                    aimVector = base.GetAimRay().direction,
                    minSpread = 0f,
                    maxSpread = 3f,
                    bulletCount = 1u,
                    damage = damageStat,
                    force = 3,
                    tracerEffectPrefab = FireBarrage.tracerEffectPrefab,
                    isCrit = RollCrit(),
                    radius = 0.01f,
                    muzzleName = GetRandomMuzzle(),
                    smartCollision = false,
                    damageType = DamageType.Stun1s,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    procCoefficient = 1f,
                    maxDistance = 100000f
                };

                if (base.isAuthority)
                {
                    bulletAttack.Fire();
                }
                bulletsFired += 1;
            }

            if (fixedAge >= duration && bulletsFired >= totalBullets)
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
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

            if (!Run.instance) {
                return "Muzzle0";
            }

            return muzzles[Run.instance.runRNG.RangeInt(0, muzzles.Count - 1)];
         }
    }
}