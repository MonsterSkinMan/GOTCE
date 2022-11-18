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
            base.OnEnter();
            if (NetworkServer.active) {
                GameObject.DestroyImmediate(base.characterBody.masterObject);
                GameObject.DestroyImmediate(base.gameObject);
            }
        }
    }
}