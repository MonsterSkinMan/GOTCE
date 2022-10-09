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

        public static float baseDurationBetweenShots = 7 / 60f;

        public static float totalDuration = 2f;

        public static float bulletRadius = 1.5f;

        public static int baseBulletCount = 25;

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
<<<<<<< HEAD
                BulletAttack bulletAttack = new BulletAttack();
                bulletAttack.owner = base.gameObject;
                bulletAttack.weapon = base.gameObject;
                bulletAttack.origin = aimRay.origin;
                bulletAttack.aimVector = aimRay.direction;
                bulletAttack.minSpread = 3;
                bulletAttack.maxSpread = 6;
                bulletAttack.bulletCount = 1u;
                bulletAttack.damage = base.characterBody.damage * 0;
                bulletAttack.force = 25;
                bulletAttack.tracerEffectPrefab = tracerEffectPrefab;
                // bulletAttack.muzzleName = muzzleName;
                bulletAttack.hitEffectPrefab = hitEffectPrefab;
                bulletAttack.isCrit = Util.CheckRoll(critStat, base.characterBody.master);
                bulletAttack.radius = 2;
                bulletAttack.smartCollision = true;
                bulletAttack.maxDistance = 9000;
                bulletAttack.damageType = DamageType.Stun1s | DamageType.Nullify | DamageType.SlowOnHit | DamageType.CrippleOnHit | DamageType.LunarSecondaryRootOnHit | DamageType.Freeze2s | DamageType.ClayGoo | DamageType.WeakOnHit;
=======
                BulletAttack bulletAttack = new()
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 1f,
                    maxSpread = 2f,
                    bulletCount = 1u,
                    damage = base.characterBody.damage * 0.1f,
                    force = 30,
                    tracerEffectPrefab = tracerEffectPrefab,
                    // bulletAttack.muzzleName = muzzleName;
                    hitEffectPrefab = hitEffectPrefab,
                    isCrit = Util.CheckRoll(critStat, base.characterBody.master),
                    radius = 2,
                    smartCollision = false,
                    damageType = DamageType.Stun1s
                };
>>>>>>> a3433fbbf2b870fa40ae36599a9a42f8cfa0e4dd
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
                    damage = base.characterBody.damage * 0.1f,
                    force = 300,
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