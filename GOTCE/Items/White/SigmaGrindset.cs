using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using GOTCE.Components;

namespace GOTCE.Items.White
{
    public class SigmaGrindset : ItemBase<SigmaGrindset>
    {
        public override bool CanRemove => true;
        public override string ConfigName => ItemName;
        public override string ItemFullDescription => "Gain <style=cIsUtility>5% sprint crit chance</style>. On '<style=cIsUtility>Critical Sprint</style>', permanently boost your <style=cIsHealing>regeneration</style> by <style=cIsHealing>0.7/s</style> <style=cStack>(+0.7/s per stack)</style>.";
        public override Sprite ItemIcon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Items/SigmaGrindset.png");
        public override string ItemLangTokenName => "GOTCE_SigmaGrindset";
        public override string ItemLore => "";
        public override GameObject ItemModel => null;
        public override string ItemName => "Sigma Grindset";
        public override string ItemPickupDesc => "On sprint crit, increase regeneration permanently.";
        public override Enum[] ItemTags => new Enum[] { ItemTag.Healing, GOTCETags.Crit };
        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            CriticalTypes.OnSprintCrit += Critted;
            RecalculateStatsAPI.GetStatCoefficients += Sigma;
            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory)
                    {
                        args.Stats.SprintCritChanceAdd += GetCount(args.Stats.body) > 0 ? 5 : 0;
                    }
                }
            };
        }

        public void Critted(object sender, SprintCritEventArgs args)
        {
            if (args.Body && NetworkServer.active)
            {
                if (args.Body.masterObject && args.Body.masterObject.GetComponent<GOTCE_StatsComponent>())
                {
                    args.Body.masterObject.GetComponent<GOTCE_StatsComponent>().total_sprint_crits++;
                }
            }
        }

        public void Sigma(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (NetworkServer.active && body.masterObject && body.masterObject.GetComponent<GOTCE_StatsComponent>())
            {
                GOTCE_StatsComponent stats = body.masterObject.GetComponent<GOTCE_StatsComponent>();
                if (body.inventory && body.inventory.GetItemCount(ItemDef) > 0)
                {
                    float increase = (0.7f + (0.7f * GetCount(body))) * stats.total_sprint_crits;
                    args.baseRegenAdd += increase;
                }
            }
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }
    }
}