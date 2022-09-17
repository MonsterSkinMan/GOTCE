using BepInEx.Configuration;
using GOTCE.Items;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace GOTCE.Items
{
    public class MoldySteak : ItemBase<MoldySteak>
    {
        public static GameObject BruhSteak;

        public override string ConfigName => "Moldy Steak";

        public override string ItemName => "Moldy Steak";

        public override string ItemLangTokenName => "GOTCE_ShittyFlatHealth";

        public override string ItemPickupDesc => "Lose 25 max health.";

        public override string ItemFullDescription => "Decreases <style=cIsHealing>maximum health</style> by <style=cIsHealing>25</style> <style=cStack>(+25 per stack)</style>.";

        public override string ItemLore => "Hey, uhh… I know I'm kinda new to this whole 'dry aging' thing and you've been doing it for years, but are you sure it's supposed to be green like that? I don't think the health inspectors are gonna like this, and I don't particularly feel like getting shut down. Help?";

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Healing, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/MoldySteak.png");

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
            RecalculateStatsAPI.GetStatCoefficients += new RecalculateStatsAPI.StatHookEventHandler(MoldySteak.HealthIncrease);
        }

        public static void HealthIncrease(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.inventory)
            {
                var stack = body.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.baseHealthAdd += (float)Instance.GetCount(body) * -25f;
                }
            }
        }
    }
}
