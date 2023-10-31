using R2API;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using System.Linq;
using EntityStates;

namespace GOTCE.Enemies.Minibosses
{
    public class FastBear : EnemyBase<FastBear>
    {
        public override string PathToClone => "Assets/Prefabs/Enemies/FreddyFastbear/FreddyFastBearBody.prefab";
        public override string CloneName => "FreddyFastBear";
        public override string PathToCloneMaster => "Assets/Prefabs/Enemies/FreddyFastbear/FreddyFastBearMaster.prefab";

        public override bool local => true;
        public CharacterBody body;
        public CharacterMaster master;
        public override bool localMaster => true;

        public override void CreatePrefab()
        {
            base.CreatePrefab();

            On.RoR2.GlobalEventManager.OnCharacterDeath += (orig, self, report) => {
                orig(self, report);

                if (report.victimBody && report.attackerBody) {
                    if (!report.victimBody.isPlayerControlled || report.attackerBody.baseNameToken != "GOTCE_FASTBEAR_NAME") {
                        return;
                    }

                    GameObject.Instantiate(Main.SecondaryAssets.LoadAsset<GameObject>("FreddyJumpscare.prefab"));
                }
            };
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
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            isc.sendOverNetwork = true;
            isc.prefab = prefabMaster;
            isc.name = "cscFastBear";
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
        
            LanguageAPI.Add("GOTCE_FASTBEAR_NAME", "Freddy Fast Bear");
            LanguageAPI.Add("GOTCE_FASTBEAR_LORE", "HOLY SHIT WAS THAT THE BITE OF 87???");
            LanguageAPI.Add("GOTCE_FASTBEAR_SUBTITLE", "Horde of Many");
        }

        public override void PostCreation()
        {
            base.PostCreation();
            List<DirectorAPI.Stage> stages = new() {
                DirectorAPI.Stage.SulfurPools,
                DirectorAPI.Stage.AbandonedAqueduct,
                DirectorAPI.Stage.GildedCoast,
                DirectorAPI.Stage.RallypointDelta,
                DirectorAPI.Stage.ScorchedAcres,
                DirectorAPI.Stage.TitanicPlains,
                DirectorAPI.Stage.AphelianSanctuary,
                DirectorAPI.Stage.DistantRoost
            };
            RegisterEnemy(prefab, prefabMaster, stages, DirectorAPI.MonsterCategory.BasicMonsters, false);
        }
    }
}