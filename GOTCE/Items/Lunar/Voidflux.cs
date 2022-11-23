using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2.Skills;

namespace GOTCE.Items.Lunar
{
    public class Voidflux : ItemBase<Voidflux>
    {
        public override string ConfigName => "Voidflux Pauldron";

        public override string ItemName => "Voidflux Pauldron";

        public override string ItemLangTokenName => "GOTCE_Voidflux";

        public override string ItemPickupDesc => "Randomize EVERYTHING periodically. <style=cIsVoid>Corrupts all other Pauldrons.</style>";

        public override string ItemFullDescription => "Every 10 (-25% per stack) seconds, randomize EVERYTHING. <style=cIsVoid>Corrupts all other Pauldrons.</style>";

        public override string ItemLore => "";

        public override ItemTier Tier => Tiers.LunarVoid.Instance.TierEnum;
        public override ItemTierDef OverrideTierDef => Tiers.LunarVoid.Instance.tier;

        public override Enum[] ItemTags => new Enum[] { GOTCETags.Bullshit, GOTCETags.Unstable, GOTCETags.Pauldron };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.CharacterBody.OnInventoryChanged += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    self.AddItemBehavior<VoidfluxBehavior>(GetCount(self));
                }
            };

            On.RoR2.Items.ContagiousItemManager.Init += (orig) => {
                ItemHelpers.RegisterCorruptions(ItemDef, new() { 
                    Items.Lunar.DarkFluxPauldron.Instance.ItemDef,
                    Items.Lunar.WindFluxPauldron.Instance.ItemDef,
                    DLC1Content.Items.HalfAttackSpeedHalfCooldowns,
                    DLC1Content.Items.HalfSpeedDoubleHealth,
                    RoR2Content.Items.WarCryOnMultiKill
                });
                orig();
            };
        }

        private class VoidfluxBehavior : CharacterBody.ItemBehavior {
            float stopwatch = 0f;
            float delay = 10f;
            public float damage;
            public float shield;
            public float attackSpeed;
            public float moveSpeed;
            public TeamIndex teamIndex = TeamIndex.Player;
            public float jumpHeight;
            public float armor;
            private Xoroshiro128Plus rng => Run.instance.treasureRng;
            private List<SkillDef> defs;
            private TeamIndex[] teamIndexes = {
                TeamIndex.Neutral,
                TeamIndex.Player,
                TeamIndex.Monster,
                TeamIndex.Lunar,
                TeamIndex.Void
            };

            private void Start() {
                defs = new();
                foreach (SkillDef def in SkillCatalog.allSkillDefs) {
                    defs.Add(def);
                }

                RecalculateStatsAPI.GetStatCoefficients += Stats;
                delay = 10 * Mathf.Pow(0.75f, stack - 1);
            }

            private void FixedUpdate() {
                if (NetworkServer.active) {
                    stopwatch += Time.fixedDeltaTime;
                    if (stopwatch >= delay) {
                        stopwatch = 0f;
                        Randomize();
                    }
                }
            }

            private void Randomize() {
                if (NetworkServer.active) {
                    damage = rng.RangeFloat(1, 25 * body.level);
                    shield = rng.RangeFloat(1, 250 * body.level);
                    attackSpeed = rng.RangeFloat(1, 3 * body.level);
                    moveSpeed = rng.RangeFloat(1, 7 * body.level);
                    teamIndex = teamIndexes[rng.RangeInt(0, teamIndexes.Length)];
                    jumpHeight = rng.RangeFloat(1, 16 * body.level);
                    armor = rng.RangeFloat(1, 50 * body.level);

                    SkillLocator sl = body.skillLocator;
                    sl.primary.SetSkillOverride(gameObject, defs[rng.RangeInt(0, defs.Count)], GenericSkill.SkillOverridePriority.Replacement);
                    sl.secondary.SetSkillOverride(gameObject, defs[rng.RangeInt(0, defs.Count)], GenericSkill.SkillOverridePriority.Replacement);
                    sl.utility.SetSkillOverride(gameObject, defs[rng.RangeInt(0, defs.Count)], GenericSkill.SkillOverridePriority.Replacement);
                    sl.special.SetSkillOverride(gameObject, defs[rng.RangeInt(0, defs.Count)], GenericSkill.SkillOverridePriority.Replacement);
                }
            }

            private void Stats(CharacterBody cb, RecalculateStatsAPI.StatHookEventArgs args) {
                if (NetworkServer.active && cb == body) {
                    args.baseDamageAdd += damage;
                    args.baseShieldAdd += shield;
                    args.baseJumpPowerAdd += jumpHeight;
                    args.armorAdd += armor;
                    args.baseMoveSpeedAdd += moveSpeed;
                    args.baseAttackSpeedAdd += attackSpeed;

                    cb.teamComponent.teamIndex = teamIndex;
                }
            }

            private void OnDestroy() {
                RecalculateStatsAPI.GetStatCoefficients -= Stats;
            }
        }
    }
}