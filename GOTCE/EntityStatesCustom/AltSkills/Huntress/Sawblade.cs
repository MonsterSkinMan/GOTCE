using RoR2;
using EntityStates;
using EntityStates.Railgunner.Weapon;
using UnityEngine;
using Unity;
using System;
using RoR2.Projectile;
using R2API;
using System.Reflection;

namespace GOTCE.EntityStatesCustom.AltSkills.Huntress {
    public class Sawblade : BaseState {
        private float duration = 0.5f;
        // private GameObject prefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ClayBoss/TarSeeker.prefab").WaitForCompletion().InstantiateClone("huntresssaw");
        // private GameObject prefabGhost = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Saw/SawmerangGhost.prefab").WaitForCompletion().InstantiateClone("huntresssawghost");
        private GameObject prefab = Main.SecondaryAssets.LoadAsset<GameObject>("Assets/Prefabs/Projectiles/Saw/SawPrefab.prefab");

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = GetAimRay();
            StartAimMode(aimRay);

            PlayAnimation("Gesture", "FireGlaive", "FireGlaive.playbackRate", duration);

            FireProjectileInfo info = default;
            info.owner = base.gameObject;
            info.crit = base.RollCrit();
            info.rotation = Util.QuaternionSafeLookRotation(base.GetAimRay().direction);
            info.position = base.characterBody.corePosition;
            info.damage = base.damageStat * 1.5f;
            info.damageColorIndex = DamageColorIndex.Bleed;
            info.damageTypeOverride = DamageType.BleedOnHit;
            info.speedOverride = 250f;
            info.projectilePrefab = prefab;

            if (base.isAuthority) {
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
            if (base.fixedAge >= duration) {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }

    /* public class MoveForward : MonoBehaviour {

        public Vector3 forward = new Vector3(0, 0, 0);

        public void OnEnable() {
            forward = gameObject.transform.forward;
        }
        public void FixedUpdate() {
            if (NetworkServer.active) {
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                gameObject.GetComponent<Rigidbody>().velocity = forward * 15;
                // gameObject.GetComponent<Rigidbody>().velocity -= new Vector3(0, , 0);
            }
            
        }
    } */
}