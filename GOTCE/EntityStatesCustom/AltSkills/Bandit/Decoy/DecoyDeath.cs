using RoR2;
using EntityStates;
using System;
using RoR2.Projectile;
using Unity;
using UnityEngine;

namespace GOTCE.EntityStatesCustom.AltSkills.Bandit.Decoy
{
    public class DecoyDeath : GenericCharacterDeath
    {
        public override void OnEnter()
        {
            if (NetworkServer.active)
            {
                if (base.characterBody && base.characterBody.master)
                {
                    CharacterMaster master = base.characterBody.master;
                    if (master.minionOwnership && master.minionOwnership.ownerMaster)
                    {
                        CharacterMaster ownerMaster = master.minionOwnership.ownerMaster;
                        if (ownerMaster.GetBody() && base.isAuthority)
                        {
                            BlastAttack blast = new()
                            {
                                radius = 11f,
                                baseDamage = ownerMaster.GetBody().damage * 2f,
                                attacker = ownerMaster.GetBodyObject(),
                                position = base.characterBody.corePosition,
                                crit = Util.CheckRoll(ownerMaster.GetBody().crit, ownerMaster),
                                damageType = DamageType.Stun1s,
                                procChainMask = new(),
                                procCoefficient = 1f,
                                teamIndex = ownerMaster.teamIndex,
                                falloffModel = BlastAttack.FalloffModel.None
                            };

                            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXQuick"), new EffectData
                            {
                                origin = base.characterBody.corePosition,
                                scale = 7,
                                rotation = Quaternion.identity
                            }, transmit: true);

                            blast.Fire();
                        }
                    }

                    if (base.modelLocator)
                    {
                        if (base.modelLocator.modelBaseTransform)
                        {
                            GameObject.Destroy(base.modelLocator.modelTransform.gameObject);
                            GameObject.Destroy(base.modelLocator.modelBaseTransform.gameObject);
                        }
                    }
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}