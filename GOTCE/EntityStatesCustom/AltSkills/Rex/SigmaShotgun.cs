using RoR2;
using EntityStates;
using EntityStates.Treebot.Weapon;
using UnityEngine;
using Unity;
using System;
using RoR2.Projectile;
using R2API;

namespace GOTCE.EntityStatesCustom.AltSkills.Rex {
    public class SigmaShotgun : BaseState {
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
            for (int i = 0; i < count; i++) {
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

                if (base.isAuthority) {
                    ProjectileManager.instance.FireProjectile(info);
                }
            }

            DamageInfo selfdamage = default;
            selfdamage.damage = base.healthComponent.combinedHealth * 0.05f; // why is 0.05f, a literal float, null??????
            selfdamage.damageType = DamageType.NonLethal;
            selfdamage.position = base.characterBody.corePosition;
            selfdamage.attacker = base.gameObject;
            selfdamage.procCoefficient = 0f;

            if (base.isAuthority) {
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