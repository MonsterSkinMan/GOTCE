using EntityStates;
using RoR2;
using UnityEngine;
using GOTCE;
using EntityStates.Commando.CommandoWeapon;
using RoR2.Projectile;
using R2API;

namespace GOTCE.EntityStatesCustom.CrackedMando
{
    public class PhaseRounder : BaseSkillState
    {
        public static float totalDuration = 0.5f;
        private Animator modelAnimator;
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            base.characterBody.SetSpreadBloom(0.2f, canOnlyIncreaseBloom: false);
            duration = totalDuration;
            modelAnimator = GetModelAnimator();
            PlayAnimation("Gesture, Override", "Fire", "Fire.playbackRate", 1f);
            // modelTransform = base.characterBody.modelLocator.modelTransform.GetChild(1).transform;
            base.characterDirection.forward = base.GetAimRay().direction;

            for (int i = 0; i < 8; i++)
            {
                FireBullet();
            }
        }

        private void FireBullet()
        {
            Ray aimRay = GetAimRay();
            GameObject prefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Junk/Commando/FMJ.prefab").WaitForCompletion().InstantiateClone("rounder");
            prefab.AddComponent<R2API.DamageAPI.ModdedDamageTypeHolderComponent>();
            prefab.GetComponent<R2API.DamageAPI.ModdedDamageTypeHolderComponent>().Add(DamageTypes.FullChainLightning);

            // string muzzleName = "MuzzleRight";
            // Ray aimRay = GetAim();
            if (base.isAuthority)
            {
                FireProjectileInfo info = new()
                {
                    damage = base.damageStat * 3.6f,
                    projectilePrefab = prefab,
                    crit = Util.CheckRoll(base.critStat, base.characterBody.master),
                    damageColorIndex = DamageColorIndex.WeakPoint,
                    position = base.characterBody.corePosition,
                    rotation = Util.QuaternionSafeLookRotation(Util.ApplySpread(aimRay.direction, -1f, 3f, -1f, 3f)),
                    owner = base.gameObject,
                    force = 30000f
                };
                ProjectileManager.instance.FireProjectile(info);
            }

            AkSoundEngine.PostEvent(1069717260, base.gameObject); // Play_wFMJ
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration && base.isAuthority)
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