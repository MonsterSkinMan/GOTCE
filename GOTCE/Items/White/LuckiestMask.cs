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
        public override string ItemFullDescription => "Increase the <style=cIsDamage>proc coefficient</style> of all of your attacks by <style=cIsDamage>1</style> <style=cStack>(+1 per stack)</style>. Taking damage to below <style=cIsHealth>25% health</style> <style=cIsUtility>breaks</style> this item.";
        public override Sprite ItemIcon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Items/LuckiestMask.png");
        public override string ItemLangTokenName => "GOTCE_LuckiestMask";
        public override string ItemLore => "[Intro]\n(Secure the bag, know what I mean? Banrisk on the beat)\n(Ayo, Perish, this is hot, boy)\n\n[Verse 1]\nI wear a mask with a smile for hours at a time\nStare at the ceiling while I hold back what's on my mind\nAnd when they ask me how I'm doing, I say \"I'm just fine\"\nAnd when they ask me how I'm doing, I say \"I'm just fine\"\nBut the fact is\nI can never got off of my matress\nAnd all that they ask is\n\"Why are you so sad, kid?\" (Why are you so sad, kid?)\n[Pre-Chorus}\nThat's what the mask is\nThat's what the point of the mask is\n\n[Chorus]\nSo you can see I'm tryin', you won't see my cryin'\nI'll just keep on smilin', I'm good (Yeah I'm good)\nAnd it just keeps on pilin', it's so terrifying\nBut I keep on smilin', I'm good (Yeah, I'm good)\nI've been carin' too much for so long\nBeen comparin' myself for so long\nBeen wearin' a smile for so long, it's real\nSo long, it's real, so long, it's real\nYeah I'm tired of writing this shit just pretend that I put the rest of the lyrics for dream mask here I'm done.";
        public override GameObject ItemModel => null;
        public override string ItemName => "Luckiest Mask";
        public override string ItemPickupDesc => "Increase proc coefficient. Breaks at low health.";
        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.Consumable };
        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.DamageInfo.ModifyDamageInfo += Proc;
        }

        [RunMethod(RunAfter.Items)]
        private static void RegisterFragile()
        {
            Fragile.AddFragileItem(LuckiestMask.Instance.ItemDef, new Fragile.FragileInfo
            {
                broken = Items.NoTier.DreamPDF.Instance.ItemDef,
                shouldGiveBroken = true,
                fraction = 25f
            });
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
                    if (!self.procChainMask.HasProc(ProcType.Behemoth))
                    {
                        self.procCoefficient += count;
                    }
                }
            }
            orig(self, mod);
        }
    }
}