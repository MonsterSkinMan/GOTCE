using BepInEx.Configuration;
using R2API;
using RoR2;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GOTCE.Items.Lunar
{
    public class DopeDope : ItemBase<DopeDope>
    {
        public override string ConfigName => "Dope Dope";

        public override string ItemName => "Dope Dope";

        public override string ItemLangTokenName => "GOTCE_DopeDope";

        public override string ItemPickupDesc => "<color=#FF7F7F>Your base damage is converted into attack speed.</color>\n";

        public override string ItemFullDescription => "Divide your <style=cIsDamage>base damage</style> by 12 and multiply your <style=cIsDamage>attack speed</style> by 12.";

        public override string ItemLore => "What's up, brother? It's ya boi, Matrix speaking. Hot damn, bro! I figured out how to turn these moon rocks, which were previously considered worthless but have recently found a use in that they could be used to make booze, into hard fucking drugs! Yes brother, you have heard me correctly. All it took was a bit of that soul glass from the core of this dead rock, and once I combined that with the rocks, they combined into a substance that I am now going to call Dope Dope. Not only is it fucking psychedelic as shit, but these hot damn bad boys also increase your movement speed and reverse your controls!\n...\nHoly shit I fucking hate talking to you, Providence. You are a worthless fucking piece of goddamn shit. <i>Takes long swig of Pale Ale</i>\nYoooooo, what if I took some Dope Dope while I’m drunk on Pale Ale? That would be funny, I think. <i>Pops way too fucking many Dope Dopes</i>\nWOOOOOOOOOO HOLY SHIT I AM SO FUCKING HIGH RIGHT NOW PROVIDENCE. I'M GONNA GO MAKE TIERLISTS AND SHIT AND LIKE PUT HUNTRESS IN HIGH TIER AND RAILGUNNER IN LOW TIER. AND THEN I’M GOING TO POST IT IN GD1 I MEAN ROR2 DISCUSSION. THAT'S HOW HIGH I AM. I’M SO HIGH, IN FACT, THAT I THINK ROR2 DISCUSSION HAS MORE DIFFERENCES THAN GD1 BESIDES STRICTER MODERATION AND MORE ANEMIC CONVERSATIONS DUE TO THE RESTRICTION OF OFF TOPIC DISCUSSIONS LEADING TO THE SAME FUCKING ARGUMENTS OVER AND OVER AGAIN ABOUT DUMB ROR2 SHIT. I ALSO THINK SUNCLES IS A GREAT MODERATOR AND THAT PRAYMORDIS SHOULDN’T HAVE BEEN BANNED.\nAnyways Mithrix out, fuckers.";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.Utility, GOTCETags.Masochist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/DopeDope.png");

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
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.attackSpeedMultAdd += 11f + (12f * (stack - 1));
                    args.damageMultAdd -= 1f - (1f / 12f);
                }
            }
        }
    }
}