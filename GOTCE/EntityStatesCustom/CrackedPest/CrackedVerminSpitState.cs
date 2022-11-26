using EntityStates;
using RoR2;
using UnityEngine;
using GOTCE;
using EntityStates.FlyingVermin.Weapon;
using RoR2.Projectile;

namespace GOTCE.EntityStatesCustom.CrackedPest
{
    public class CrackedVerminSpitState : BaseSkillState
    {
        private float duration;
        private GameObject prefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/FlyingVermin/VerminSpitProjectile.prefab").WaitForCompletion();

        public override void OnEnter()
        {
            base.OnEnter();
            // CharacterBody body = this.GetComponent<CharacterBody>();
            duration = 1f / base.attackSpeedStat;
            Ray aimray = base.GetAimRay();
            base.StartAimMode(aimray);
            if (base.isAuthority)
            {
                FireProjectileInfo info = new FireProjectileInfo();
                ProcChainMask behemoth = new ProcChainMask();
                behemoth.AddProc(ProcType.AACannon);
                behemoth.AddProc(ProcType.Behemoth);
                info.projectilePrefab = prefab;
                info.position = aimray.origin;
                info.rotation = Util.QuaternionSafeLookRotation(aimray.direction);
                info.owner = base.gameObject;
                info.damage = base.characterBody.damage * 2f;
                info.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                info.procChainMask = behemoth;
                info.damageColorIndex = DamageColorIndex.Poison;
                for (int i = 0; i < 5; i++)
                {
                    info.rotation = Util.QuaternionSafeLookRotation(Util.ApplySpread(aimray.direction, 1, 3, 1, 3));
                    ProjectileManager.instance.FireProjectile(info);
                }
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
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}