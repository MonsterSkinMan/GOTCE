/* using RoR2;
using EntityStates;
using System;
using RoR2.Projectile;
using Unity;
using UnityEngine;

namespace GOTCE.EntityStatesCustom.AltSkills.Captain.Core {
    public class Core : GenericCharacterDeath {
        public override void OnEnter()
        {
            if (NetworkServer.active) {
                if (base.characterBody && base.characterBody.master) {

                    if (base.modelLocator) {
                        if (base.modelLocator.modelBaseTransform) {
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
} */