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
        }

        public override void MakeSpawnCard()
        {
            base.MakeSpawnCard();
            isc.directorCreditCost = 15;
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
            isc.slightlyRandomizeOrientation = false;
            isc.maxSpawnsPerStage = 3;
            isc.weightScalarWhenSacrificeArtifactEnabled = 1f;
            isc.sendOverNetwork = true;
        }

        public override void MakeDirectorCard()
        {
            base.MakeDirectorCard();
            card.selectionWeight = 4;
        }
    }

    public class DisposableBehavior : MonoBehaviour {
        private float delay = 0.15f;
        private float stopwatch = 0f;
        private int remainingMissiles = 0;
        private GameObject projectilePrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MissileProjectile");
        public void Start() {
            PurchaseInteraction interaction = GetComponent<PurchaseInteraction>();
            interaction.onPurchase.AddListener(OnInteract);
            transform.position -= new Vector3(0, 1.5f, 0);
            interaction.setUnavailableOnTeleporterActivated = false;
        }

        public void OnInteract(Interactor interactor) {
            if (NetworkServer.active) {
                remainingMissiles += 12;
                GetComponent<PurchaseInteraction>().SetAvailable(true);
            }
        }

        public void FixedUpdate() {
            transform.rotation = Quaternion.Euler(-90, 0, 0);
            transform.localRotation = Quaternion.Euler(-90, 0, 0);

            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= delay) {
                stopwatch = 0f;
                if (remainingMissiles > 0) {
                    MissileUtils.FireMissile(gameObject.transform.position, gameObject.GetComponent<PurchaseInteraction>().lastActivator.gameObject.GetComponent<CharacterBody>(), new ProcChainMask(), null, gameObject.GetComponent<PurchaseInteraction>().lastActivator.gameObject.GetComponent<CharacterBody>().damage*3f, false, projectilePrefab, DamageColorIndex.Item, false);
                    remainingMissiles--;
                }
            }
        }
    }
}