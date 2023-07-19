using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using R2API;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class ZeroPebbles : ItemBase<ZeroPebbles>
    {
        public override string ConfigName => "0 Pebbles";

        public override string ItemName => "0 Pebbles";

        public override string ItemLangTokenName => "GOTCE_ZeroPebbles";

        public override string ItemPickupDesc => "Gain 0 Pebbles.";

        public override string ItemFullDescription => "Gain 0 Pebbles";

        public override string ItemLore => "(there's Nothing There...)";

        public override ItemTier Tier => ItemTier.Tier2;

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/0Pebbles.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }
    }
}
