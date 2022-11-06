using RoR2;
using EntityStates;
using R2API;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using RoR2.Projectile;

namespace GOTCE.EntityStatesCustom.NaeNaeLord {
    public class NaeNaeUlt : BaseSkillState {
        float duration = 2f;
        float delay = 0.5f;
        bool hasFired = false;

        public Ray ray;

        public override void OnEnter()
        {
            base.OnEnter();
            if (NetworkServer.active) {
                ray = base.GetAimRay();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= delay && !hasFired && NetworkServer.active) {
                BulletAttack whip = new();
                whip.damage = base.damageStat * 2f;
                whip.radius = 2.5f;
                whip.damageType = DamageType.LunarSecondaryRootOnHit;
                whip.maxDistance = 60f;
                whip.tracerEffectPrefab = EntityStates.Commando.CommandoWeapon.FireBarrage.tracerEffectPrefab;
                whip.isCrit = Util.CheckRoll(base.characterBody.crit, 0);
                whip.smartCollision = false;
                whip.damageColorIndex = DamageColorIndex.WeakPoint;
                whip.maxSpread = 0f;
                whip.minSpread = 0f;
                whip.procChainMask = default;
                whip.owner = base.gameObject;
                whip.origin = base.transform.position;
                whip.weapon = base.gameObject;
                whip.aimVector = ray.direction;
                whip.AddModdedDamageType(Main.truekill);

                whip.Fire();

                GameObject proj = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ArtifactShell/ArtifactShellSeekingSolarFlare.prefab").WaitForCompletion(), "whipproj");
                DamageAPI.ModdedDamageTypeHolderComponent holder = proj.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
                holder.Add(Main.truekill);

                for (int i = 0; i < 12; i++) {
                    ray = base.GetAimRay();
                    FireProjectileInfo info = new()
                    {
                        damage = base.characterBody.damage,
                        projectilePrefab = proj,
                        speedOverride = UnityEngine.Random.Range(10, 100f),
                        fuseOverride = 1000000f,
                        crit = false,
                        damageColorIndex = DamageColorIndex.WeakPoint,
                        position = base.characterBody.corePosition,
                        rotation = Util.QuaternionSafeLookRotation(Util.ApplySpread(ray.direction, -3f, 3f, -3f, 3f)),
                        owner = base.gameObject,
                    };

                    ProjectileManager.instance.FireProjectile(info);
                }
                hasFired = true;
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