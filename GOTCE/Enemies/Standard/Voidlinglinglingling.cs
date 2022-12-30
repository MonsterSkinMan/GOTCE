using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

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
            body.levelArmor = 0;
            body.attackSpeed = 1f;
            body.levelAttackSpeed = 0f;
            body.damage = 3f;
            body.baseMaxHealth = 40f;
            body.baseMoveSpeed = 120f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_VOIDLINGLINGLINGLING_NAME";
            body.subtitleNameToken = "GOTCE_VOIDLINGLINGLINGLING_SUBTITLE";
            body.baseRegen = 0f;
            body.levelRegen = 0f;
            body.portraitIcon = Main.MainAssets.LoadAsset<Texture2D>("Assets/Textures/Icons/Enemies/Voidlingling.png");
            body.preferredInitialStateType = new EntityStates.SerializableEntityStateType(typeof(EntityStates.Uninitialized));
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 100;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.TeleporterOK;
            isc.hullSize = HullClassification.BeetleQueen;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Air;
            isc.sendOverNetwork = true;
            isc.prefab = prefabMaster;
            isc.name = "cscVoidlinglinglingling";
        }

        public override void AddDirectorCard()
        {
            base.AddDirectorCard();
            card.minimumStageCompletions = 5;
            card.selectionWeight = 1;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
        }

        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = prefab;

            prefab.GetComponent<CharacterDeathBehavior>().deathState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.GenericCharacterDeath));

            prefab.transform.Find("Model Base").gameObject.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

            DeathRewards deathRewards = prefab.GetComponent<DeathRewards>();
            if (deathRewards)
            {
            }
            else
            {
                deathRewards = prefab.AddComponent<DeathRewards>();
                deathRewards.characterBody = body;
                deathRewards.logUnlockableDef = ScriptableObject.CreateInstance<UnlockableDef>();
                deathRewards.logUnlockableDef.nameToken = "Voidlinglinglingling";
            }

            LanguageAPI.Add("GOTCE_VOIDLINGLINGLINGLING_NAME", "Voidlinglinglingling");
            LanguageAPI.Add("GOTCE_VOIDLINGLINGLINGLING_LORE", "J");
            LanguageAPI.Add("GOTCE_VOIDLINGLINGLINGLING_SUBTITLE", "horde of Many");
        }

        public override void PostCreation()
        {
            base.PostCreation();
            List<DirectorAPI.Stage> stages = new() {
                DirectorAPI.Stage.DistantRoost,
                DirectorAPI.Stage.AphelianSanctuary,
                DirectorAPI.Stage.VoidLocus,
                DirectorAPI.Stage.VoidCell,
                DirectorAPI.Stage.RallypointDelta,
                DirectorAPI.Stage.SirensCall,
                DirectorAPI.Stage.TitanicPlains,
                DirectorAPI.Stage.AbyssalDepths,
                DirectorAPI.Stage.AbandonedAqueductSimulacrum,
                DirectorAPI.Stage.AbyssalDepthsSimulacrum,
                DirectorAPI.Stage.AphelianSanctuarySimulacrum,
                DirectorAPI.Stage.CommencementSimulacrum,
                DirectorAPI.Stage.RallypointDeltaSimulacrum,
                DirectorAPI.Stage.SkyMeadowSimulacrum,
                DirectorAPI.Stage.TitanicPlainsSimulacrum
            };
            RegisterEnemy(prefab, prefabMaster, stages, DirectorAPI.MonsterCategory.Champions);
        }
    }
}