using RoR2;
using EntityStates;
using R2API;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;

namespace GOTCE.EntityStatesCustom.NaeNaeLord {
    public class NaeNaeWhip : BaseSkillState {
        float delay = 0.2f;
        public Ray ray;
        public override void OnEnter()
        {
            base.OnEnter();
            ray = base.GetAimRay();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= delay) {
                if (NetworkServer.active) {
                    FireWhip();
                }
                outer.SetNextStateToMain();
            }
        }

        public void FireWhip() {
            BulletAttack whip = new();
            whip.damage = base.damageStat * 2f;
            whip.radius = 2.5f;
            whip.maxDistance = 240f;
            whip.tracerEffectPrefab = EntityStates.Commando.CommandoWeapon.FireBarrage.tracerEffectPrefab;
            whip.isCrit = Util.CheckRoll(base.characterBody.crit, 0);
            whip.smartCollision = true;
            whip.damageColorIndex = DamageColorIndex.WeakPoint;
            whip.maxSpread = 0f;
            whip.minSpread = 0f;
            whip.procChainMask = default;
            whip.owner = base.gameObject;
            whip.origin = base.transform.position;
            whip.weapon = base.gameObject;
            whip.aimVector = ray.direction;
            whip.AddModdedDamageType(Main.root);
            whip.Fire();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}