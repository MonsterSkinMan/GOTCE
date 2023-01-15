using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items.White
{
    public class OSPGenerator : ItemBase<OSPGenerator>
    {
        public override string ConfigName => "Personal OSP Generator";

        public override string ItemName => "Personal OSP Generator";

        public override string ItemLangTokenName => "GOTCE_OSPGenerator";

        public override string ItemPickupDesc => "Increases One Shot Protection threshold and invincibility frames";

        public override string ItemFullDescription => "Increases OSP threshold by 7% (+7% per stack) and increases OSP invincibility time by 0.1 (+0.1 per stack) seconds.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Healing };

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
            On.RoR2.CharacterBody.RecalculateStats += OSP;
            On.RoR2.HealthComponent.TriggerOneShotProtection += Timer;
        }

        public void OSP(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody body)
        {
            orig(body);
            int c = GetCount(body);
            if (c > 0) {
                body.oneShotProtectionFraction += (0.07f * c);
            }
        }

        public void Timer(On.RoR2.HealthComponent.orig_TriggerOneShotProtection orig, HealthComponent self) {
            orig(self);
            int c = GetCount(self.body);
            if (c > 0) {
                self.ospTimer += (0.1f * c);
            }
        }
    }
}