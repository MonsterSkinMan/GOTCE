using UnityEngine;
using RoR2.Orbs;

namespace GOTCE.Items.Green
{
    public class LtGMk1 : ItemBase<LtGMk1>
    {
        public override string ItemName => "LtG Mk. 1";

        public override string ConfigName => "LtG Mk 1";

        public override string ItemLangTokenName => "GOTCE_LtGMk1";

        public override string ItemPickupDesc => "You should love yourself NOW ! !";

        public override string ItemFullDescription => "Gain a <style=cIsDamage>10%</style> chance on hit to <style=cIsDamage>summon lightning</style> for <style=cIsDamage>1500%</style> <style=cStack>(+750% per stack)</style> base damage.";

        public override string ItemLore => "Keep yourself safe.";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/LtGMk1.png");

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

                if (hurtBox)
                {
                    OrbManager.instance.AddOrb(new LightningStrikeOrb
                    {
                        attacker = attackerBody.gameObject,
                        damageColorIndex = DamageColorIndex.Item,
                        damageValue = attackerBody.damage * (15f + 7.5f * (stack - 1)),
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