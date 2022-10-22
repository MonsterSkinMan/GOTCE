using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class AnalyticalAegis : ItemBase<AnalyticalAegis>
    {
        public override string ConfigName => "Analytical Aegis";

        public override string ItemName => "Analytical Aegis";

        public override string ItemLangTokenName => "GOTCE_AnalyticalAegis";

        public override string ItemPickupDesc => "Gain a miniscule temporary barrier on 'Critcal FOV Strike'";

        public override string ItemFullDescription => "Gain 2 (+5 per stack) barrier on 'Critical FOV Strike'";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier2;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Healing };

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
            White.ZoomLenses.Instance.OnFovCrit += Aegis;
        }

        public void Aegis(object sender, White.FovCritEventArgs args) {
            if (args.Body) {
                if (args.Body.inventory) {
                    if (NetworkServer.active) {
                        Inventory inv = args.Body.inventory;
                        int count = inv.GetItemCount(ItemDef);
                        int barrier = 5 * (count-1);
                        if (count > 0) {
                            barrier += 2;
                            args.Body.healthComponent.AddBarrier(barrier);
                        }
                    }
                }
            }
        }
    }
}