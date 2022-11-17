using RoR2;
using EntityStates;
using Unity;
using UnityEngine;
using System;
using RoR2.Projectile;

namespace GOTCE.EntityStatesCustom.AltSkills.VoidFiend {
    public class DrainUpgrade : BaseSkillState {
        public float gained = 0f;

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!inputBank.skill4.down || gained > 75f) {
                outer.SetNextStateToMain();
            } 
            if (NetworkServer.active && base.gameObject.GetComponent<VoidSurvivorController>()) {
                VoidSurvivorController con = base.gameObject.GetComponent<VoidSurvivorController>();
                con.AddCorruption(Time.fixedDeltaTime * 25);
                gained += Time.fixedDeltaTime * 25;
                AkSoundEngine.PostEvent(3778899369, gameObject);
                base.characterBody.AddTimedBuff(RoR2Content.Buffs.LunarSecondaryRoot, 0.2f, 1);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (NetworkServer.active) {
                FireProjectileInfo fireProjectileInfo = default(FireProjectileInfo);
                fireProjectileInfo.projectilePrefab = EntityStates.NullifierMonster.DeathState.deathBombProjectile;
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