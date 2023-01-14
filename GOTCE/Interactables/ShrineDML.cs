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
    public class ShrineDML : InteractableBase<ShrineDML>
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

        public override DirectorAPI.InteractableCategory category => DirectorAPI.InteractableCategory.Shrines;
        public GameObject prefab = Main.SecondaryAssets.LoadAsset<GameObject>("Assets/Prefabs/Interactables/ShrineDML/ShrineDML.prefab");

        public override void Modify()
        {
            base.Modify();
            prefab.AddComponent<DisposableBehavior>();

            LanguageAPI.Add("GOTCE_DML_NAME", "Shrine of Disposability");
            LanguageAPI.Add("GOTCE_DML_CONTEXT", "Purchase");

            PrefabAPI.RegisterNetworkPrefab(prefab);

            // prevent the teleporter from locking this interactable
            On.RoR2.OutsideInteractableLocker.LockPurchasable += (orig, self, interactable) => {
                if (self.lockInside) {
                    orig(self, interactable); // this is a void seed, lock the shrine
                }
                else {
                    if (interactable.displayNameToken == "GOTCE_DML_NAME") {
                        // do nothing
                    }
                    else {
                        orig(self, interactable);
                    }
                }
            };
        }

        public override void MakeSpawnCard()
        {
            base.MakeSpawnCard();
            isc.directorCreditCost = 30;
            isc.name = "iscDml";
            isc.prefab = prefab;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            isc.hullSize = HullClassification.Human;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.None;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoChestSpawn | RoR2.Navigation.NodeFlags.NoShrineSpawn;
            isc.occupyPosition = true;
            isc.orientToFloor = false;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.skipSpawnWhenSacrificeArtifactEnabled = false;
            isc.slightlyRandomizeOrientation = true;
            isc.maxSpawnsPerStage = 1;
            isc.weightScalarWhenSacrificeArtifactEnabled = 1f;
            isc.sendOverNetwork = true;
        }

        public override void MakeDirectorCard()
        {
            base.MakeDirectorCard();
            card.selectionWeight = 3;
        }
    }

    public class DisposableBehavior : MonoBehaviour
    {
        private float delay = 0.5f;
        private float stopwatch = 0f;
        private GameObject projectilePrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MissileProjectile");
        private struct MissileStream {
            public int count;
        }
        private List<MissileStream> missileStreams;

        public void Start()
        {
            PurchaseInteraction interaction = GetComponent<PurchaseInteraction>();
            interaction.onPurchase.AddListener(OnInteract);
            transform.position -= new Vector3(0, 0.5f, 0);
            interaction.setUnavailableOnTeleporterActivated = false;
            missileStreams = new();
        }

        public void OnInteract(Interactor interactor)
        {
            if (NetworkServer.active)
            {
                MissileStream stream = new MissileStream {
                    count = 12,
                };
                missileStreams.Add(stream);
                GetComponent<PurchaseInteraction>().SetAvailable(true);
            }
        }

        public void FixedUpdate()
        {
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= delay)
            {
                stopwatch = 0f;

                for (int i = 0; i < missileStreams.Count; i++) {
                    MissileStream stream = missileStreams[i];
                    stream.count--;
                    MissileUtils.FireMissile(gameObject.transform.position + new Vector3(0, 2f, 0), gameObject.GetComponent<PurchaseInteraction>().lastActivator.gameObject.GetComponent<CharacterBody>(), new ProcChainMask(), null, gameObject.GetComponent<PurchaseInteraction>().lastActivator.gameObject.GetComponent<CharacterBody>().damage * 3f, false, projectilePrefab, DamageColorIndex.Item, false);
                }

                missileStreams.RemoveAll(x => x.count <= 0);
            }
        }
    }
}