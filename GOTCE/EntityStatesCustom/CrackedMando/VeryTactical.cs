using RoR2;
using EntityStates;
using R2API;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using EntityStates.Commando.CommandoWeapon;

namespace GOTCE.EntityStatesCustom.CrackedMando
{
    public class VeryTactical : BaseState
    {
        public float duration = 5f;
        public CharacterMotor motor;
        public ICharacterFlightParameterProvider flightprov;
        public ICharacterGravityParameterProvider gravprov;
        public bool isFlying = false;

        public override void OnEnter()
        {
            base.OnEnter();
            motor = base.characterMotor;
            motor.useGravity = false;
            base.characterDirection.forward = base.GetAimRay().direction;
            motor.isFlying = true;
            PlayAnimation("FullBody, Override", "Slide", "Slide.playbackRate", duration);
            //Debug.Log("starting flight");
            isFlying = true;
            CriticalTypes.OnSprintCrit?.Invoke(gameObject, new(base.characterBody));
            base.characterBody.RecalculateStats();

            AkSoundEngine.PostEvent(4030773325, base.gameObject); // Play_commando_shift

            base.characterBody.AddTimedBuffAuthority(Buffs.TacticalSpeed.instance.BuffDef.buffIndex, 5f);
        }

        public override void OnExit()
        {
            base.OnExit();
            motor.useGravity = true;
            motor.isFlying = false;
            isFlying = false;
            base.characterBody.RecalculateStats();
            //Debug.Log("stopping flight");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            motor.useGravity = false;
            motor.isFlying = true;
            if (base.fixedAge >= duration)
            {
                motor.useGravity = true;
                motor.isFlying = false;
                outer.SetNextStateToMain();
                //Debug.Log("ending skill");
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}