using RoR2;
using EntityStates;
using EntityStates.Treebot.Weapon;
using UnityEngine;
using Unity;
using System;
using RoR2.Projectile;
using R2API;
using UnityEngine.Networking;

namespace GOTCE.EntityStatesCustom.AltSkills.Rex
{
    public class SigmaShotgun : BaseState
    {
        private float duration = 0.75f;
        private int count = 9;
        private GameObject proj = FireSyringe.projectilePrefab.InstantiateClone("sigmaneedle");

        public override void OnEnter()
        {
            base.OnEnter();
            duration = duration / base.attackSpeedStat;
            Ray ray = base.GetAimRay();
            CharacterBody body = base.characterBody;
            proj.GetComponent<ProjectileController>().procCoefficient = 0.6725f;

            AkSoundEngine.PostEvent(1706423866, base.gameObject); // Play_treeBot_m1_shoot
            for (int i = 0; i < count; i++)
            {
                FireProjectileInfo info = default;
                info.damage = base.damageStat * 0.5f;
                info.projectilePrefab = proj;
                info.position = body.corePosition + new Vector3(0, 2, 0);
                info.speedOverride = 350f;
                info.crit = base.RollCrit();
                info.damageColorIndex = DamageColorIndex.Poison;
                info.owner = base.gameObject;
                info.damageTypeOverride = DamageType.WeakOnHit;
                info.rotation = Util.QuaternionSafeLookRotation(Util.ApplySpread(ray.direction, -2.5f, 2.5f, -2.5f, 2.5f));

                if (base.isAuthority)
                {
                    ProjectileManager.instance.FireProjectile(info);
                }
            }
            if (NetworkServer.active && base.healthComponent && 0.05f >= Mathf.Epsilon)
            {
                DamageInfo selfdamage = new()
                {
                    damage = base.healthComponent.combinedHealth * 0.05f,
                    damageType = DamageType.NonLethal,
                    position = base.characterBody.corePosition,
                    attacker = null,
                    inflictor = null,
                    crit = false,
                    force = Vector3.zero,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 0f
                };
                base.healthComponent.TakeDamage(selfdamage);
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