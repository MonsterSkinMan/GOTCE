﻿using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items.White
{
    public class BangSnap : ItemBase<BangSnap>
    {
        public override string ConfigName => "Bang Snap";

        public override string ItemName => "Bang Snap";

        public override string ItemLangTokenName => "GOTCE_BangSnap";

        public override string ItemPickupDesc => "+2 AOE effect to all explosions.";

        public override string ItemFullDescription => "The <style=cIsDamage>radius</style> of all area of effect attacks is increased by <style=cIsDamage>2m</style> <style=cStack>(+2m per stack)</style>.";

        public override string ItemLore => "10 seconds nearby enemy charges 10 power. every 45 power gain +2 aoe effect on your primary. hitting 2 enemies with primary gives m2 +1 charge. Landing 2 m2 hits in a row gives you a temporary barrier that when destroyed deals 500% damage to enemies within the equivalent meters to current power. (edited)";

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/BangSnap.png");

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
            On.RoR2.BlastAttack.Fire += BlastAttack_Fire;
        }

        private BlastAttack.Result BlastAttack_Fire(On.RoR2.BlastAttack.orig_Fire orig, BlastAttack self)
        {
            if (self.attacker)
            {
                var body = self.attacker.GetComponent<CharacterBody>();
                if (body && body.inventory)
                {
                    var stack = body.inventory.GetItemCount(Instance.ItemDef);
                    if (stack > 0)
                    {
                        self.radius += 2 * stack;
                    }
                }
            }
            orig(self);
            return default(BlastAttack.Result);
        }
    }
}