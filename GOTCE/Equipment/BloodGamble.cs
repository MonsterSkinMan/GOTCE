using RoR2;
using R2API;
using UnityEngine;
using BepInEx.Configuration;
using System;

namespace GOTCE.Equipment
{
    public class BloodGamble : EquipmentBase<BloodGamble>
    {
        public override string EquipmentName => "Blood Gamble";

        public override string EquipmentLangTokenName => "GOTCE_SkeletonNuclearThrone";

        public override string EquipmentPickupDesc => "Fire a singular missile with a chance to die.";

        public override string EquipmentFullDescription => "Passively halves your <style=cIsHealing>maximum health</style> and <style=cIsUtility>movement speed</style>. Fire <style=cIsDamage>1</style> missile that deals <style=cIsDamage>1x300% damage</style>. Inflict <style=cIsHealing>25% of your maximum hp</style>, apply <style=cIsUtility>4 random debuffs</style> and the <style=cIsHealing>malachite debuff</style> to yourself for <style=cIsUtility>4</style> seconds.";

        public override string EquipmentLore => "This is fucking trash.\n-Literally everybody";

        public override GameObject EquipmentModel => null;

        public override Sprite EquipmentIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Equipment/BloodGamble.png");

        public override float Cooldown => 0f;
        private System.Random rand = new System.Random();

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateEquipment();
            Hooks();
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += new RecalculateStatsAPI.StatHookEventHandler(Skissue);
        }

        public static void Skissue(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body != null && body.inventory != null && body.inventory.currentEquipmentIndex != null && body.inventory.currentEquipmentIndex == Instance.EquipmentDef.equipmentIndex)
            {
                args.healthMultAdd -= 0.5f;
                args.moveSpeedReductionMultAdd += 1f;
                // 1f is 50% move speed, or 50% actual reduction using the game's stupid formula
            }
        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            if (slot.characterBody)
            {
                /* if (Util.CheckRoll(25f, slot.characterBody.master))
                {
                    slot.characterBody.healthComponent.Suicide(slot.characterBody.gameObject, slot.characterBody.gameObject, DamageType.BypassArmor | DamageType.BypassBlock | DamageType.BypassOneShotProtection);
                } */
                DamageInfo info = new()
                {
                    attacker = null,
                    damage = slot.characterBody.healthComponent.fullCombinedHealth * 0.25f
                };
                int[] damageTypes = (int[])Enum.GetValues(typeof(DamageType));
                info.damageType = (DamageType)damageTypes[rand.Next(0, damageTypes.Length - 1)] | (DamageType)damageTypes[rand.Next(0, damageTypes.Length - 1)] | (DamageType)damageTypes[rand.Next(0, damageTypes.Length - 1)] | DamageType.NonLethal;
                info.procCoefficient = 0f;
                slot.characterBody.healthComponent.TakeDamage(info);
                slot.characterBody.AddTimedBuff(RoR2Content.Buffs.HealingDisabled, 4f);
                slot.characterBody.GetComponent<CharacterBody>().equipmentSlot.remainingMissiles += 1;
            }
            return true;
        }
    }
}