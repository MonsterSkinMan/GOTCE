using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class FrenchHorn : ItemBase<FrenchHorn>
    {
        public override string ConfigName => "French Horn";

        public override string ItemName => "French Horn";

        public override string ItemLangTokenName => "GOTCE_FrenchHorn";

        public override string ItemPickupDesc => "Activating your Equipment gives you a burst of movement speed.";

        public override string ItemFullDescription => "Activating your Equipment gives you <style=cIsUtility>+70% movement speed</style> for <style=cIsUtility>8s</style> <style=cStack>(+4s per stack)</style>.";

        public override string ItemLore => "I don't fully understand why the man spent an hour on his trip to my home if his plan was to insult my instrument. I've had the horn for 40 years now, and it is in perfect condition. The man had told me, in his letter, that he was just a big fan and wanted to take some pictures. He seemed like a polite chap, from his greeting and manners. He even insisted on paying me for the pictures, though I would have gladly helped him for free. And then he casually mentioned that the horn was French. At first I thought this was some language barrier, and asked him what he meant, but he seemed to very intentionally be calling the horn French. I told him to take it back, he just pretended to act confused, and I kicked him out. Well, at least I'm not the one who wasted an hour getting here...";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.EquipmentRelated, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            EquipmentSlot.onServerEquipmentActivated += Kiss;
        }

        private void Kiss(EquipmentSlot slot, EquipmentIndex index)
        {
            if (slot.characterBody && slot.inventory)
            {
                int stack = slot.inventory.GetItemCount(ItemDef);
                if (stack > 0)
                {
                    slot.characterBody.AddTimedBuff(GOTCE.Buffs.Frenching.instance.BuffDef.buffIndex, (float)(8 + 4 * (stack - 1)));
                }
            }
        }
    }
}
