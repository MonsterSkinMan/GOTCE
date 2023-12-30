using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using GOTCE.Items.Lunar;
using HarmonyLib;

namespace GOTCE.Items.Void_Lunar
{
    public class BlueWhip : ItemBase<BlueWhip>
    {
        public override string ConfigName => "Blue Whip";

        public override string ItemName => "Blue Whip";

        public override string ItemLangTokenName => "GOTCE_BlueWhip";

        public override string ItemPickupDesc => "Your movement speed is tripled while in combat... <color=#FF7F7F>BUT you can't move while out of combat.</color> <style=cIsVoid>Corrupts all Purple Whips.</style>";

        public override string ItemFullDescription => "Increase your <style=cIsUtility>movement speed</style> by <style=cIsUtility>200%</style> <style=cIsStack>(+100% per stack)</style> while in combat. You cannot move while out of combat. <style=cIsHealing>Decrease armor</style> by <style=cIsHealing>0</style> <style=cIsStack(+100 per stack)</style> while out of combat.";

        public override string ItemLore => "Ok so the reason this is funny is because Blue Whip is a void item, which is the purple tier (I mean technically it's a void lunar which is an indigo color but um shut the fuck up) and Purple Whip is a lunar item, which is the purple tier. See, it's funny because we put the items in the tiers that weren't their color. I mean I guess it's not really like \"funny\" but I think it's kinda silly idk.";

        public override ItemTier Tier => Tiers.LunarVoid.Instance.TierEnum;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, GOTCETags.Bullshit, GOTCETags.WhipAndNaeNae };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/BlueWhip.png");

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
            On.RoR2.Items.ContagiousItemManager.Init += WoolieDimension;
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
                        args.moveSpeedMultAdd -= 1f;
                        args.armorAdd -= 100f * (stack - 1);
                        
                    }
                }
                else
                {
                    if (stack > 0)
                    {
                        args.moveSpeedMultAdd += 1.0f + (1.0f * stack);
                    }
                }
            }
        }

        private void WoolieDimension(On.RoR2.Items.ContagiousItemManager.orig_Init orig)
        {
            ItemDef.Pair transformation = new ItemDef.Pair()
            {
                itemDef1 = PurpleWhip.Instance.ItemDef,
                itemDef2 = this.ItemDef
            };
            ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem] = ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem].AddToArray(transformation);
            orig();
        }
    }
}
