using BepInEx.Configuration;
using R2API;
using RoR2;
using System.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace GOTCE.Items.White
{
    public class GameDiscussion : ItemBase<GameDiscussion>
    {
        public override string ConfigName => "#gd1-nullified";

        public override string ItemName => "#gd1-nullified";

        public override string ItemLangTokenName => "GOTCE_GD1";

        public override string ItemPickupDesc => "'Critical Strikes' poison enemies.";

        public override string ItemFullDescription => "On Critical Strike, poison target for 5 (+3 stack) seconds. Gain +5% critical strike chance.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };

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
            On.RoR2.GlobalEventManager.ServerDamageDealt += Poison;
            RecalculateStatsAPI.GetStatCoefficients += Crit;
        }

        public void Poison(On.RoR2.GlobalEventManager.orig_ServerDamageDealt orig, DamageReport report)
        {
            orig(report);
            if (report.attacker && report.attackerBody)
            {
                if (report.attackerBody.inventory)
                {
                    int count = report.attackerBody.inventory.GetItemCount(ItemDef);
                    float duration = 5f + (3f * (count - 1));
                    if (count > 0 && report.damageInfo.crit)
                    {
                        if (report.victim && report.victimBody)
                        {
                            report.victimBody.AddTimedBuff(RoR2Content.Buffs.Poisoned, duration); // ror2 is not a spreadsheet
                        }
                    }
                }
            }
        }

        public void Crit(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.inventory && body.inventory.GetItemCount(ItemDef) > 0)
            {
                args.critAdd += 5f;
            }
        }
    }
}