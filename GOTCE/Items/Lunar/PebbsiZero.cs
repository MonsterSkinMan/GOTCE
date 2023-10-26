using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items.Lunar
{
    public class PebbsiZero : ItemBase<PebbsiZero>
    {
        public override string ConfigName => "Pebbsi Zero";

        public override string ItemName => "Pebbsi Zero";

        public override string ItemLangTokenName => "GOTCE_RotSoda";

        public override string ItemPickupDesc => "The Triple Affirmitive.";

        public override string ItemFullDescription => "<style=cIsUtility>Pull enemies towards you on hit</style> and <style=cIsDamage>kill enemies on contact</style>. Gain <style=cIsUtility>10% per second</style> health regeneration. <style=cDeath>You are blind.</style>";

        public override string ItemLore => 
        """
        Hello, creature. You've come at the perfect time.

        The solution has been found.

        The solution is portable.

        The solution has been implemented.

        And it comes in three delicious flavors.

        Five Pebbsi: Triple Affirmative
        
        ...

        Would you like to try a sample?
        """;

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.Damage, GOTCETags.Bullshit };

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
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.CharacterBody.RecalculateStats += Blind;
            On.RoR2.HealthComponent.TakeDamage += Smash4PacManGrabIsSoAwesomeBro;
        }

        private void Smash4PacManGrabIsSoAwesomeBro(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (damageInfo?.attacker)
            {
                CharacterBody ThePacIsBack = damageInfo.attacker.GetComponent<RoR2.CharacterBody>();
                if (ThePacIsBack)
                {
                    int stack = GetCount(ThePacIsBack);
                    if (stack > 0)
                    {
                        self.body.AddTimedBuff(RoR2Content.Buffs.LunarSecondaryRoot, 2f);
                        float mass;
                        if (self.body.characterMotor) mass = (self.body.characterMotor as IPhysMotor).mass;
                        else if (self.body.rigidbody) mass = self.body.rigidbody.mass;
                        else mass = 1f;

                        float DahRoFus = -10f - (5f * (stack - 1));
                        if (self.body.isChampion) DahRoFus *= 0.2f; 
                        DahRoFus *= 15;
                        damageInfo.force += Vector3.Normalize(self.body.corePosition - ThePacIsBack.corePosition) * DahRoFus * mass;
                    }
                }
            }
            orig(self, damageInfo);
        }

        private void Blind(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self) {
            orig(self);

            if (self.inventory && self.inventory.GetItemCount(ItemDef) > 0) {
                self.visionDistance = 1;
            }
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.baseRegenAdd += sender.maxHealth * 0.05f;
                }

                sender.AddItemBehavior<RotCoreDamage>(stack);

                if (stack > 1) {
                    sender.healthComponent.Suicide();
                }
            }
        }

        public class RotCoreDamage : CharacterBody.ItemBehavior {
            public void FixedUpdate() {
                BlastAttack attack = new();
                attack.radius = 1.5f;
                attack.baseDamage = 99999999;
                attack.damageType = DamageType.VoidDeath;
                attack.position = body.corePosition;
                attack.attacker = body.gameObject;
                attack.teamIndex = TeamIndex.Player;
                attack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                
                if (NetworkServer.active) {
                    attack.Fire();
                }
            }
        }
    }
}