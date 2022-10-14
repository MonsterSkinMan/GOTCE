using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;

namespace GOTCE.Enemies.Bosses
{
    public class Voidlingling : EnemyBase<Voidlingling>
    {
        public override string PathToClone => "RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyBase.prefab";
        public override string CloneName => "Voidlingling";
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
            body.damage = 21f;
            body.levelDamage = 4.2f;
            body.baseMaxHealth = 2400f;
            body.levelMaxHealth = 720f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_VOIDLINGLING_NAME";
            body.baseRegen = 0f;
            body.levelRegen = 0f;
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 600;
            isc.eliteRules = SpawnCard.EliteRules.ArtifactOnly;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.TeleporterOK;
            isc.hullSize = HullClassification.BeetleQueen;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Air;
            isc.sendOverNetwork = true;
            isc.prefab = prefab;
            isc.name = "cscVoidlingling";
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

            prefab.transform.Find("Model Base").gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

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

            DeathRewards deathRewards = prefab.GetComponent<DeathRewards>();
            if (deathRewards)
            {
            }
            else
            {
                deathRewards = prefab.AddComponent<DeathRewards>();
                deathRewards.characterBody = body;
            }
            ExplicitPickupDropTable dt = ScriptableObject.CreateInstance<ExplicitPickupDropTable>();
            dt.pickupEntries = new ExplicitPickupDropTable.PickupDefEntry[]
            {
                new ExplicitPickupDropTable.PickupDefEntry {pickupDef = Equipment.VoidDonut.Instance.EquipmentDef, pickupWeight = 1f},
            };
            deathRewards.bossDropTable = dt;

            LanguageAPI.Add("GOTCE_VOIDLINGLING_NAME", "Voidlingling");
            LanguageAPI.Add("GOTCE_VOIDLINGLING_LORE", "Literally just Voidling as a teleporter boss.");
            LanguageAPI.Add("GOTCE_VOIDLINGLING_SUBTITLE", "A Fun and Engaging Boss");
            RegisterEnemy(prefab, prefabMaster, stages, DirectorAPI.MonsterCategory.Champions);
        }
    }
}