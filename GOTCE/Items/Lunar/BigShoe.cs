using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GOTCE.Items.Lunar
{
    public class BigShoe : ItemBase<BigShoe>
    {
        public override string ConfigName => "Big Shoe";

        public override string ItemName => "Big Shoe";

        public override string ItemLangTokenName => "GOTCE_BigShoe";

        public override string ItemPickupDesc => "Reduce the proc coefficient of all your attacks to 3.0... <color=#FF7F7F>EVEN if they are normally higher.</color>\n";

        public override string ItemFullDescription => "Reduce the <style=cIsDamage>proc coefficient</style> of all your attacks to <style=cIsDamage>3.0</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Lunar;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage, ItemTag.Utility };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/bigshoe.png");
        
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
            On.RoR2.DamageInfo.ModifyDamageInfo += DamageInfo_ModifyDamageInfo;
        }

        private void DamageInfo_ModifyDamageInfo(On.RoR2.DamageInfo.orig_ModifyDamageInfo orig, DamageInfo self, HurtBox.DamageModifier damageModifier)
        {
            if (self.attacker)
            {
                var body = self.attacker.GetComponent<CharacterBody>();
                if (body.inventory)
                {
                    var stack = body.inventory.GetItemCount(Instance.ItemDef);
                    if (stack > 0)
                    {
                        if (!self.procChainMask.HasProc(ProcType.Behemoth))
                        {
                            self.procCoefficient = 3f;
                            self.procChainMask = default(ProcChainMask);
                        }
                    }
                }
            }
            orig(self, damageModifier);
        }

        // this works too well lmaooo
        // sticky bombs infinitely proc themselves, prepatch singuband style
    }
}