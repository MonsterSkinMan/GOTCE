using RoR2;
using System;
using Unity;
using UnityEngine;
using EntityStates;
using RoR2.Projectile;
using R2API;

namespace GOTCE.EntityStatesCustom.AltSkills.VoidFiend {
    public class PearlUpgrade : BaseSkillState {
        public float duration = 0.5f;

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aim = GetAimRay();
            
            if (NetworkServer.active) {
                GameObject projectile = Main.SecondaryAssets.LoadAsset<GameObject>("Assets/Prefabs/Projectiles/AltSkills/CorruptedPearl/PearlUpgrade.prefab").InstantiateClone("pearlupgradeprojectile");
                projectile.AddComponent<ViendPearlUpgradeBehavior>();
                projectile.GetComponent<ViendPearlUpgradeBehavior>().owner = gameObject;
                projectile.GetComponent<ProjectileImpactExplosion>().explosionEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/CritGlassesVoid/CritGlassesVoidExecuteEffect.prefab").WaitForCompletion();

                FireProjectileInfo info = new();
                info.owner = gameObject;
                info.damage = base.damageStat;
                info.damageColorIndex = DamageColorIndex.Void;
                info.projectilePrefab = projectile;
                info.position = aim.origin;
                info.rotation = Util.QuaternionSafeLookRotation(aim.direction);
                info.crit = RollCrit();
                info.procChainMask = new();

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
}