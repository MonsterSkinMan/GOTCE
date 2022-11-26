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

        // private GameObject prefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ClayBoss/TarSeeker.prefab").WaitForCompletion().InstantiateClone("huntresssaw");
        // private GameObject prefabGhost = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Saw/SawmerangGhost.prefab").WaitForCompletion().InstantiateClone("huntresssawghost");
        private GameObject prefab = Main.SecondaryAssets.LoadAsset<GameObject>("Assets/Prefabs/Projectiles/AltSkills/Saw/SawPrefab.prefab");

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = GetAimRay();
            StartAimMode(aimRay);

            PlayAnimation("Gesture", "FireGlaive", "FireGlaive.playbackRate", duration);

            prefab.AddComponent<SlowVelocty>();

            AkSoundEngine.PostEvent(2486049627, base.gameObject); // Play_huntress_m2_throw

            FireProjectileInfo info = default;
            info.owner = base.gameObject;
            info.crit = base.RollCrit();
            info.rotation = Util.QuaternionSafeLookRotation(base.GetAimRay().direction);
            info.position = base.characterBody.corePosition;
            info.damage = base.damageStat * 1.5f;
            info.damageColorIndex = DamageColorIndex.Bleed;
            info.damageTypeOverride = DamageType.BleedOnHit | DamageType.Stun1s;
            info.speedOverride = 250f;
            info.projectilePrefab = prefab;

            if (base.isAuthority)
            {
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

    public class SlowVelocty : MonoBehaviour
    {
        private void FixedUpdate()
        {
            if (NetworkServer.active && gameObject.name == "huntresssawhashit")
            {
                gameObject.GetComponent<ProjectileCharacterController>().velocity = 2;
            }
        }
    }
}