using RoR2;
using Unity;
using UnityEngine;
using System;
using R2API;
using EntityStates;
using RoR2.Projectile;

namespace GOTCE.EntityStatesCustom.AltSkills.VoidFiend
{
    public class Pearl : BaseSkillState
    {
        public float duration = 0.25f;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = duration / attackSpeedStat;
            AkSoundEngine.PostEvent(4021527550, base.gameObject); // Play_voidman_m2_shoot_fullCharge
            Ray aim = GetAimRay();

            if (NetworkServer.active)
            {
                if (!characterBody.gameObject.GetComponent<ViendPearlManager>())
                {
                    characterBody.gameObject.AddComponent<ViendPearlManager>();
                }

                GameObject prefab = Main.SecondaryAssets.LoadAsset<GameObject>("Assets/Prefabs/Projectiles/AltSkills/Pearl/Pearl.prefab").InstantiateClone("pearlclone");
                prefab.AddComponent<ViendPearlBehavior>();
                prefab.GetComponent<ViendPearlBehavior>().owner = gameObject;

                FireProjectileInfo info = new()
                {
                    owner = gameObject,
                    damage = damageStat * 1.5f,
                    crit = RollCrit(),
                    // info.speedOverride = 350;
                    projectilePrefab = prefab,
                    position = aim.origin + new Vector3(0, 0.5f, 0),
                    rotation = Util.QuaternionSafeLookRotation(aim.direction),
                    damageColorIndex = DamageColorIndex.Void,
                    force = 3f,
                    procChainMask = new ProcChainMask()
                };

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
            if (base.fixedAge >= duration)
            {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}