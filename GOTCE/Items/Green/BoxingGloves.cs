using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items.Green
{
    public class BoxingGloves : ItemBase<BoxingGloves>
    {
        public override string ConfigName => "Boxing Gloves";

        public override string ItemName => "Boxing Gloves";

        public override string ItemLangTokenName => "GOTCE_BoxingGloves";

        public override string ItemPickupDesc => "Knock enemies back on hit.";

        public override string ItemFullDescription => "100% chance on hit to knock enemies back, increasing in force per stack.";

        public override string ItemLore => "TBA";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility };

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
            On.RoR2.HealthComponent.TakeDamage += HolyShitIsThatAnArmsReferenceHolyShitILoveArmsItsSuchAGoodGame;
        }

        private void HolyShitIsThatAnArmsReferenceHolyShitILoveArmsItsSuchAGoodGame(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (damageInfo?.attacker)
            {
                var SpringManFromArms = damageInfo.attacker.GetComponent<RoR2.CharacterBody>();
                if (SpringManFromArms)
                {
                    int stack = GetCount(SpringManFromArms);
                    if (stack > 0)
                    {
                        float mass;
                        if (self.body.characterMotor) mass = (self.body.characterMotor as IPhysMotor).mass;
                        else if (self.body.rigidbody) mass = self.body.rigidbody.mass;
                        else mass = 1f;

                        var FusRoDah = 20f + (10f * (stack - 1));
                        damageInfo.force += Vector3.Normalize(self.body.corePosition - SpringManFromArms.corePosition) * FusRoDah * mass;
                    }
                }
            }
            orig(self, damageInfo);
        }
    }
}
