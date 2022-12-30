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
                base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Nullified.buffIndex, 0.2f);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (base.isAuthority)
            {
                int max = Mathf.CeilToInt(5f + (1f + (gained * 0.6f)));
                for (int i = 0; i < max; i++) {
                    BulletAttack attack = new();
                    attack.owner = base.gameObject;
                    attack.weapon = base.gameObject;
                    attack.origin = base.characterBody.corePosition;
                    attack.damage = 3.2f * base.damageStat;
                    attack.falloffModel = BulletAttack.FalloffModel.Buckshot;
                    attack.maxDistance = 35f;
                    attack.damageColorIndex = DamageColorIndex.Void;
                    attack.radius = 0.5f;
                    attack.smartCollision = false;
                    attack.damageType = DamageType.Nullify;
                    attack.isCrit = base.RollCrit();
                    attack.procCoefficient = 0.75f;
                    attack.aimVector = base.GetAimRay().direction;
                    attack.minSpread = 0;
                    attack.maxSpread = 6;
                    attack.tracerEffectPrefab = Addressables.LoadAssetAsync<GameObject>(Utils.Paths.GameObject.VoidSurvivorBeamTracer).WaitForCompletion();
                    attack.hitEffectPrefab = Addressables.LoadAssetAsync<GameObject>(Utils.Paths.GameObject.VoidSurvivorBeamImpact).WaitForCompletion();
                    attack.muzzleName = "MuzzleRight";
                    attack.Fire();
                }
                AkSoundEngine.PostEvent(Events.Play_voidman_m2_shoot_fullCharge, base.gameObject);
            }
        }
    }
}