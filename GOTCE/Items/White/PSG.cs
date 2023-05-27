using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;
using R2API;
using RoR2;

namespace GOTCE.Items.White
{
    public class PSG : ItemBase<PSG>
    {
        public override string ConfigName => "Personal Shield Generator";

        public override string ItemName => "Personal Shield Generator";

        public override string ItemLangTokenName => "GOTCE_PSG";

        public override string ItemPickupDesc => "gain 8% of yrou max hp in shield";

        public override string ItemFullDescription => "gain 8% <style=cStack>(+8% per stack)</style> of yrou max hp in shield. recharges outside of danger. thirteen personal shield generators is a videogame won. ensures a good run.";

        public override string ItemLore => "TBA";

        public override ItemTier Tier => ItemTier.Tier1;
        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, GOTCETags.Shield };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Texture/Icons/Item/Personal_Shield_Generator.png");

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
            RecalculateStatsAPI.GetStatCoefficients += new RecalculateStatsAPI.StatHookEventHandler(videogameWon);
        }
        public static void videogameWon(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.inventory)
            {
                var stack = body.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    float gamewon = (body.healthComponent.fullHealth * 0.08f);
                    args.baseShieldAdd += gamewon;
                }
            }
        }
    }
}
