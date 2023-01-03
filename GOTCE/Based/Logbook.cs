using UnityEngine;
using RoR2;
using UnityEngine.SceneManagement;
using RoR2.Skills;
using R2API;
using System;
using RoR2.EntityLogic;
using RoR2.UI.LogBook;
using System.Collections.Generic;
using HG;
using RoR2.ExpansionManagement;

namespace GOTCE.Based {
    public static class Logbook {
        public static void RunHooks() {
            On.RoR2.UI.LogBook.LogBookController.BuildPickupEntries += FixLogbook;
        }

        private static void RemoveFromPool(On.RoR2.Run.orig_Start orig, Run self) {
            orig(self);
            foreach (EquipmentDef equipmentDef in EquipmentCatalog.equipmentDefs) {
                if (equipmentDef.isBoss) {
                    PickupIndex index = equipmentDef.CreatePickupDef().pickupIndex;
                    self.availableEquipmentDropList.Remove(index);
                }
            }
        }

        private static Entry[] FixLogbook(On.RoR2.UI.LogBook.LogBookController.orig_BuildPickupEntries orig, Dictionary<ExpansionDef, bool> expansion) {
            Entry[] entries = orig(expansion);
            int numBoss = -1;
            int numVoid = -1;
            int counter = 0;
            foreach (Entry entry in entries) {
                PickupIndex index = (PickupIndex)entry.extraData;
                PickupDef pickupDef = index.pickupDef;
                if (pickupDef.itemIndex != ItemIndex.None) {
                    ItemDef def = ItemCatalog.GetItemDef(pickupDef.itemIndex);
                    if (def) {
                        if (def.deprecatedTier == ItemTier.VoidBoss) {
                            numVoid = counter;
                        }
                    }
                }

                counter++;
            }

            numVoid++;

            numBoss = entries.Length - 1;

            if (numBoss != -1) {
                for (int i = 0; i < entries.Length; i++) {
                    Entry entry = entries[i];
                    PickupIndex index = (PickupIndex)entry.extraData;
                    PickupDef pickupDef = index.pickupDef;
                    EquipmentIndex equipIndex = pickupDef.equipmentIndex;
                    if (equipIndex != EquipmentIndex.None) {
                        EquipmentDef def = EquipmentCatalog.GetEquipmentDef(equipIndex);
                        if (def.isBoss) {
                            ArrayUtils.ArrayRemoveAtAndResize(ref entries, i);
                            entry.bgTexture = Main.SecondaryAssets.LoadAsset<Texture>("Assets/Icons/BG/EquipBoss.png");
                            ArrayUtils.ArrayInsert(ref entries, numBoss, in entry);
                        }
                    }
                }
            }

            if (numVoid != -1) {
                foreach (ItemDef def in ItemCatalog.allItemDefs.Where(x => x._itemTierDef == Tiers.LunarVoid.Instance.tier)) {
                    Entry entry = new Entry{
                        nameToken = def.nameToken,
                        iconTexture = def.pickupIconTexture,
                        bgTexture = Main.SecondaryAssets.LoadAsset<Texture>("Assets/Icons/BG/LunarVoid.png"),
                        pageBuilderMethod = PageBuilder.SimplePickup,
                        color = ColorCatalog.GetColor(def.darkColorIndex),
                        isWIPImplementation = LogBookController.IsEntryPickupItemWithoutLore,
                        getStatusImplementation = LogBookController.GetPickupStatus,
                        getTooltipContentImplementation = LogBookController.GetPickupTooltipContent,
                        extraData = PickupCatalog.FindPickupIndex(def.itemIndex),
                        modelPrefab = def.pickupModelPrefab
                    };
                    ArrayUtils.ArrayInsert(ref entries, numVoid, in entry);
                }
            }

            return entries;
        }
    }
}