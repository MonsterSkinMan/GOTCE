/*using R2API;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using System.Linq;
using EntityStates;

namespace GOTCE.Enemies.Standard
{
    public class SolusPylon : EnemyBase<SolusPylon>
    {
        public override string PathToClone => "Assets/Prefabs/Enemies/SolusPylon/SolusPylonBody.prefab";
        public override string CloneName => "Solus Pylon";
        public override string PathToCloneMaster => "Assets/Prefabs/Enemies/SolusPylon/SolusPylonMaster.prefab";

        public override bool local => true;
        public override bool localMaster => true;
        public CharacterBody body;
        public CharacterMaster master;

        public override void CreatePrefab()
        {
            base.CreatePrefab();
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 340;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.None;
            isc.hullSize = HullClassification.Human;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            isc.sendOverNetwork = true;
            isc.prefab = prefabMaster;
            isc.name = "cscSolusPylon";
            isc.forbiddenAsBoss = true;
        }

        public override void AddDirectorCard()
        {
            base.AddDirectorCard();
            card.minimumStageCompletions = 0;
            card.selectionWeight = 1;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
        }

        public override void Modify()
        {
            base.Modify();

            EntityStateMachine machine = prefab.GetComponents<EntityStateMachine>().First(x => x.customName == "Pylon");
            machine.initialStateType = new SerializableEntityStateType(typeof(EntityStatesCustom.SolusPylon.PylonMainState));
            machine.mainStateType = new SerializableEntityStateType(typeof(EntityStatesCustom.SolusPylon.PylonMainState));

            LanguageAPI.Add("GOTCE_PYLON_NAME", "Solus Pylon");
            LanguageAPI.Add("GOTCE_PYLON_LORE", "");
            LanguageAPI.Add("GOTCE_PYLON_SUBTITLE", "Horde of Many");
        }

        public override void PostCreation()
        {
            base.PostCreation();
            List<DirectorAPI.Stage> stages = new() {
                DirectorAPI.Stage.SirensCall,
                DirectorAPI.Stage.SkyMeadow,
                DirectorAPI.Stage.SkyMeadowSimulacrum
            };
            RegisterEnemy(prefab, prefabMaster, stages, DirectorAPI.MonsterCategory.BasicMonsters, false);
        }
    }
}*/