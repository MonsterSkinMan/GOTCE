using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using EntityStates;
using GOTCE.EntityStatesCustom.AltSkills.Bandit.Decoy;
using RoR2.Skills;

namespace GOTCE.Enemies.Standard
{
    public class ExplodingDecoy : EnemyBase<ExplodingDecoy>
    {
        public override string PathToClone => "RoR2/Junk/Bandit/BanditBody.prefab";
        public override string CloneName => "Explosive Decoy";
        public override string PathToCloneMaster => "RoR2/Base/Beetle/BeetleMaster.prefab";
        public CharacterBody body;
        public CharacterMaster master;

        public override void CreatePrefab()
        {
            base.CreatePrefab();
            body = prefab.GetComponent<CharacterBody>();
            body.baseArmor = 0;
            body.attackSpeed = 1f;
            body.damage = 12;
            body.baseMaxHealth = 120f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_EXPLOSIVEDECOY_NAME";
            body.baseRegen = 0f;
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = int.MaxValue;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.None;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.None;
            isc.hullSize = HullClassification.Human;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            isc.sendOverNetwork = true;
            isc.prefab = prefabMaster;
            isc.name = "cscDecoy";
        }

        public override void Modify()
        {
            base.Modify();
            prefab.GetComponents<EntityStateMachine>();
            foreach (EntityStateMachine esm in prefab.GetComponents<EntityStateMachine>())
            {
                esm.initialStateType = new SerializableEntityStateType(typeof(DecoyTimer));
                esm.mainStateType = new SerializableEntityStateType(typeof(DecoyTimer));
            }

            prefab.GetComponent<CharacterDeathBehavior>().deathState = new SerializableEntityStateType(typeof(DecoyDeath));
            prefabMaster.GetComponent<CharacterMaster>().bodyPrefab = prefab;
        }

        public override void PostCreation()
        {
            base.PostCreation();
            RegisterEnemy(prefab, prefabMaster, null, DirectorAPI.MonsterCategory.BasicMonsters, false);
        }

        public override void AddDirectorCard()
        {
            base.AddDirectorCard();
            card.minimumStageCompletions = int.MaxValue;
            card.selectionWeight = int.MinValue;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
        }
    }
}
