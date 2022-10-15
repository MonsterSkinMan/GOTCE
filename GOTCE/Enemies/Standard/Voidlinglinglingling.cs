/*
using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;

namespace GOTCE.Enemies.Standard
{
    public class Voidlinglinglingling : EnemyBase<Voidlinglinglingling>
    {
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
            body.attackSpeed = 1f;
            body.damage = 9.5f;
            body.levelDamage = 1.9f;
            body.baseMaxHealth = 120f;
            body.levelMaxHealth = 36f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_VOIDLINGLINGLINGLING_NAME";
            body.baseRegen = 0f;
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 50;
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
                DirectorAPI.Stage.DistantRoost,
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

    // TODO:

    // remove spawn vfx (probably in an entitystate)
    // remove footstep vfx
    // make the spawn entitystate much faster
}
*/