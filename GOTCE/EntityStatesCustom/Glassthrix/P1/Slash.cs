using RoR2;
using EntityStates;
using System;
using UnityEngine;
using EntityStates.BrotherMonster;
using RoR2.Navigation;
using RoR2.Projectile;

namespace GOTCE.EntityStatesCustom.Glassthrix.P1 {
    public class Slash : SprintBash {
        public override void OnEnter()
        {
            base.damageCoefficient = 8f;
            base.duration = 1f;
            base.swingEffectPrefab = Utils.Paths.GameObject.BrotherSwing1Kickup.Load<GameObject>();
            base.procCoefficient = 1;
            base.pushAwayForce = 6000;
            Slash.durationBeforePriorityReduces = 0.5f;
            base.hitBoxGroupName = "Weapon";
            base.forceVector = Vector3.up * 2000;
            base.hitPauseDuration = 0.1f;
            base.swingEffectMuzzleString = "MuzzleSprintBash";
            base.mecanimHitboxActiveParameter = "weapon.hitBoxActive";
            base.beginSwingSoundString = "Play_moonBrother_swing_horizontal";
            base.forceForwardVelocity = true;
            base.forwardVelocityCurve = new();
            base.baseDuration = 1f;
            base.OnEnter();
            base.overlapAttack.AddModdedDamageType(DamageTypes.StealItem);

            GameObject prefab = Utils.Paths.GameObject.LunarShardProjectile.Load<GameObject>();

            for (int i = 0; i < 90; i++) {
                FireProjectileInfo info = new();
                info.position = base.transform.position;
                info.rotation = Util.QuaternionSafeLookRotation(Util.ApplySpread(base.inputBank.aimDirection, -50f, 50f, 1f, 1f));
                info.damage = base.damageCoefficient * 2f;
                info.crit = base.RollCrit();
                info.owner = base.gameObject;
                info.projectilePrefab = prefab;

                if (NetworkServer.active) {
                    ProjectileManager.instance.FireProjectile(info);
                }
            }
        }
    }
}
