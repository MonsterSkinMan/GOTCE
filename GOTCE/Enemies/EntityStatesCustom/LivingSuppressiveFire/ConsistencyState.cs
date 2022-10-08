using EntityStates;
using RoR2;
using UnityEngine;
using GOTCE;
using EntityStates.Commando.CommandoWeapon;

namespace GOTCE.Enemies.EntityStatesCustom {
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
                BulletAttack bulletAttack = new BulletAttack();
                bulletAttack.owner = base.gameObject;
                bulletAttack.weapon = base.gameObject;
                bulletAttack.origin = aimRay.origin;
                bulletAttack.aimVector = aimRay.direction;
                bulletAttack.minSpread = 5;
                bulletAttack.maxSpread = 7;
                bulletAttack.bulletCount = 1u;
                bulletAttack.damage = base.characterBody.damage * 0.1f;
                bulletAttack.force = 3;
                bulletAttack.tracerEffectPrefab = tracerEffectPrefab;
                // bulletAttack.muzzleName = muzzleName;
                bulletAttack.hitEffectPrefab = hitEffectPrefab;
                bulletAttack.isCrit = Util.CheckRoll(critStat, base.characterBody.master);
                bulletAttack.radius = 2;
                bulletAttack.smartCollision = true;
                bulletAttack.damageType = DamageType.Stun1s;
                bulletAttack.Fire();
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
