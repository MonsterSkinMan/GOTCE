using RoR2;
using EntityStates;
using System;
using Unity;
using UnityEngine;
using RoR2.CharacterAI;
using RoR2.Projectile;

namespace GOTCE.EntityStatesCustom.Providence {
    public class ProviDeath : GenericCharacterDeath {
        public override void OnEnter()
        {
            if (NetworkServer.active) {
                GameObject.DestroyImmediate(modelLocator.modelBaseTransform.gameObject);
                GameObject.DestroyImmediate(characterBody.masterObject);
                GameObject.DestroyImmediate(gameObject);
            }
        }
    }
}