using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using UnityEngine.Networking;
using GOTCE.Items.White;
using HarmonyLib;

namespace GOTCE.Items.VoidWhite
{
    public class SaferSpace : ItemBase<SaferSpace>
    {
        public override string ConfigName => "SAFER SPACE";

        public override string ItemName => "SAFER SPACE";

        public override string ItemLangTokenName => "GOTCE_SaferSpace";

        public override string ItemPickupDesc => "just give it a chance!";

        public override string ItemFullDescription => "<style=cIsDamage>10%(+10%)</style> chance of enemies that damage you getting <style=cDeath>Strained</style> for <style=cIsDamage>10s(+10s)</style>.";

        public override string ItemLore => "SIGN UP & UNLOCK MERCENARY!";

        public override ItemTier Tier => ItemTier.VoidTier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Healing, ItemTag.Utility };

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override GameObject ItemModel => Utils.Paths.GameObject.PickupBearVoid.Load<GameObject>();

        public override Sprite ItemIcon => Utils.Paths.Texture2D.texBearVoidIcon.Load<Sprite>();

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.HealthComponent.TakeDamage += SAFER;
        }

        private void SAFER(On.RoR2.HealthComponent.orig_TakeDamage orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo)
        {
            orig(self, damageInfo);
            if (!self.body.inventory || !damageInfo.attacker || !damageInfo.attacker.GetComponent<CharacterBody>()) {
                return;
            }

            float chance = 10f * self.body.inventory.GetItemCount(ItemDef);

            if (Util.CheckRoll(chance)) {
                SetStateOnHurt ssoh = damageInfo.attacker.GetComponent<SetStateOnHurt>();
                if (ssoh) {
                    ssoh.SetStun(chance);
                }
            }
        }
    }
}