using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items.White
{
    public class BangSnap : ItemBase<BangSnap>
    {
        public override string ConfigName => "Bang Snap";

        public override string ItemName => "Bang Snap";

        public override string ItemLangTokenName => "GOTCE_BangSnap";

        public override string ItemPickupDesc => "+2 AOE effect to all explosions.";

        public override string ItemFullDescription => "The <style=cIsDamage>radius</style> of all area of effect attacks is increased by <style=cIsDamage>2m</style> <style=cStack>(+2m per stack)</style>.";

        public override string ItemLore => "\"Weren't you arrested for arson?\"\n\"Yeah, I was. But the court order got nullified, and I stole Jay's ID to make it here.\"\n\"It's just fireworks. Are you seriously going to risk going back to jail for this?\"\n\"Oh, these aren't just fireworks. I just got the wrappers FROM the fireworks dude. They're full of a secret surprise-\" <i>BOOM</i>\n\"What the hell did you put in there?\"\n\"Bang snaps and uranium.\"\n<color=#e64b13>And then they both died lmao.</color>";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/BangSnap.png");

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
            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory)
                    {
                        args.Stats.AOEAdd += GetCount(args.Stats.body) * 2;
                    }
                }
            };
        }
    }
}