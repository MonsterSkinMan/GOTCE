using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items.Lunar
{
    public class PurpleWhip : ItemBase<PurpleWhip>
    {
        public override string ConfigName => "Purple Whip";

        public override string ItemName => "Purple Whip";

        public override string ItemLangTokenName => "GOTCE_PurpleWhip";

        public override string ItemPickupDesc => "Your movement speed is increased while out of combat... <color=#FF7F7F>BUT you can't move while in combat.</color>";

        public override string ItemFullDescription => "Increase your <style=cIsUtility>movement speed</style> by <style=cIsUtility>50%</style> <style=cIsStack>(+50% per stack)</style> while out of combat. You cannot move while in combat. <style=cIsHealing>Decrease armor</style> by <style=cIsHealing>0</style> <style=cIsStack(+100 per stack)</style> while in combat.";

        public override string ItemLore => "TBA";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, GOTCETags.Bullshit, GOTCETags.WhipAndNaeNae };

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
            RecalculateStatsAPI.GetStatCoefficients += new RecalculateStatsAPI.StatHookEventHandler(Speeder);
        }

        public void Speeder(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.inventory)
            {
                var stack = body.inventory.GetItemCount(Instance.ItemDef);
                if (body.outOfCombat)
                {
                    if (stack > 0)
                    {
                        args.moveSpeedMultAdd += 0.5f * stack;
                    }
                }
                else
                {
                    if (stack > 0)
                    {
                        args.moveSpeedMultAdd -= 1f;
                        args.armorAdd -= 100f * (stack - 1);
                    }
                }    
            }
        }
    }
}
