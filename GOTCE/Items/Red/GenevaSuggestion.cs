using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using BepInEx.Configuration;
using System.Linq;

namespace GOTCE.Items.Red
{
    public class GenevaSuggestion : ItemBase<GenevaSuggestion>
    {
        public override string ConfigName => "Geneva Suggestion";

        public override string ItemName => "Geneva Suggestion";

        public override string ItemLangTokenName => "GOTCE_GenevaSuggestion";

        public override string ItemPickupDesc => "War Crimes make you stronger.";

        public override string ItemFullDescription => "Gain <style=cIsDamage>varying boosts</style> BASED on your <style=cIsUtility>most recently committed War Crime</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.Utility, ItemTag.Healing };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/GenevaSuggestion.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += (body, args) =>
            {
                if (NetworkServer.active && body.inventory && HasItem(body) && body.masterObject && body.masterObject.GetComponent<GOTCE_StatsComponent>())
                {
                    if (body.masterObject.GetComponent<GOTCE_StatsComponent>().mostRecentlyCommitedWarCrime == WarCrime.Homemade)
                    {
                        args.baseAttackSpeedAdd += 0.15f;
                    }
                }
            };
        }
    }
}