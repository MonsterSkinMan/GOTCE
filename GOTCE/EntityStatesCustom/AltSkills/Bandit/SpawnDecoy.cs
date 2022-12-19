using RoR2;
using EntityStates;
using System;
using RoR2.Projectile;
using Unity;
using R2API.Networking.Interfaces;
using UnityEngine;

namespace GOTCE.EntityStatesCustom.AltSkills.Bandit
{
    public class SpawnDecoy : BaseSkillState
    {
        public float duration = 0.3f;

        public override void OnEnter()
        {
            base.OnEnter();
            AkSoundEngine.PostEvent(850833398, base.gameObject); // Play_bandit2_shift_exit
            PlayAnimation("Gesture, Additive", "ThrowSmokebomb", "ThrowSmokebomb.playbackRate", duration);
            if (base.isAuthority)
            {
                new DecoySync(base.gameObject).Send(R2API.Networking.NetworkDestination.Server);
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