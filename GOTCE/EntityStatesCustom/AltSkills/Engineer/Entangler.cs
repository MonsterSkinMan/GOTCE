using RoR2;
using EntityStates;
using Unity;
using UnityEngine;
using RoR2.CharacterAI;
using R2API.Networking.Interfaces;

namespace GOTCE.EntityStatesCustom.AltSkills.Engineer
{
    public class Entangler : BaseSkillState
    {
        private IEnumerable<CharacterMaster> minions;
        private float delay = 0.05f;
        private float stopwatch = 0f;

        public override void OnEnter()
        {
            base.OnEnter();
            CharacterMaster owner = base.characterBody.master;
            minions = CharacterMaster.readOnlyInstancesList.Where(x => x.minionOwnership && x.minionOwnership.ownerMaster == owner && x.GetBody() && (x.GetBody().bodyFlags.HasFlag(CharacterBody.BodyFlags.Mechanical)));

            foreach (CharacterMaster minion in minions)
            {
                if (minion.GetBody() && base.isAuthority)
                {
                    if (!minion.GetComponent<EntanglerController>())
                    {
                        new EntanglerSync(gameObject, minion.gameObject).Send(R2API.Networking.NetworkDestination.Server);
                    }
                }
            }

            new EntanglerControlSync(gameObject, true).Send(R2API.Networking.NetworkDestination.Server);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!inputBank.skill1.down && base.isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            CharacterMaster owner = base.characterBody.master;
            new EntanglerControlSync(gameObject, false).Send(R2API.Networking.NetworkDestination.Server);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}