using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items.White
{
    public class LeviathanLarva : ItemBase<LeviathanLarva>
    {
        public override string ConfigName => "Leviathan Larva";

        public override string ItemName => "Leviathan Larva";

        public override string ItemLangTokenName => "GOTCE_LeviathanLarva";

        public override string ItemPickupDesc => "Increased critical strike damage for every Shield item you have.";

        public override string ItemFullDescription => "Critical strikes deal an additional 5% (+2.5% per stack) damage for every Shield category item in your inventory.";

        public override string ItemLore => "TBA";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.Damage, GOTCETags.Crit, GOTCETags.Shield };

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
                float stackMult = 0.05f + (0.025f * (GetCount(body) - 1));
                float crits = (stackMult * total);

                args.critDamageMultAdd += crits;
            }
        }
    }
}
