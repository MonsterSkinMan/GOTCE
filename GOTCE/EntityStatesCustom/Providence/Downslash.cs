using RoR2;
using EntityStates;
using System;
using Unity;
using UnityEngine;
using RoR2.CharacterAI;

namespace GOTCE.EntityStatesCustom.Providence {
    public class Downslash : BaseSkillState {
        public float delay = 0.3f;
        public float duration = 1.6f;
        public bool hasSwung = false;
        public override void OnEnter()
        {
            base.OnEnter();
            if (base.characterBody.isPlayerControlled) {
                return;
            }
            else {
                if (base.characterBody.master) {
                    BaseAI ai = base.characterBody.masterObject.GetComponent<BaseAI>();
                    if (ai) {
                        GameObject target = ai.currentEnemy.gameObject;
                        if (target) {
                            TeleportHelper.TeleportBody(base.characterBody, target.transform.position + new Vector3(0, 2, 0));
                        }
                    }
                }
            }

            AkSoundEngine.PostEvent(1820467490, base.gameObject); // Play_moonBrother_blueWall_explode
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= delay && !hasSwung) {
                if (NetworkServer.active) {
                    OverlapAttack attack = new();
                    attack.damage = 9.2f * base.damageStat;
                    attack.procCoefficient = 1f;
                    attack.damageType = DamageType.Generic;
                    attack.attacker = base.gameObject;
                    attack.hitBoxGroup = base.FindHitBoxGroup("downslash");
                    attack.Fire();
                }
                hasSwung = true;
                AkSoundEngine.PostEvent(4028970009, base.gameObject); // Play_merc_m1_hard_swing
            }

            if (base.fixedAge >= duration) {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}