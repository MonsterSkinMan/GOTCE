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
    public class DisposableFunder : InteractableBase<DisposableFunder>
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

        public override DirectorAPI.InteractableCategory category => DirectorAPI.InteractableCategory.Chests;
        public GameObject prefab = Main.SecondaryAssets.LoadAsset<GameObject>("Assets/Prefabs/Interactables/DisposableFunder/DisposableFunder.prefab");

        public override void Modify()
        {
            base.Modify();
            ChestBehavior behavior = prefab.GetComponent<ChestBehavior>();
            ExplicitPickupDropTable drops = (ExplicitPickupDropTable)behavior.dropTable;
            float weight = 1 / 5;
            drops.pickupEntries = new ExplicitPickupDropTable.PickupDefEntry[] {
                new ExplicitPickupDropTable.PickupDefEntry {pickupDef = RoR2Content.Equipment.GoldGat, pickupWeight = weight},
                new ExplicitPickupDropTable.PickupDefEntry {pickupDef = RoR2Content.Equipment.CommandMissile, pickupWeight = weight},
                new ExplicitPickupDropTable.PickupDefEntry {pickupDef = Equipment.BloodGamble.Instance.EquipmentDef, pickupWeight = weight},
                new ExplicitPickupDropTable.PickupDefEntry {pickupDef = Equipment.Aegisfunder.Instance.EquipmentDef, pickupWeight = weight},
                new ExplicitPickupDropTable.PickupDefEntry {pickupDef = Equipment.LunarCrunder.Instance.EquipmentDef, pickupWeight = weight},
            };
            LanguageAPI.Add("GOTCE_DISPOSABLE_NAME", "The Disposable Funder");
            LanguageAPI.Add("GOTCE_DISPOSABLE_CONTEXT", "Purchase");

            PrefabAPI.RegisterNetworkPrefab(prefab);
        }

        public override void MakeSpawnCard()
        {
            base.MakeSpawnCard();
            isc.directorCreditCost = 10;
            isc.name = "iscFunder";
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
    }
}