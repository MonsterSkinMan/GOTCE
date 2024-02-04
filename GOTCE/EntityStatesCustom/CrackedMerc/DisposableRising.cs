using RoR2;
using EntityStates;
using R2API;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using Rewired.ComponentControls.Effects;
using EntityStates.Merc.Weapon;
using EntityStates.Merc;
using HG;

namespace GOTCE.EntityStatesCustom.CrackedMerc {
    public class DisposableRising : Uppercut {
        public float missileTimer = 0f;
        public override void OnEnter()
        {
            DisposableRising.baseDamageCoefficient = 3f;
            base.OnEnter();
            base.overlapAttack.damageType = DamageType.ApplyMercExpose;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            missileTimer -= Time.fixedDeltaTime;
            if (missileTimer <= 0f)
            {
                FireMissile();
                missileTimer = 0.05f;
            }
        }

        public void FireMissile() {
            if (base.isAuthority)
            {
                MissileUtils.FireMissile(base.transform.position, characterBody, default, null, damageStat * DisposableRising.baseDamageCoefficient, base.RollCrit(), Utils.Paths.GameObject.MissileProjectile.Load<GameObject>(), DamageColorIndex.Default, false);
            }
        }

        [RunMethod(RunAfter.Start)]
        public static void SetupESC() {
            EntityStateConfiguration configuration = UnityEngine.Object.Instantiate(Utils.Paths.EntityStateConfiguration.EntityStatesMercUppercut.Load<EntityStateConfiguration>());
            configuration.targetType = (SerializableSystemType)typeof(DisposableRising);
            ContentAddition.AddEntityStateConfiguration(configuration);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}