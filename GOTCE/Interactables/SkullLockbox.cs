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
    public class SkullLockbox : InteractableBase<SkullLockbox>
    {
        public override DirectorAPI.Stage[] stages => null;
        public override DirectorAPI.InteractableCategory category => DirectorAPI.InteractableCategory.Chests;

        public override string Name => "Skull Key Lockbox";

        public GameObject prefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/TreasureCache/Lockbox.prefab").WaitForCompletion().InstantiateClone("SkullIssue");
        
        public override void Modify()
        {
            base.Modify();
            PurchaseInteraction interaction = prefab.GetComponent<PurchaseInteraction>();
            ChestBehavior behavior = prefab.GetComponent<ChestBehavior>();
            interaction.cost = 0;
            interaction.costType = CostTypeIndex.None;
            interaction.displayNameToken = "SKULL_LOCKBOX_NAME";
            interaction.contextToken = "SKULL_LOCKBOX_CONTEXT";
            LanguageAPI.Add("SKULL_LOCKBOX_NAME", ":Skull: Lockbox");
            LanguageAPI.Add("SKULL_LOCKBOX_CONTEXT", "Use");
            // behavior.itemTier = ItemTier.Tier3;
            // SkullDropTable drops = new SkullDropTable();
            BasicPickupDropTable drops = ScriptableObject.CreateInstance<BasicPickupDropTable>();
            drops.tier1Weight = 0;
            drops.tier2Weight = 0;
            drops.tier3Weight = 1;
            drops.bossWeight = 1;
            behavior.dropTable = drops;
            // prefab.GetComponent<ModelLocator>()._modelTransform.localScale += new Vector3(1.5f, 1.5f, 1.5f);

            PrefabAPI.RegisterNetworkPrefab(prefab);
        }

        public override void MakeSpawnCard()
        {
            base.MakeSpawnCard();
            isc.directorCreditCost = 0;
            isc.name = "iscUlley";
            isc.prefab = prefab;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            isc.hullSize = HullClassification.Human;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.None;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoChestSpawn;
            isc.directorCreditCost = 0;
            isc.occupyPosition = true;
            isc.orientToFloor = true;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.skipSpawnWhenSacrificeArtifactEnabled = false;
            isc.slightlyRandomizeOrientation = true;
            isc.maxSpawnsPerStage = -1;
            isc.weightScalarWhenSacrificeArtifactEnabled = 1f;
            isc.sendOverNetwork = true;
        }
    }

    public class SkullDropTable : PickupDropTable
    {
        public WeightedSelection<PickupIndex> weighted = new WeightedSelection<PickupIndex>();

        public override int GetPickupCount()
        {
            return weighted.Count;
        }

        public override void Regenerate(Run run)
        {
            base.Regenerate(run);
            weighted.Clear();
            weighted.AddChoice(RoR2Content.Items.Clover.CreatePickupDef().pickupIndex, 11f);
            weighted.AddChoice(RoR2Content.Items.CaptainDefenseMatrix.CreatePickupDef().pickupIndex, 11f);
            weighted.AddChoice(RoR2Content.Items.Behemoth.CreatePickupDef().pickupIndex, 11f);
            weighted.AddChoice(RoR2Content.Items.RoboBallBuddy.CreatePickupDef().pickupIndex, 11f);
            weighted.AddChoice(RoR2Content.Items.LightningStrikeOnHit.CreatePickupDef().pickupIndex, 11f);
            weighted.AddChoice(RoR2Content.Items.FireballsOnHit.CreatePickupDef().pickupIndex, 11f);
            weighted.AddChoice(DLC1Content.Items.MoreMissile.CreatePickupDef().pickupIndex, 11f);
            weighted.AddChoice(DLC1Content.Items.DroneWeapons.CreatePickupDef().pickupIndex, 11f);
            weighted.AddChoice(DLC1Content.Items.PermanentDebuffOnHit.CreatePickupDef().pickupIndex, 11f);
        }

        public override PickupIndex[] GenerateUniqueDropsPreReplacement(int maxDrops, Xoroshiro128Plus rng)
        {
            return GenerateUniqueDropsFromWeightedSelection(maxDrops, rng, weighted);
        }

        public override PickupIndex GenerateDropPreReplacement(Xoroshiro128Plus rng)
        {
            return GenerateDropFromWeightedSelection(rng, weighted);
        }
    }
}