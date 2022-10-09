using EntityStates;
using RoR2;
using UnityEngine;
using GOTCE;
using EntityStates.Commando.CommandoWeapon;

namespace GOTCE.Enemies.EntityStatesCustom
{
    public class ConsistencyState : BaseSkillState
    {
        public static GameObject hitEffectPrefab = FireBarrage.hitEffectPrefab;

        public static GameObject tracerEffectPrefab = FireBarrage.tracerEffectPrefab;

        public static float damageCoefficient;

        public static float force;

        public static float minSpread;

        public static float maxSpread;

        public static float baseDurationBetweenShots = 0.001f;

        public static float totalDuration = 2f;

        public static float bulletRadius = 1.5f;

        public static int baseBulletCount = 2000;

        public static string fireBarrageSoundString;

        public static float recoilAmplitude;

        public static float spreadBloomValue;

        private int totalBulletsFired;

        private int bulletCount;

        public float stopwatchBetweenShots;

        private Animator modelAnimator;

        private Transform modelTransform;

        private float duration;

        private float durationBetweenShots;

        public override void OnEnter()
        {
            base.OnEnter();
            base.characterBody.SetSpreadBloom(0.2f, canOnlyIncreaseBloom: false);
            duration = totalDuration;
            durationBetweenShots = baseDurationBetweenShots / attackSpeedStat;
            bulletCount = (int)((float)baseBulletCount * attackSpeedStat);
            modelAnimator = GetModelAnimator();
            modelTransform = GetModelTransform();
            FireBullet();
        }

        private void FireBullet()
        {
            Ray aimRay = GetAimRay();
            // string muzzleName = "MuzzleRight";
            if (base.isAuthority)
            {
                BulletAttack bulletAttack = new()
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 1f,
                    maxSpread = 2f,
                    bulletCount = 1u,
                    damage = base.characterBody.damage * 0.08f,
                    force = 3,
                    tracerEffectPrefab = tracerEffectPrefab,
                    // bulletAttack.muzzleName = muzzleName;
                    hitEffectPrefab = hitEffectPrefab,
                    isCrit = Util.CheckRoll(critStat, base.characterBody.master),
                    radius = 2,
                    smartCollision = false,
                    damageType = DamageType.Stun1s
                };
                bulletAttack.Fire();
                BulletAttack bulletAttack2 = new()
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 90f,
                    maxSpread = 360f,
                    bulletCount = 1u,
                    damage = base.characterBody.damage * 0.02f,
                    force = 30,
                    tracerEffectPrefab = tracerEffectPrefab,
                    // bulletAttack.muzzleName = muzzleName;
                    hitEffectPrefab = hitEffectPrefab,
                    isCrit = Util.CheckRoll(critStat, base.characterBody.master),
                    radius = 2,
                    smartCollision = false,
                    damageType = DamageType.Stun1s
                };
                bulletAttack2.Fire();
            }
            totalBulletsFired++;
            Util.PlaySound(fireBarrageSoundString, base.gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatchBetweenShots += Time.fixedDeltaTime;
            if (stopwatchBetweenShots >= durationBetweenShots && totalBulletsFired < bulletCount)
            {
                stopwatchBetweenShots -= durationBetweenShots;
                FireBullet();
            }
            if (base.fixedAge >= duration && totalBulletsFired == bulletCount && base.isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}