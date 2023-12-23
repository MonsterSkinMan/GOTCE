using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items.Green
{
    public class LuckyDay : ItemBase<LuckyDay>
    {
        public override string ConfigName => "Lucky Day";

        public override string ItemName => "Lucky Day";

        public override string ItemLangTokenName => "GOTCE_LuckyDay";

        public override string ItemPickupDesc => "Increase the chance of all 'Critical Types' upon dying. Gain a chance to cheat death.";

        public override string ItemFullDescription => "On death, increase all crit type chances by 15% (+15% per stack). Gain a 20% revive chance.";

        public override string ItemLore => "TBA";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.Damage, GOTCETags.OnDeathEffect, GOTCETags.Crit };

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
            On.RoR2.CharacterMaster.OnBodyDeath += CharacterMaster_OnBodyDeath;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;

            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && args.Stats.master && args.Stats.master.inventory)
                {
                    if (GetCount(args.Stats.master) > 0)
                    {
                        args.Stats.reviveChanceAdd += 20f * GetCount(args.Stats.body);

                        args.Stats.StageCritChanceAdd += GetCount(args.Stats.body) * 15 * args.Stats.deathCount;
                        args.Stats.DeathCritChanceAdd += GetCount(args.Stats.body) * 15 * args.Stats.deathCount;
                        args.Stats.SprintCritChanceAdd += GetCount(args.Stats.body) * 15 * args.Stats.deathCount;
                        args.Stats.FovCritChanceAdd += GetCount(args.Stats.body) * 15 * args.Stats.deathCount;
                        args.Stats.RotationCritChanceAdd += GetCount(args.Stats.body) * 15 * args.Stats.deathCount;
                    }
                }
            };
        }

        private void CharacterMaster_OnBodyDeath(On.RoR2.CharacterMaster.orig_OnBodyDeath orig, CharacterMaster self, CharacterBody body)
        {
            orig(self, body);
            if (NetworkServer.active)
            {
                if (self.GetComponent<GOTCE_StatsComponent>())
                {
                    var stats = self.GetComponent<GOTCE_StatsComponent>();
                    var stack = body.inventory.GetItemCount(Instance.ItemDef);
                    if (stack > 0 && Util.CheckRoll(stats.reviveChance))
                    {
                        self.preventGameOver = true;
                        stats.Invoke(nameof(stats.RespawnExtraLife), 1f);
                        stats.deathCount++;
                    }
                }
            }
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                var stats = sender.gameObject.GetComponent<GOTCE_StatsComponent>();
                if (stack > 0 && stats)
                {
                    args.critAdd += (0.15f + 0.15f * (stack - 1)) * stats.deathCount;
                }
            }
        }
    }
}
