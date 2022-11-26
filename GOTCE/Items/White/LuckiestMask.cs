using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BepInEx.Configuration;
using GOTCE.Items.NoTier;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items.White
{
    public class LuckiestMask : ItemBase<LuckiestMask>
    {
        public override bool CanRemove => true;
        public override string ConfigName => ItemName;
        public override string ItemFullDescription => "Increase the <style=cIsDamage>proc coefficient</style> of all of your attacks by <style=cIsDamage>1</style> <style=cStack>(+1 per stack)</style>. Breaks at low health.";
        public override Sprite ItemIcon => null;
        public override string ItemLangTokenName => "GOTCE_LuckiestMask";
        public override string ItemLore => "";
        public override GameObject ItemModel => null;
        public override string ItemName => "Luckiest Mask";
        public override string ItemPickupDesc => "Increase proc coefficient. Breaks at low health.";
        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.Consumable };
        public override ItemTier Tier => ItemTier.Tier1;

        public void Break(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo info)
        {
            orig(self, info);
            if (self.body && self.body.inventory)
            {
                if (self.isHealthLow)
                {
                    self.body.inventory.RemoveItem(ItemDef, self.body.inventory.GetItemCount(ItemDef));
                    self.body.inventory.GiveItem(DreamPDF.Instance.ItemDef);
                    CharacterMasterNotificationQueue.SendTransformNotification(self.body.master, Instance.ItemDef.itemIndex, DreamPDF.Instance.ItemDef.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);
                }
            }
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.DamageInfo.ModifyDamageInfo += Proc;
            On.RoR2.HealthComponent.TakeDamage += Break;
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public void Proc(On.RoR2.DamageInfo.orig_ModifyDamageInfo orig, DamageInfo self, HurtBox.DamageModifier mod)
        {
            if (self.attacker && self.attacker.GetComponent<CharacterBody>() && NetworkServer.active)
            {
                CharacterBody body = self.attacker.GetComponent<CharacterBody>();
                if (body.inventory && body.inventory.GetItemCount(ItemDef) > 0)
                {
                    float count = 1f * body.inventory.GetItemCount(ItemDef);
                    self.procCoefficient += count;
                }
            }
            orig(self, mod);
        }
    }
}