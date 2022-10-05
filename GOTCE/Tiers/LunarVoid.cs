using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GOTCE.Tiers {
    public class LunarVoid : TierBase<LunarVoid>
    {
        public override string TierName => "LunarVoid";
        public override bool CanScrap => false;
        public override ColorCatalog.ColorIndex ColorIndex => ColorCatalog.ColorIndex.VoidItem;
        public override ColorCatalog.ColorIndex DarkColorIndex => ColorCatalog.ColorIndex.VoidItemDark;
        public override GameObject DropletDisplayPrefab => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Common/VoidOrb.prefab").WaitForCompletion();
        public override bool IsDroppable => false;
        public override ItemTierDef.PickupRules PickupRules => ItemTierDef.PickupRules.ConfirmAll;
        public override ItemTier TierEnum => (ItemTier)42;
        public override GameObject HighlightPrefab => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Common/VoidOrb.prefab").WaitForCompletion();
    }
}