using RoR2;
using EntityStates;
using UnityEngine;
using Unity;
using System;
using R2API;

namespace GOTCE.EntityStatesCustom.AltSkills.Commando
{
    public class SuperShotgun : BaseState
    {
        public float duration = 1.63f;
        public int bulletsTotal = 20;
        public GameObject hitEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/BulletImpactSoft.prefab").WaitForCompletion();

        public override void OnEnter()
        {
            base.OnEnter();
            duration = duration / base.attackSpeedStat;
            if (base.isAuthority)
            {
                bulletsTotal = 20;
                for (int i = 0; i < bulletsTotal; i++)
                {
                    BulletAttack bullet = new();
                    bullet.smartCollision = false;
                    bullet.radius = 0.5f;
                    bullet.damage = base.damageStat;
                    bullet.falloffModel = BulletAttack.FalloffModel.None;
                    bullet.damageType = DamageType.Generic;
                    bullet.aimVector = base.GetAimRay().direction;
                    bullet.minSpread = 0f;
                    bullet.maxSpread = 11f;
                    bullet.isCrit = base.RollCrit();
                    bullet.procCoefficient = 1f;
                    bullet.hitEffectPrefab = hitEffectPrefab;
                    bullet.tracerEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.tracerEffectPrefab;
                    bullet.origin = base.characterBody.corePosition;
                    bullet.owner = base.gameObject;

                    bullet.Fire();
                }
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
}
