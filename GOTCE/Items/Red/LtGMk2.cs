using UnityEngine;
using RoR2.Orbs;

namespace GOTCE.Items.Green
{
    public class LtGMk2 : ItemBase<LtGMk2>
    {
        public override string ItemName => "LtG Mk. 2";

        public override string ConfigName => "LtG Mk 2";

        public override string ItemLangTokenName => "GOTCE_LtGMk2";

        public override string ItemPickupDesc => "Guys love yourself";

        public override string ItemFullDescription => "Gain a <style=cIsDamage>10%</style> chance on hit to <style=cIsDamage>summon lightning</style> for <style=cIsDamage>700%</style> <style=cStack>(-100% per stack)</style> TOTAL damage.";

        public override string ItemLore => "You serve a lot of purpose.";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/LtGMk2.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
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

            var stack = GetCount(attackerBody);
            if (stack > 0 && Util.CheckRoll(10f * report.damageInfo.procCoefficient, victimBody.master))
            {
                var hurtBox = attackerBody.mainHurtBox;

                var totalDamage = Util.OnHitProcDamage(report.damageInfo.damage, attackerBody.damage, 7f - 1f * (stack - 1));

                if (hurtBox)
                {
                    OrbManager.instance.AddOrb(new LightningStrikeOrb
                    {
                        attacker = attackerBody.gameObject,
                        damageColorIndex = DamageColorIndex.Item,
                        damageValue = totalDamage,
                        isCrit = Util.CheckRoll(attackerBody.crit, attackerBody.master),
                        procChainMask = default,
                        procCoefficient = 1f,
                        target = hurtBox,
                    });
                }
            }
        }
    }
}