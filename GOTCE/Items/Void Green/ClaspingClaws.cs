using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items.VoidGreen
{
    public class ClaspingClaws : ItemBase<ClaspingClaws>
    {
        public override string ConfigName => "Clasping Claws";

        public override string ItemName => "Clasping Claws";

        public override string ItemLangTokenName => "GOTCE_ClaspingClaws";

        public override string ItemPickupDesc => "Pull enemies towards you on hit. <style=cIsVoid>Corrupts all Boxing Gloves</style>.";

        public override string ItemFullDescription => "On hit, <style=cIsUtility>pull enemies</style> towards you. <style=cStack>Pull strength increases per stack</style>. <style=cIsVoid>Corrupts all Boxing Gloves</style>.";

        public override string ItemLore => "TBA";

        public override ItemTier Tier => ItemTier.VoidTier2;

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
            On.RoR2.HealthComponent.TakeDamage += Smash4PacManGrabIsSoAwesomeBro;
        }

        private void Smash4PacManGrabIsSoAwesomeBro(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (damageInfo?.attacker)
            {
                var ThePacIsBack = damageInfo.attacker.GetComponent<RoR2.CharacterBody>();
                if (ThePacIsBack)
                {
                    int stack = GetCount(ThePacIsBack);
                    if (stack > 0)
                    {
                        float mass;
                        if (self.body.characterMotor) mass = (self.body.characterMotor as IPhysMotor).mass;
                        else if (self.body.rigidbody) mass = self.body.rigidbody.mass;
                        else mass = 1f;

                        var DahRoFus = -10f - (5f * (stack - 1));
                        damageInfo.force += Vector3.Normalize(self.body.corePosition - ThePacIsBack.corePosition) * DahRoFus * mass;
                    }
                }
            }
            orig(self, damageInfo);
        }
    }
}