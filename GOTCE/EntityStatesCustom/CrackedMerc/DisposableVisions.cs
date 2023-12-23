using RoR2;
using EntityStates;
using R2API;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using Rewired.ComponentControls.Effects;
using EntityStates.Merc.Weapon;

namespace GOTCE.EntityStatesCustom.CrackedMerc {
    public class DisposableVisions : BaseState {
        public float damageCoeff = 3f;
        public float duration = 0.2f;

        public override void OnEnter()
        {
            base.OnEnter();
            PlayCrossfade("Gesture, Additive", "GroundLight1", "GroundLight.playbackRate", duration * 4f, 0.05f);
            PlayCrossfade("Gesture, Override", "GroundLight1", "GroundLight.playbackRate", duration * 4f, 0.05f);

            duration /= base.attackSpeedStat;

            if (base.isAuthority)
            {
                MissileUtils.FireMissile(base.transform.position, characterBody, default, null, damageStat * damageCoeff, base.RollCrit(), Utils.Paths.GameObject.MissileProjectile.Load<GameObject>(), DamageColorIndex.Default, base.inputBank.aimDirection, 200f, false);
            }

            AkSoundEngine.PostEvent(Events.Play_merc_m1_hard_swing, base.gameObject);
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
            return InterruptPriority.PrioritySkill;
        }
    }
}