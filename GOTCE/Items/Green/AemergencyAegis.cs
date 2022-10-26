using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class AegmergencyAegis : ItemBase<AegmergencyAegis>
    {
        public override string ConfigName => "Aegmergency Aegis";

        public override string ItemName => "Aegmergency Aegis";

        public override string ItemLangTokenName => "GOTCE_AegmergencyAegis";

        public override string ItemPickupDesc => "For every Backup Magazine in your inventory, all sources of barrier are increased.";

        public override string ItemFullDescription => "All sources of barrier are increased by a flat +30 (+15 per stack) for every Backup Magazine you have.";

        public override string ItemLore => "i love aegis so much";

        public override ItemTier Tier => ItemTier.Tier2;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Healing, ItemTag.Utility, ItemTag.AIBlacklist };

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
            On.RoR2.HealthComponent.AddBarrier += Aegis;
        }

        public void Aegis(On.RoR2.HealthComponent.orig_AddBarrier orig, HealthComponent self, float barrier) {
            if (self.body && self.body.inventory && self.body.inventory.GetItemCount(ItemDef) > 0) {
                float increase = 30f + (15f*(self.body.inventory.GetItemCount(ItemDef) - 1));
                increase = increase * self.body.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine);
                barrier += increase;
            }
            orig(self, barrier);
        }
    }
}