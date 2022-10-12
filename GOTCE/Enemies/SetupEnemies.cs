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
            CharacterSpawnCard LivingSuppCSC = new()
            {
                name = "cscKirn",
                prefab = LivingSuppressiveFire.LivingSuppressiveFireMaster,
                sendOverNetwork = true,
                nodeGraphType = MapNodeGroup.GraphType.Air,
                requiredFlags = NodeFlags.None,
                forbiddenFlags = NodeFlags.NoCharacterSpawn,
                directorCreditCost = 20,
                eliteRules = SpawnCard.EliteRules.Default
            };

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

            DirectorCard LivingSuppDC = new()
            {
                spawnCard = LivingSuppCSC,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Standard,
                selectionWeight = 1,
                preventOverhead = false
            };
            DirectorCard CrackedPestDC = new()
            {
                spawnCard = CrackedPestCSC,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Standard,
                selectionWeight = 1,
                preventOverhead = false
            };

            DirectorAPI.Helpers.AddNewMonster(LivingSuppDC, DirectorAPI.MonsterCategory.BasicMonsters);
            DirectorAPI.Helpers.RemoveExistingMonsterFromStage("cscKirn", DirectorAPI.Stage.Commencement);
            DirectorAPI.Helpers.RemoveExistingMonsterFromStage("cscKirn", DirectorAPI.Stage.SiphonedForest);
            DirectorAPI.Helpers.RemoveExistingMonsterFromStage("cscKirn", DirectorAPI.Stage.TitanicPlains);
            DirectorAPI.Helpers.RemoveExistingMonsterFromStage("cscKirn", DirectorAPI.Stage.DistantRoost);
            DirectorAPI.Helpers.RemoveExistingMonsterFromStage("cscKirn", DirectorAPI.Stage.VoidLocus);
            DirectorAPI.Helpers.AddNewMonsterToStage(CrackedPestDC, DirectorAPI.MonsterCategory.Minibosses, DirectorAPI.Stage.SulfurPools);
            DirectorAPI.Helpers.AddNewMonsterToStage(CrackedPestDC, DirectorAPI.MonsterCategory.Minibosses, DirectorAPI.Stage.RallypointDelta);
            DirectorAPI.Helpers.AddNewMonsterToStage(CrackedPestDC, DirectorAPI.MonsterCategory.Minibosses, DirectorAPI.Stage.AphelianSanctuary);
            DirectorAPI.Helpers.AddNewMonsterToStage(CrackedPestDC, DirectorAPI.MonsterCategory.Minibosses, DirectorAPI.Stage.SiphonedForest);
        }
    }
}