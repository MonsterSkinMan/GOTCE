using RoR2;
using GOTCE.Components;
using System;
using UnityEngine;
using Unity;
using System.Collections.Generic;
using RoR2.Orbs;
using System.Reflection;

namespace GOTCE.Based
{
    public class AOEffect
    {
        public static void Hooks()
        {
            On.RoR2.BlastAttack.Fire += BlastHook;
            On.RoR2.Orbs.LightningOrb.Begin += OrbHook;
            On.RoR2.OverlapAttack.Fire += OverlapHook;
        }

        private static BlastAttack.Result BlastHook(On.RoR2.BlastAttack.orig_Fire orig, BlastAttack self)
        {
            if (NetworkServer.active)
            {
                if (self.attacker)
                {
                    CharacterBody body = self.attacker.GetComponent<CharacterBody>();
                    if (body && body.master)
                    {
                        GOTCE_StatsComponent stats = body.masterObject.GetComponent<GOTCE_StatsComponent>();
                        if (stats)
                        {
                            self.radius += stats.aoeEffect;
                        }
                    }
                }
            }
            return orig(self);
        }

        private static void OrbHook(On.RoR2.Orbs.LightningOrb.orig_Begin orig, LightningOrb self)
        {
            if (NetworkServer.active)
            {
                if (self.attacker)
                {
                    CharacterBody body = self.attacker.GetComponent<CharacterBody>();
                    if (body && body.master)
                    {
                        GOTCE_StatsComponent stats = body.masterObject.GetComponent<GOTCE_StatsComponent>();
                        if (stats)
                        {
                            self.bouncesRemaining += stats.aoeEffect;
                        }
                    }
                }
            }
            orig(self);
        }

        private static bool OverlapHook(On.RoR2.OverlapAttack.orig_Fire orig, OverlapAttack self, List<HurtBox> hitResults)
        {
            bool shouldReset = false;
            Dictionary<HitBox, Vector3> originalScales = new();
            if (NetworkServer.active)
            {
                if (self.attacker)
                {
                    CharacterBody body = self.attacker.GetComponent<CharacterBody>();
                    if (body && body.master)
                    {
                        GOTCE_StatsComponent stats = body.masterObject.GetComponent<GOTCE_StatsComponent>();
                        if (stats.aoeEffect > 0)
                        {
                            shouldReset = true;
                            foreach (HitBox hitbox in self.hitBoxGroup.hitBoxes)
                            {
                                originalScales.Add(hitbox, hitbox.gameObject.transform.localScale);
                                hitbox.gameObject.transform.localScale *= stats.aoeEffect;
                            }
                        }
                    }
                }
            }
            bool guh = orig(self, hitResults);
            if (shouldReset)
            {
                foreach (HitBox hitbox in self.hitBoxGroup.hitBoxes)
                {
                    Vector3 scale = new();
                    if (NetworkServer.active)
                    {
                        bool found = originalScales.TryGetValue(hitbox, out scale);
                        if (found)
                        {
                            hitbox.gameObject.transform.localScale = scale;
                        }
                        else
                        {
                            Main.ModLogger.LogFatal("Could not reset the scale after an overlap attack!");
                        }
                    }
                }
            }
            return guh;
        }
    }
}