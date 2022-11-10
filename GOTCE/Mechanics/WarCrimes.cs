using System;
using RoR2;
using Unity;
using UnityEngine;
using R2API;

namespace GOTCE.Mechanics {
    public enum WarCrime : int {
        Serrated = 1,
        Homemade = 2,
        Chemical = 3,
        Incendiary = 4,
        Corpse = 5,
        POW = 6,
        Nuclear = 7,
        Cluster = 8,
        Medical = 9,
        None = 10
    }

    public class WarCrimes {

        public static Dictionary<WarCrime, string> CrimeToName = new() {
            {WarCrime.Serrated, "Serrated Weaponry"},
            {WarCrime.Cluster, "Cluster Munitions"},
            {WarCrime.Corpse, "Corpse Desecration"},
            {WarCrime.Homemade, "Homemade Weaponry"},
            {WarCrime.Incendiary, "Incendiary Weaponry"},
            {WarCrime.Medical, "Killing Medics"},
            {WarCrime.Nuclear, "Nuclear Weaponry"},
            {WarCrime.Chemical, "Chemical Warfare"},
            {WarCrime.None, "N/A"}
        };
        public static void Hooks() {
            On.RoR2.GlobalEventManager.ServerDamageDealt += (orig, report) => {
                if (NetworkServer.active) {
                    if (report.attackerBody && report.attackerBody.masterObject && report.attackerBody.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                        GOTCE_StatsComponent stats = report.attackerBody.masterObject.GetComponent<GOTCE_StatsComponent>();
                        switch (report.damageInfo.damageType) {
                            case DamageType.IgniteOnHit:
                                stats.mostRecentlyCommitedWarCrime = WarCrime.Incendiary;
                                break;
                            case DamageType.PercentIgniteOnHit:
                                stats.mostRecentlyCommitedWarCrime = WarCrime.Incendiary;
                                break;
                            case DamageType.BleedOnHit:
                                stats.mostRecentlyCommitedWarCrime = WarCrime.Serrated;
                                break;
                            case DamageType.WeakOnHit:
                                stats.mostRecentlyCommitedWarCrime = WarCrime.Chemical;
                                break;
                            case DamageType.SlowOnHit:
                                stats.mostRecentlyCommitedWarCrime = WarCrime.Chemical;
                                break;
                            case DamageType.PoisonOnHit:
                                stats.mostRecentlyCommitedWarCrime = WarCrime.Chemical;
                                break;
                            case DamageType.BlightOnHit:
                                stats.mostRecentlyCommitedWarCrime = WarCrime.Chemical;
                                break;
                            default:
                                break;
                        }

                        if (report.attackerBody.inventory.GetItemCount(Items.Red.GenevaSuggestion.Instance.ItemDef) > 0 ) {
                            switch (stats.mostRecentlyCommitedWarCrime) {
                                case WarCrime.Serrated:
                                    report.damageInfo.damageType |= DamageType.BleedOnHit;
                                    break;
                                case WarCrime.Incendiary:
                                    report.damageInfo.damageType |= DamageType.PercentIgniteOnHit;
                                    break;
                                case WarCrime.Corpse:
                                    report.damageInfo.damageType |= DamageType.PoisonOnHit;
                                    break;
                                case WarCrime.Cluster:
                                    // report.damageInfo.procChainMask.AddProc(ProcType.AACannon); this crashes the game
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                orig(report);
            };

            // killing a mending core
            On.RoR2.GlobalEventManager.OnCharacterDeath += (orig, self, report) => {
                if (NetworkServer.active && report.victimBody && report.victimBody.baseNameToken == GlobalEventManager.CommonAssets.eliteEarthHealerMaster.GetComponent<CharacterMaster>().bodyPrefab.GetComponent<CharacterBody>().baseNameToken && NetworkServer.active) {
                    if (report.attackerBody && report.attackerBody.masterObject && report.attackerBody.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                        report.attackerBody.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime = WarCrime.Medical;
                    }
                }
                orig(self, report);
            };

            // healing bonus
            On.RoR2.HealthComponent.Heal += (orig, self, amount, mask, boolv) => {
                if (self.body && self.body.masterObject && self.body.masterObject.GetComponent<GOTCE_StatsComponent>() && NetworkServer.active && self.body.inventory.GetItemCount(Items.Red.GenevaSuggestion.Instance.ItemDef) > 0 ) {
                    if (self.body.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime == WarCrime.Medical) {
                        amount *= 1.25f;
                    }
                }
                return orig(self, amount, mask, boolv);
            };

            // buff increase
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += (orig, self, def, duration) => {
                 if (self && self.masterObject && self.masterObject.GetComponent<GOTCE_StatsComponent>() && NetworkServer.active && self.inventory.GetItemCount(Items.Red.GenevaSuggestion.Instance.ItemDef) > 0 ) {
                    if (self.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime == WarCrime.Chemical && def.isDebuff) {
                        duration *= 2f;
                    }
                }
                orig(self, def, duration);
            };
 
            // bandit
            On.EntityStates.Bandit2.Weapon.SlashBlade.OnEnter += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    if (self.characterBody && self.characterBody.masterObject && self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                        self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime = WarCrime.Serrated;
                    }
                }
            };

            On.EntityStates.Bandit2.Weapon.Bandit2FireShiv.OnEnter += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    if (self.characterBody && self.characterBody.masterObject && self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                        self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime = WarCrime.Serrated;
                    }
                }
            };

            // rex
            On.EntityStates.Treebot.Weapon.AimFlower.FireProjectile += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    if (self.characterBody && self.characterBody.masterObject && self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                        self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime = WarCrime.Corpse;
                    }
                }
            };

