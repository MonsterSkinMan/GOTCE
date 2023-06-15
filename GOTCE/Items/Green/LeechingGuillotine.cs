using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class LeechingGuillotine : ItemBase<LeechingGuillotine>
    {
        public override string ConfigName => "Leeching Guillotine";

        public override string ItemName => "Leeching Guillotine";

        public override string ItemLangTokenName => "GOTCE_LeechingGuillotine";

        public override string ItemPickupDesc => "Dealing damage heals you and you instantly kill low health Elite monsters.";

        public override string ItemFullDescription => "Dealing damage <style=cIsHealing>heals</style> you for <style=cIsHealing>0.5</style> <style=cStack>(+0.5 per stack)</style> health and you instantly kill Elite monsters below <style=cIsHealth>6.5%</style> <style=cStack>(+6.5% per stack)</style> <style=cIsHealth>health</style>.";

        public override string ItemLore => "Now you have outdone yourself, for you are truly commendable.\nThis, this combination is one of utmost greatness.\nSuch brilliance, to combine the best items we have at our disposal.\nThe perfect strategy - to combine tending wounds and killing priority targets quickly.\n\n<style=cMono>End of transmission.</style>";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.Healing, GOTCETags.Bullshit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/ExpressionOfTheImmolated.png");

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
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            var victimBody = self.body;
            var attacker = damageInfo.attacker;
            if (attacker)
            {
                var attackerBody = attacker.GetComponent<CharacterBody>();
                if (attackerBody)
                {
                    var stack = GetCount(attackerBody);
                    if (stack > 0)
                    {
                        if (victimBody && victimBody.isElite)
                        {
                            var threshold = Util.ConvertAmplificationPercentageIntoReductionPercentage(6.5f * stack) / 100f;
                            if (threshold > 0 && self.combinedHealthFraction <= threshold)
                            {
                                if (self.health > 0f)
                                {
                                    self.Networkhealth = 0f;
                                }
                                if (self.shield > 0f)
                                {
                                    self.Networkshield = 0f;
                                }
                                if (self.barrier > 0f)
                                {
                                    self.Networkbarrier = 0f;
                                }
                                EffectManager.SimpleEffect(Utils.Paths.GameObject.OmniImpactExecute.Load<GameObject>(), self.transform.position, Quaternion.identity, true);
                            }
                        }
                    }
                }
            }

            orig(self, damageInfo);
        }

        private void GlobalEventManager_onServerDamageDealt(DamageReport report)
        {
            var attackerBody = report.attackerBody;
            if (!attackerBody)
            {
                return;
            }

            var victimBody = report.victimBody;
            if (!victimBody)
            {
                return;
            }

            var victimHc = victimBody.healthComponent;
            if (!victimHc)
            {
                return;
            }

            var stack = GetCount(attackerBody);
            if (stack > 0)
            {
                attackerBody.healthComponent?.Heal(0.5f * stack * report.damageInfo.procCoefficient, default);
            }
        }
    }
}