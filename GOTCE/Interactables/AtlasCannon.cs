using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using System;
using System.Collections.Generic;
using EntityStates;
using AK.Wwise;

namespace GOTCE.Interactables
{
    public class AtlasCannon : InteractableBase<AtlasCannon>
    {
        public override DirectorAPI.Stage[] stages => new DirectorAPI.Stage[] {
            
        };

        public override DirectorAPI.InteractableCategory category => DirectorAPI.InteractableCategory.Misc;

        public override string Name => "Atlas Cannon";

        public GameObject prefab = Main.SecondaryAssets.LoadAsset<GameObject>("Assets/Prefabs/Interactables/AtlasCannon/AtlasCannon.prefab");

        public override void Modify()
        {
            base.Modify();
            
            Debug.Log("initialized atlas cannon");

            LanguageAPI.Add("GOTCE_ATLAS_NAME", "Atlas Cannon");
            LanguageAPI.Add("GOTCE_ATLAS_CONTEXT", "Authorize Usage");

            prefab.AddComponent<AtlasCannonBehaviour>();

            PrefabAPI.RegisterNetworkPrefab(prefab);
        }

        public override void MakeSpawnCard()
        {
            base.MakeSpawnCard();
        }

        public override void MakeDirectorCard()
        {
            base.MakeDirectorCard();
        }

        public class AtlasCannonBehaviour : MonoBehaviour {
            public PurchaseInteraction interaction;
            public float atlasCd = 45f;
            public float stopwatch = 0f;
            public void Start() {
                interaction = GetComponent<PurchaseInteraction>();
                interaction.onPurchase.AddListener(TriggerEffect);
            }

            public void FixedUpdate() {
                stopwatch += Time.fixedDeltaTime;

                if (stopwatch < atlasCd && interaction.available) {
                    interaction.SetAvailable(false);
                }
                else if (stopwatch >= atlasCd && !interaction.available) {
                    interaction.SetAvailable(true);
                }
            }

            public void TriggerEffect(Interactor interactor) {
                GetComponent<EntityStateMachine>().SetNextState(new AtlasCannonBeamState());
                stopwatch = 0f;
            }

            public class AtlasCannonBeamState : BaseState {
                public GameObject beamInstance;
                public CharacterBody target;

                public override void OnEnter()
                {
                    base.OnEnter();

                    target = CharacterBody.readOnlyInstancesList.FirstOrDefault(x => x.gameObject.name.Contains("SafeTravels"));

                    Debug.Log("target exists?: " + target != null);

                    if (!target) {
                        outer.SetNextStateToMain();
                    }

                    beamInstance = GameObject.Instantiate(Utils.Paths.GameObject.LaserEngiTurret.Load<GameObject>(), base.transform);
                    beamInstance.transform.localScale *= 5f;
                    Transform laserEffectInstanceEndTransform = beamInstance.GetComponent<ChildLocator>().FindChild("LaserEnd");
                    laserEffectInstanceEndTransform.position = base.transform.position + (Vector3.up * 899f);

                    LineRenderer lr = beamInstance.GetComponentInChildren<LineRenderer>();
                    lr.endWidth = 5f;
                    lr.startWidth = 5f;
                    lr.widthMultiplier = 1f;
                    lr.transform.localPosition = new Vector3(-0.1f, 0.7f, 0);
                }

                public override void OnExit()
                {
                    base.OnExit();
                    Destroy(beamInstance);
                }

                public override void FixedUpdate()
                {
                    base.FixedUpdate();

                    if (!target) {
                        outer.SetNextStateToMain();
                        return;
                    }

                    if (base.fixedAge >= 4f) {
                        if (target && NetworkServer.active) {
                            target.healthComponent.TakeDamage(new DamageInfo() {
                                damage = target.healthComponent.fullCombinedHealth * 0.25f,
                                position = target.transform.position
                            });

                            EffectManager.SpawnEffect(Utils.Paths.GameObject.OmniExplosionVFXEngiTurretDeath.Load<GameObject>(), new EffectData() {
                                origin = target.transform.position,
                                scale = 9000f
                            }, true);
                        }

                        outer.SetNextStateToMain();
                    }
                }
            }
        }
    }
}