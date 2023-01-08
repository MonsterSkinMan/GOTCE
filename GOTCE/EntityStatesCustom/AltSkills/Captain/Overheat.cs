using RoR2;
using Unity;
using EntityStates;
using System;
using UnityEngine;

namespace GOTCE.EntityStatesCustom.AltSkills.Captain
{
    public class Overheat : BaseSkillState
    {
        public float duration = 2f;
        public float bulletsTotal = 20;
        public float secondsCharged = 0f;
        private bool isFiring = false;
        private float stopwatch = 0f;
        private float delay = 0.01f;
        private float bulletsFired = 0;
        public GameObject hitEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/BulletImpactSoft.prefab").WaitForCompletion();

        public override void OnEnter()
        {
            base.OnEnter();
            if (base.isAuthority)
            {
                bulletsTotal = Mathf.CeilToInt(bulletsTotal *= base.attackSpeedStat);
            }
            PlayCrossfade("Gesture, Override", "ChargeCaptainShotgun", "ChargeCaptainShotgun.playbackRate", duration, 0.1f);

            AkSoundEngine.PostEvent(2544146878, base.gameObject); // Play_captain_m1_chargeStart
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && inputBank.skill1.down)
            {
                secondsCharged += Time.fixedDeltaTime;
            }
            else if (!inputBank.skill1.down && !isFiring)
            {
                isFiring = true;
                bulletsTotal = Mathf.Clamp(bulletsTotal * secondsCharged, 5, int.MaxValue);
                base.characterDirection.forward = base.GetAimRay().direction;
            }
            if (base.fixedAge >= duration)
            {
                if (!isFiring) {
                    isFiring = true;
                    bulletsTotal = Mathf.Clamp(bulletsTotal * secondsCharged, 5, int.MaxValue);
                    base.characterDirection.forward = base.GetAimRay().direction;
                }
                if (bulletsFired >= bulletsTotal) {
                    outer.SetNextStateToMain();
                }
            }

            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= delay && isFiring) {
                stopwatch = 0f;

                if (bulletsFired >= bulletsTotal) {
                    outer.SetNextStateToMain();
                }

                BulletAttack bullet = new()
                {
                    smartCollision = false,
                    radius = 0.3f,
                    damage = base.damageStat * 0.3f,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    damageType = DamageType.IgniteOnHit,
                    aimVector = base.GetAimRay().direction,
                    minSpread = 0f,
                    maxSpread = Mathf.Lerp(0, 4, secondsCharged),
                    isCrit = base.RollCrit(),
                    hitEffectPrefab = hitEffectPrefab,
                    tracerEffectPrefab = EntityStates.Commando.CommandoWeapon.FireBarrage.tracerEffectPrefab,
                    origin = base.characterBody.corePosition,
                    owner = base.gameObject,
                    muzzleName = "MuzzleGun"
                };

                bullet.Fire();
                bulletsFired++;

                PlayAnimation("Gesture, Additive", "FireCaptainShotgun");
                PlayAnimation("Gesture, Override", "FireCaptainShotgun");
                AkSoundEngine.PostEvent(532604026, base.gameObject); // Play_captain_m1_shootWide
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
    }
}