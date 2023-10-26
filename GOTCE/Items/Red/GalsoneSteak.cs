using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;
using R2API;
using RoR2;

namespace GOTCE.Items.Red
{
    public class GalsoneSteak : ItemBase<GalsoneSteak>
    {
        public override string ConfigName => "Galsone Steak";

        public override string ItemName => "Galsone Steak";

        public override string ItemLangTokenName => "GOTCE_SteakGalsone";

        public override string ItemPickupDesc => "+25 AOE effect.";

        public override string ItemFullDescription => "The <style=cIsDamage>radius</style> of all area of effect attacks is increased by <style=cIsDamage>25m</style> <style=cStack>(+25m per stack)</style>.";

        public override string ItemLore => "\"I mean it's common sense, right? When we're in the kitchen preparing meat and I tell you to get the oil, I'm talking about the oil used for COOKING in the KITCHEN.\"\n\"You did not specify.\"\n\"Oh my fucking god... Where did you even get a full canister of this stuff? Do you know how expensive Galsone is, not to mention the ingredients in the steak?\"\n\"I do not.\"\n\"You've wasted the steak now too, throw it in the bin. NOT as hard-\"\nAnd then there wasn't a bin, or a steak, or a chef, or a kitchen.";

        public override ItemTier Tier => ItemTier.Tier3;

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/GalsoneSteak.png");

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
            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory)
                    {
                        args.Stats.AOEAdd += GetCount(args.Stats.body) * 25;
                    }
                }
            };
        }
    }
}
