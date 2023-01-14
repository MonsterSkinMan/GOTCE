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
            base.OnEnter();
            base.overlapAttack.AddModdedDamageType(DamageTypes.StealItem);
        }
    }
}
