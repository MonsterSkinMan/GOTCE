using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GOTCE.Items.Lunar
{
    public class DopeDope : ItemBase<DopeDope>
    {
        public override string ConfigName => "Dope Dope";

        public override string ItemName => "Dope Dope";

        public override string ItemLangTokenName => "GOTCE_DopeDope";

        public override string ItemPickupDesc => "<color=#FF7F7F>Your base damage is converted into attack speed.</color>\n";

        public override string ItemFullDescription => "Convert your <style=cIsDamage>base damage</style> to <style=cIsDamage>attack speed</style>. Gain a shield equal to <style=cIsHealing>2%</style> <style=cStack>(+2% per stack)</style> of your maximum health.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Lunar;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage, ItemTag.Utility };

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
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.shieldMultAdd += 2f * stack;
                    args.baseAttackSpeedAdd = sender.damage;
                    args.baseDamageAdd =- sender.damage + 1;
                }
            }
        }
    }
}
