using RoR2;
using EntityStates;
using EntityStates.Railgunner.Weapon;
using UnityEngine;
using Unity;
using System;
using RoR2.Projectile;
using R2API;
using System.Reflection;

namespace GOTCE.EntityStatesCustom.AltSkills.Railgunner {
    public class DumbRounds : BaseSkillState
    {

        private float duration = 0.1f;
        private GameObject prefab;

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

        public override void OnEnter()
        {
            base.OnEnter();
            prefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/RailgunnerPistolProjectile.prefab").WaitForCompletion().InstantiateClone("dumbrounds");
            GameObject.DestroyImmediate(prefab.GetComponent<ProjectileSteerTowardTarget>());

            FireProjectileInfo info = default;
            info.damage = base.damageStat;
            info.projectilePrefab = prefab;
            info.owner = base.gameObject;
            info.position = base.characterBody.corePosition;
            info.crit = base.RollCrit();
            info.damageTypeOverride = DamageType.Generic;
            info.speedOverride = 250f;
            info.rotation = Util.QuaternionSafeLookRotation(Util.ApplySpread(base.GetAimRay().direction, -1.5f, 1.5f, -1.5f, 1.5f));

            if (base.isAuthority) {
                ProjectileManager.instance.FireProjectile(info);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}