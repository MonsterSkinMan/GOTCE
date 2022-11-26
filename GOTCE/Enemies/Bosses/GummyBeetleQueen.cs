using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace GOTCE.Enemies.Bosses
{
    public class GummyBeetleQueen : EnemyBase<GummyBeetleQueen>
    {
        public override string PathToClone => "RoR2/Base/Beetle/BeetleQueen2Body.prefab"; // is literally just a beetle queen with the gummy item cry about it
        public override string CloneName => "GummyBeetleQueen";
        public override string PathToCloneMaster => "RoR2/Base/Beetle/BeetleQueenMaster.prefab";
        public CharacterBody body;
        public CharacterMaster master;

        public override void CreatePrefab()
        {
            base.CreatePrefab();
            body = prefab.GetComponent<CharacterBody>();
            body.baseNameToken = "GOTCE_GUMMYQUEEN_NAME";
            body.subtitleNameToken = "GOTCE_GUMMYQUEEN_SUBTITLE";
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 1500;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.TeleporterOK;
            isc.hullSize = HullClassification.BeetleQueen;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            isc.sendOverNetwork = true;
            isc.prefab = prefabMaster;
            isc.name = "cscGummyBeetleQueen";
        }

        public override void AddDirectorCard()
        {
            base.AddDirectorCard();
            card.minimumStageCompletions = 0;
            card.selectionWeight = 2;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
        }

        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();

            prefab.transform.Find("ModelBase").transform.Find("mdlBeetleQueen").GetComponent<ModelSkinController>().enabled = false;
            prefab.transform.Find("ModelBase").transform.Find("mdlBeetleQueen").GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial = Addressables.LoadAssetAsync<Material>("RoR2/DLC1/GummyClone/matGummyClone.mat").WaitForCompletion();

            LanguageAPI.Add("GOTCE_GUMMYQUEEN_NAME", "Gummy Beetle Queen");
            LanguageAPI.Add("GOTCE_GUMMYQUEEN_LORE", "Gummy Beetle Queen");
            LanguageAPI.Add("GOTCE_GUMMYQUEEN_SUBTITLE", "Gummy Beetle Queen");

            master.bodyPrefab = prefab;
        }

        public override void PostCreation()
        {
            base.PostCreation();
            RegisterEnemy(prefab, prefabMaster, null, DirectorAPI.MonsterCategory.Champions, true);

            On.RoR2.CharacterBody.Start += (orig, self) =>
            {
                orig(self);
                if (NetworkServer.active && self.baseNameToken == "GOTCE_GUMMYQUEEN_NAME")
                {
                    self.inventory.GiveItem(RoR2Content.Items.BoostHp, 10);
                    self.inventory.GiveItem(RoR2Content.Items.BoostDamage, 10);
                }
            };
        }
    }
}