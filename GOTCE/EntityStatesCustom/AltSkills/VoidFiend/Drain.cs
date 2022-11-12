using RoR2;
using EntityStates;
using Unity;
using UnityEngine;
using System;

namespace GOTCE.EntityStatesCustom.AltSkills.VoidFiend {
    public class Drain : BaseSkillState {
        public float drained = 0f;

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (NetworkServer.active) {
                VoidSurvivorController controller = gameObject.GetComponent<VoidSurvivorController>();
                InputBankTest bank = gameObject.GetComponent<InputBankTest>();
                if (!bank.skill4.down || controller.corruption == controller.minimumCorruption) {
                    outer.SetNextStateToMain();
                }

                controller.AddCorruption(-25f * Time.fixedDeltaTime);
                drained += Mathf.Abs(-25f * Time.fixedDeltaTime);
                AkSoundEngine.PostEvent(3778899369, gameObject);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            BlastAttack blast = new();
            blast.attacker = gameObject;
            blast.radius = 2f * (1 + (drained * 0.1f));
            blast.baseDamage = base.damageStat * (1 + (drained * 0.1f));
            blast.baseForce = 2f * (drained / 4);
            blast.position = characterBody.corePosition;
            blast.attackerFiltering = AttackerFiltering.NeverHitSelf;
            blast.teamIndex = teamComponent.teamIndex;
            blast.damageType = DamageType.LunarSecondaryRootOnHit;
            blast.damageColorIndex = DamageColorIndex.Void;
            blast.falloffModel = BlastAttack.FalloffModel.None;
            blast.crit = drained > 50 ? true : RollCrit();
            blast.procCoefficient = 0.75f;

            if (NetworkServer.active) {
                blast.Fire();
                GameObject voidVFX = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/CritGlassesVoid/CritGlassesVoidExecuteEffect.prefab").WaitForCompletion();
                EffectManager.SpawnEffect(voidVFX, new EffectData
                {
                    origin = gameObject.transform.position,
                    scale = 2f * (1 + (drained * 0.1f)),
                }, true);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}