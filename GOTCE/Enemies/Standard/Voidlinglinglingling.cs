using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Collections.Generic;
using EntityStates;

namespace GOTCE.Enemies.Bosses {
    public class Voidlinglinglingling : EnemyBase<Voidlinglinglingling> {
        public override string PathToClone => "RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyBase.prefab";
        public override string CloneName => "Voidlinglinglingling";
        public override string PathToCloneMaster => "RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabMasterBase.prefab";
        public CharacterBody body;
        public CharacterMaster master;

        public override void CreatePrefab()
        {
            base.CreatePrefab();
            body = prefab.GetComponent<CharacterBody>();
            body.baseArmor = 0;
            body.levelArmor = 0;
            body.attackSpeed = 1f;
            body.levelAttackSpeed = 0f;
            body.damage = 10f;
            body.levelDamage = 2f;
            body.baseMaxHealth = 80f;
            body.levelMaxHealth = 24f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_VOIDLINGLINGLINGLING_NAME";
            body.baseRegen = 0f;
            body.levelRegen = 0f;
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 15;
            isc.eliteRules = SpawnCard.EliteRules.ArtifactOnly;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.None;
            isc.hullSize = HullClassification.Human;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Air;
            isc.sendOverNetwork = true;
            isc.prefab = prefab;
            isc.name = "cscVoidlinglingling";
        }

        public override void AddDirectorCard()
        {
            base.AddDirectorCard();
            card.minimumStageCompletions = 1;
            card.selectionWeight = 1;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
        }

        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = prefab;

            prefab.transform.Find("Model Base").gameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            List<DirectorAPI.Stage> stages = new() {
                DirectorAPI.Stage.SiphonedForest,
                DirectorAPI.Stage.AphelianSanctuary,
                DirectorAPI.Stage.VoidLocus,
                DirectorAPI.Stage.VoidCell,
                DirectorAPI.Stage.RallypointDelta,
                DirectorAPI.Stage.SirensCall,
                DirectorAPI.Stage.TitanicPlains,
                DirectorAPI.Stage.AbyssalDepths
            };
            LanguageAPI.Add("GOTCE_VOIDLINGLINGLINGLING_NAME", "Voidlinglinglingling");
            LanguageAPI.Add("GOTCE_VOIDLINGLINGLINGLING_LORE", "Literally just Voidling as a basic enemy.");
            LanguageAPI.Add("GOTCE_VOIDLINGLINGLINGLING_SUBTITLE", "Horde of Many");
            RegisterEnemy(prefab, prefabMaster, stages, DirectorAPI.MonsterCategory.BasicMonsters);
        }
    }
}