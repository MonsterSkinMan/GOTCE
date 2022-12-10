using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using On.RoR2.Items;
using MonoMod.Cil;
using System.Linq;
using HarmonyLib;
using GOTCE.Utils;
using UnityEngine.AddressableAssets;

namespace GOTCE.Tiers
{
    public class LunarVoid : TierBase<LunarVoid>
    {
        public override string TierName => "LunarVoid";
        public override bool CanScrap => false;
        public override ColorCatalog.ColorIndex ColorIndex => ColorCatalog.ColorIndex.VoidItem;
        public override ColorCatalog.ColorIndex DarkColorIndex => ColorCatalog.ColorIndex.VoidItemDark;
        public override GameObject DropletDisplayPrefab => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Common/VoidOrb.prefab").WaitForCompletion();
        public override bool IsDroppable => false;
        public override ItemTierDef.PickupRules PickupRules => ItemTierDef.PickupRules.ConfirmAll;
        public override ItemTier TierEnum => (ItemTier)49;
        public override GameObject HighlightPrefab => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Common/VoidOrb.prefab").WaitForCompletion();

        public override void PostCreation()
        {
            base.PostCreation();
            On.RoR2.BasicPickupDropTable.GenerateWeightedSelection += (orig, self, run) => {
                orig(self, run);
                List<PickupIndex> indexes = new();
                foreach (ItemDef def in ItemCatalog.allItemDefs.Where(x => x._itemTierDef == this.tier)) {
                    indexes.Add(def.CreatePickupDef().pickupIndex);
                }
                self.Add(indexes, self.lunarItemWeight > 0 ? self.lunarItemWeight : self.voidTier2Weight);
            };
        }
    }
}