using RoR2;
using EntityStates;
using Unity;
using UnityEngine;
using System;

namespace GOTCE.EntityStatesCustom.AltSkills.VoidFiend
{
    public class Drain : BaseSkillState
    {
        public float drained = 0f;

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority)
            {
                VoidSurvivorController controller = gameObject.GetComponent<VoidSurvivorController>();
                InputBankTest bank = gameObject.GetComponent<InputBankTest>();
                if (!bank.skill4.down || controller.corruption == controller.minimumCorruption)
                {
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
            if (base.isAuthority)
            {
                BlastAttack blast = new()
                {
                    attacker = gameObject,
                    radius = 2f * (1 + (drained * 0.1f)),
                    baseDamage = base.damageStat * (1 + (drained * 0.1f)),
                    baseForce = 2f * (drained / 4),
                    position = characterBody.corePosition,
                    attackerFiltering = AttackerFiltering.NeverHitSelf,
                    teamIndex = teamComponent.teamIndex,
                    damageType = DamageType.LunarSecondaryRootOnHit,
                    damageColorIndex = DamageColorIndex.Void,
                    falloffModel = BlastAttack.FalloffModel.None,
                    crit = drained > 50 ? true : RollCrit(),
                    procCoefficient = 0.75f
                };
                blast.Fire();
            }
            GameObject voidVFX = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/CritGlassesVoid/CritGlassesVoidExecuteEffect.prefab").WaitForCompletion();
            EffectManager.SpawnEffect(voidVFX, new EffectData
            {
                origin = gameObject.transform.position,
                scale = 2f * (1 + (drained * 0.1f)),
            }, true);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}