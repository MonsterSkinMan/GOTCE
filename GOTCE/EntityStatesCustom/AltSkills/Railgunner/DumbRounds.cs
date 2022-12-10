using RoR2;
using EntityStates;
using EntityStates.Railgunner.Weapon;
using UnityEngine;
using Unity;
using System;
using RoR2.Projectile;
using R2API;
using System.Reflection;

namespace GOTCE.EntityStatesCustom.AltSkills.Railgunner
{
    public class DumbRounds : BaseSkillState
    {
        private float duration = 0.02f;
        private float knockbackForce = 300f;
        private GameObject prefab;

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

        public override void OnEnter()
        {
            base.OnEnter();
            duration = duration / base.attackSpeedStat;
            prefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/RailgunnerPistolProjectile.prefab").WaitForCompletion().InstantiateClone("dumbrounds");
            GameObject.DestroyImmediate(prefab.GetComponent<ProjectileSteerTowardTarget>());

            AkSoundEngine.PostEvent(3663213371, base.gameObject); // Play_railgunner_m1_fire

            base.cameraTargetParams.AddRecoil(-10f, 10f, -50f, 50f);
            base.characterMotor.ApplyForce((0f - knockbackForce) * base.GetAimRay().direction);

            for (int i = 0; i < 5; i++) {
                float fovScale = 1 + gameObject.GetComponent<CameraTargetParams>().currentCameraParamsData.fov.alpha;
                float fovVal = 1 + gameObject.GetComponent<CameraTargetParams>().currentCameraParamsData.fov.value;
                FireProjectileInfo info = default;
                info.damage = base.damageStat;
                info.projectilePrefab = prefab;
                info.owner = base.gameObject;
                info.position = base.GetAimRay().origin;
                info.crit = base.RollCrit();
                info.damageTypeOverride = DamageType.Generic;
                info.speedOverride = 250f;

                float spreadRangeY = 5f * fovScale;
                float spreadRangeX = 5f * fovScale;

                if (fovVal != 0) { 
                    spreadRangeY += fovVal / UnityEngine.Random.Range(0f, 5f) * fovScale;
                    spreadRangeX += fovVal / UnityEngine.Random.Range(0f, 2f) * fovScale;
                }
                info.rotation = Util.QuaternionSafeLookRotation(Util.ApplySpread(base.GetAimRay().direction, spreadRangeY, -spreadRangeY, spreadRangeX, -spreadRangeX));

                if (base.isAuthority)
                {
                    ProjectileManager.instance.FireProjectile(info);
                    base.characterDirection.forward = base.GetAimRay().direction;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}