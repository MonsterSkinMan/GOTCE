using RoR2;
using EntityStates;
using System;
using UnityEngine;
using Unity;

namespace GOTCE.EntityStatesCustom.The {
    public class TheDeath : GenericCharacterDeath {
        public float duration = 2f;
        public override bool shouldAutoDestroy => false;
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration) {
                outer.SetNextStateToMain();
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (NetworkServer.active) {
                // healthComponent.health = healthComponent.fullHealth;
                // PlayAnimation("Body", "Death");
                GameObject.DestroyImmediate(modelLocator.modelBaseTransform.gameObject);
                GameObject.DestroyImmediate(characterBody.masterObject);
                GameObject.DestroyImmediate(gameObject);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}