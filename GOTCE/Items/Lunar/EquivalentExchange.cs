using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items.Lunar
{
    public class EquivalentExchange : ItemBase<EquivalentExchange>
    {
        public override string ConfigName => "Equivalent Exchange";

        public override string ItemName => "Equivalent Exchange";

        public override string ItemLangTokenName => "GOTCE_EquivalentExchange";

        public override string ItemPickupDesc => "On death, kill the closest enemy... BUT on killing an enemy through any other method, die.";

        public override string ItemFullDescription => "On death, the closest 1 (+1 per stack) monster(s) are killed. If you kill a monster in any other way, you die 1 (+1 per stack) time(s).";

        public override string ItemLore => throw new NotImplementedException();

        public override ItemTier Tier => throw new NotImplementedException();

        public override GameObject ItemModel => throw new NotImplementedException();

        public override Sprite ItemIcon => throw new NotImplementedException();

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            throw new NotImplementedException();
        }
    }
}
