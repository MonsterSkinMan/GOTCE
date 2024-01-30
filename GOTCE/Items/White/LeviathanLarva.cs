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

        public override string ItemLore => "Entering the building, all you see is debris. You can see destroyed monitors, glass littering the floor, along with some odd looking water-like liquid. Walking around, searching for potential treasures in the old lab, you find an old-fashioned tape recorder. Curiosity gets the best of you.\n\n<i>Click<i>\n\n\"This is Dr.Faden, lead researcher in the UES-C experimental team. Today is... September 23rd, 2049, 3:15 P.M. We recently received a biological shipment with big warnings on it, the only comments describing it as a very dangerous larva, and to never submerge it in water. It came in some sort of thick liquid. We think it might be sleeping? Its vitals show something similar to rest, we'll wait until there's some form of movement to begin testing.\"\n\n<i>Click<i>\n\n\"This is Dr.Faden, today is September 25th, 2049, 9:43 A.M. Continued observations of the larva troubles us. It has awoken and is moving now, but the liquid it's contained in seems to constrict its movements, making accurate observations of what this thing can do, or even is, difficult, as it's constantly worn out. I'm kinda concerned for the thing and have sent an email to one of the higher ups asking about its containment protocols. From what we can see, it definitely has scales and fins, and its body looks... draconic? It's slender and fleshy, somewhat like a snake. Further documentation will be needed before we can begin experiments. Thankfully, one of the lab boys has an idea, so this process should hopefully speed up soon.\"\n\n<i>Click<i>\n\nIn the background of the audio, you can hear an alarm blaring.\n\n\"This is Dr.Faden, we- we're under evacuation notice. Someone mixed the larva's tank with water, and it just... if anyone finds this log, stay away from that thing. It's not a normal larv- <i>(unintelligible screeching)<i>\"\n\n<i>Click<i>";

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
