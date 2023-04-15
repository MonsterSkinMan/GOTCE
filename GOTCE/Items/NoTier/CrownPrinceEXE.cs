using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items.NoTier
{
    public class CrownPrinceEXE : ItemBase<CrownPrinceEXE>
    {
        public override string ConfigName => "CrownPrince exe";

        public override string ItemName => "CrownPrince.exe";

        public override string ItemLangTokenName => "GOTCE_CrownPrinceEXE";

        public override string ItemPickupDesc => "What's up my name is Jim Nuts. Ever since I was little I've always loved the video game known as gunfire reborn I would play it for hours on end and then looking at fanart of my favorite gunfire character crown prince. crown prince is really cool and awesome and I've had that thought ever since I was little. Well one day a few years ago I sold my gunfire reborn because I needed money for college recently I've been thinking about gunfire reborn a lot and I want to play it again. Luckily my neighbor Mrs crunder is having a garage sale so I'm going to see if she has gunfire reborn. so I went to the garage sale an couldnt find gunfire reborn and jst when I had given up I spotted a USB that was labeled \"gunfire reborn (WARNING: DUE KNOT BYE!)\" so I went to Mrs crunder to ask why she was telling people not to buy gunfire rheborr and she turned to em with a look of pure fear in her eyes and said\"that giyame is haunted and satanic and it ahs kil evrbody whomst has had the misfortune of playing it\"I didn't know what that meant and asked her how much it costed and she said \"$5\". What a steal! Gunfire reborn for a measly five dollars? This is the best day of my life! so I gave her the money and then ran home to play gunfire reborn I put the USB in my computer and saw that there were only 2 files on it one was called GunfireReborn.exe and the other was called I'M COMING TO KILL YOU! \"weird\" I thought as I clicked on GunfireReborn.exe since I'm not stupid I know .exe files aren't haunted it's literally just short for executable the game loaded almost instantly, although I saw what looked like a real butchered human corpse on the extremely brief loading screen.it was probably just my eyes playing tricks on me since I had been browsing li fanart the night before and I was very tired so I kept playing.On the title screen, I noticed that the normal title screen music wasn't playing, but it was replaced by these eerie demonic chants and wails there was also backwards audio that said \"You have entered hell your fate is sealed you will suffer for the rest of time\" \"wow that's pretty funny.they must've updated the main menu music\" I thought. I proceeded to enter the character select, and I saw that all of the characters were already unlocked. Lucky me I thought, even though I didn't have the dlc characters but I noticed that something was… off about all of the characters. They looked extremely scared, almost as if they had seen some unspeakable horrors of the outside world. I decided to pick the dog for the lulz, and when I did, he became unimaginably horrified and had a complete mental breakdown. maybe all of the explosive weapons he uses were giving him ptsd flashbacks.what a baby. as I started the run, I was shocked to see that I was starting at hyperborean jokul well I couldn't tell it was hyperborean jokul at first because of all the hyper realistic blood and gore all over the ground but whatever. actually, it was right before the final boss fight. must be a new game+ feature I thought I entered the elevator up to his boss arena except it was made of bones and intestines and flesh. pole monarch is really cheaping out on the elevator now, huh? I walked into the boss arena, but pole monarch wasn't there. instead, a new boss called blood monarch appeared.it was at that point that I noticed I had no weapons.I began to get extremely afraid.suddenly, a lantern spirit appeared behind me and I died. before I could revive myself, blood monarch ran up to me and began butchering me and consuming me \"damn I didn't know gunfire had a RimWorld crossover\" I was sent to a very scary a nd gruesome version of the game over screen that showed a hyper ralistic version of the dog's mutilated corpse, alongside an actual picture of a real life mutilated dog corpse. oh no, I lost. guess I'll just try again.when I went to the character select screen, I noticed that the dog was missing, and in his place was a hyper realistic tombstone.I beagn to cry a little bit because I loved the dog.I then decided to play as the bird. I went back to fight blood monarch, and again, a lantern spirit appeared behind me and blood monarch ate me. this time, the game over screen showed a hyper relstialc  version of bird's corpse and a group of real life mutilated bird corpses. I didn't really care since bird is lame because he has feathers I then decided to play tiger because everybody knows you can't eat a tiger. it would seem that blood monarch didn't get the meme o, because after tiger was downed by lantern spirit, he savagely and brutally and bloodily hyper realistically killed and ate tiger before cutting to a game over screen with a hyper realistic mutilated tiger corpse and a real life tiger corpse. before I could even send in turtle, blood monarch broke in and began to eat him before running off \"uh oh this isn't good\" I thoguht next, I decided to play tao, one of my favorite childhood characters I remember spending many nights browsing tao fanart in bedso I've always had a soft spot for her however when I went to face blood monarch he tore off Tao's clothes and skin and then brutally bvored her i began to cry very hard now because tap was dead and the game over screen showed her lifeless remains with a picture of a bunny extracted from the stomach of a fox. before I could begin my attempt with crown prince, he turned to face me directly with hyper realistic tears in his eyes and said to me \"stop. we will all die. I fuckin hate you. You killed all of my friends. go to hell, fucker. stop playing the game! get out while you still can!\" oh, classic crown prince.always a jokester.I thougt back to all of the crown prince fanart I've looked at over the years and let out a happy chuckle. I clicked the play button and made my way wit crown prince to blood monarch. This time, I remembered that I had a dash button so I dodged the lantern spirit, but then blood monarch dashed up and grabbed me. crown prince began to sob introllably in fear and a cutscwne began to play. \"Guh...No  blood monarch please spare me please I don't want to die I want to live please spare me I am so afraid.\" crown prince sobbed \"Too bad. I will xonsum you, just like I did all your friends\" blood moancr roared.Crown prince statred at me in anger ad he was brutally devoured.suddenly, I heard a sound behind me. I turned around and I saw a partially devoured ghost of crown prince! In real life! I blushed profusely and he began to approach me. \"You didn't listen. It's time for you to die now\" as he said that he approached me and killed me. the… end?????";

        public override string ItemFullDescription => "penis idk";

        public override string ItemLore => "there is no lore";

        public override ItemTier Tier => ItemTier.NoTier;

        public override Enum[] ItemTags => new Enum[] { ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/CrownPrince.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.HealthComponent.TakeDamage += Instakill;
        }

        public void Instakill(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo info)
        {
            orig(self, info);
            if (self.body && NetworkServer.active)
            {
                // CharacterBody attacker = info.attacker.GetComponent<CharacterBody>();
                if (self.body.inventory && self.body.inventory.GetItemCount(ItemDef) > 0)
                {
                    self.Suicide();
                }
            }
        }
    }
}
