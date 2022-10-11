using EntityStates.Mage;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates.Mage.Weapon;

namespace GOTCE.Enemies.EntityStatesCustom {
    public class IonSurgerRigidState : EntityStates.GenericCharacterMain
    {
        public static float duration = 0.3f;


        public static AnimationCurve speedCoefficientCurve;

        public static GameObject muzzleflashEffect;


        private Vector3 flyVector = Vector3.up;

        private Transform modelTransform;

        private CharacterModel characterModel;

        private HurtBoxGroup hurtboxGroup;

        private Vector3 blastPosition;

        public override void OnEnter()
        {
            base.OnEnter();
            // Util.PlaySound(, base.gameObject);
            modelTransform = GetModelTransform();
            flyVector = Vector3.up;
            // CreateBlinkEffect(Util.GetCorePosition(base.gameObject));
            PlayCrossfade("Body", "FlyUp", "FlyUp.playbackRate", duration, 0.1f);
            base.characterMotor.Motor.ForceUnground();
            base.characterMotor.velocity = Vector3.zero;
            EffectManager.SimpleMuzzleFlash(muzzleflashEffect, base.gameObject, "MuzzleLeft", transmit: false);
            EffectManager.SimpleMuzzleFlash(muzzleflashEffect, base.gameObject, "MuzzleRight", transmit: false);
            if (base.isAuthority)
            {
                blastPosition = base.characterBody.corePosition;
            }
            if (NetworkServer.active)
            {
                BlastAttack obj = new BlastAttack
                {
                    radius = 5,
                    procCoefficient = 0,
                    position = blastPosition,
                    attacker = base.gameObject,
                    crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master),
                    baseDamage = base.characterBody.damage * 8,
                    falloffModel = BlastAttack.FalloffModel.None,
                    baseForce = 3
                };
                obj.teamIndex = TeamComponent.GetObjectTeam(obj.attacker);
                obj.damageType = DamageType.Stun1s;
                obj.attackerFiltering = AttackerFiltering.NeverHitSelf;
                obj.Fire();
            }
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(blastPosition);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            blastPosition = reader.ReadVector3();
        }

        public override void HandleMovements()
        {
            base.HandleMovements();
            base.rigidbodyMotor.rootMotion += flyVector * (moveSpeedStat * speedCoefficientCurve.Evaluate(base.fixedAge / duration) * Time.fixedDeltaTime);
        }

        public override void UpdateAnimationParameters()
        {
            base.UpdateAnimationParameters();
        } 

        private void CreateBlinkEffect(Vector3 origin)
        {
            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(flyVector);
            effectData.origin = origin;
            // EffectManager.SpawnEffect(FlyUpState.blinkPrefab, effectData, transmit: false);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration && base.isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            if (!outer.destroying)
            {
                // Util.PlaySound(endSoundString, base.gameObject);
            }
            base.OnExit();
        }
    }
}