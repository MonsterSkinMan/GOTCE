using RoR2;
using Unity;
using UnityEngine;
using System;
using System.Collections.Generic;

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
            fragileDefs = new();
            fragileMap = new();
        }

        private static void UpdateLastHitServer(On.RoR2.HealthComponent.orig_UpdateLastHitTime orig, HealthComponent self, float damage, Vector3 damagePosition, bool silent, GameObject attacker)
        {
            if (NetworkServer.active && self.body && self.body.inventory)
            {
                foreach (ItemIndex index in self.body.inventory.itemAcquisitionOrder)
                {
                    ItemDef def = ItemCatalog.GetItemDef(index);
                    if (def && fragileDefs.Contains(def))
                    {
                        Debug.Log("FragileDefs contained: " + def.nameToken);

                        bool found = fragileMap.TryGetValue(def, out FragileInfo info);

                        if (found)
                        {
                            Debug.Log("Found FragileInfo for: " + def.nameToken);
                            if (self.fullCombinedHealth * (0.01f * info.fraction) >= self.health)
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
            // TODO: this doesnt work
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