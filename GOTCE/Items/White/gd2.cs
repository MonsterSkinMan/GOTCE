using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using GOTCE.Components;

namespace GOTCE.Items.White
{
    public class gd2 : ItemBase<gd2>
    {
        public override bool CanRemove => true;
        public override string ConfigName => ItemName;
        public override string ItemFullDescription => "After a 'critical sprint', all of your attacks inflict blight for the next 4 (+2 per stack) seconds. Gain 10% sprint crit chance.";
        public override Sprite ItemIcon => null;
        public override string ItemLangTokenName => "GOTCE_gd2";
        public override string ItemLore => "";
        public override GameObject ItemModel => null;
        public override string ItemName => "#ror2-discussion";
        public override string ItemPickupDesc => "On 'critcal sprint', your attacks inflict blight for a short duration.";
        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.Crit };
        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            RoR2BlightBuff.Awake();
            GummyVitamins.Instance.OnSprintCrit += Blight;
            On.RoR2.DamageInfo.ModifyDamageInfo += Inflict;
        }

        public void Blight(object sender, SprintCritEventArgs args) {
            if (args.Body && NetworkServer.active) {
                args.Body.AddTimedBuff(RoR2BlightBuff.buff, 4f + (2f * (GetCount(args.Body) - 1)));
            }
        }

        public void Inflict(On.RoR2.DamageInfo.orig_ModifyDamageInfo orig, DamageInfo self, HurtBox.DamageModifier mod) {
            if (self.attacker && self.attacker.GetComponent<CharacterBody>() && NetworkServer.active) {
                if (self.attacker.GetComponent<CharacterBody>().HasBuff(RoR2BlightBuff.buff)) {
                    self.damageType |= DamageType.BlightOnHit;
                }
            }
            orig(self, mod);
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

    }

    public class RoR2BlightBuff {
        public static BuffDef buff;

        public static void Awake() {
            buff = ScriptableObject.CreateInstance<BuffDef>();
            buff.canStack = false;
            buff.isDebuff = false;
            buff.name = "spcrit";
            buff.isHidden = true;

            R2API.ContentAddition.AddBuffDef(buff);
        }
    }

}