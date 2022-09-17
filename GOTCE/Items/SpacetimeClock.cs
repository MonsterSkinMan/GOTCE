using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items
{
    public class SpacetimeClock : ItemBase<SpacetimeClock>
    {
        public override string ConfigName => "Faulty Spacetime Clock";

        public override string ItemName => "Faulty Spacetime Clock";

        public override string ItemLangTokenName => "GOTCE_StageCritClock";

        public override string ItemPickupDesc => "Gain 10% chance to critically stage transition, skipping the next stage and opening up powerful synergies.";

        public override string ItemFullDescription => "Gain 10% <style=cStack>(+10% per stack)</style> stage transition crit chance, which makes you progress by two stages instead of one.";

        public override string ItemLore => "Order: [ERR 04: database corruption]\nTracking Number: 95******\nEstimated Delivery: 15/02/2051\nShipping Method: Delicate\nShipping Address: **** Espl.des Particules, Switzerland, Earth\nShipping Details:\n\n\"...think I’m on the verge of a breakthrough. This broken clock I found seems to have incredibly anomalous properties, dramatically affecting the flow of local time and space. It’s easily the weirdest thing I’ve ever found. I genuinely believe it could revolutionize everything, to the point where I...\"";

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility, ItemTag.AIBlacklist, ItemTag.OnStageBeginEffect };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

        public override bool Hidden => true;

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }
    }
}