            On.EntityStates.Treebot.Weapon.FirePlantSonicBoom.OnEnter += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    if (self.characterBody && self.characterBody.masterObject && self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                        self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime = WarCrime.Homemade;
                    }
                }
            };

            On.EntityStates.Treebot.Weapon.FireMortar.OnEnter += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    if (self.characterBody && self.characterBody.masterObject && self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                        self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime = WarCrime.Homemade;
                    }
                }
            };

            On.EntityStates.Treebot.Weapon.FireMortar.OnEnter += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    if (self.characterBody && self.characterBody.masterObject && self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                        self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime = WarCrime.Homemade;
                    }
                }
            };

            // toolbot
            On.EntityStates.Toolbot.AimGrenade.OnEnter += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    if (self.characterBody && self.characterBody.masterObject && self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                        self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime = WarCrime.Cluster;
                    }
                }
            };

            On.EntityStates.Toolbot.ChargeSpear.OnEnter += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    if (self.characterBody && self.characterBody.masterObject && self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                        self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime = WarCrime.Homemade;
                    }
                }
            };

            On.EntityStates.Toolbot.FireNailgun.OnEnter += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    if (self.characterBody && self.characterBody.masterObject && self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                        self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime = WarCrime.Homemade;
                    }
                }
            };

            On.EntityStates.Toolbot.ToolbotDash.OnEnter += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    if (self.characterBody && self.characterBody.masterObject && self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                        self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime = WarCrime.Homemade;
                    }
                }
            };

            On.EntityStates.Toolbot.FireBuzzsaw.OnEnter += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    if (self.characterBody && self.characterBody.masterObject && self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                        self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime = WarCrime.Homemade;
                    }
                }
            };

            On.EntityStates.Toolbot.FireGrenadeLauncher.OnEnter += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    if (self.characterBody && self.characterBody.masterObject && self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                        self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime = WarCrime.Homemade;
                    }
                }
            };

            // lodr
            On.EntityStates.Loader.BigPunch.OnEnter += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    if (self.characterBody && self.characterBody.masterObject && self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                        self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime = WarCrime.Homemade;
                    }
                }
            };

            On.EntityStates.Loader.BeginOvercharge.OnEnter += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    if (self.characterBody && self.characterBody.masterObject && self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                        self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime = WarCrime.Homemade;
                    }
                }
            };

            On.EntityStates.Loader.BeginOvercharge.OnEnter += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    if (self.characterBody && self.characterBody.masterObject && self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                        self.characterBody.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime = WarCrime.Homemade;
                    }
                }
            };
        }
    }
}