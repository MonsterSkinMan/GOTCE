using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using UnityEngine.Networking;

namespace GOTCE.Items.NoTier
{
    public class MagneticPropulsor : ItemBase<MagneticPropulsor>
    {
        public override string ConfigName => "Magnetic Propulsor";

        public override string ItemName => "Magnetic Propulsor";

        public override string ItemLangTokenName => "GOTCE_MagneticPropulsor";

        public override string ItemPickupDesc => "THIS is a tasty burger.";

        public override string ItemFullDescription => "Motherfucker you're not supposed to read this.";
        public override bool Hidden => true;
        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.NoTier;

        public override Enum[] ItemTags => new Enum[] { ItemTag.AIBlacklist };

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
            RecalculateStatsAPI.GetStatCoefficients += Guh;
        }

        public void Guh(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (NetworkServer.active && GetCount(body) > 0)
            {
                args.jumpPowerMultAdd += (3f * body.crit);
                args.critAdd -= body.crit;
            }
        }
    }
}