using RoR2;
using EntityStates;
using System;
using UnityEngine;
using Unity;

namespace GOTCE.EntityStatesCustom.The
{
    public class TheHurtState : BaseState
    {
        public float duration = 3f;

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration)
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (NetworkServer.active)
            {
                healthComponent.health = healthComponent.fullHealth;
                PlayAnimation("Body", "Death");
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}