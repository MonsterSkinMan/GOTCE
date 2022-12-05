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
                    BulletAttack bullet = new()
                    {
                        smartCollision = false,
                        radius = 0.5f,
                        damage = base.damageStat,
                        falloffModel = BulletAttack.FalloffModel.None,
                        damageType = DamageType.Generic,
                        aimVector = base.GetAimRay().direction,
                        minSpread = 0f,
                        maxSpread = 11f,
                        isCrit = base.RollCrit(),
                        procCoefficient = 1f,
                        hitEffectPrefab = hitEffectPrefab,
                        tracerEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.tracerEffectPrefab,
                        origin = base.characterBody.corePosition,
                        owner = base.gameObject
                    };

                    bullet.Fire();
                }
            }

            AkSoundEngine.PostEvent(4206201632, base.gameObject); // Play_bandit2_m1_shotgun
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