using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items.White
{
    public class Balls : ItemBase<Balls>
    {
        public override string ConfigName => "Balls";

        public override string ItemName => "Balls";

        public override string ItemLangTokenName => "GOTCE_Balls";

        public override string ItemPickupDesc => "They will stay.";

        public override string ItemFullDescription => "Increase <style=cIsUtility>'Critical FOV Strike'</style> chance by <style=cIsDamage>2%</style>. Gain <style=cIsDamage>8%</style> <style=cStack>(+4% per stack)</style> of your maximum <style=cIsHealing>health</style> as regenerating <style=cIsUtility>shields</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.Healing, GOTCETags.FovRelated };

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
            RecalculateStatsAPI.GetStatCoefficients += Synergy;
        }

        public void Synergy(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args) {
            int total = 0;
            if (!body.inventory) {
                return;
            }
            foreach (ItemIndex index in body.inventory.itemAcquisitionOrder) {
                if (ContainsTag(ItemCatalog.GetItemDef(index), GOTCETags.FovRelated)) {
                    total += body.inventory.GetItemCount(index);
                }
            }

            if (GetCount(body) > 0) {
                float stackMult = 0.08f + (0.04f * (GetCount(body) - 1));
                float shields = (body.healthComponent.fullHealth * stackMult) * total;

                args.baseShieldAdd += shields;
            }
        }
    }
}