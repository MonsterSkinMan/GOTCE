using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using R2API;
using UnityEngine;
using BepInEx.Configuration;
using System.Linq;

namespace GOTCE.Equipment
{
    public class BloodGamble : EquipmentBase<BloodGamble>
    {
        public override string EquipmentName => "Blood Gamble";

        public override string EquipmentLangTokenName => "GOTCE_SkeletonNuclearThrone";

        public override string EquipmentPickupDesc => "Fire a singular missile with a chance to die.";

        public override string EquipmentFullDescription => "Passively halves your health and movement speed. Fire <style=cIsDamage>a</style> missile that deals <style=cIsDamage>300% damage</style>, has a 25% chance to kill you, and applies the malachite debuff to yourself for 4 seconds.";

        public override string EquipmentLore => "This is fucking trash.\n-Literally everybody";

        public override GameObject EquipmentModel => null;

        public override Sprite EquipmentIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Equipment/BloodGamble.png");

        public override float Cooldown => 0f;

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
            RecalculateStatsAPI.GetStatCoefficients += new RecalculateStatsAPI.StatHookEventHandler(BloodGamble.Skissue);
        }

        public static void Skissue(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body != null && body.inventory != null && body.inventory.currentEquipmentIndex != null && body.inventory.currentEquipmentIndex == Instance.EquipmentDef.equipmentIndex)
            {
                args.healthMultAdd -= 0.5f;
                args.moveSpeedMultAdd -= 0.5f;
            }
        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            if (slot.characterBody)
            {
                if (Util.CheckRoll(25f, slot.characterBody.master))
                {
                    slot.characterBody.healthComponent.Suicide(slot.characterBody.gameObject, slot.characterBody.gameObject, DamageType.BypassArmor | DamageType.BypassBlock | DamageType.BypassOneShotProtection);
                }
                slot.characterBody.AddTimedBuff(RoR2Content.Buffs.HealingDisabled, 4f);
                slot.characterBody.GetComponent<CharacterBody>().equipmentSlot.remainingMissiles += 1;
            }
            return true;
        }
    }
}
