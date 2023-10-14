using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.White
{
    public class SummertimeSoda : ItemBase<SummertimeSoda>
    {
        public override string ConfigName => "Summertime Soda";

        public override string ItemName => "Summertime Soda";

        public override string ItemLangTokenName => "GOTCE_SummertimeSoda";

        public override string ItemPickupDesc => "Gain extra shield, damage reduction, and max HP if the current season is summer.";

        public override string ItemFullDescription => "If the <style=cIsUtility>current month</style> is either <style=cIsUtility>June</style>, <style=cIsUtility>July</style>, or <style=cIsUtility>August</style>, gain <style=cIsUtility>4%</style> <style=cStack>(+4% per stack)</style> shield, <style=cIsHealing>4</style> <style=cStack>(+4 per stack)</style> armor, and <style=cIsHealing>10</style> <style=cStack>(+10 per stack)</style> health.";

        public override string ItemLore => "We work nonstop. We work ourselves to the bone for our jobs. What's worst? We can't complain. They'll just replace us. Summer would be the worst with the heat. We used to die of exhaustion before our bosses got involved.\n\nNow we have theses sodas. They make things slightly more bearable. This is the only thing that keeps us alive during the summer sometimes.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.Healing, GOTCETags.Shield, GOTCETags.TimeDependant, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/SummertimeSoda.png");

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
            RecalculateStatsAPI.GetStatCoefficients += Soda;
        }

        public void Soda(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            int c = GetCount(body);
            if (c > 0)
            {
                int min = 5;
                int max = 9;

                if (DateTime.Now.Month < max && DateTime.Now.Month > min)
                {
                    args.baseShieldAdd += body.healthComponent.fullHealth * (0.04f * c);
                    args.baseHealthAdd += 10 * c;
                    args.armorAdd += 4 * c;
                }
            }
        }
    }
}