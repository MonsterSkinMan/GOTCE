using RoR2;
using EntityStates;
using R2API;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;

namespace GOTCE.EntityStatesCustom.NaeNaeLord
{
    public class NaeNaeUtil : BaseState
    {
        private float duration = 5f;

        public override void OnEnter()
        {
            base.OnEnter();
            if (NetworkServer.active)
            {
                base.characterBody.inventory.GiveItem(RoR2Content.Items.Bear, 10);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (NetworkServer.active)
            {
                base.characterBody.inventory.RemoveItem(RoR2Content.Items.Bear, base.characterBody.inventory.GetItemCount(RoR2Content.Items.Bear));
            }
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