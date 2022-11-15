using RoR2;
using GOTCE.Components;
using System;
using UnityEngine;
using Unity;
using System.Collections.Generic;
using RoR2.Orbs;
using System.Reflection;

namespace GOTCE.Based {
    public class AOEffect {
        public static void Hooks() {
            On.RoR2.BlastAttack.Fire += BlastHook;
            On.RoR2.Orbs.LightningOrb.Begin += OrbHook;
        }

        private static BlastAttack.Result BlastHook(On.RoR2.BlastAttack.orig_Fire orig, BlastAttack self) {
            if (NetworkServer.active) {
                if (self.attacker) {
                    CharacterBody body = self.attacker.GetComponent<CharacterBody>();
                    if (body && body.master) {
                        GOTCE_StatsComponent stats = body.masterObject.GetComponent<GOTCE_StatsComponent>();
                        if (stats) {
                            self.radius += stats.aoeEffect;
                        }
                    }
                }
            }
            return orig(self);
        }

        private static void OrbHook(On.RoR2.Orbs.LightningOrb.orig_Begin orig, LightningOrb self) {
            if (NetworkServer.active) {
                if (self.attacker) {
                    CharacterBody body = self.attacker.GetComponent<CharacterBody>();
                    if (body && body.master) {
                        GOTCE_StatsComponent stats = body.masterObject.GetComponent<GOTCE_StatsComponent>();
                        if (stats) {
                            self.bouncesRemaining += stats.aoeEffect;
                        }
                    }
                }
            }
            orig(self);
        }
    }
}