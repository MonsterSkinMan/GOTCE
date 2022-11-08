using RoR2;
using EntityStates;
using System;
using UnityEngine;
using Unity;

namespace GOTCE.EntityStatesCustom.The {
    public class TheDeath : GenericCharacterDeath {
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (NetworkServer.active) {
                if (base.modelLocator) {
                    if (base.modelLocator.modelBaseTransform) {
                        GameObject.Destroy(base.modelLocator.modelTransform.gameObject);
                        GameObject.Destroy(base.modelLocator.modelBaseTransform.gameObject);
                    }
                    /* if (base.characterBody.masterObject) {
                        GameObject.Destroy(base.characterBody.masterObject);
                    }
                    GameObject.Destroy(base.gameObject); */
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}