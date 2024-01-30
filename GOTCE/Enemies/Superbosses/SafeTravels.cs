using System;
using UnityEngine;
using RoR2;
using RoR2.Skills;
using Unity;
using RoR2.Orbs;
using EntityStates.BrotherMonster;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Utilities;
using HarmonyLib;
using Rewired.ComponentControls.Effects;

namespace GOTCE.Enemies.Superbosses {
    public class SafeTravels : EnemyBase<SafeTravels> {
        public override string PathToClone => Utils.Paths.GameObject.UrchinTurretBody18;
        public override string CloneName => "SafeTravels";
        public override string PathToCloneMaster => Utils.Paths.GameObject.AffixEarthHealerMaster;
        private CharacterBody body;
        public static CharacterSpawnCard safeTravelsCard;
        private static GameObject safeTravelsObj;

        public override void CreatePrefab()
        {
            base.CreatePrefab();
            body = prefab.GetComponent<CharacterBody>();
            body.baseMaxHealth = 14000;
            body.rootMotionInMainState = true;
            prefab.transform.position = Vector3.zero;
            prefab.transform.localPosition = Vector3.zero;
        }

        public override void Modify()
        {
            base.Modify();
            SceneAssetAPI.AddAssetRequest("moon", SetupST);

            body.baseNameToken = "SAFETRAVELS_BODY_NAME";
            body.subtitleNameToken = "SAFETRAVELS_BODY_DESC";

            prefabMaster.GetComponent<CharacterMaster>().bodyPrefab = prefab;

            safeTravelsCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            safeTravelsCard.prefab = prefabMaster;
            safeTravelsCard.nodeGraphType = MapNodeGroup.GraphType.Air;
            safeTravelsCard.sendOverNetwork = true;
            safeTravelsCard.name = "UES Safe Travels"; 
            safeTravelsCard.forbiddenFlags = NodeFlags.NoCharacterSpawn;;
            safeTravelsCard.hullSize = HullClassification.Golem;

            body.GetComponent<ModelLocator>().modelBaseTransform.rotation = Quaternion.Euler(0, 90, 90);

            body.GetComponentInChildren<AimAssistTarget>(true).gameObject.SetActive(false);
            
            prefabMaster.GetComponent<BaseAI>().enabled = false;

            GameObject.Destroy(prefab.GetComponent<CharacterDirection>());

            LanguageAPI.Add("SAFETRAVELS_BODY_NAME", "UES Safe Travels");
            LanguageAPI.Add("SAFETRAVELS_BODY_DESC", "Derailed Craft");
        }

        public void SetupST(GameObject[] sceneObjs) {
            GameObject safeTravels = sceneObjs[0].transform.Find("EscapeSequenceObjects").Find("ColonyShipHolder").gameObject;
            safeTravelsObj = PrefabAPI.InstantiateClone(safeTravels, "SafeTravelsBodyMdl");

            ObjectTransformCurve[] curves = safeTravelsObj.GetComponentsInChildren<ObjectTransformCurve>();

            for (int i = 0; i < curves.Length; i++) {
                ObjectTransformCurve curve = curves[i];
                curve.enabled = false;
                curve.transform.localPosition = Vector3.zero;
                // curve.transform.position = Vector3.zero;
                GameObject.Destroy(curve);
            }

            safeTravelsObj.transform.Find("ColonyShipTranslator").localScale = new(0.1f, 0.1f, 0.1f);

            safeTravelsObj.transform.Find("ColonyShipTranslator").transform.localPosition = Vector3.zero;

            safeTravelsObj.GetComponentInChildren<RotateAroundAxis>().enabled = true;

            Debug.Log("spawning ST model");

            // Main.ModLogger.LogError("FUCKING KILL YOURSELF !!");

            SetupModel(prefab, safeTravelsObj);
        }

        public override void PostCreation()
        {
            base.PostCreation();
            RegisterEnemy(prefab, prefabMaster, null);
        }
    }
}