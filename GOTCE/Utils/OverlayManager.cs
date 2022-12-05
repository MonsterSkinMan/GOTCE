using GOTCE.Utils.Components;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using GOTCE.Items;
using R2API;
using HarmonyLib;
using Mono.Cecil;
using MonoMod.Cil;
using Mono.Cecil.Cil;

namespace GOTCE.Utils {
    public class OverlayManager {
        public struct Overlay {
            public EliteDef def;
            public Texture ramp;
        };

        private static List<Overlay> overlays;
        public static void AddOverlay(Overlay overlay) {
            if (overlay.def && overlay.ramp) {
                overlays.Add(overlay);
            }
        }

        public static void Hooks() {
            // On.RoR2.CharacterModel.UpdateMaterials += UpdateOverlays;
        }

        private static void UpdateOverlays(On.RoR2.CharacterModel.orig_UpdateMaterials orig, CharacterModel self) {
            orig(self);

            foreach (Overlay overlay in overlays) {
                if (self.body.equipmentSlot) {
                    EquipmentDef def = EquipmentCatalog.GetEquipmentDef(self.body.equipmentSlot.equipmentIndex);

                    bool isElite = def?.passiveBuffDef?.eliteDef?.eliteIndex == overlay.def.eliteIndex;

                    if (isElite) {
                        self.propertyStorage.SetTexture(Shader.PropertyToID("_EliteRamp"), overlay.ramp);
                    }
                }
            }
        }
    }
}