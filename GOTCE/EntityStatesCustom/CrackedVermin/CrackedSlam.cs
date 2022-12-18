using RoR2;
using Unity;
using UnityEngine;
using System;
using EntityStates;

namespace GOTCE.EntityStatesCustom.CrackedVermin {
    public class CrackedSlam : BaseSkillState {
        private float duration = 0.7f;
        public override void OnEnter()
        {
            base.OnEnter();

            duration = 0.7f / base.attackSpeedStat;

            if (base.isAuthority) {
                BlastAttack attack = new();
                attack.radius = 10f;
                attack.attacker = base.gameObject;
                attack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                attack.baseDamage = base.damageStat * 12f;
                attack.damageColorIndex = DamageColorIndex.Default;
                attack.damageType = DamageType.AOE;
                attack.baseForce = 3f;
                attack.crit = base.RollCrit();
                attack.falloffModel = BlastAttack.FalloffModel.None;
                attack.inflictor = base.gameObject;
                attack.procCoefficient = 1f;
                attack.procChainMask = new();
                attack.teamIndex = base.teamComponent.teamIndex;
                attack.impactEffect = EffectCatalog.FindEffectIndexFromPrefab(EntityStates.Loader.GroundSlam.blastImpactEffectPrefab);

                attack.Fire();
            }

            EffectManager.SpawnEffect(
                effectPrefab: EntityStates.Loader.GroundSlam.blastEffectPrefab,
                effectData: new EffectData {
                    scale = 10f,
                    rotation = Quaternion.identity,
                    origin = base.characterBody.corePosition
                },
                transmit: true
            );

            AkSoundEngine.PostEvent(2640687082, base.gameObject); // Play_loader_R_variant_slam
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration) {
                if (base.isAuthority) {
                    base.inputBank.moveVector = Vector3.zeroVector;
                }
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}