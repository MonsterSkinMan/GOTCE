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
            body.baseArmor = 0;
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

            AddESM(prefab, "STMain", new(typeof(SafeTravelsMainState)));

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

        public class SafeTravelsMainState : BaseState {
            public List<CharacterBody> viableTargets = new();
            public GameObject diabloPrefab;
            public GameObject phaseRound;
            public float[] stopwatch = new float[2];
            public float[] timers = new float[] { 1f, 0.001f }; 

            public override void OnEnter()
            {
                base.OnEnter();
                
                foreach (PlayerCharacterMasterController master in PlayerCharacterMasterController._instancesReadOnly) {
                    if (master.body) {
                        viableTargets.Add(master.body);
                    }
                }

                diabloPrefab = Utils.Paths.GameObject.CaptainAirstrikeAltProjectile.Load<GameObject>();
                phaseRound = Utils.Paths.GameObject.FMJRamping.Load<GameObject>();
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();

                viableTargets.RemoveAll(x => x == null || !x.healthComponent.alive);

                if (viableTargets.Count == 0) {
                    return;
                }

                stopwatch[0] += Time.fixedDeltaTime;
                stopwatch[1] += Time.fixedDeltaTime;

                if (stopwatch[0] >= timers[0]) {
                    stopwatch[0] = 0f;
                    FireDiablo();
                }

                if (stopwatch[1] >= timers[1]) {
                    stopwatch[1] = 0f;
                    FirePhaseRound();
                }
            }

            public void FireDiablo() {
                if (!NetworkServer.active) {
                    return;
                }

                CharacterBody body = viableTargets.GetRandom();

                FireProjectileInfo info = new();
                Vector3 origin = Random.onUnitSphere * 3 + body.corePosition;
                Vector3 pos = origin;

                if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 4000f, LayerIndex.world.mask, QueryTriggerInteraction.Ignore)) {
                    pos = hit.point;
                }

                info.position = pos;
                info.crit = true;
                info.damage = base.damageStat * 200f;
                info.owner = base.gameObject;
                info.projectilePrefab = diabloPrefab;

                ProjectileManager.instance.FireProjectile(info);
            }

            public void FirePhaseRound() {
                if (!NetworkServer.active) {
                    return;
                }

                CharacterBody body = viableTargets.GetRandom();

                FireProjectileInfo info = new();
                info.position = base.transform.position + (Random.onUnitSphere * 50);
                info.crit = true;
                info.damage = base.damageStat * 0.50f;
                info.owner = base.gameObject;
                info.speedOverride = 200f;
                info.useSpeedOverride = true;
                info.force = 40000000000f;
                info.rotation = Util.QuaternionSafeLookRotation(((body.corePosition + Random.onUnitSphere * 20) - info.position).normalized);
                info.projectilePrefab = phaseRound;

                ProjectileManager.instance.FireProjectile(info);
            }
        }
    }
}