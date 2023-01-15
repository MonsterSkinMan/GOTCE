using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items.White
{
    public class SeethingOpioids : ItemBase<SeethingOpioids>
    {
        public override string ConfigName => "Seething Opioids";

        public override string ItemName => "Seething Opioids";

        public override string ItemLangTokenName => "GOTCE_SeethingOpioids";

        public override string ItemPickupDesc => "Gain a flat amount of recharging shield, which is increased by other shield items.";

        public override string ItemFullDescription => "Gain 30 (+30 per stack) shield. Increased by 10 (+5 per stack) for each shield item you have. ";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Healing, GOTCETags.Shield };

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

        public void Synergy(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            int total = 0;
            if (!body.inventory)
            {
                return;
            }
            foreach (ItemIndex index in body.inventory.itemAcquisitionOrder)
            {
                if (ContainsTag(ItemCatalog.GetItemDef(index), GOTCETags.Shield))
                {
                    total += body.inventory.GetItemCount(index);
                }
            }

            if (GetCount(body) > 0)
            {
                float shields = (30 * GetCount(body)) + (10 + ((GetCount(body) - 1)) * 5);

                args.baseShieldAdd += shields;
            }
        }
    }
}