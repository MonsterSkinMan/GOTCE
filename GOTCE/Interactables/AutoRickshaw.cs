using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using System;
using System.Collections.Generic;
using EntityStates;

namespace GOTCE.Interactables
{
    public class AutoRickshaw : InteractableBase<AutoRickshaw>
    {
        public override DirectorAPI.Stage[] stages => new DirectorAPI.Stage[] {
            DirectorAPI.Stage.AbandonedAqueduct, DirectorAPI.Stage.AbandonedAqueductSimulacrum, DirectorAPI.Stage.AbyssalDepths,
            DirectorAPI.Stage.AbyssalDepthsSimulacrum, DirectorAPI.Stage.AphelianSanctuary, DirectorAPI.Stage.AphelianSanctuarySimulacrum,
            DirectorAPI.Stage.CommencementSimulacrum, DirectorAPI.Stage.DistantRoost, DirectorAPI.Stage.RallypointDelta,
            DirectorAPI.Stage.RallypointDeltaSimulacrum, DirectorAPI.Stage.ScorchedAcres, DirectorAPI.Stage.SiphonedForest,
            DirectorAPI.Stage.SirensCall, DirectorAPI.Stage.SkyMeadow, DirectorAPI.Stage.SkyMeadowSimulacrum, DirectorAPI.Stage.SulfurPools,
            DirectorAPI.Stage.SulfurPools, DirectorAPI.Stage.SunderedGrove, DirectorAPI.Stage.TitanicPlains, DirectorAPI.Stage.TitanicPlainsSimulacrum,
            DirectorAPI.Stage.WetlandAspect
            // default stage list, doesnt include hidden realms or commencement
        };

        public override DirectorAPI.InteractableCategory category => DirectorAPI.InteractableCategory.Drones;

        public override string Name => "Auto Rickshaw";

        public GameObject prefab = Main.SecondaryAssets.LoadAsset<GameObject>("AutoRickshaw.prefab");

        public override void Modify()
        {
            base.Modify();

            LanguageAPI.Add("GOTCE_ENTER_RICKSHAW", "Enter Auto Rickshaw");
            LanguageAPI.Add("GOTCE_LEAVE_RICKSHAW", "Exit Auto Rickshaw");

            prefab.AddComponent<RickshawController>();

            PrefabAPI.RegisterNetworkPrefab(prefab);
        }

        public override void MakeSpawnCard()
        {
            base.MakeSpawnCard();
            isc.directorCreditCost = 60;
            isc.name = "iscAutoRickshaw";
            isc.prefab = prefab;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            isc.hullSize = HullClassification.Human;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.None;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoChestSpawn;
            isc.occupyPosition = true;
            isc.orientToFloor = true;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.skipSpawnWhenSacrificeArtifactEnabled = true;
            isc.slightlyRandomizeOrientation = false;
            isc.maxSpawnsPerStage = 1;
            isc.weightScalarWhenSacrificeArtifactEnabled = 1f;
            isc.sendOverNetwork = true;
        }

        public override void MakeDirectorCard()
        {
            base.MakeDirectorCard();
            card.selectionWeight = 3;
        }

        public class RickshawController : MonoBehaviour {
            public VehicleSeat seat;
            public Rigidbody rb;
            public OverlapAttack attack;
            public GameObject passenger;
            public Transform passengerOldPivot;
            public Highlight highlight;
            public InputBankTest input => seat.currentPassengerInputBank;
            public float speed = 90f;
            public float acceleration => speed / 2f;
            public float damage;
            public float stopwatch = 0f;
            public float hitDelay = 0.1f;
            public float pushAwayForceBase = 50000f;
            public Vector3 lastGroundNormal;

            public void Start() {
                seat = GetComponent<VehicleSeat>();
                rb = GetComponent<Rigidbody>();
                highlight = GetComponent<Highlight>();
                
                attack = new();
                attack.hitBoxGroup = GetComponent<HitBoxGroup>();
                attack.procCoefficient = 1f;
                attack.teamIndex = TeamIndex.Player;
                attack.hitEffectPrefab = Utils.Paths.GameObject.OmniImpactVFXLoader.Load<GameObject>();
                attack.attackerFiltering = AttackerFiltering.NeverHitSelf;

                seat.onPassengerEnter += OnPassengerEnter;
                seat.onPassengerExit += OnPassengerExit;
            }

            public void FixedUpdate() {
                if (passenger) {
                    Vector3 dir = input.moveVector;

                    if (dir != Vector3.zero) {
                        if (rb.velocity.magnitude < speed) {
                            rb.velocity += dir * acceleration * Time.fixedDeltaTime;
                        }

                        base.transform.forward = Vector3.RotateTowards(base.transform.forward, -1f * new Vector3(rb.velocity.x, 0, rb.velocity.z), 20f * Time.fixedDeltaTime, 20f * Time.fixedDeltaTime);
                    }
                    else {
                        if (rb.velocity.magnitude > 0) {
                            float y = rb.velocity.y;
                            rb.velocity *= 0.8f * Time.fixedDeltaTime;
                            rb.velocity = new Vector3(rb.velocity.x, y, rb.velocity.z);
                        }
                    }
                }

                stopwatch += Time.fixedDeltaTime;
                    
                if (stopwatch > hitDelay) {
                    stopwatch = 0f;

                    if (rb.velocity.magnitude > 5) {
                        float cur = rb.velocity.magnitude / speed;

                        attack.pushAwayForce = pushAwayForceBase * cur;
                        attack.damage = damage * cur;
                        attack.ResetIgnoredHealthComponents();
                        attack.Fire();
                    }
                }

                bool hit = Physics.Raycast(base.transform.position + (Vector3.up * 0.3f), Vector3.down, out var info, 0.8f, LayerIndex.world.mask, QueryTriggerInteraction.Ignore);

                if (hit)
                {
                    lastGroundNormal = info.normal;
                    lastGroundNormal = Vector3.RotateTowards(Vector3.up, lastGroundNormal, 70f * (MathF.PI / 180f), float.PositiveInfinity);
                }
                else
                {
                    lastGroundNormal = Vector3.up;
                }

                base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.FromToRotation(Vector3.up, lastGroundNormal) * base.transform.rotation, 20f * Time.fixedDeltaTime);
            }

            private void OnPassengerExit(GameObject passenger)
            {
                this.passenger = null;

                if (passenger.TryGetComponent<CameraTargetParams>(out var ctp)) {
                    ctp.cameraPivotTransform = passengerOldPivot;
                }

                highlight.strength = 1f;
            }

            private void OnPassengerEnter(GameObject passenger)
            {
                this.passenger = passenger;

                if (passenger.TryGetComponent<CharacterBody>(out var cb)) {
                    attack.attacker = passenger;
                    damage = cb.damage * 30f;
                }
                
                if (passenger.TryGetComponent<CameraTargetParams>(out var ctp)) {
                    passengerOldPivot = ctp.cameraPivotTransform;
                    ctp.cameraPivotTransform = base.transform.Find("pivot");
                }

                highlight.strength = 0f;
            }
        }
    }
}