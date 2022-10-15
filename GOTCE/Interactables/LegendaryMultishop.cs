/*
using R2API;
using RoR2;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace GOTCE.Interactables
{
    internal class LegendaryMultishop : InteractableBase<LegendaryMultishop>
    {
        public override DirectorAPI.InteractableCategory category => DirectorAPI.InteractableCategory.Chests;
        public GameObject prefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/TripleShopLarge/TripleShopLarge.prefab").WaitForCompletion().InstantiateClone("LegendaryTripleShop");
        public Material fuckYouUnity = Object.Instantiate(Addressables.LoadAssetAsync<Material>("RoR2/Base/MultiShopTerminal/matMultishop.mat").WaitForCompletion());

        public override void Modify()
        {
            base.Modify();
            prefab.name = "LegendaryTripleShop";
            fuckYouUnity.name = "matLegendaryMultiShop";
            var pole = prefab.transform.GetChild(4).GetComponent<MeshRenderer>();
            pole.name = "mdlLegendaryMultiShopTerminalCenter";
            pole.sharedMaterial = fuckYouUnity;
            pole.sharedMaterial.color = new Color32(107, 40, 41, 255);

            var multiShopController = prefab.GetComponent<MultiShopController>();
            multiShopController.baseCost = 500;
            var terminalPrefab = multiShopController.terminalPrefab;
            terminalPrefab.name = "LegendaryMultiShopTerminal";

            var skinnedMeshRenderer = terminalPrefab.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>();
            skinnedMeshRenderer.sharedMaterials[0] = fuckYouUnity;
            skinnedMeshRenderer.sharedMaterials[0].color = new Color32(107, 40, 41, 255);

            var purchaseInteraction = terminalPrefab.GetComponent<PurchaseInteraction>();
            purchaseInteraction.cost = 500;
            purchaseInteraction.automaticallyScaleCostWithDifficulty = true;

            var shopTerminalBehavior = terminalPrefab.GetComponent<ShopTerminalBehavior>();
            BasicPickupDropTable drops = ScriptableObject.CreateInstance<BasicPickupDropTable>();
            drops.tier1Weight = 0;
            drops.tier2Weight = 0;
            drops.tier3Weight = 1;
            shopTerminalBehavior.dropTable = drops;

            purchaseInteraction.displayNameToken = "LEGENDARY_MULTISHOP_NAME";
            purchaseInteraction.contextToken = "LEGENDARY_MULTISHOP_CONTEXT";
            LanguageAPI.Add("LEGENDARY_MULTISHOP_NAME", "Legendary Multishop Terminal");
            LanguageAPI.Add("LEGENDARY_MULTISHOP_CONTEXT", "Open Terminal");

            PrefabAPI.RegisterNetworkPrefab(prefab);
        }

        public override void MakeSpawnCard()
        {
            base.MakeSpawnCard();
            isc.name = "iscLegendaryMultishop";
            isc.prefab = prefab;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            isc.hullSize = HullClassification.Human;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.None;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoChestSpawn;
            isc.directorCreditCost = 120;
            isc.occupyPosition = true;
            isc.orientToFloor = false;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.skipSpawnWhenSacrificeArtifactEnabled = true;
            isc.weightScalarWhenSacrificeArtifactEnabled = 0f;
            isc.slightlyRandomizeOrientation = false;
            isc.maxSpawnsPerStage = 1;
            isc.sendOverNetwork = true;
        }
    }
}
*/