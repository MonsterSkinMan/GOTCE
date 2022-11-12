using RoR2;
using Unity;
using UnityEngine;
using System;
using R2API;
using EntityStates;
using RoR2.Projectile;

namespace GOTCE.EntityStatesCustom.AltSkills.VoidFiend {
    public class Pearl : BaseSkillState {
        public float duration = 1f;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = duration / attackSpeedStat;
            Ray aim = GetAimRay();

            if (NetworkServer.active) {
                if (!characterBody.gameObject.GetComponent<ViendPearlManager>()) {
                    characterBody.gameObject.AddComponent<ViendPearlManager>();
                }

                GameObject prefab = Main.SecondaryAssets.LoadAsset<GameObject>("Assets/Prefabs/Projectiles/AltSkills/Pearl/Pearl.prefab").InstantiateClone("pearlclone");
                prefab.AddComponent<ViendPearlBehavior>();
                prefab.GetComponent<ViendPearlBehavior>().owner = gameObject;

                FireProjectileInfo info = new();
                info.owner = gameObject;
                info.damage = damageStat * 1.5f;
                info.crit = RollCrit();
                // info.speedOverride = 350;
                info.projectilePrefab = prefab;
                info.position = aim.origin + new Vector3(0, 0.5f, 0);
                info.rotation = Util.QuaternionSafeLookRotation(aim.direction);
                info.damageColorIndex = DamageColorIndex.Void;
                info.force = 3f;
                info.procChainMask = new ProcChainMask();

                ProjectileManager.instance.FireProjectile(info);

                characterBody.skillLocator.secondary.SetSkillOverride(gameObject, Skills.PearlTeleport.Instance.SkillDef, GenericSkill.SkillOverridePriority.Replacement);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration) {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}