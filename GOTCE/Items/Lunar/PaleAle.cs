/*
using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items.Lunar
{
    public class PaleAle : ItemBase<PaleAle>
    {

        public override string ConfigName => "Pale Ale";

        public override string ItemName => "Pale Ale";

        public override string ItemLangTokenName => "GOTCE_PaleAle";

        public override string ItemPickupDesc => "Increase your damage... <color=#FF7F7F>BUT invert your camera movement.</color>\n";

        public override string ItemFullDescription => "Increase your <style=cIsDamage>damage</style> by <style=cIsUtility>60%</style> <style=cStack>(+40% per stack)</style>. Invert your camera movement. Increase <style=cIsUtility>cooldowns</style> by <style=cIsUtility>0%</style> <style=cStack>(+35% per stack)</style>.";

        public override string ItemLore => "Holy shit brother! I figured out how to turn these worthless moon rocks into booze! Hot damn!\r\nSo basically I [REDACTED] and then I [REDACTED] and then after a little [REDACTED] I [REDACTED] Commando’s mother. Hot damn indeed! Anyways, it’s time to get wasted. Glug glug glug…\r\nHoooly shit this stuff is strong, brother. I feel so much more powerful, but I also feel like my camera controls have been inverted. Ohhhhh god…\r\nFuck you, brother. I hate you so fucking much, and I hate you even more because I’m drunk as shit. That gift should’ve been for ME! Not… YOU! It reminds me of a little ditty I once heard on my smartphone. It went a little something like this: There once was a man named Joe, and my tooth fell out on his head, and he put it under his pillow. And then, the Tooth Fairy took it and gave him a one dollar bill! That money should have been mine, not his! And so, I punched his guts in and poured acid all over his butt! And then I threw bananas at him like a wild monkey! After that, Joe ran away to the bottom of the Atlantic Ocean, with his glasses and moustache. But when he was running away, he dropped the one dollar bill, and it was finally mine!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\nSo what did you think of the song, brother? W-what? You thought it was shit? Well, fuck you, you insignificant WHORE!!!\r\n";

        public override ItemTier Tier => ItemTier.Lunar;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

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
            On.RoR2.PlayerCharacterMasterController.FixedUpdate += PlayerCharacterMasterController_FixedUpdate;
            On.RoR2.PlayerCharacterMasterController.Update += PlayerCharacterMasterController_Update;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.damageMultAdd += 0.6f + 0.4f * (stack - 1);
                    args.cooldownMultAdd += 0f + 0.35f * (stack - 1);
                }
            }
        }

        private bool hasItem = false;

        private void PlayerCharacterMasterController_FixedUpdate(On.RoR2.PlayerCharacterMasterController.orig_FixedUpdate orig, PlayerCharacterMasterController self)
        {
            orig(self);
            var body = self.gameObject.GetComponent<CharacterMaster>().GetBody();
            if (body && body.inventory && body.inventory.GetItemCount(Instance.ItemDef) > 0)
            {
                hasItem = true;
            }
        }

        private void PlayerCharacterMasterController_Update(On.RoR2.PlayerCharacterMasterController.orig_Update orig, PlayerCharacterMasterController self)
        {
            orig(self);
            if (hasItem)
            {
                self.bodyInputs.aimDirection = -self.bodyInputs.aimDirection;
            }
        }
    }
}
*/