using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.White
{
    public class TeachersPet : ItemBase<TeachersPet>
    {
        public override string ConfigName => "Teachers Pet";

        public override string ItemName => "Teacher's Pet";

        public override string ItemLangTokenName => "GOTCE_TeachersPet";

        public override string ItemPickupDesc => "Deal extra damage if your survivor has the letter A in their name.";

        public override string ItemFullDescription => "You gain a 20% (+20% per stack) damage boost if the character you are playing as has the letter \"A\" in their name.";

        public override string ItemLore => "We used to be friends, you know? We had a great friendship going on. And we had a good thing going. You'd help me with class, and I'd help you with the hit on you head. I really wasn't sure what exactly you'd have to do to have a hit on a child's head, but I didn't look into it too much. And I know we had a single agreement, which was that I wasn't to miss a meeting of ours.\n\nExcept I uh, kinda had to dude. My brother was being shipped off to war and I figured I could miss a single day. But you. Oh you rotten fucking teacher's pet, you did that shit to me. You wanna know what happened for me? I had to walk in, and the teach threw an apple at me. An actual apple, and then told me how I apparently \"cheated\" on a test. I denied it as much as I possibly could, but you, with your high fucking reputation, ruined it all. Now I'm out of classes! My parents disowned me!\n\nSo, now that I got you here, that hit on your head? I decided to finally ask. Oh I'm telling you, being a teacher's pet? You might have a teach, but you have a lot of people who want you dead. And just saying, that cash looks very tasty right now.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] {ItemTag.Damage };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/TeachersPet.png");

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
            RecalculateStatsAPI.GetStatCoefficients += A;
        }

        public void A(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args) {
            int c = GetCount(body);
            if (c > 0) {
                if (Language.GetString(body.baseNameToken).ToLower().Contains("a")) {
                    args.damageMultAdd += 0.2f * c;
                }
            }
        }
    }
}