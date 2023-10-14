using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class SpeedyFinger : ItemBase<SpeedyFinger>
    {
        public override string ConfigName => "Speedy Finger";

        public override string ItemName => "Speedy Finger";

        public override string ItemLangTokenName => "GOTCE_SpeedyFinger";

        public override string ItemPickupDesc => "Gain bonus attack speed for each secondary skill charge.";

        public override string ItemFullDescription => "Increase <style=cIsDamage>attack speed</style> by <style=cIsDamage>20%</style> <style=cStack>(+10% per stack)</style> for each <style=cIsUtility>secondary skill charge</style> you have.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.BackupMagSynergy, GOTCETags.Bullshit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/DelicaterWatch.png");

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
            if (sender)
            {
                var skillLocator = sender.skillLocator;
                if (skillLocator)
                {
                    var stack = GetCount(sender);
                    if (stack > 0)
                    {
                        args.baseAttackSpeedAdd += (0.2f + 0.1f * (stack - 1)) * sender.skillLocator.secondary.maxStock;
                    }
                }
            }
        }
    }
}