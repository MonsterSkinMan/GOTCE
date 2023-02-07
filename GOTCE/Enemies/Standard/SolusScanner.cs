/*using R2API;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using System.Linq;
using EntityStates;

namespace GOTCE.Enemies.Standard
{
    public class SolusScanner : EnemyBase<SolusScanner>
    {
        public override string PathToClone => "Assets/Prefabs/Enemies/SolusScanner/SolusScannerBody.prefab";
        public override string CloneName => "Solus Scanner";
        public override string PathToCloneMaster => "Assets/Prefabs/Enemies/SolusScanner/SolusScannerMaster.prefab";

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
            isc.directorCreditCost = 120;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.None;
            isc.hullSize = HullClassification.Human;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Air;
            isc.sendOverNetwork = true;
            isc.prefab = prefabMaster;
            isc.name = "cscSolusScanner";
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
            SkillLocator sl = prefab.GetComponent<SkillLocator>();
            ReplaceSkill(sl.primary, Skills.BeamEnemy.Instance.SkillDef);
            ReplaceSkill(sl.secondary, Skills.BeamFriendly.Instance.SkillDef);

            LanguageAPI.Add("GOTCE_SCANNER_NAME", "Solus Scanner");
            LanguageAPI.Add("GOTCE_SCANNER_LORE", "");
            LanguageAPI.Add("GOTCE_SCANNER_SUBTITLE", "Horde of Many");
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