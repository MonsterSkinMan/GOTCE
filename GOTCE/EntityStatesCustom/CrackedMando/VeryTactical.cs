using RoR2;
using EntityStates;
using R2API;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using EntityStates.Commando.CommandoWeapon;

namespace GOTCE.EntityStatesCustom.CrackedMando {
    public class VeryTactical : BaseState {
        public float duration = 5f;
        public CharacterMotor motor;
        public ICharacterFlightParameterProvider flightprov;
        public ICharacterGravityParameterProvider gravprov;

        public override void OnEnter()
        {
            base.OnEnter();
            motor = base.characterMotor;
            motor.useGravity = false;
            motor.isFlying = true;
            PlayAnimation("Body", "SlideForward", "SlideForward.playbackRate", duration);
            //Debug.Log("starting flight");
        }

        public override void OnExit()
        {
            base.OnExit();
            motor.useGravity = true;
            motor.isFlying = false;
            //Debug.Log("stopping flight");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            motor.useGravity = false;
            motor.isFlying = true;
            if (base.fixedAge >= duration) {
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