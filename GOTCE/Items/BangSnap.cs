using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items
{
    public class BangSnap : ItemBase<BangSnap>
    {
        public override string ConfigName => "Bang Snap";

        public override string ItemName => "Bang Snap";

        public override string ItemLangTokenName => "GOTCE_Plus2aoeEffect";

        public override string ItemPickupDesc => "+2 AOE effect to all explosions.";

        public override string ItemFullDescription => "The radius of all area of effect attacks is increased by <style=cIsDamage>2m</style> <style=cStack>(+2m per stack)</style>.";

        public override string ItemLore => "among us";

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
            On.RoR2.BlastAttack.Fire += BlastAttack_Fire;
            On.RoR2.OverlapAttack.Fire += OverlapAttack_Fire;
        }

        private bool OverlapAttack_Fire(On.RoR2.OverlapAttack.orig_Fire orig, OverlapAttack self, List<HurtBox> hitResults)
        {
            throw new NotImplementedException();
        }

        private BlastAttack.Result BlastAttack_Fire(On.RoR2.BlastAttack.orig_Fire orig, BlastAttack self)
        {
            orig(self);
            if (self.attacker)
            {
                CharacterBody component = self.attacker.GetComponent<CharacterBody>();
                if (component && component.inventory)
                {
                    var stack = component.inventory.GetItemCount(Instance.ItemDef);
                    if (stack > 0)
                    {
                        self.radius += 2 * stack;
                    }
                }
            }
            return default(BlastAttack.Result);
        }
    }
}
