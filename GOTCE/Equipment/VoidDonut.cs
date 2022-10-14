using RoR2;
using R2API;
using UnityEngine;
using BepInEx.Configuration;
using System;
using EntityStates.VoidRaidCrab;
namespace GOTCE.Equipment
{
    public class VoidDonut : EquipmentBase<VoidDonut>
    {
        public override string EquipmentName => "Void Donut";

        public override string EquipmentLangTokenName => "GOTCE_VoidDonut";

        public override string EquipmentPickupDesc => "Briefly unleash a devastating void laser.";

        public override string EquipmentFullDescription => "Fire a controllable void laser for 1500% damage per frame.";

        public override string EquipmentLore => "";

        public override GameObject EquipmentModel => null;

        public override Sprite EquipmentIcon => null;

        public override float Cooldown => 45f;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateEquipment();
            Hooks();
        }
        
        public override void Hooks()
        {
            
        }


        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            if (slot.characterBody)
            {
                slot.characterBody.gameObject.AddComponent<VoidLaser>();
            }
            return true;
        }
    }

    public class VoidLaser : MonoBehaviour {
        private CharacterBody body;
        private float stopwatch = 0f;
        private GameObject prefab = SpinBeamAttack.beamVfxPrefab;
        private GameObject beamVfxInstance;

        public void Awake() {
            body = gameObject.GetComponent<CharacterBody>();
            beamVfxInstance = UnityEngine.Object.Instantiate(prefab);
		    beamVfxInstance.transform.SetParent(body.transform);
            beamVfxInstance.transform.localScale += new Vector3(-10, -10, -10);
        }

        public void FixedUpdate() {
            stopwatch += Time.fixedDeltaTime;
            Ray beamRay = body.equipmentSlot.GetAimRay();
            if (body.localPlayerAuthority) {
                BulletAttack bulletAttack = new BulletAttack();
                // bulletAttack.muzzleName = BaseSpinBeamAttackState.muzzleTransformNameInChildLocator;
                bulletAttack.origin = beamRay.origin;
                bulletAttack.aimVector = beamRay.direction;
                bulletAttack.minSpread = 0f;
                bulletAttack.maxSpread = 0f;
                bulletAttack.maxDistance = 400f;
                bulletAttack.hitMask = LayerIndex.CommonMasks.bullet;
                bulletAttack.stopperMask = 0;
                bulletAttack.bulletCount = 1u;
                bulletAttack.radius = 5f;
                bulletAttack.smartCollision = false;
                bulletAttack.queryTriggerInteraction = QueryTriggerInteraction.Ignore;
                bulletAttack.procCoefficient = 1f;
                bulletAttack.procChainMask = default(ProcChainMask);
                bulletAttack.owner = base.gameObject;
                bulletAttack.weapon = base.gameObject;
                bulletAttack.damage = 1500f * body.damage;
                bulletAttack.damageColorIndex = DamageColorIndex.Default;
                bulletAttack.damageType = DamageType.Generic;
                bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
                bulletAttack.force = 0f;
                bulletAttack.hitEffectPrefab = SpinBeamAttack.beamImpactEffectPrefab;
                bulletAttack.tracerEffectPrefab = null;
                bulletAttack.isCrit = false;
                bulletAttack.HitEffectNormal = false;
                bulletAttack.Fire();
            }

		    beamVfxInstance.transform.SetPositionAndRotation(beamRay.origin, Quaternion.LookRotation(beamRay.direction));

            if (stopwatch >= 10f) {
                VfxKillBehavior.KillVfxObject(beamVfxInstance);
                GameObject.Destroy(gameObject.GetComponent<VoidLaser>());
            }
         }
    }
}