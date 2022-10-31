using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class Racecar : ItemBase<Racecar>
    {
        public override string ConfigName => "HIFUs Racecar";

        public override string ItemName => "HIFU's Racecar";

        public override string ItemLangTokenName => "GOTCE_Racecar";

        public override string ItemPickupDesc => "‘Critical Sprints’ instantly recharge all charges of your Secondary skill. Reduce Secondary skill cooldown.";

        public override string ItemFullDescription => "On sprint crit, recharge all of your m2 charges. Gain 10% (+15% per stack) cooldown reduction for your m2. Gain 5% sprint crit chance.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.AIBlacklist, GOTCETags.Crit, GOTCETags.BackupMagSynergy };

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
            White.GummyVitamins.Instance.OnSprintCrit += Vroom;
            RecalculateStatsAPI.GetStatCoefficients += CDR;
        }

        public void Vroom(object sender, White.SprintCritEventArgs args) {
            if (NetworkServer.active && args.Body) {
                if (GetCount(args.Body) > 0) {
                    args.Body.skillLocator.secondary.stock = args.Body.skillLocator.secondary.maxStock;
                }
            }
        }

        public void CDR(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args) {
            if (NetworkServer.active && body.inventory) {
                if (GetCount(body) > 0) {
                    args.secondaryCooldownMultAdd -= Utils.MathHelpers.InverseHyperbolicScaling(0.1f, 0.15f, 1f, GetCount(body));
                }
            }
        }
    }
}