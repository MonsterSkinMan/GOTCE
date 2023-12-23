using RoR2;
using EntityStates;
using R2API;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using Rewired.ComponentControls.Effects;
using EntityStates.Merc.Weapon;

namespace GOTCE.EntityStatesCustom.CrackedMerc {
    public class DisposableEgg : BaseState {
        public float damageCoeff = 1000f;
        public float duration = 10f;
        public GameObject prefab => Utils.Paths.GameObject.MissileGhost.Load<GameObject>();
        public GameObject missileInstance;
        public GameObject model;
        public float colCheckTimer = 0f;
        public override void OnEnter()
        {
            base.OnEnter();
            EffectManager.SimpleEffect(Utils.Paths.GameObject.ExplosionSolarFlare.Load<GameObject>(), base.transform.position, Quaternion.identity, false);
            model = base.GetModelTransform().gameObject;
            model.SetActive(false);
            characterMotor._flightParameters.channeledFlightGranterCount++;
            characterMotor._gravityParameters.channeledAntiGravityGranterCount++;
            characterMotor.isFlying = true;
            characterMotor.useGravity = false;

            characterBody.baseMoveSpeed *= 3f;
            characterBody.statsDirty = true;

            missileInstance = UnityEngine.Object.Instantiate<GameObject>(prefab);
            missileInstance.GetComponent<ProjectileGhostController>().enabled = false;
            missileInstance.transform.localScale *= 5f;

            colCheckTimer = 0.25f;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= duration) {
                Detonate();
            }

            colCheckTimer -= Time.fixedDeltaTime;
            if (colCheckTimer <= 0f) {
                colCheckTimer = 0.1f;
                Collider[] array = Physics.OverlapSphere(characterBody.corePosition, 1.5f, LayerIndex.defaultLayer.mask | LayerIndex.world.mask);

                foreach (Collider collider in array) {
                    if (collider.gameObject != base.gameObject) {
                        Detonate();
                    }
                }
            }

            missileInstance.transform.position = characterBody.corePosition;
            missileInstance.transform.forward = characterBody.inputBank.aimDirection;
        }

        public void Detonate() {
            BlastAttack blastAttack = new BlastAttack();
            blastAttack.radius = 20f;
            blastAttack.procCoefficient = 5f;
            blastAttack.position = characterBody.corePosition;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = base.RollCrit();
            blastAttack.baseDamage = damageStat * damageCoeff;
            blastAttack.baseForce = 0f;
            blastAttack.teamIndex = base.GetTeam();
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
            blastAttack.losType = BlastAttack.LoSType.None;

            EffectManager.SpawnEffect(Utils.Paths.GameObject.OmniExplosionVFX.Load<GameObject>(), new EffectData() {
                origin = characterBody.corePosition,
                scale = 20f
            }, false);
            
            if (NetworkServer.active) {
                blastAttack.Fire();
            }

            outer.SetNextStateToMain();
        }

        public override void OnExit()
        {
            base.OnExit();
            characterMotor._flightParameters.channeledFlightGranterCount--;
            characterMotor._gravityParameters.channeledAntiGravityGranterCount--;
            characterMotor.isFlying = false;
            characterMotor.useGravity = true;
            characterBody.baseMoveSpeed /= 3f;
            characterBody.statsDirty = true;
            UnityEngine.Object.Destroy(missileInstance);
            model.SetActive(true);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}