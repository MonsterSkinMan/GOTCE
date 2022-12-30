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

        public override string ItemFullDescription => "All sources of barrier are increased by a flat <style=cIsHealing>30</style> <style=cStack>(+15 per stack)</style> for every Backup Magazine you have.";

        public override string ItemLore => "Man, I'm in a real pickle here! It's a good thing I packed my trusty Aegis! Now I know that I'll survi- <i>fucking dies</i>";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Healing, ItemTag.Utility, ItemTag.AIBlacklist, GOTCETags.BarrierRelated, GOTCETags.BackupMagSynergy };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/AemergencyAegis.png");

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

        public void Aegis(On.RoR2.HealthComponent.orig_AddBarrier orig, HealthComponent self, float barrier)
        {
            if (self.body && self.body.inventory && self.body.inventory.GetItemCount(ItemDef) > 0)
            {
                float increase = 30f + (15f * (self.body.inventory.GetItemCount(ItemDef) - 1));
                increase = increase * self.body.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine);
                barrier += increase;
            }
            orig(self, barrier);
        }
    }
}