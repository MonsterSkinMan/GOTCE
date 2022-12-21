using EntityStates;
using RoR2;
using UnityEngine;
using GOTCE;
using EntityStates.Commando.CommandoWeapon;
using RoR2.Projectile;

namespace GOTCE.EntityStatesCustom.CrackedMando
{
    public class SuppressiveNader : BaseSkillState
    {
        public static float damageCoefficient;

        public static float force;

        public static float minSpread;

        public static float maxSpread;

        public static float baseDurationBetweenShots = 7 / 60f;

        public static float totalDuration = 2f;

        public static float bulletRadius = 1.5f;

        public static int baseBulletCount = 6;

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
            PlayAnimation("Gesture, Override", "Grenade", "Grenade.playbackRate", duration);
            base.characterDirection.forward = base.GetAimRay().direction;
            // modelTransform = base.characterBody.modelLocator.modelTransform.GetChild(1).transform;
            FireBullet();
            Ray aimRay = GetAimRay();
            GameObject pRound = Addressables.LoadAssetAsync<GameObject>("RoR2/Junk/Commando/FMJ.prefab").WaitForCompletion();

            FireProjectileInfo info2 = new()
            {
                damage = base.damageStat *= 3.6f,
                projectilePrefab = pRound,
                crit = Util.CheckRoll(base.critStat, base.characterBody.master),
                damageColorIndex = DamageColorIndex.WeakPoint,
                position = base.characterBody.aimOriginTransform.position,
                rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                owner = base.gameObject,
            };

            ProjectileManager.instance.FireProjectile(info2);
        }

        private void FireBullet()
        {
            Ray aimRay = GetAimRay();

            // string muzzleName = "MuzzleRight";
            // Ray aimRay = GetAim();
            if (base.isAuthority)
            {
                FireProjectileInfo info = new()
                {
                    damage = base.damageStat *= 7f,
                    projectilePrefab = Based.AltSkills.suppressiveNaderPrefab,
                    crit = Util.CheckRoll(base.critStat, base.characterBody.master),
                    damageColorIndex = DamageColorIndex.WeakPoint,
                    position = GetModelChildLocator().FindChild("Mouth").position,
                    rotation = Util.QuaternionSafeLookRotation(Util.ApplySpread(aimRay.direction, -2f, 2f, -2f, 2f)),
                    owner = base.gameObject,
                };
                ProjectileManager.instance.FireProjectile(info);
            }
            totalBulletsFired++;
            AkSoundEngine.PostEvent(2891623396, base.gameObject); // Play_commando_M2_grenade_throw
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