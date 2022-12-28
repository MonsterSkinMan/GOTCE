using RoR2;
using EntityStates;
using Unity;
using UnityEngine;
using System;

namespace GOTCE.EntityStatesCustom.AltSkills.MULT {
    public class Scorch : BaseState {
        private GameObject flamePrefab => Addressables.LoadAssetAsync<GameObject>(Utils.Paths.GameObject.DroneFlamethrowerEffect).WaitForCompletion();
        private float totalDamageCoeff = 15f;
        private float baseDuration = 1.3f;
        private int ticks = 15;
        private float duration;
        private float stopwatch = 0f;
        private float delay;
        private float damagePerTick;
        private bool isCrit;
        private GameObject flamethrowerInstance;
        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / base.attackSpeedStat;
            damagePerTick = totalDamageCoeff / ticks;
            delay = duration / ticks;
            isCrit = base.RollCrit();

            flamethrowerInstance = GameObject.Instantiate(flamePrefab, FindModelChild("Head"));
            flamethrowerInstance.transform.forward = base.GetAimRay().direction;
            flamethrowerInstance.transform.localScale += new Vector3(0, 0, 1.4f);
            
            AkSoundEngine.PostEvent(Utils.Events.Play_lemurianBruiser_m2_shoot, base.gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            flamethrowerInstance.transform.forward = base.GetAimRay().direction;

            if (base.isAuthority) {
                stopwatch += Time.fixedDeltaTime;

                if (stopwatch >= delay) {
                    stopwatch = 0f;

                    BulletAttack attack = new();
                    attack.damage = damagePerTick * base.damageStat;
                    attack.owner = base.gameObject;
                    attack.weapon = base.gameObject;
                    attack.maxDistance = 18f;
                    attack.damageType = DamageType.IgniteOnHit;
                    attack.radius = 2f;
                    attack.smartCollision = true;
                    attack.origin = base.GetAimRay().origin;
                    attack.stopperMask = LayerIndex.world.mask;
                    attack.isCrit = isCrit;
                    attack.aimVector = base.GetAimRay().direction;
                    attack.procCoefficient = 1f;
                    attack.force = 0f;
                    attack.Fire();
                }
            }

            if (base.fixedAge >= duration) {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnExit()
        {
            base.OnExit();
            Destroy(flamethrowerInstance);
            AkSoundEngine.PostEvent(Utils.Events.Play_lemurianBruiser_m2_end, base.gameObject);
        }
    }
}