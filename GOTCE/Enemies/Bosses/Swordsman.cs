using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Skills;
using UnityEngine.Networking;
using RoR2.CharacterAI;

namespace GOTCE.Enemies.Bosses
{
    public class Provi : EnemyBase<Provi>
    {
        public override string PathToClone => "Assets/Prefabs/Enemies/Provi/ProviBody.prefab";
        public override bool local => true;
        public override string CloneName => "Provi";
        public override string PathToCloneMaster => "RoR2/Base/ClayBruiser/ClayBruiserMaster.prefab";
        public CharacterBody body;
        public CharacterMaster master;

        public override void CreatePrefab()
        {
            base.CreatePrefab();
            body = prefab.GetComponent<CharacterBody>();
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 1200;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.TeleporterOK;
            isc.hullSize = HullClassification.Human;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            isc.sendOverNetwork = true;
            isc.prefab = prefabMaster;
            isc.name = "cscProvi";
        }

        public override void AddDirectorCard()
        {
            base.AddDirectorCard();
            card.minimumStageCompletions = 2;
            card.selectionWeight = 1;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
        }

        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = prefab;

            SkillLocator sl = prefab.GetComponent<SkillLocator>();
            ReplaceSkill(sl.primary, Skills.DumbRounds.Instance.SkillDef);

            LanguageAPI.Add("GOTCE_PROVI_NAME", "Providence");
            LanguageAPI.Add("GOTCE_PROVI_LORE", "lol, lmao");
            LanguageAPI.Add("GOTCE_PROVI_SUBTITLE", "Bulwark of the Weak");

        }

        public override void PostCreation()
        {
            base.PostCreation();
            List<DirectorAPI.Stage> stages = new() {
                DirectorAPI.Stage.ScorchedAcres,
                DirectorAPI.Stage.SulfurPools,
                DirectorAPI.Stage.RallypointDelta
            };
            RegisterEnemy(prefab, prefabMaster, stages, DirectorAPI.MonsterCategory.Champions, false);
        }
    }
}