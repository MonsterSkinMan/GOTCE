using BepInEx.Configuration;
using R2API;
using RoR2;
using System.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace GOTCE.Items.White
{
    public class AnimalHead : ItemBase<AnimalHead>
    {
        public override string ConfigName => "Animal Head";

        public override string ItemName => "Animal Head";

        public override string ItemLangTokenName => "GOTCE_AnimalHead";

        public override string ItemPickupDesc => "'Critical Strikes' reduce ability cooldowns.";

        public override string ItemFullDescription => "Gain <style=cIsDamage>5% critical strike chance</style>. <style=cIsUtility>Reduce skill cooldowns</style> on <style=cIsDamage>critical strike</style> by <style=cIsUtility>5%</style> <style=cStack>(+5% per stack)</style>.";

        public override string ItemLore => "I got this thing on my last hunt. Big guy must have taken four shots to go down, I damn near broke my trigger finger! My wall's already covered with my past hunts, so I'm sending this to you. Think of it as a little good luck charm.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.Crit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/animalhead.png");

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
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            orig(self, damageInfo);
            if (damageInfo.crit && !damageInfo.rejected && damageInfo.attacker)
            {
                var body = damageInfo.attacker.GetComponent<CharacterBody>();
                if (body && body.master && body.master.inventory.GetItemCount(Instance.ItemDef) > 0)
                {
                    int itemCount = body.master.inventory.GetItemCount(Instance.ItemDef);
                    if (itemCount > 0)
                    {
                        if (Random.Range(0f, 1f) > 0.7f)
                            Util.PlaySound("Play_item_proc_crit_cooldown", body.gameObject);
                        var sl = body.GetComponent<SkillLocator>();
                        if (sl && sl.hasEffectiveAuthority)
                        {
                            float num = itemCount * damageInfo.procCoefficient;
                            if (sl.primary && sl.primary.stock < sl.primary.maxStock)
                            {
                                // sl.primary.RunRecharge(num * sl.primary.finalRechargeInterval);
                                sl.primary.rechargeStopwatch += num * sl.primary.finalRechargeInterval * 0.05f;
                            }
                            if (sl.secondary && sl.secondary.stock < sl.secondary.maxStock)
                            {
                                // sl.secondary.RunRecharge(num * sl.secondary.finalRechargeInterval);
                                sl.secondary.rechargeStopwatch += num * sl.secondary.finalRechargeInterval * 0.05f;
                            }
                            if (sl.utility && sl.utility.stock < sl.utility.maxStock)
                            {
                                // sl.utility.RunRecharge(num * sl.utility.finalRechargeInterval);
                                sl.utility.rechargeStopwatch += num * sl.utility.finalRechargeInterval * 0.05f;
                            }
                            if (sl.special && sl.special.stock < sl.special.maxStock)
                            {
                                // sl.special.RunRecharge(num * sl.special.finalRechargeInterval);
                                sl.special.rechargeStopwatch += num * sl.special.finalRechargeInterval * 0.05f;
                            }
                        }
                    }
                }
            }
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.critAdd += 5f;
                }
            }
        }
    }
}