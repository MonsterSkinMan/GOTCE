using EntityStates;
using RoR2;
using UnityEngine;
using GOTCE;
using EntityStates.Commando.CommandoWeapon;
using RoR2.Projectile;
using R2API;

namespace GOTCE.EntityStatesCustom.NemCore
{
    public class Collapse : BaseSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();

            BlastAttack attack = new();
            attack.radius = 15;
            attack.baseDamage = 80;
            attack.attacker = base.gameObject;
            attack.position = base.transform.position;
            attack.teamIndex = TeamIndex.Neutral;
            attack.attackerFiltering = AttackerFiltering.AlwaysHitSelf;
            attack.procCoefficient = 150;
    
            EffectManager.SpawnEffect(Utils.Paths.GameObject.ExplodeOnDeathVoidExplosionEffect.Load<GameObject>(), new EffectData {
                scale = 15f,
                origin = base.transform.position
            }, true);

            attack.Fire();

            characterBody.healthComponent.Suicide();
        }
    }
}