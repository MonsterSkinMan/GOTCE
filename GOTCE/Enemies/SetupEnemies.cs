using R2API;
using RoR2;
using RoR2.Navigation;
using GOTCE.Enemies.Standard;
using GOTCE.Enemies.Minibosses;

namespace GOTCE.Enemies
{
    public static class SetupEnemies
    {
        public static void Init()
        {

            CharacterSpawnCard CrackedPestCSC = new()
            {
                name = "cscCracked",
                prefab = CrackedPest.CrackedPestMaster,
                sendOverNetwork = true,
                nodeGraphType = MapNodeGroup.GraphType.Air,
                requiredFlags = NodeFlags.None,
                forbiddenFlags = NodeFlags.NoCharacterSpawn,
                directorCreditCost = 100,
                eliteRules = SpawnCard.EliteRules.Default
            };

            DirectorCard CrackedPestDC = new()
            {
                spawnCard = CrackedPestCSC,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Standard,
                selectionWeight = 1,
                preventOverhead = false
            };

            DirectorAPI.Helpers.AddNewMonsterToStage(CrackedPestDC, DirectorAPI.MonsterCategory.Minibosses, DirectorAPI.Stage.SulfurPools);
            DirectorAPI.Helpers.AddNewMonsterToStage(CrackedPestDC, DirectorAPI.MonsterCategory.Minibosses, DirectorAPI.Stage.RallypointDelta);
            DirectorAPI.Helpers.AddNewMonsterToStage(CrackedPestDC, DirectorAPI.MonsterCategory.Minibosses, DirectorAPI.Stage.AphelianSanctuary);
            DirectorAPI.Helpers.AddNewMonsterToStage(CrackedPestDC, DirectorAPI.MonsterCategory.Minibosses, DirectorAPI.Stage.SiphonedForest);
        }
    }
}