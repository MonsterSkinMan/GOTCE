using RoR2;
using EntityStates;
using System;
using RoR2.Projectile;
using Unity;
using UnityEngine;

namespace GOTCE.EntityStatesCustom.AltSkills.Bandit
{
    public class SpawnDecoy : BaseSkillState
    {
        public float duration = 0.8f;

        public override void OnEnter()
        {
            base.OnEnter();
            AkSoundEngine.PostEvent(850833398, base.gameObject); // Play_bandit2_shift_exit
            PlayAnimation("Gesture, Additive", "ThrowSmokebomb", "ThrowSmokebomb.playbackRate", duration);
            if (base.isAuthority)
            {
                try
                {
                    MasterSummon masterSummon2 = new()
                    {
                        position = base.characterBody.corePosition,
                        ignoreTeamMemberLimit = true,
                        masterPrefab = Enemies.Standard.ExplodingDecoy.Instance.prefabMaster,
                        summonerBodyObject = base.characterBody.gameObject,
                        rotation = Quaternion.LookRotation(base.characterBody.transform.forward)
                    };
                    masterSummon2.Perform();
                }
                catch
                {
                }
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