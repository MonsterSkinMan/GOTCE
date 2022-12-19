using RoR2;
using EntityStates;
using EntityStates.Railgunner.Weapon;
using UnityEngine;
using Unity;
using System;
using RoR2.Projectile;
using R2API;
using System.Reflection;

namespace GOTCE.EntityStatesCustom.AltSkills.Huntress
{
    public class Sawblade : BaseState
    {
        private float duration = 0.5f;

        public override void OnEnter()
        {
            base.OnEnter();

            Ray aimRay = GetAimRay();
            StartAimMode(aimRay);

            PlayAnimation("Gesture", "FireGlaive", "FireGlaive.playbackRate", duration);

            AkSoundEngine.PostEvent(2486049627, base.gameObject); // Play_huntress_m2_throw

            if (base.isAuthority)
            {
                FireProjectileInfo info = default;
                info.owner = base.gameObject;
                info.crit = base.RollCrit();
                info.rotation = Util.QuaternionSafeLookRotation(base.GetAimRay().direction);
                info.position = base.characterBody.corePosition;
                info.damage = base.damageStat * 1.5f;
                info.damageColorIndex = DamageColorIndex.Bleed;
                info.damageTypeOverride = DamageType.BleedOnHit | DamageType.Stun1s;
                info.projectilePrefab = Based.AltSkills.huntressSawPrefab;
                ProjectileManager.instance.FireProjectile(info);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration)
            {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }

    public class SawBehavior : MonoBehaviour
    {
        private bool HasSawLaunched = false;
        private float gravityRampUp = 0f;
        private float gravityRampUpMultiplier = 2f;
        private float fallSpeed = 16f;
        private float launchSpeed = 150f;
        private Rigidbody rb => gameObject.GetComponent<Rigidbody>();
        private HurtBox currentTarget => gameObject.GetComponent<ProjectileSphereTargetFinder>().lastFoundHurtBox;
        private void FixedUpdate()
        {
            gravityRampUp += Time.fixedDeltaTime;
            if (currentTarget && !HasSawLaunched) {
                HasSawLaunched = true;
                gameObject.transform.LookAt(currentTarget.transform.position, Vector3.up);
                rb.AddForce(gameObject.transform.forward * launchSpeed, ForceMode.VelocityChange);
            }
        }
    }
}