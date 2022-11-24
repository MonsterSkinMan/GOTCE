using RoR2;
using Unity;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace GOTCE.Misc {
    public class Fragile {
        private static List<ItemDef> fragileDefs;
        private static Dictionary<ItemDef, ItemDef> fragileMap;

        public static void Hook() {
            On.RoR2.HealthComponent.UpdateLastHitTime += UpdateLastHitServer;
        }

        private static void UpdateLastHitServer(On.RoR2.HealthComponent.orig_UpdateLastHitTime orig, HealthComponent self, float damage, Vector3 damagePosition, bool silent, GameObject attacker) {
            orig(self, damage, damagePosition, silent, attacker);
            /* if (NetworkServer.active && self.body.inventory) {
                foreach (ItemIndex index in self.body.inventory.itemAcquisitionOrder) {
                    ItemDef def = ItemCatalog.GetItemDef(index);

                    if (fragileDefs.Contains(def)) {
                        ItemDef broken = null;

                        bool found = fragileMap.TryGetValue(def, out broken);
                        
                        if (found && self.isHealthLow) {
                            Inventory inv = self.body.inventory;
                            int count = inv.GetItemCount(def);
                            inv.RemoveItem(def, count);
                            inv.GiveItem(broken, count);
                        }
                    }
                }
            } */ 
            // TODO: this doesnt work
        }

        /// <summary>
        /// makes an item break below 25% health and give the player the broken version
        /// </summary>
        /// <param name="fragileItem"> the fragile itemdef </param>
        /// <param name="brokenVersion"> the broken version to grant </param>
        public static void AddFragileItem(ItemDef fragileItem, ItemDef brokenVersion) {
            if (fragileItem && brokenVersion) {
                fragileDefs.Add(fragileItem);
                fragileMap.Add(fragileItem, brokenVersion);
            }
        }
    }
}