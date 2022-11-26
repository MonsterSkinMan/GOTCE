using RoR2;
using Unity;
using EntityStates;
using System;
using UnityEngine;

namespace GOTCE.EntityStatesCustom.AltSkills.Captain
{
    public class Overheat : BaseSkillState
    {
        public float duration = 2f;
        public float bulletsTotal = 20;
        public float secondsCharged = 0f;
        public GameObject hitEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/BulletImpactSoft.prefab").WaitForCompletion();

        public override void OnEnter()
        {
            base.OnEnter();
            if (base.isAuthority)
            {
                bulletsTotal = Mathf.CeilToInt(bulletsTotal *= base.attackSpeedStat);
            }
            PlayCrossfade("Gesture, Override", "ChargeCaptainShotgun", "ChargeCaptainShotgun.playbackRate", duration, 0.1f);
            PlayCrossfade("Gesture, Additive", "ChargeCaptainShotgun", "ChargeCaptainShotgun.playbackRate", duration, 0.1f);

            AkSoundEngine.PostEvent(2544146878, base.gameObject); // Play_captain_m1_chargeStart
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && inputBank.skill1.down)
            {
                secondsCharged += Time.fixedDeltaTime;
            }
            else if (!inputBank.skill1.down)
            {
                outer.SetNextStateToMain();
            }

            if (base.fixedAge >= duration)
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            PlayAnimation("Gesture, Additive", "FireCaptainShotgun");
            PlayAnimation("Gesture, Override", "FireCaptainShotgun");
            if (base.isAuthority)
            {
                bulletsTotal = Mathf.Clamp(bulletsTotal * secondsCharged, 5, int.MaxValue);
                AkSoundEngine.PostEvent(532604026, base.gameObject); // Play_captain_m1_shootWide
                for (int i = 0; i < bulletsTotal; i++)
                {
                    BulletAttack bullet = new()
                    {
                        smartCollision = false,
                        radius = 0.3f,
                        damage = base.damageStat * 0.6f,
                        falloffModel = BulletAttack.FalloffModel.Buckshot,
                        damageType = DamageType.IgniteOnHit,
                        aimVector = base.GetAimRay().direction,
                        minSpread = 0f,
                        maxSpread = Mathf.Lerp(0, 4, secondsCharged),
                        isCrit = base.RollCrit(),
                        hitEffectPrefab = hitEffectPrefab,
                        tracerEffectPrefab = EntityStates.Commando.CommandoWeapon.FireBarrage.tracerEffectPrefab,
                        origin = base.characterBody.corePosition,
                        owner = base.gameObject
                    };

                    bullet.Fire();
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}