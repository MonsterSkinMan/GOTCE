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

        public override string ItemFullDescription => "Gain <style=cIsUtility>3 charges</style> of your <style=cIsUtility>utility skill</style> and <style=cIsUtility>50%</style> <style=cStack>(+30% per stack)</style> <style=cIsUtility>movement speed</style>. Reduce your <style=cIsDamage>damage</style> by <style=cIsDamage>90%</style> <style=cStack>(-90% per stack)</style>.";

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/HuntressHarpoon.png");

        public override string ItemLangTokenName => "GOTCE_HuntressHarpoon";

        public override string ItemLore => "\"The Huntress is an agile survivor with a high damage output\"\n- Hopoo Games";

        public override GameObject ItemModel => null;

        public override string ItemName => "Huntress Harpoon";

        public override string ItemPickupDesc => "Gain extra utility charges and movement speed... <color=#FF7F7F>BUT greatly reduce your damage.</color>";

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, GOTCETags.Masochist };

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
            orig(body);
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }
    }
}