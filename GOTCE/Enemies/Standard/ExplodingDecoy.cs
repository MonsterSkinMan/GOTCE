using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using EntityStates;
using RoR2.Skills;

namespace GOTCE.Enemies.Standard
{
    public class ExplodingDecoy : EnemyBase<ExplodingDecoy>
    {
        public override string PathToClone => "RoR2/Junk/Bandit/BanditBody.prefab";
        public override string CloneName => "Explosive Decoy";
        public override string PathToCloneMaster => "RoR2/Base/Beetle/BeetleBody.prefab";
        public CharacterBody body;
        public CharacterMaster master;

        public override void CreatePrefab()
        {
            base.CreatePrefab();
            body = prefab.GetComponent<CharacterBody>();
            body.baseArmor = 0;
            body.attackSpeed = 1f;
            body.damage = 12;
            body.baseMaxHealth = 15f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_EXPLOSIVEDECOY_NAME";
            body.baseRegen = -3f;
        }

        public override void Modify()
        {
            base.Modify();
            prefab.GetComponents<EntityStateMachine>();
            foreach (EntityStateMachine esm in prefab.GetComponents<EntityStateMachine>())
            {
                esm.initialStateType = new SerializableEntityStateType(typeof(EntityStates.Idle));
                esm.mainStateType = new SerializableEntityStateType(typeof(EntityStates.Idle));
            }
        }
    }
}
