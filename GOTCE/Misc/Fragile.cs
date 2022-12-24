using RoR2;
using Unity;
using UnityEngine;
using System;
using System.Collections.Generic;
using RoR2.UI;

namespace GOTCE.Misc
{
    public class Fragile
    {
        private static List<ItemDef> fragileDefs;
        private static Dictionary<ItemDef, FragileInfo> fragileMap;

        public struct FragileInfo
        {
            public float fraction;
            public ItemDef broken;
            public bool shouldGiveBroken;
        }

        public static void Hook()
        {
            On.RoR2.HealthComponent.UpdateLastHitTime += UpdateLastHitServer;
            On.RoR2.UI.HealthBar.UpdateBarInfos += UpdateBarInfos;
            fragileDefs = new();
            fragileMap = new();
        }

        private static void UpdateLastHitServer(On.RoR2.HealthComponent.orig_UpdateLastHitTime orig, HealthComponent self, float damage, Vector3 damagePosition, bool silent, GameObject attacker)
        {
            if (NetworkServer.active && self.body && self.body.inventory)
            {
                ItemIndex[] indexes = self.body.inventory.itemAcquisitionOrder.ToArray();
                for (int i = 0; i < indexes.Length; i++)
                {
                    ItemDef def = ItemCatalog.GetItemDef(indexes[i]);
                    if (def && fragileDefs.Contains(def))
                    {
                        bool found = fragileMap.TryGetValue(def, out FragileInfo info);

                        if (found)
                        {
                            if (!self.IsAboveFraction(info.fraction))
                            {
                                int count = self.body.inventory.GetItemCount(def);

                                self.body.inventory.RemoveItem(def, count);

                                if (info.shouldGiveBroken)
                                {
                                    self.body.inventory.GiveItem(info.broken, count);
                                    CharacterMasterNotificationQueue.SendTransformNotification(self.body.master, def.itemIndex, info.broken.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);
                                }
                            }
                        }
                    }
                }
            }
            orig(self, damage, damagePosition, silent, attacker);
        }

        private static void UpdateBarInfos(On.RoR2.UI.HealthBar.orig_UpdateBarInfos orig, HealthBar self) {
            orig(self);
            float highest = 0.25f;
            if (self.source.body.inventory) {
                CharacterBody body = self.source.body;
                Inventory inventory = body.inventory;

                foreach (ItemIndex item in inventory.itemAcquisitionOrder) {
                    ItemDef def = ItemCatalog.GetItemDef(item);
                    if (fragileMap.TryGetValue(def, out FragileInfo info)) {
                        self.hasLowHealthItem = true;
                        float newFraction = info.fraction * 0.01f;
                        if (newFraction > highest) {
                            highest = newFraction;
                        }
                    }
                }
            }

            var val = self.source.GetHealthBarValues();
            self.barInfoCollection.lowHealthOverBarInfo.enabled = self.hasLowHealthItem && self.source.IsAboveFraction(highest * 100);
            self.barInfoCollection.lowHealthOverBarInfo.normalizedXMin = highest * (1f - val.curseFraction);
            self.barInfoCollection.lowHealthOverBarInfo.normalizedXMax = highest * (1f - val.curseFraction) + 0.005f;
            self.barInfoCollection.lowHealthOverBarInfo.color = self.style.lowHealthOverStyle.baseColor;
            self.barInfoCollection.lowHealthOverBarInfo.sprite = self.style.lowHealthOverStyle.sprite;
            self.barInfoCollection.lowHealthOverBarInfo.imageType = self.style.lowHealthOverStyle.imageType;
            self.barInfoCollection.lowHealthOverBarInfo.sizeDelta = self.style.lowHealthOverStyle.sizeDelta;
            
            self.barInfoCollection.lowHealthUnderBarInfo.enabled = self.hasLowHealthItem && !self.source.IsAboveFraction(highest * 100);
            self.barInfoCollection.lowHealthUnderBarInfo.normalizedXMin = 0f;
            self.barInfoCollection.lowHealthUnderBarInfo.normalizedXMax = highest * (1f - val.curseFraction);
            self.barInfoCollection.lowHealthUnderBarInfo.color = self.style.lowHealthUnderStyle.baseColor;
            self.barInfoCollection.lowHealthUnderBarInfo.sprite = self.style.lowHealthUnderStyle.sprite;
            self.barInfoCollection.lowHealthUnderBarInfo.imageType = self.style.lowHealthUnderStyle.imageType;
            self.barInfoCollection.lowHealthUnderBarInfo.sizeDelta = self.style.lowHealthUnderStyle.sizeDelta;
        }

        /// <summary>
        /// makes an item break below 25% health and give the player the broken version
        /// </summary>
        /// <param name="fragileItem"> the fragile itemdef </param>
        /// <param name="brokenVersion"> the broken version to grant </param>
        public static void AddFragileItem(ItemDef fragileItem, FragileInfo info)
        {
            if (fragileItem)
            {
                fragileDefs.Add(fragileItem);
                fragileMap.Add(fragileItem, info);
            }
        }
    }
}