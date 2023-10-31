using RoR2;
using System;
using UnityEngine;
using Unity;
using GOTCE;

namespace GOTCE.Utils {
    public static class Extensions {
        /// <summary>Gets the associated GOTCE_StatsComponent of a CharacterBody</summary>
        /// <param name="stats">The GOTCE_StatsComponent associated with the CharacterBody, null when returning false</param>
        /// <returns>Whether or not there is an associated GOTCE_StatsComponent</returns>
        public static bool GetStatsComponent(this CharacterBody body, out GOTCE_StatsComponent stats) {
            if (body && body.masterObject) {
                if (body.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                    stats = body.masterObject.GetComponent<GOTCE_StatsComponent>();
                    return true;
                }
            }
            stats = null;
            return false;
        }

        /// <summary>Gets the associated GOTCE_StatsComponent of a CharacterMaster</summary>
        /// <param name="stats">The GOTCE_StatsComponent associated with the CharacterMaster, null when returning false</param>
        /// <returns>Whether or not there is an associated GOTCE_StatsComponent</returns>
        public static bool GetStatsComponent(this CharacterMaster master, out GOTCE_StatsComponent stats) {
            if (master) {
                if (master.gameObject.GetComponent<GOTCE_StatsComponent>()) {
                    stats = master.gameObject.GetComponent<GOTCE_StatsComponent>();
                    return true;
                }
            }
            stats = null;
            return false;
        }
        /// <summary>Checks if the HealthComponent is above a specific health threshold</summary>
        /// <param name="fraction">The threshold in percent</param>
        /// <returns>If the HealthComponent's combinedHealth is above the threshold</returns>
        public static bool IsAboveFraction(this HealthComponent healthComponent, float fraction) {
            float newFraction = fraction * 0.01f;
            float health = healthComponent.fullHealth * newFraction;
            return healthComponent.combinedHealth > health;
        }

        /// <summary>Checks if an ItemDef has a GOTCE ItemTag</summary>
        /// <param name="tag">The GOTCETag to check for</param>
        /// <returns>Whether or not the tag was present</returns>
        public static bool HasTag(this ItemDef def, GOTCETags tag) {
            return def.ContainsTag((ItemTag)tag);
        }

        /// <summary>Gets a random element from the collection</summary>
        /// <returns>the chosen element</returns>
        public static T GetRandom<T>(this IEnumerable<T> self) {
            return self.ElementAt(Random.Range(0, self.Count()));
        }

        public static string Add(this string self, string text) {
            LanguageAPI.Add(self, text);
            return self;
        }

        /// <summary>Gets a random element from the collection</summary>
        /// <param name="rng">the Xoroshiro128Plus instance</param>
        /// <returns>the chosen element</returns>
        public static T GetRandom<T>(this IEnumerable<T> self, Xoroshiro128Plus rng) {
            return self.ElementAt(rng.RangeInt(0, self.Count()));
        }

        /// <summary>Gets a random element from the collection that matches the predicate</summary>
        /// <param name="predicate">the predicate to match</param>
        /// <returns>the chosen element</returns>
        public static T GetRandom<T>(this IEnumerable<T> self, System.Func<T, bool> predicate) {
            try {
                return self.Where(predicate).ElementAt(Random.Range(0, self.Count()));
            }
            catch {
                return default(T);
            }
        }

        /// <summary>Gets a random element from the collection that matches the predicate</summary>
        /// <param name="rng">the Xoroshiro128Plus instance</param>
        /// <param name="predicate">the predicate to match</param>
        /// <returns>the chosen element</returns>
        public static T GetRandom<T>(this IEnumerable<T> self, Xoroshiro128Plus rng, System.Func<T, bool> predicate) {
            try {
                return self.Where(predicate).ElementAt(rng.RangeInt(0, self.Count()));
            }
            catch {
                return default(T);
            }
        }
    }
}