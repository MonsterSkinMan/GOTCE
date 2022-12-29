using RoR2;
using EntityStates;
using Unity;
using UnityEngine;
using System;
using R2API;
using RoR2.Projectile;

namespace GOTCE.EntityStatesCustom.AltSkills.MULT {
    public class Hook : BaseState {
        private GameObject prefab => Main.SecondaryAssets.LoadAsset<GameObject>("Assets/Prefabs/Projectiles/AltSkills/Hook/Hook.prefab");
        private float damageCoeff = 6f;
        private HookController instance = null;
        private bool hasHooked = false;
        public static float pullForce = 60f;

        public override void OnEnter() {
            base.OnEnter();
            Ray aim = base.GetAimRay();
            AkSoundEngine.PostEvent(Events.Play_gravekeeper_attack2_shoot, base.gameObject);

            if (base.isAuthority) {
                if (!prefab.GetComponent<HookController>()) {
                    prefab.AddComponent<HookController>();
                } 

                FireProjectileInfo info = new();
                info.damage = base.damageStat * damageCoeff;
                info.crit = base.RollCrit();
                info.owner = base.gameObject;
                info.position = base.FindModelChild("MuzzleNailgun").position;
                info.procChainMask = new();
                info.rotation = Util.QuaternionSafeLookRotation(aim.direction);
                info.projectilePrefab = prefab;
                
                ProjectileManager.instance.FireProjectile(info);
            }
        }

        public override void FixedUpdate() {
            if (base.isAuthority) {
                if ((!instance && hasHooked)) {
                    outer.SetNextStateToMain();
                }
                if (instance && !base.inputBank.skill3.down) {
                    instance.CancelPull();
                    outer.SetNextStateToMain();
                }
                if (!instance && !base.inputBank.skill3.down) {
                    outer.SetNextStateToMain();
                };
            }
        }

        public void AssignInstance(GameObject _instance) {
            instance = _instance.GetComponent<HookController>();
            instance.GetComponent<ChildLocator>().FindChild("StartTransform").parent = base.FindModelChild("MuzzleNailgun");
        }

        public override void OnExit() {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Skill;
        }

        private class HookController : MonoBehaviour {
            private ChildLocator childLocator => base.GetComponent<ChildLocator>();
            private ProjectileStickOnImpact stickOnImpact => base.GetComponent<ProjectileStickOnImpact>();
            private bool shouldPull = true;
            private Transform target;
            private GameObject owner => base.GetComponent<ProjectileController>().owner;

            public void CancelPull() {
                if (owner.GetComponent<CharacterMotor>()) {
                    owner.GetComponent<CharacterMotor>().useGravity = true;
                }
                shouldPull = false;
                GameObject.DestroyImmediate(base.gameObject);
            }

            private void Start() {
                stickOnImpact.stickEvent.AddListener(Stuck);
                EntityStateMachine machine = null;
                Hook hookState;
                if (owner) {
                    foreach (EntityStateMachine smachine in owner.GetComponents<EntityStateMachine>()) {
                        if (smachine.customName == "Weapon") {
                            machine = smachine;
                        }
                    }
                }

                if ((hookState = machine.state as Hook) != null) {
                    hookState.AssignInstance(base.gameObject);
                }

            }

            private void Stuck() {
                target = stickOnImpact.rigidbody.transform;
            }

            private void FixedUpdate() {
                if (shouldPull && target) {
                    Vector3 force = (owner.transform.position - target.position).normalized * -(pullForce * Time.fixedDeltaTime);

                    if (Vector3.Distance(owner.transform.position, target.position) < 5) {
                        CancelPull();
                        return;
                    }

                    if (owner.GetComponent<CharacterMotor>()) {
                        if (!owner.GetComponent<InputBankTest>().skill3.down) {
                            CancelPull();
                            return;
                        }
                        PhysForceInfo info = new();
                        info.force = force;
                        info.ignoreGroundStick = true;
                        info.disableAirControlUntilCollision = true;
                        info.massIsOne = true;
                        owner.GetComponent<CharacterMotor>().ApplyForceImpulse(in info);
                        owner.GetComponent<CharacterMotor>().useGravity = false;
                    }

                }
            }
        }
    }
}