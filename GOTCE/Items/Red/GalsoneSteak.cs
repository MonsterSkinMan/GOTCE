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

        public override string ItemLore => "TBA";

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
