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

        public override string ItemLore => "Walter saw the world for what it was. A ruthless hellhole where only the strong could survive. He saw the exploitive nature of the corporations that controlled the world, the wars that were fought over earth, the death of everyone he knew, and even how medication was a corrupt system where money could only talk. Some would say this is what allowed the strongest to survive...\n\n...And Walter did not think he was one of them. He sealed himself within his basement. Anything he could do inside, away from the hellish world, he would do. He would not let the world get to him, and he would survive.\n\nYears went by for Walter. The time that he had merely slipped away in his fear. He was eventually considered missing or, for some, even dead. But he could not be remembered by anyone. They were all gone. Some people who pass by a house on 22 Liowitz Street on Earth say that they can feel a solemn feeling from it, as if someone was always alone, and that nothing would change...\n\nWhen Walter's story was made public by a house explorer, corporations knew they were in trouble. They made a statement that Walter was delusional, that he forgot to take his medicine, which is why he thought the world was against him. In the end, the corporations won.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Healing, GOTCETags.Shield };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/SeethingOpioids.png");

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