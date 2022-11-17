using RoR2;
using EntityStates;
using System;
using UnityEngine;
using Unity;

namespace GOTCE.EntityStatesCustom.The {
    public class TheDeath : BaseState {
        public float duration = 2f;
        public override void FixedUpdate()
        {
            if (base.fixedAge >= duration) {
                outer.SetNextStateToMain();
            }
        }

        public override void OnEnter()
        {
            if (NetworkServer.active) {
                healthComponent.health = healthComponent.fullHealth;
                PlayAnimation("Body", "Death");
            }
        }

        public override void OnExit()
        {
            
        }
    }
}