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
    public class GummyVitamins : ItemBase<GummyVitamins>
    {
        public override bool CanRemove => true;
        public override string ConfigName => ItemName;
        public override string ItemFullDescription => "Gain a <style=cIsUtility>10%</style> <style=cStack>(+10% per stack)</style> chance to '<style=cIsUtility>critically sprint</style>', <style=cIsUtility>doubling your sprinting speed</style>.";
        public override Sprite ItemIcon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Items/GummyVitamins.png");
        public override string ItemLangTokenName => "GOTCE_GummyVitamins";
        public override string ItemLore => "Feeling tired? Need some pep in your step? Then try our new FTL gummy vitamins! They're full of lots of radical chemicals and shit that'll make you move extra fast!\nWARNING: TAKING MORE THAN ONE FTL GUMMY VITAMIN A DAY WILL RESULT IN CERTAIN DEATH! ADDITIONALLY, DO NOT INGEST IF YOU ARE ALLERGIC TO ANY OF THE FOLLOWING: NICOTINE, CHEETAH PISS, HEROINE, METHAMPHETAMINE, METHYLPHENIDATE, GASOLINE, COCAINE, KETAMINE, LEAD, SULFUR, NITROGEN, CHLORINE, RADON, OR PEANUTS.";
        public override GameObject ItemModel => null;
        public override string ItemName => "Gummy Vitamins";
        public override string ItemPickupDesc => "Gain a chance to 'critically sprint', doubling your sprinting speed.";
        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.Crit };
        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory)
                    {
                        args.Stats.SprintCritChanceAdd += GetCount(args.Stats.body) * 10;
                    }
                }
            };
        }


        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }
    }
}