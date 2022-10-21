using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GOTCE.Items.Lunar
{
    public class HuntressHarpoon : ItemBase<HuntressHarpoon>
    {
        public override string ConfigName => "Huntress Harpoon";

        public override string ItemFullDescription => "Gain 3 charges of your utility skill and 50% (+30% per stack) movement speed. Reduce your damage by 90% (-90%)";

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/HuntressHarpoon.png");

        public override string ItemLangTokenName => "GOTCE_HuntressHarpoon";

        public override string ItemLore => "'The Huntress is an agile survivor with a high damage output'\n- Hopoo Games";

        public override GameObject ItemModel => null;

        public override string ItemName => "Huntress Harpoon";

        public override string ItemPickupDesc => "Gain extra utility charges and movement speed, but greatly reduce your damage.";

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

        public override ItemTier Tier => ItemTier.Lunar;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.CharacterBody.RecalculateStats += Huntress;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.damageMultAdd -= Utils.MathHelpers.InverseHyperbolicScaling(0.9f, 0.9f, 1f, stack);
                    args.moveSpeedMultAdd += 0.5f + 0.3f * (stack - 1);
                }
            }
        }

        public void Huntress(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody body)
        {
            orig(body);
            if (body.inventory)
            {
                int count = body.inventory.GetItemCount(ItemDef);
                if (count > 0)
                {
                    /*
                    float reduction = 0.1f * Mathf.Pow((1 - 0.1f), (count - 1));
                    float increase = 1.5f + 0.35f * (count - 1);
                    // args.damageMultAdd -= reduction;
                    body.damage *= reduction;
                    body.moveSpeed *= increase;
                    */
                    body.skillLocator.utility.SetBonusStockFromBody(body.skillLocator.utility.bonusStockFromBody + 3);
                }
            }
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }
    }
}