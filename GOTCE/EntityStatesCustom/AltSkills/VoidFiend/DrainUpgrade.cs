using RoR2;
using EntityStates;
using Unity;
using UnityEngine;
using System;
using RoR2.Projectile;

namespace GOTCE.EntityStatesCustom.AltSkills.VoidFiend
{
    public class DrainUpgrade : BaseSkillState
    {
        public float gained = 0f;

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!inputBank.skill4.down || gained > 75f && base.isAuthority)
            {
                outer.SetNextStateToMain();
            }
            if (base.isAuthority && base.gameObject.GetComponent<VoidSurvivorController>())
            {
                VoidSurvivorController con = base.gameObject.GetComponent<VoidSurvivorController>();
                con.AddCorruption(Time.fixedDeltaTime * 25);
                gained += Time.fixedDeltaTime * 25;
                AkSoundEngine.PostEvent(3778899369, gameObject);
                base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.LunarSecondaryRoot.buffIndex, 0.2f);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (base.isAuthority)
            {
                FireProjectileInfo fireProjectileInfo = default(FireProjectileInfo);
                fireProjectileInfo.projectilePrefab = EntityStates.NullifierMonster.DeathState.deathBombProjectile;
                // replace with voidling laser, make the healing slightly less and the stun slightly longer
                fireProjectileInfo.position = base.characterBody.corePosition;
                fireProjectileInfo.rotation = Quaternion.identity;
                fireProjectileInfo.owner = base.gameObject;
                fireProjectileInfo.damage = base.damageStat;
                fireProjectileInfo.crit = base.characterBody.RollCrit();
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            }
        }
    }
}