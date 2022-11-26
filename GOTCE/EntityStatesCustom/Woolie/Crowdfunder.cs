using System;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace GOTCE.EntityStatesCustom.Woolie
{
    public class Crowdfunder : BaseState
    {
        public float stopwatch = 0f;
        public float delay = 0.07f;

        public GameObject tracer = EntityStates.GoldGat.GoldGatFire.tracerEffectPrefab;

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (NetworkServer.active)
            {
                stopwatch += Time.fixedDeltaTime;
                if (stopwatch >= delay)
                {
                    stopwatch = 0f;
                    FireCrunder("muzzle1");
                    FireCrunder("muzzle2");
                    FireCrunder("muzzle3");
                    FireCrunder("muzzle4");
                    FireCrunder("muzzle5");
                    FireCrunder("muzzle6");
                }
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        public void FireCrunder(string muzzle)
        {
            if (NetworkServer.active)
            {
                BulletAttack attack = new()
                {
                    owner = base.gameObject,
                    origin = base.characterBody.corePosition,
                    damage = base.damageStat * 0.08f,
                    tracerEffectPrefab = tracer,
                    maxDistance = int.MaxValue,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    minSpread = 0f,
                    maxSpread = 3f,
                    muzzleName = muzzle,
                    force = 3,
                    isCrit = base.RollCrit(),
                    radius = 0.01f,
                    smartCollision = false,
                    aimVector = base.GetAimRay().direction
                };

                attack.Fire();
            }
            AkSoundEngine.PostEvent(981107519, base.gameObject); // Play_item_goldgat_fire
        }
    }
}