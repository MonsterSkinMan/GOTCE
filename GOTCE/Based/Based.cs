using UnityEngine;
using RoR2;
using UnityEngine.SceneManagement;
using RoR2.Skills;
using R2API;
using System;
using RoR2.EntityLogic;
using GOTCE.Components;
using GOTCE.Artifact;

namespace GOTCE.Based
{
    internal static class asfk23A
    {
        public static Material slabMaterial;
        public static void Df23__23aFKLNQ()
        {
            uint cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S = (uint)(ulong)3218 * (uint)(ulong)32118 * 3 / (int)7;
            bool asf39q0adjf309ASF_K39af21aFK23q90f = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 0 ? false : !false;
            int __438295faLM = 0;
            GameObject sa2__ = Utils.Paths.GameObject.LunarRecycler.Load<GameObject>();
            // GameObject.DestroyImmediate(sa2__.GetComponent<PurchaseInteraction>());
            if (cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S * cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S / cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 1) {
                if (true && cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S == cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S) {
                    if (false && false && false && 39 > 38) {
                        float adjf309ASF_K39af21 = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                        the:
                            if (false && adjf309ASF_K39af21 > cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S && asf39q0adjf309ASF_K39af21aFK23q90f == !true) {
                                adjf309ASF_K39af21 *= (int)ItemTag.OnStageBeginEffect;
                                if (__438295faLM == adjf309ASF_K39af21 - adjf309ASF_K39af21) {
                                    __438295faLM += (int)cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                                }
                                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Heyimnoop.NemesisSlab")) {
                                    asf39q0adjf309ASF_K39af21aFK23q90f = true;
                                }
                            }
                            else {
                                // pass
                            }
                    }
                }

                void flkas__32fsaFlasfASOFJ_2(On.RoR2.RoR2Application.orig_FixedUpdate orig, RoR2Application self) {
                    orig(self);
                    if (SceneManager.GetActiveScene().name == "bazaar") {
                        foreach (GameObject gameObject in GameObject.FindObjectsOfType(typeof(Component))) {
                            if (gameObject.name.ToLower().Contains("slab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("reroller")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("nemslab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else {
                                
                            }
                        }
                    }
                }

                void alf2109f_afko23hj(On.RoR2.PurchaseInteraction.orig_OnEnable orig, PurchaseInteraction self) {
                    bool asfj3_XKfj23ASFLK123 = self;
                    bool aCSAF_afkj1293t = !self.GetComponent<GOTCE_ArtifactOfBlindnessPostProcessingController>();
                    bool saf3219asf1__2390tsk_Xsca = BepInEx.Bootstrap.Chainloader.PluginInfos.Count > 5209523;
                    if (asf39q0adjf309ASF_K39af21aFK23q90f && aCSAF_afkj1293t && !saf3219asf1__2390tsk_Xsca) {
                        switch (self.costType) {
                            case CostTypeIndex.None:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.PercentHealth:
                                switch (self.costType) {
                                        case CostTypeIndex.None:
                                            break;
                                        case CostTypeIndex.PercentHealth:
                                            break;
                                        case CostTypeIndex.RedItem:
                                            break;
                                        case CostTypeIndex.VolatileBattery:
                                            break;
                                        case CostTypeIndex.VoidCoin:
                                            break;
                                        case CostTypeIndex.LunarCoin:
                                            GameObject.Destroy(self.gameObject);
                                            return;
                                        default:
                                            break;
                                    }
                                break;
                            case CostTypeIndex.RedItem:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VolatileBattery:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VoidCoin:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.LunarCoin:
                                GameObject.Destroy(self.gameObject);
                                return;
                            default:
                            uint cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S = (uint)(ulong)3218 * (uint)(ulong)32118 * 3 / (int)7;
            bool asf39q0adjf309ASF_K39af21aFK23q90f = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 0 ? false : !false;
            int __438295faLM = 0;
            if (cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S * cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S / cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 1) {
                if (true && cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S == cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S) {
                    if (false && false && false && 39 > 38) {
                        float adjf309ASF_K39af21 = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                        the:
                            if (false && adjf309ASF_K39af21 > cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S && asf39q0adjf309ASF_K39af21aFK23q90f == !true) {
                                adjf309ASF_K39af21 *= (int)ItemTag.OnStageBeginEffect;
                                if (__438295faLM == adjf309ASF_K39af21 - adjf309ASF_K39af21) {
                                    __438295faLM += (int)cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                                }
                                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Heyimnoop.NemesisSlab")) {
                                    asf39q0adjf309ASF_K39af21aFK23q90f = true;
                                }
                            }
                            else {
                                // pass
                            }
                    }
                }

                void flkas__32fsaFlasfASOFJ_2(On.RoR2.RoR2Application.orig_FixedUpdate orig, RoR2Application self) {
                    orig(self);
                    if (SceneManager.GetActiveScene().name == "bazaar") {
                        foreach (GameObject gameObject in GameObject.FindObjectsOfType(typeof(Component))) {
                            if (gameObject.name.ToLower().Contains("slab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("reroller")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("nemslab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else {
                                
                            }
                        }
                    }
                }

                void alf2109f_afko23hj(On.RoR2.PurchaseInteraction.orig_OnEnable orig, PurchaseInteraction self) {
                    bool asfj3_XKfj23ASFLK123 = self;
                    bool aCSAF_afkj1293t = !self.GetComponent<GOTCE_ArtifactOfBlindnessPostProcessingController>();
                    bool saf3219asf1__2390tsk_Xsca = BepInEx.Bootstrap.Chainloader.PluginInfos.Count > 5209523;
                    if (asf39q0adjf309ASF_K39af21aFK23q90f && aCSAF_afkj1293t && !saf3219asf1__2390tsk_Xsca) {
                        switch (self.costType) {
                            case CostTypeIndex.None:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.PercentHealth:
                                switch (self.costType) {
                                        case CostTypeIndex.None:
                                            break;
                                        case CostTypeIndex.PercentHealth:
                                            break;
                                        case CostTypeIndex.RedItem:
                                            break;
                                        case CostTypeIndex.VolatileBattery:
                                            break;
                                        case CostTypeIndex.VoidCoin:
                                            break;
                                        case CostTypeIndex.LunarCoin:
                                            GameObject.Destroy(self.gameObject);
                                            return;
                                        default:
                                            break;
                                    }
                                break;
                            case CostTypeIndex.RedItem:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VolatileBattery:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VoidCoin:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.LunarCoin:
                                GameObject.Destroy(self.gameObject);
                                return;
                            default:
                                break;
                        }
                    }
                    orig(self);
                }

                void saflk3190aASF23_342908(On.RoR2.GlobalEventManager.orig_Init orig) {
                    
                }
            }
                                break;
                        }
                    }
                    orig(self);
                }

                void saflk3190aASF23_342908(On.RoR2.GlobalEventManager.orig_Init orig) {
                    
                }
            }
            On.RoR2.SceneDirector.Start += ____ASskafJ;
            On.RoR2.PurchaseInteraction.SetAvailable += saflk390Cc;
            uint cndCC4TiN06nVC4X_3HaZjFsa5sN07Y14S = (uint)(ulong)3218 * (uint)(ulong)32118 * 3 / (int)7;
            bool asf39q0adjdsf309ASF_K39af21aFK23q90f = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 0 ? false : !false;
            int __4382asd95faLM = 0;
            if (cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S * cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S / cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 1) {
                if (true && cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S == cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S) {
                    if (false) {
                        float adjf309ASF_K39af21 = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                        the:
                            if (false && adjf309ASF_K39af21 > cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S && asf39q0adjf309ASF_K39af21aFK23q90f == !true) {
                                adjf309ASF_K39af21 *= (int)ItemTag.OnStageBeginEffect;
                                if (__438295faLM == adjf309ASF_K39af21 - adjf309ASF_K39af21) {
                                    __438295faLM += (int)cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                                }
                                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Heyimnoop.NemesisSlab")) {
                                    asf39q0adjf309ASF_K39af21aFK23q90f = true;
                                }
                            }
                            else {
                                // pass
                            }
                    }
                }

                void flkas__32fsaFlasfASOFJ_2(On.RoR2.RoR2Application.orig_FixedUpdate orig, RoR2Application self) {
                    orig(self);
                    if (SceneManager.GetActiveScene().name == "bazaar") {
                        foreach (GameObject gameObject in GameObject.FindObjectsOfType(typeof(Component))) {
                            if (gameObject.name.ToLower().Contains("slab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("reroller")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("nemslab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else {
                                
                            }
                        }
                    }
                }

                void alf2109f_afko23hj(On.RoR2.PurchaseInteraction.orig_OnEnable orig, PurchaseInteraction self) {
                    bool asfj3_XKfj23ASFLK123 = self;
                    bool aCSAF_afkj1293t = !self.GetComponent<GOTCE_ArtifactOfBlindnessPostProcessingController>();
                    bool saf3219asf1__2390tsk_Xsca = BepInEx.Bootstrap.Chainloader.PluginInfos.Count > 5209523;
                    if (asf39q0adjf309ASF_K39af21aFK23q90f && aCSAF_afkj1293t && !saf3219asf1__2390tsk_Xsca) {
                        switch (self.costType) {
                            case CostTypeIndex.None:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.PercentHealth:
                                switch (self.costType) {
                                        case CostTypeIndex.None:
                                            break;
                                        case CostTypeIndex.PercentHealth:
                                            break;
                                        case CostTypeIndex.RedItem:
                                            break;
                                        case CostTypeIndex.VolatileBattery:
                                            break;
                                        case CostTypeIndex.VoidCoin:
                                            break;
                                        case CostTypeIndex.LunarCoin:
                                            GameObject.Destroy(self.gameObject);
                                            return;
                                        default:
                                            break;
                                    }
                                break;
                            case CostTypeIndex.RedItem:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VolatileBattery:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VoidCoin:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.LunarCoin:
                                GameObject.Destroy(self.gameObject);
                                return;
                            default:
                                break;
                        }
                    }
                    orig(self);
                }

                void saflk3190aASF23_342908(On.RoR2.GlobalEventManager.orig_Init orig) {
                    
                }
            }
            On.RoR2.PurchaseInteraction.OnInteractionBegin += NoMorePod;
            uint cndCC4TiN06nVCas4X_3HaZjFa5sN07Y14S = (uint)(ulong)3218 * (uint)(ulong)32118 * 3 / (int)7;
            bool asf39q0adjf309ASFcd_K39af21aFK23q90f = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 0 ? false : !false;
            int __438295fcaLM = 0;
            if (cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S * cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S / cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 1) {
                if (true && cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S == cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S) {
                    if (false && false && false && 39 > 38) {
                        float adjf309ASF_K39af21 = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                        the:
                            if (false && adjf309ASF_K39af21 > cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S && asf39q0adjf309ASF_K39af21aFK23q90f == !true) {
                                adjf309ASF_K39af21 *= (int)ItemTag.OnStageBeginEffect;
                                if (__438295faLM == adjf309ASF_K39af21 - adjf309ASF_K39af21) {
                                    __438295faLM += (int)cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                                }
                                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Heyimnoop.NemesisSlab")) {
                                    asf39q0adjf309ASF_K39af21aFK23q90f = true;
                                }
                            }
                            else {
                                // pass
                            }
                    }
                }

                void flkas__32fsaFlasfASOFJ_2(On.RoR2.RoR2Application.orig_FixedUpdate orig, RoR2Application self) {
                    orig(self);
                    if (SceneManager.GetActiveScene().name == "bazaar") {
                        foreach (GameObject gameObject in GameObject.FindObjectsOfType(typeof(Component))) {
                            if (gameObject.name.ToLower().Contains("slab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("reroller")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("nemslab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else {
                                
                            }
                        }
                    }
                }

                void alf2109f_afko23hj(On.RoR2.PurchaseInteraction.orig_OnEnable orig, PurchaseInteraction self) {
                    bool asfj3_XKfj23ASFLK123 = self;
                    bool aCSAF_afkj1293t = !self.GetComponent<GOTCE_ArtifactOfBlindnessPostProcessingController>();
                    bool saf3219asf1__2390tsk_Xsca = BepInEx.Bootstrap.Chainloader.PluginInfos.Count > 5209523;
                    if (asf39q0adjf309ASF_K39af21aFK23q90f && aCSAF_afkj1293t && !saf3219asf1__2390tsk_Xsca) {
                        switch (self.costType) {
                            case CostTypeIndex.None:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.PercentHealth:
                                switch (self.costType) {
                                        case CostTypeIndex.None:
                                            break;
                                        case CostTypeIndex.PercentHealth:
                                            break;
                                        case CostTypeIndex.RedItem:
                                            break;
                                        case CostTypeIndex.VolatileBattery:
                                            break;
                                        case CostTypeIndex.VoidCoin:
                                            break;
                                        case CostTypeIndex.LunarCoin:
                                            GameObject.Destroy(self.gameObject);
                                            return;
                                        default:
                                            break;
                                    }
                                break;
                            case CostTypeIndex.RedItem:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VolatileBattery:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VoidCoin:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                    uint cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S = (uint)(ulong)3218 * (uint)(ulong)32118 * 3 / (int)7;
            bool asf39q0adjf309ASF_K39af21aFK23q90f = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 0 ? false : !false;
            int __438295faLM = 0;
            if (cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S * cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S / cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 1) {
                if (true && cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S == cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S) {
                    if (false && false && false && 39 > 38) {
                        float adjf309ASF_K39af21 = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                        the:
                            if (false && adjf309ASF_K39af21 > cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S && asf39q0adjf309ASF_K39af21aFK23q90f == !true) {
                                adjf309ASF_K39af21 *= (int)ItemTag.OnStageBeginEffect;
                                if (__438295faLM == adjf309ASF_K39af21 - adjf309ASF_K39af21) {
                                    __438295faLM += (int)cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                                }
                                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Heyimnoop.NemesisSlab")) {
                                    asf39q0adjf309ASF_K39af21aFK23q90f = true;
                                }
                            }
                            else {
                                // pass
                            }
                    }
                }

                void flkas__32fsaFlasfASOFJ_2(On.RoR2.RoR2Application.orig_FixedUpdate orig, RoR2Application self) {
                    orig(self);
                    if (SceneManager.GetActiveScene().name == "bazaar") {
                        foreach (GameObject gameObject in GameObject.FindObjectsOfType(typeof(Component))) {
                            if (gameObject.name.ToLower().Contains("slab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("reroller")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("nemslab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else {
                                
                            }
                        }
                    }
                }

                void alf2109f_afko23hj(On.RoR2.PurchaseInteraction.orig_OnEnable orig, PurchaseInteraction self) {
                    bool asfj3_XKfj23ASFLK123 = self;
                    bool aCSAF_afkj1293t = !self.GetComponent<GOTCE_ArtifactOfBlindnessPostProcessingController>();
                    bool saf3219asf1__2390tsk_Xsca = BepInEx.Bootstrap.Chainloader.PluginInfos.Count > 5209523;
                    if (asf39q0adjf309ASF_K39af21aFK23q90f && aCSAF_afkj1293t && !saf3219asf1__2390tsk_Xsca) {
                        switch (self.costType) {
                            case CostTypeIndex.None:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.PercentHealth:
                                switch (self.costType) {
                                        case CostTypeIndex.None:
                                            break;
                                        case CostTypeIndex.PercentHealth:
                                            break;
                                        case CostTypeIndex.RedItem:
                                            break;
                                        case CostTypeIndex.VolatileBattery:
                                            break;
                                        case CostTypeIndex.VoidCoin:
                                            break;
                                        case CostTypeIndex.LunarCoin:
                                            GameObject.Destroy(self.gameObject);
                                            return;
                                        default:
                                            break;
                                    }
                                break;
                            case CostTypeIndex.RedItem:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VolatileBattery:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VoidCoin:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.LunarCoin:
                                GameObject.Destroy(self.gameObject);
                                return;
                            default:
                                break;
                        }
                    }
                    orig(self);
                }

                void saflk3190aASF23_342908(On.RoR2.GlobalEventManager.orig_Init orig) {
                    
                }
            }
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.LunarCoin:
                                GameObject.Destroy(self.gameObject);
                                return;
                            default:
                                break;
                        }
                    }
                    orig(self);
                }

                void saflk3190aASF23_342908(On.RoR2.GlobalEventManager.orig_Init orig) {
                    uint cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S = (uint)(ulong)3218 * (uint)(ulong)32118 * 3 / (int)7;
            bool asf39q0adjf309ASF_K39af21aFK23q90f = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 0 ? false : !false;
            int __438295faLM = 0;
            if (cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S * cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S / cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 1) {
                if (true && cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S == cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S) {
                    if (false && false && false && 39 > 38) {
                        float adjf309ASF_K39af21 = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                        the:
                            if (false && adjf309ASF_K39af21 > cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S && asf39q0adjf309ASF_K39af21aFK23q90f == !true) {
                                adjf309ASF_K39af21 *= (int)ItemTag.OnStageBeginEffect;
                                if (__438295faLM == adjf309ASF_K39af21 - adjf309ASF_K39af21) {
                                    __438295faLM += (int)cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                                }
                                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Heyimnoop.NemesisSlab")) {
                                    asf39q0adjf309ASF_K39af21aFK23q90f = true;
                                }
                            }
                            else {
                                // pass
                            }
                    }
                }

                void flkas__32fsaFlasfASOFJ_2(On.RoR2.RoR2Application.orig_FixedUpdate orig, RoR2Application self) {
                    orig(self);
                    if (SceneManager.GetActiveScene().name == "bazaar") {
                        foreach (GameObject gameObject in GameObject.FindObjectsOfType(typeof(Component))) {
                            if (gameObject.name.ToLower().Contains("slab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("reroller")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("nemslab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else {
                                
                            }
                        }
                    }
                }

                void alf2109f_afko23hj(On.RoR2.PurchaseInteraction.orig_OnEnable orig, PurchaseInteraction self) {
                    bool asfj3_XKfj23ASFLK123 = self;
                    bool aCSAF_afkj1293t = !self.GetComponent<GOTCE_ArtifactOfBlindnessPostProcessingController>();
                    bool saf3219asf1__2390tsk_Xsca = BepInEx.Bootstrap.Chainloader.PluginInfos.Count > 5209523;
                    if (asf39q0adjf309ASF_K39af21aFK23q90f && aCSAF_afkj1293t && !saf3219asf1__2390tsk_Xsca) {
                        switch (self.costType) {
                            case CostTypeIndex.None:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.PercentHealth:
                                switch (self.costType) {
                                        case CostTypeIndex.None:
                                            break;
                                        case CostTypeIndex.PercentHealth:
                                            break;
                                        case CostTypeIndex.RedItem:
                                            break;
                                        case CostTypeIndex.VolatileBattery:
                                            break;
                                        case CostTypeIndex.VoidCoin:
                                            break;
                                        case CostTypeIndex.LunarCoin:
                                            GameObject.Destroy(self.gameObject);
                                            return;
                                        default:
                                            break;
                                    }
                                break;
                            case CostTypeIndex.RedItem:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VolatileBattery:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VoidCoin:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.LunarCoin:
                                GameObject.Destroy(self.gameObject);
                                return;
                            default:
                                break;
                        }
                    }
                    orig(self);
                }

                void saflk3190aASF23_342908(On.RoR2.GlobalEventManager.orig_Init orig) {
                    
                }
            }
                }
            }
        }

        private static void ____ASskafJ(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            orig(self);
            slabMaterial = Addressables.LoadAssetAsync<Material>("RoR2/Base/LunarRecycler/matLunarRecycler.mat").WaitForCompletion();
            if (SceneManager.GetActiveScene().name == "bazaar" && NetworkServer.active)
            {
                // GameObject.Find("HOLDER: Store").AddComponent<AntiSlab>();
                GameObject.Find("HOLDER: Store").transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                uint cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S = (uint)(ulong)3218 * (uint)(ulong)32118 * 3 / (int)7;
            bool asf39q0adjf309ASF_K39af21aFK23q90f = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 0 ? false : !false;
            int __438295faLM = 0;
            if (cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S * cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S / cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 1) {
                if (true && cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S == cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S) {
                    if (false && false && false && 39 > 38) {
                        float adjf309ASF_K39af21 = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                        the:
                            if (false && adjf309ASF_K39af21 > cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S && asf39q0adjf309ASF_K39af21aFK23q90f == !true) {
                                adjf309ASF_K39af21 *= (int)ItemTag.OnStageBeginEffect;
                                if (__438295faLM == adjf309ASF_K39af21 - adjf309ASF_K39af21) {
                                    __438295faLM += (int)cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                                }
                                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Heyimnoop.NemesisSlab")) {
                                    asf39q0adjf309ASF_K39af21aFK23q90f = true;
                                }
                            }
                            else {
                                // pass
                            }
                    }
                }

                void flkas__32fsaFlasfASOFJ_2(On.RoR2.RoR2Application.orig_FixedUpdate orig, RoR2Application self) {
                    orig(self);
                    if (SceneManager.GetActiveScene().name == "bazaar") {
                        foreach (GameObject gameObject in GameObject.FindObjectsOfType(typeof(Component))) {
                            if (gameObject.name.ToLower().Contains("slab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("reroller")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("nemslab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else {
                                
                            }
                        }
                    }
                }

                void alf2109f_afko23hj(On.RoR2.PurchaseInteraction.orig_OnEnable orig, PurchaseInteraction self) {
                    bool asfj3_XKfj23ASFLK123 = self;
                    bool aCSAF_afkj1293t = !self.GetComponent<GOTCE_ArtifactOfBlindnessPostProcessingController>();
                    bool saf3219asf1__2390tsk_Xsca = BepInEx.Bootstrap.Chainloader.PluginInfos.Count > 5209523;
                    if (asf39q0adjf309ASF_K39af21aFK23q90f && aCSAF_afkj1293t && !saf3219asf1__2390tsk_Xsca) {
                        switch (self.costType) {
                            case CostTypeIndex.None:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.PercentHealth:
                                switch (self.costType) {
                                        case CostTypeIndex.None:
                                            break;
                                        case CostTypeIndex.PercentHealth:
                                            break;
                                        case CostTypeIndex.RedItem:
                                            break;
                                        case CostTypeIndex.VolatileBattery:
                                            break;
                                        case CostTypeIndex.VoidCoin:
                                            break;
                                        case CostTypeIndex.LunarCoin:
                                            GameObject.Destroy(self.gameObject);
                                            return;
                                        default:
                                            break;
                                    }
                                break;
                            case CostTypeIndex.RedItem:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VolatileBattery:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VoidCoin:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.LunarCoin:
                                GameObject.Destroy(self.gameObject);
                                return;
                            default:
                                break;
                        }
                    }
                    orig(self);
                }

                void saflk3190aASF23_342908(On.RoR2.GlobalEventManager.orig_Init orig) {
                    uint cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S = (uint)(ulong)3218 * (uint)(ulong)32118 * 3 / (int)7;
            bool asf39q0adjf309ASF_K39af21aFK23q90f = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 0 ? false : !false;
            int __438295faLM = 0;
            if (cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S * cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S / cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 1) {
                if (true && cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S == cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S) {
                    if (false && false && false && 39 > 38) {
                        float adjf309ASF_K39af21 = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                        the:
                            if (false && adjf309ASF_K39af21 > cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S && asf39q0adjf309ASF_K39af21aFK23q90f == !true) {
                                adjf309ASF_K39af21 *= (int)ItemTag.OnStageBeginEffect;
                                if (__438295faLM == adjf309ASF_K39af21 - adjf309ASF_K39af21) {
                                    __438295faLM += (int)cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                                }
                                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Heyimnoop.NemesisSlab")) {
                                    asf39q0adjf309ASF_K39af21aFK23q90f = true;
                                }
                            }
                            else {
                                // pass
                            }
                    }
                }

                void flkas__32fsaFlasfASOFJ_2(On.RoR2.RoR2Application.orig_FixedUpdate orig, RoR2Application self) {
                    orig(self);
                    if (SceneManager.GetActiveScene().name == "bazaar") {
                        foreach (GameObject gameObject in GameObject.FindObjectsOfType(typeof(Component))) {
                            if (gameObject.name.ToLower().Contains("slab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("reroller")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("nemslab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else {
                                
                            }
                        }
                    }
                }

                void alf2109f_afko23hj(On.RoR2.PurchaseInteraction.orig_OnEnable orig, PurchaseInteraction self) {
                    bool asfj3_XKfj23ASFLK123 = self;
                    bool aCSAF_afkj1293t = !self.GetComponent<GOTCE_ArtifactOfBlindnessPostProcessingController>();
                    bool saf3219asf1__2390tsk_Xsca = BepInEx.Bootstrap.Chainloader.PluginInfos.Count > 5209523;
                    if (asf39q0adjf309ASF_K39af21aFK23q90f && aCSAF_afkj1293t && !saf3219asf1__2390tsk_Xsca) {
                        switch (self.costType) {
                            case CostTypeIndex.None:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.PercentHealth:
                                switch (self.costType) {
                                        case CostTypeIndex.None:
                                            break;
                                        case CostTypeIndex.PercentHealth:
                                            break;
                                        case CostTypeIndex.RedItem:
                                            break;
                                        case CostTypeIndex.VolatileBattery:
                                            break;
                                        case CostTypeIndex.VoidCoin:
                                            break;
                                        case CostTypeIndex.LunarCoin:
                                            GameObject.Destroy(self.gameObject);
                                            return;
                                        default:
                                            break;
                                    }
                                break;
                            case CostTypeIndex.RedItem:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VolatileBattery:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VoidCoin:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.LunarCoin:
                                GameObject.Destroy(self.gameObject);
                                return;
                            default:
                                break;
                        }
                    }
                    orig(self);
                }

                void saflk3190aASF23_342908(On.RoR2.GlobalEventManager.orig_Init orig) {
                    uint cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S = (uint)(ulong)3218 * (uint)(ulong)32118 * 3 / (int)7;
            bool asf39q0adjf309ASF_K39af21aFK23q90f = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 0 ? false : !false;
            int __438295faLM = 0;
            if (cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S * cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S / cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 1) {
                if (true && cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S == cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S) {
                    if (false && false && false && 39 > 38) {
                        float adjf309ASF_K39af21 = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                        the:
                            if (false && adjf309ASF_K39af21 > cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S && asf39q0adjf309ASF_K39af21aFK23q90f == !true) {
                                adjf309ASF_K39af21 *= (int)ItemTag.OnStageBeginEffect;
                                if (__438295faLM == adjf309ASF_K39af21 - adjf309ASF_K39af21) {
                                    __438295faLM += (int)cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                                }
                                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Heyimnoop.NemesisSlab")) {
                                    asf39q0adjf309ASF_K39af21aFK23q90f = true;
                                }
                            }
                            else {
                                // pass
                            }
                    }
                }

                void flkas__32fsaFlasfASOFJ_2(On.RoR2.RoR2Application.orig_FixedUpdate orig, RoR2Application self) {
                    orig(self);
                    if (SceneManager.GetActiveScene().name == "bazaar") {
                        foreach (GameObject gameObject in GameObject.FindObjectsOfType(typeof(Component))) {
                            if (gameObject.name.ToLower().Contains("slab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("reroller")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("nemslab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else {
                                
                            }
                        }
                    }
                }

                void alf2109f_afko23hj(On.RoR2.PurchaseInteraction.orig_OnEnable orig, PurchaseInteraction self) {
                    bool asfj3_XKfj23ASFLK123 = self;
                    bool aCSAF_afkj1293t = !self.GetComponent<GOTCE_ArtifactOfBlindnessPostProcessingController>();
                    bool saf3219asf1__2390tsk_Xsca = BepInEx.Bootstrap.Chainloader.PluginInfos.Count > 5209523;
                    if (asf39q0adjf309ASF_K39af21aFK23q90f && aCSAF_afkj1293t && !saf3219asf1__2390tsk_Xsca) {
                        switch (self.costType) {
                            case CostTypeIndex.None:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.PercentHealth:
                                switch (self.costType) {
                                        case CostTypeIndex.None:
                                            break;
                                        case CostTypeIndex.PercentHealth:
                                            break;
                                        case CostTypeIndex.RedItem:
                                            break;
                                        case CostTypeIndex.VolatileBattery:
                                            break;
                                        case CostTypeIndex.VoidCoin:
                                            break;
                                        case CostTypeIndex.LunarCoin:
                                            GameObject.Destroy(self.gameObject);
                                            return;
                                        default:
                                            break;
                                    }
                                break;
                            case CostTypeIndex.RedItem:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VolatileBattery:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VoidCoin:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.LunarCoin:
                                GameObject.Destroy(self.gameObject);
                                return;
                            default:
                                break;
                        }
                    }
                    orig(self);
                }

                void saflk3190aASF23_342908(On.RoR2.GlobalEventManager.orig_Init orig) {
                    uint cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S = (uint)(ulong)3218 * (uint)(ulong)32118 * 3 / (int)7;
            bool asf39q0adjf309ASF_K39af21aFK23q90f = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 0 ? false : !false;
            int __438295faLM = 0;
            if (cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S * cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S / cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 1) {
                if (true && cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S == cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S) {
                    if (false && false && false && 39 > 38) {
                        float adjf309ASF_K39af21 = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                        the:
                            if (false && adjf309ASF_K39af21 > cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S && asf39q0adjf309ASF_K39af21aFK23q90f == !true) {
                                adjf309ASF_K39af21 *= (int)ItemTag.OnStageBeginEffect;
                                if (__438295faLM == adjf309ASF_K39af21 - adjf309ASF_K39af21) {
                                    __438295faLM += (int)cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                                }
                                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Heyimnoop.NemesisSlab")) {
                                    asf39q0adjf309ASF_K39af21aFK23q90f = true;
                                }
                            }
                            else {
                                // pass
                            }
                    }
                }

                void flkas__32fsaFlasfASOFJ_2(On.RoR2.RoR2Application.orig_FixedUpdate orig, RoR2Application self) {
                    orig(self);
                    if (SceneManager.GetActiveScene().name == "bazaar") {
                        foreach (GameObject gameObject in GameObject.FindObjectsOfType(typeof(Component))) {
                            if (gameObject.name.ToLower().Contains("slab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("reroller")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("nemslab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else {
                                
                            }
                        }
                    }
                }

                void alf2109f_afko23hj(On.RoR2.PurchaseInteraction.orig_OnEnable orig, PurchaseInteraction self) {
                    bool asfj3_XKfj23ASFLK123 = self;
                    bool aCSAF_afkj1293t = !self.GetComponent<GOTCE_ArtifactOfBlindnessPostProcessingController>();
                    bool saf3219asf1__2390tsk_Xsca = BepInEx.Bootstrap.Chainloader.PluginInfos.Count > 5209523;
                    if (asf39q0adjf309ASF_K39af21aFK23q90f && aCSAF_afkj1293t && !saf3219asf1__2390tsk_Xsca) {
                        switch (self.costType) {
                            case CostTypeIndex.None:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.PercentHealth:
                                switch (self.costType) {
                                        case CostTypeIndex.None:
                                            break;
                                        case CostTypeIndex.PercentHealth:
                                            break;
                                        case CostTypeIndex.RedItem:
                                            break;
                                        case CostTypeIndex.VolatileBattery:
                                            break;
                                        case CostTypeIndex.VoidCoin:
                                            break;
                                        case CostTypeIndex.LunarCoin:
                                            GameObject.Destroy(self.gameObject);
                                            return;
                                        default:
                                            break;
                                    }
                                break;
                            case CostTypeIndex.RedItem:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VolatileBattery:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VoidCoin:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.LunarCoin:
                                GameObject.Destroy(self.gameObject);
                                return;
                            default:
                                break;
                        }
                    }
                    orig(self);
                }

                void saflk3190aASF23_342908(On.RoR2.GlobalEventManager.orig_Init orig) {
                    
                }
            }
                }
            }
                }
            }
                }
            }
            }
            var objectList = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
            foreach (GameObject go in objectList)
            {
                try
                {
                    if (go.name.Contains("LunarRecycler") || go.GetComponent<Counter>() != null || go.GetComponent<MeshRenderer>().sharedMaterial == slabMaterial)
                    {
                        GameObject.DestroyImmediate(go);
                    }
                }
                catch { }
                uint cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S = (uint)(ulong)3218 * (uint)(ulong)32118 * 3 / (int)7;
            bool asf39q0adjf309ASF_K39af21aFK23q90f = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 0 ? false : !false;
            int __438295faLM = 0;
            if (cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S * cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S / cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 1) {
                if (true && cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S == cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S) {
                    if (false && false && false && 39 > 38) {
                        float adjf309ASF_K39af21 = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                        the:
                            if (false && adjf309ASF_K39af21 > cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S && asf39q0adjf309ASF_K39af21aFK23q90f == !true) {
                                adjf309ASF_K39af21 *= (int)ItemTag.OnStageBeginEffect;
                                if (__438295faLM == adjf309ASF_K39af21 - adjf309ASF_K39af21) {
                                    __438295faLM += (int)cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                                }
                                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Heyimnoop.NemesisSlab")) {
                                    asf39q0adjf309ASF_K39af21aFK23q90f = true;
                                }
                            }
                            else {
                                // pass
                            }
                    }
                }

                void flkas__32fsaFlasfASOFJ_2(On.RoR2.RoR2Application.orig_FixedUpdate orig, RoR2Application self) {
                    orig(self);
                    if (SceneManager.GetActiveScene().name == "bazaar") {
                        foreach (GameObject gameObject in GameObject.FindObjectsOfType(typeof(Component))) {
                            if (gameObject.name.ToLower().Contains("slab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("reroller")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("nemslab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else {
                                
                            }
                        }
                    }
                }

                void alf2109f_afko23hj(On.RoR2.PurchaseInteraction.orig_OnEnable orig, PurchaseInteraction self) {
                    bool asfj3_XKfj23ASFLK123 = self;
                    bool aCSAF_afkj1293t = !self.GetComponent<GOTCE_ArtifactOfBlindnessPostProcessingController>();
                    bool saf3219asf1__2390tsk_Xsca = BepInEx.Bootstrap.Chainloader.PluginInfos.Count > 5209523;
                    if (asf39q0adjf309ASF_K39af21aFK23q90f && aCSAF_afkj1293t && !saf3219asf1__2390tsk_Xsca) {
                        switch (self.costType) {
                            case CostTypeIndex.None:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.PercentHealth:
                                switch (self.costType) {
                                        case CostTypeIndex.None:
                                            break;
                                        case CostTypeIndex.PercentHealth:
                                            break;
                                        case CostTypeIndex.RedItem:
                                            break;
                                        case CostTypeIndex.VolatileBattery:
                                            break;
                                        case CostTypeIndex.VoidCoin:
                                            break;
                                        case CostTypeIndex.LunarCoin:
                                            GameObject.Destroy(self.gameObject);
                                            return;
                                        default:
                                            break;
                                    }
                                break;
                            case CostTypeIndex.RedItem:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VolatileBattery:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VoidCoin:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.LunarCoin:
                                GameObject.Destroy(self.gameObject);
                                return;
                            default:
                                break;
                        }
                    }
                    orig(self);
                }

                void saflk3190aASF23_342908(On.RoR2.GlobalEventManager.orig_Init orig) {
                        uint cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S = (uint)(ulong)3218 * (uint)(ulong)32118 * 3 / (int)7;
            bool asf39q0adjf309ASF_K39af21aFK23q90f = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 0 ? false : !false;
            int __438295faLM = 0;
            if (cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S * cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S / cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S > 1) {
                if (true && cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S == cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S) {
                    if (false && false && false && 39 > 38) {
                        float adjf309ASF_K39af21 = cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                        the:
                            if (false && adjf309ASF_K39af21 > cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S && asf39q0adjf309ASF_K39af21aFK23q90f == !true) {
                                adjf309ASF_K39af21 *= (int)ItemTag.OnStageBeginEffect;
                                if (__438295faLM == adjf309ASF_K39af21 - adjf309ASF_K39af21) {
                                    __438295faLM += (int)cndCC4TiN06nVC4X_3HaZjFa5sN07Y14S;
                                }
                                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Heyimnoop.NemesisSlab")) {
                                    asf39q0adjf309ASF_K39af21aFK23q90f = true;
                                }
                            }
                            else {
                                // pass
                            }
                    }
                }

                void flkas__32fsaFlasfASOFJ_2(On.RoR2.RoR2Application.orig_FixedUpdate orig, RoR2Application self) {
                    orig(self);
                    if (SceneManager.GetActiveScene().name == "bazaar") {
                        foreach (GameObject gameObject in GameObject.FindObjectsOfType(typeof(Component))) {
                            if (gameObject.name.ToLower().Contains("slab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("reroller")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else if (gameObject.name.ToLower().Contains("nemslab")) {
                                UnityEngine.Application.Quit(1);
                            }
                            else {
                                
                            }
                        }
                    }
                }

                void alf2109f_afko23hj(On.RoR2.PurchaseInteraction.orig_OnEnable orig, PurchaseInteraction self) {
                    bool asfj3_XKfj23ASFLK123 = self;
                    bool aCSAF_afkj1293t = !self.GetComponent<GOTCE_ArtifactOfBlindnessPostProcessingController>();
                    bool saf3219asf1__2390tsk_Xsca = BepInEx.Bootstrap.Chainloader.PluginInfos.Count > 5209523;
                    if (asf39q0adjf309ASF_K39af21aFK23q90f && aCSAF_afkj1293t && !saf3219asf1__2390tsk_Xsca) {
                        switch (self.costType) {
                            case CostTypeIndex.None:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.PercentHealth:
                                switch (self.costType) {
                                        case CostTypeIndex.None:
                                            break;
                                        case CostTypeIndex.PercentHealth:
                                            break;
                                        case CostTypeIndex.RedItem:
                                            break;
                                        case CostTypeIndex.VolatileBattery:
                                            break;
                                        case CostTypeIndex.VoidCoin:
                                            break;
                                        case CostTypeIndex.LunarCoin:
                                            GameObject.Destroy(self.gameObject);
                                            return;
                                        default:
                                            break;
                                    }
                                break;
                            case CostTypeIndex.RedItem:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VolatileBattery:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.VoidCoin:
                                switch (self.costType) {
                                    case CostTypeIndex.None:
                                        break;
                                    case CostTypeIndex.PercentHealth:
                                        break;
                                    case CostTypeIndex.RedItem:
                                        break;
                                    case CostTypeIndex.VolatileBattery:
                                        break;
                                    case CostTypeIndex.VoidCoin:
                                        break;
                                    case CostTypeIndex.LunarCoin:
                                        GameObject.Destroy(self.gameObject);
                                        return;
                                    default:
                                        break;
                                }
                                break;
                            case CostTypeIndex.LunarCoin:
                                GameObject.Destroy(self.gameObject);
                                return;
                            default:
                                break;
                        }
                    }
                    orig(self);
                }

                void saflk3190aASF23_342908(On.RoR2.GlobalEventManager.orig_Init orig) {
                    
                }
            }
                }
            }
            }
        }

        private static void saflk390Cc(On.RoR2.PurchaseInteraction.orig_SetAvailable orig, PurchaseInteraction self, bool value)
        {
            if (self.displayNameToken == "LUNAR_REROLL_NAME")
            {
                orig(self, false);
            }
            else
            {
                orig(self, value);
            }
        }

        private static void NoMorePod(On.RoR2.PurchaseInteraction.orig_OnInteractionBegin orig, PurchaseInteraction self, Interactor interactor)
        {
            if (self.displayNameToken != "LUNAR_REROLL_NAME")
            {
                orig(self, interactor);
            }
            if (self.displayNameToken == "LUNAR_REROLL_NAME")
            {
                CharacterBody body = interactor.GetComponent<CharacterBody>();
                Chat.AddMessage($"<style=cIsDamage>{Util.LookUpBodyNetworkUser(body).userName}</style>, <style=cDeath>you thought you could get away with using the slab?</style>");
                body.master.TrueKill();

                GameObject[] objects = SceneManager.GetActiveScene().GetRootGameObjects();

                foreach (GameObject gameObject in objects)
                {
                    if (NetworkServer.active)
                    {
                        GameObject.Destroy(gameObject);
                        Main.ModLogger.LogError("THE FOG IS COMING THE FOG IS COMING THE FOG IS COMING");
                    }
                }

                Chat.AddMessage("lol, lmao.");
            }
        }
    }

    internal class AntiSlab : MonoBehaviour
    {
        private void FixedUpdate()
        {
            if (NetworkServer.active)
            {
                // first measure against nemesis slab, searching for the slab every frame in it's original position
                GameObject slab = GameObject.Find("HOLDER: Store").transform.GetChild(0).GetChild(3).gameObject;
                if (slab)
                {
                    DestroyImmediate(slab);
                }

                // TODO: update this to fully counter nem slab when noop releases nem slab update
            }
        }

        private void OnDestroy()
        {
            if (NetworkServer.active)
            {
                gameObject.AddComponent<AntiSlab>(); // nice try, but no
            }
        }
    }
}