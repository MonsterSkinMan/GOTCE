using EntityStates;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Enemies.Standard
{
    public class Voidlinglinglingling : EnemyBase<Voidlinglinglingling>
    {
        public override string PathToClone => "RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyPhase1.prefab";
        public override string CloneName => "Voidlinglinglingling";
        public override string PathToCloneMaster => "RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabMasterPhase1.prefab";
        public CharacterBody body;
        public CharacterMaster master;

        public override void CreatePrefab()
        {
            base.CreatePrefab();
            body = prefab.GetComponent<CharacterBody>();
            body.baseArmor = 0;
            body.attackSpeed = 1f;
            body.damage = 6.5f;
            body.baseMaxHealth = 120f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_VOIDLINGLINGLINGLING_NAME";
            body.baseRegen = 0f;
            body.preferredInitialStateType = new SerializableEntityStateType(typeof(EntityStatesCustom.SpawnState));
            body.GetComponent<EntityStateMachine>().initialStateType = new SerializableEntityStateType(typeof(EntityStatesCustom.SpawnState));
            body.GetComponent<CharacterDeathBehavior>().deathState = new SerializableEntityStateType(typeof(EntityStatesCustom.DeathState));
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
            isc.prefab = prefabMaster;
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
        }

        public override void PostCreation()
        {
            base.PostCreation();
            RegisterEnemy(prefab, prefabMaster, null, DirectorAPI.MonsterCategory.BasicMonsters, true);
        }
    }

    // TODO:

    // remove spawn vfx (probably in an entitystate)
    // remove footstep vfx
    // make the spawn entitystate much faster
}