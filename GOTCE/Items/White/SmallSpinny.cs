using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.White
{
    public class SmallSpinny : ItemBase<SmallSpinny>
    {
        public override string ConfigName => "Small Spinny";

        public override string ItemName => "Small Spinny";

        public override string ItemLangTokenName => "GOTCE_SmallSpinny";

        public override string ItemPickupDesc => "On Camera Rotation Crit, increase attack speed for a short duration.";

        public override string ItemFullDescription => "On '<style=cIsUtility>Camera Rotation Crit</style>', increase <style=cIsDamage>attack speed</style> by <style=cIsDamage>35%</style> <style=cStack>(+25% per stack)</style> for <style=cIsDamage>2 seconds</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.Bullshit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/SkullReaction.png");

        public static BuffDef smallSpinnyBuff;

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
            smallSpinnyBuff = ScriptableObject.CreateInstance<BuffDef>();
            smallSpinnyBuff.isDebuff = false;
            smallSpinnyBuff.isCooldown = false;
            smallSpinnyBuff.canStack = false;
            smallSpinnyBuff.isHidden = false;
            smallSpinnyBuff.buffColor = new Color32(121, 178, 196, 255);
            smallSpinnyBuff.iconSprite = Utils.Paths.BuffDef.bdEnergized.Load<BuffDef>().iconSprite;

            ContentAddition.AddBuffDef(smallSpinnyBuff);

            CriticalTypes.OnRotationCrit += TheTheoryOfDistanceFuckingOldUnprocessedUgghhFemboyskissinguhfdugijdiugdhg;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.HasBuff(smallSpinnyBuff))
            {
                var stack = GetCount(sender);
                if (stack > 0)
                    args.baseAttackSpeedAdd += 0.35f + 0.2f * (stack - 1);
            }
        }

        public void TheTheoryOfDistanceFuckingOldUnprocessedUgghhFemboyskissinguhfdugijdiugdhg(object sender, RotationCritEventArgs args)
        {
            if (args.Body && NetworkServer.active)
            {
                args.Body.AddTimedBuffAuthority(smallSpinnyBuff.buffIndex, 2f);
            }
        }
    }
}