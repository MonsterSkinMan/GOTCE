using BepInEx.Configuration;
using GOTCE.Components;
using GOTCE.Items.Lunar;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace GOTCE.Items.White
{
    public class BloodyShank : ItemBase<BloodyShank>
    {
        public override string ConfigName => "Bloody Shank";

        public override string ItemName => "Bloody Shank";

        public override string ItemLangTokenName => "GOTCE_BloodyShank";

        public override string ItemPickupDesc => "Gain crit chance and bleed chance.";

        public override string ItemFullDescription => "Gain <style=cIsDamage>5%</style> <style=cStack>(+5% per stack)</style> chance to <style=cIsDamage>crit</style>, and a <style=cIsDamage>5%</style> <style=cStack>(+5% per stack)</style> chance to <style=cIsDamage>bleed</style> enemies on hit for <style=cIsDamage>240% base damage</style>.";

        public override string ItemLore => "Fun fact, <style=cIsDamagE>Hopoo Games</style>, in their <style=cIsUtility>infinite wisdom</style> thought that this would be an <style=cIsHealing>interesting<style> and <style=cIsHealing>good</style> item for <style=cIsVoid>Survivors of The Void</style>, as it was the <style=cIsUtility>first iteration</style> of <style=cIsDamage>Shuriken</style> as we know it.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.Crit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/BloodyShank.png");

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
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            var stack = GetCount(self);
            if (stack > 0)
            {
                self.bleedChance += 5f * stack;
            }
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            var stack = GetCount(sender);
            if (stack > 0)
            {
                args.critAdd += 5f * stack;
            }
        }
    }
}