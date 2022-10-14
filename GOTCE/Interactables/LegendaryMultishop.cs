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
        public GameObject prefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/TripleShop/TripleShop.prefab").WaitForCompletion().InstantiateClone("LegendaryTripleShop");
        public Material fuckYouUnity = Object.Instantiate(Addressables.LoadAssetAsync<Material>("RoR2/Base/MultiShopTerminal/matMultishop.mat").WaitForCompletion());

        public override void Modify()
        {
            base.Modify();
            // prefab.name = "LegendaryMultishop";
            var tex = fuckYouUnity.mainTexture;
            var pole = prefab.transform.GetChild(4).GetComponent<MeshRenderer>();
            pole.sharedMaterial.mainTexture = tex;
            pole.sharedMaterial.color = new Color32(107, 40, 41, 255);
            var msc = prefab.GetComponent<MultiShopController>();
            msc.baseCost = 500;
            var tPrefab = msc.terminalPrefab;
            var smr = tPrefab.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>();
            smr.sharedMaterials[0].mainTexture = tex;
            smr.sharedMaterials[0].color = new Color32(107, 40, 41, 255);
            var beh = tPrefab.GetComponent<ShopTerminalBehavior>();
            var pInter = tPrefab.GetComponent<PurchaseInteraction>();
            pInter.cost = 500;
            pInter.displayNameToken = "LEGENDARY_MULTISHOP_NAME";
            pInter.contextToken = "LEGENDARY_MULTISHOP_CONTEXT";
            pInter.automaticallyScaleCostWithDifficulty = true;
            LanguageAPI.Add("LEGENDARY_MULTISHOP_NAME", "Legendary Multishop");
            LanguageAPI.Add("LEGENDARY_MULTISHOP_CONTEXT", "Open Terminal");
            BasicPickupDropTable drops = ScriptableObject.CreateInstance<BasicPickupDropTable>();
            drops.tier1Weight = 0;
            drops.tier2Weight = 0;
            drops.tier3Weight = 1;
            beh.dropTable = drops;

            PrefabAPI.RegisterNetworkPrefab(prefab);
        }

        public override void MakeSpawnCard()
        {
            base.MakeSpawnCard();
            isc.name = "iscLegendaryMultishop";
            isc.prefab = prefab;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            isc.hullSize = HullClassification.Golem;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.None;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.None;
            isc.directorCreditCost = 1;
            isc.occupyPosition = true;
            isc.orientToFloor = true;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.skipSpawnWhenSacrificeArtifactEnabled = true;
            isc.slightlyRandomizeOrientation = true;
            isc.maxSpawnsPerStage = 69;
        }
    }
}
*/