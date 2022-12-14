using RoR2;
using EntityStates;
using System;
using Unity;
using UnityEngine;
using RoR2.CharacterAI;
using RoR2.Projectile;

namespace GOTCE.EntityStatesCustom.Providence
{
    public class Shockwave : BaseSkillState
    {
        public float duration = 2f;

        public override void OnEnter()
        {
            base.OnEnter();
            if (base.isAuthority)
            {
                FireRingAuthority();
                base.characterBody.AddTimedBuff(RoR2Content.Buffs.LunarSecondaryRoot, 2f);
            }

            AkSoundEngine.PostEvent(1820467490, base.gameObject); // Play_moonBrother_blueWall_explode
        }

        private void FireRingAuthority()
        {
            float num = 360f / 8;
            Vector3 vector = Vector3.ProjectOnPlane(base.inputBank.aimDirection, Vector3.up);
            Vector3 footPosition = base.characterBody.footPosition;
            for (int i = 0; i < 8; i++)
            
            {
                Vector3 forward = Quaternion.AngleAxis(num * i, Vector3.up) * vector;
                if (base.isAuthority)
                {
                    FireProjectileInfo info = new();
                    info.projectilePrefab = EntityStates.BrotherMonster.ExitSkyLeap.waveProjectilePrefab;
                    info.position = footPosition;
                    info.rotation = Util.QuaternionSafeLookRotation(forward);
                    info.owner = base.gameObject;
                    info.damage = base.characterBody.damage * 6f;
                    info.force = 3.2f;
                    info.crit = base.RollCrit();
                    info.speedOverride = 75f;
                    info.useSpeedOverride = true;
                    ProjectileManager.instance.FireProjectile(info);
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