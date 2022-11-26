using RoR2;
using Unity;
using UnityEngine;
using System;
using R2API;
using EntityStates;
using RoR2.Projectile;

namespace GOTCE.EntityStatesCustom.AltSkills.VoidFiend
{
    public class PearlTeleport : BaseSkillState
    {
        public float duration = 0.5f;

        public override void OnEnter()
        {
            base.OnEnter();
            ViendPearlManager manager = characterBody.gameObject.GetComponent<ViendPearlManager>();
            AkSoundEngine.PostEvent(4021527550, base.gameObject); // Play_voidman_m2_shoot_fullCharge
            if (manager && NetworkServer.active)
            {
                manager.Swap();
            }
            else
            {
                Main.ModLogger.LogFatal("PearlTeleport was run when the player does not have a ViendPearlManager component!");
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration)
            {
                outer.SetNextStateToMain();
                if (NetworkServer.active)
                {
                    characterBody.skillLocator.secondary.UnsetSkillOverride(gameObject, Skills.PearlTeleport.Instance.SkillDef, GenericSkill.SkillOverridePriority.Replacement);
                }
            }
        }
    }
}