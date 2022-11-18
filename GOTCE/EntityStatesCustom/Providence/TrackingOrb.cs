using RoR2;
using EntityStates;
using System;
using Unity;
using UnityEngine;
using RoR2.CharacterAI;
using RoR2.Projectile;

namespace GOTCE.EntityStatesCustom.Providence {
    public class TrackingOrb : BaseSkillState {
        public float delay = 0.3f;
        public float duration = 2f;
        public float stopwatch = 0f;
        public float stopwatchFire = 0f;
        public float delayFire = 0f;
        public bool shouldFire = false;
        public Vector3 pos;
        public GameObject prefab = Main.SecondaryAssets.LoadAsset<GameObject>("Assets/Prefabs/Enemies/Provi/Projectile/ProviOrb.prefab");
        public override void OnEnter()
        {
            base.OnEnter();
            if (base.characterBody.isPlayerControlled) {
                return;
            }
            if (NetworkServer.active) {
                base.characterBody.AddTimedBuff(RoR2Content.Buffs.LunarSecondaryRoot, 2f);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && !base.characterBody.isPlayerControlled) {
                stopwatch += Time.fixedDeltaTime;

                if (stopwatch >= delay) {
                    stopwatch = 0f;
                    shouldFire = true;
                    BaseAI ai = base.characterBody.masterObject.GetComponent<BaseAI>();
                    if (ai) {
                        CharacterBody target = ai.currentEnemy.characterBody;
                        if (target) {
                            SetPos(target.previousPosition);
                        }
                    }
                }
            }

            if (base.isAuthority && shouldFire) {
                stopwatchFire += Time.fixedDeltaTime;
                if (stopwatchFire >= delay) {
                    stopwatchFire = 0f;
                    BaseAI ai = base.characterBody.masterObject.GetComponent<BaseAI>();
                    if (ai) {
                        if (pos != null) {
                            Fire();
                        }
                    }
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

        private void Fire() {
            if (base.isAuthority) {
                FireProjectileInfo info = new();
                info.speedOverride = 0f;
                info.position = pos;
                info.damage = base.damageStat;
                info.crit = base.RollCrit();
                info.rotation = Quaternion.identity;
                info.owner = base.gameObject;
                info.projectilePrefab = prefab;

                ProjectileManager.instance.FireProjectile(info);
            }
            AkSoundEngine.PostEvent(1820467490, base.gameObject); // Play_moonBrother_blueWall_explode
            shouldFire = false;
        }

        private void SetPos(Vector3 newPos) {
            pos = newPos;
        }
    }
}