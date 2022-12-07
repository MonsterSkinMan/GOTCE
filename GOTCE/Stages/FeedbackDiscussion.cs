/* using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace GOTCE.Stages {
    public class FeedbackDiscussion : StageBase<FeedbackDiscussion> {
        public override string LangTokenName => "FeedbackDiscussion";
        public override string SceneName => "feedbackdiscussion";
        public override string SceneSubtitle => "Brainrot.";
        public override string SceneDisplayName => "Feedback Discussion";
        public override string SceneLore => "Directive: Inject does more dps than mando m1 at range";
        public override SceneType SceneType => SceneType.Stage;
        public override SceneCollection DestinationGroup => Addressables.LoadAssetAsync<SceneCollection>("RoR2/Base/SceneGroups/sgStage1.asset").WaitForCompletion();
        public override MusicTrackDef BossTrack => Addressables.LoadAssetAsync<MusicTrackDef>("RoR2/Base/Common/muMainEndingOutroA.asset").WaitForCompletion();
        public override MusicTrackDef MainTrack => Addressables.LoadAssetAsync<MusicTrackDef>("RoR2/Base/Common/muMainEndingOutroA.asset").WaitForCompletion();
        public override GameObject DioramaPrefab => Main.MainAssets.LoadAsset<GameObject>("Assets/Models/Prefabs/Item/Drill/Cube.prefab");
        public override string dccsInteractableClone => "RoR2/Base/artifactworld/dpArtifactWorldInteractables.asset";
        public override string dccsMonsterClone => "RoR2/Base/artifactworld/dpArtifactWorldMonsters.asset";
        public override bool ShouldIncludeInLogbook => false;

        public override void Hooks()
        {
            base.Hooks();
            On.RoR2.Chat.AddMessage_string += (orig, str) => {
                orig(str);
                if (str.ToLower().Contains("feedback")) {
                    if (Stage.instance) {
                        Stage.instance.BeginAdvanceStage(sceneDef);
                    }
                }
            };
        }

        public override void ModifySceneInfo(ClassicStageInfo info)
        {
            GameObject.Find("HOLDER: MapZones").transform.Find("Lava").gameObject.AddComponent<LavaController>();
        } 
    }

    public class LavaController : MonoBehaviour {
        private List<HealthComponent> healthComponents;
        public void Start() {

        }

        public void OnTriggerEnter(Collider col) {
            if (col.gameObject.GetComponent<HealthComponent>()) {
                healthComponents.Add(col.gameObject.GetComponent<HealthComponent>());
            }
        }

        public void OnTriggerExit(Collider col) {
            if (col.gameObject.GetComponent<HealthComponent>()) {
                healthComponents.Remove(col.gameObject.GetComponent<HealthComponent>());
            }
        }

        public void FixedUpdate() {
            if (NetworkServer.active) {
                foreach (HealthComponent hc in healthComponents) {
                    DamageInfo info = new();
                    info.damage = hc.body.maxHealth * 0.0002f;
                    info.damageType = DamageType.IgniteOnHit;
                    info.damageColorIndex = DamageColorIndex.Item;
                    info.position = hc.body.transform.position;

                    hc.body.healthComponent.TakeDamage(info);
                    hc.body.jumpPower *= 3f;
                }
            }
        }
    }
} */