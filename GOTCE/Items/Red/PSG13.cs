using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items.Red
{
    public class PSG13 : ItemBase<PSG13>
    {
        public override string ConfigName => "Personal Shield Generator 13";

        public override string ItemName => "Personal Shield Generator (13)";

        public override string ItemLangTokenName => "GOTCE_PersonalShieldGenerator13";

        public override string ItemPickupDesc => "videogame won.";

        public override string ItemFullDescription => "gain 104% <style=cStack>(+104% per stack)</style> of yrou max hp in shield. recharges outside of danger. thirteen personal shield generators is a videogame won. ensures a good run.";

        public override string ItemLore => "TBA";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, GOTCETags.Shield };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/PSG.png");

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
            RecalculateStatsAPI.GetStatCoefficients += new RecalculateStatsAPI.StatHookEventHandler(VideogameWon);
        }
        public static void VideogameWon(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.inventory)
            {
                var stack = body.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    float gamewon = body.healthComponent.fullHealth * 1.04f;
                    args.baseShieldAdd += gamewon * stack;
                }
            }
        }
    }
}
