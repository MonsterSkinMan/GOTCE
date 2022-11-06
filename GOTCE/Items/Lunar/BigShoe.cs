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

        public override string ItemPickupDesc => "\"Reduce\" the proc coefficient of all your attacks to 3.0... <color=#FF7F7F>EVEN if they are normally higher.</color> Yes, this affects the proc coefficient of EVERYTHING.\n";

        public override string ItemFullDescription => "Reduce the <style=cIsDamage>proc coefficient</style> of all your attacks to <style=cIsDamage>3.0</style>.";

        public override string ItemLore => "God fucking damnit! That stupid Railgunner is going to be here soon! Ugh, she's such an atrocity! She kills everything with her stupid 13567.5% 3.0 death blast and then uses her stupidly overtuned concussion mine to fly across the stage and away from danger! And her stupid bigass shoes! Fuck I hate her giant goddamn shoes! Although... perhaps I could use her stupid obsession with shoes against her? Yes, I could use some of this lunar leather to make the biggest shoes, and then when she gets here, I can use them to lure her into a false sense of security before I stomp her from existence! Hot damn that's a good idea! Let's just hope that stupid crippled newt doesn't get his deformed hands on them and sell it at his bazaar for 2 lunar coins to Railgunner and her teammates so that all of their proc coefficients are set to 3.0 and all proc masks are removed aside from behemoth because behemoth with a proc coefficient crashes the game unless you give it a proc mask and they then proceed to troll the fuck out of the game, including my boss fight. What are the chances that happens, though?";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.Utility };

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