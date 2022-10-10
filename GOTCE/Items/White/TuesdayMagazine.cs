/*
using RoR2;
using R2API;
using UnityEngine;
using BepInEx.Configuration;
using System;

namespace GOTCE.Items.White
{
    public class TuesdayMagazine : ItemBase<TuesdayMagazine>
    {
        public override string ConfigName => "Tuesday Magazine";

        public override string ItemName => "Tuesday Magazine";

        public override string ItemLangTokenName => "GOTCE_TuesdayMagazine";

        public override string ItemPickupDesc => "Increases your secondary charges if it's Tuesday.";

        public override string ItemFullDescription => "Increases your <style=cIsUtility>secondary charges</style> by <style=cIsUtility>3</style> <style=cStack>(+3 per stack)</style> if it's Tuesday.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

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
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (self && self.inventory)
            {
                var stack = self.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0 && self.skillLocator)
                {
                    bool tuesday = DateTime.Now.DayOfWeek == DayOfWeek.Tuesday;
                    var sl = self.skillLocator;
                    if (sl.secondary && tuesday)
                    {
                        sl.secondary.SetBonusStockFromBody(sl.secondary.bonusStockFromBody + 3 * stack);
                    }
                }
            }
        }
    }
}
*/

// seems to not WORK, borrow groove's code later and fix