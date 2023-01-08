using RoR2;
using EntityStates;
using System;
using Unity;
using UnityEngine;

namespace GOTCE.EntityStatesCustom.Providence
{
    public class Slash : BasicMeleeAttack
    {
        public override void OnEnter()
        {
            swingEffectPrefab = EntityStates.Merc.Weapon.GroundLight2.comboFinisherSwingEffectPrefab;
            baseDuration = 1.25f;
            damageCoefficient = 5f;
            hitBoxGroupName = "melee";

            base.OnEnter();

            AkSoundEngine.PostEvent(4028970009, base.gameObject); // Play_merc_m1_hard_swing
            base.characterDirection.forward = base.GetAimRay().direction;
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