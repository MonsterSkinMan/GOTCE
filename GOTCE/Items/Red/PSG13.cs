using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items.Red
{
    public class PSG13 : ItemBase<PSG13>
    {
        public override string ConfigName => "Personal Shield Generator 13";

        public override string ItemName => "Personal Shield Generator (13)";

        public override string ItemLangTokenName => "GOTCE_PersonalShieldGenerator13";

        public override string ItemPickupDesc => "videogame won.";

        public override string ItemFullDescription => "gain 104% <style=cStack>(+104% per stack)</style> of yrou max hp in shield. recharges outside of danger. thirteen personal shield generators is a videogame won. ensures a good run.";

        public override string ItemLore => "\"What are you doing?\"\n\"Dude, there's a shield generator printer here!\"\n\"...Seriously?\"\n\"What?\"\n\"You know the electricity on those actually make you weaker, right? Makes it so that you can die in a single hit. Plus not taking damage for 9 seconds is really hard, idiot.\"\n\"And the reward of having a dozen PSGs will be worth that.\"\n\"Psh. Doubt it. There's a scrapper right there, I'm throwing the 2 I have in there.\"\n\"Suit yourself. I think thirteen will be enough to protect me.\"\n\"Good luck putting all of them on.\"\n\"It barely even hu-\"\n<i>zzzzzt</i>\n\"You okay?\"\n<style=cMono>heinous monster</style>\n\"What?\"\n<style=cMono>donot interact with the vile mechanism</style>\n\"Dude, I already told yo- Why are you talking like that?\"\n<style=cMono>yuo will regret it</style>\n\"Whatever you say...\"\n<style=cMono>deluded fool</style>\n\"What's going o-\"\n<style=cMono>videogame won</style>";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, GOTCETags.Shield };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/psg13.png");

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
            RecalculateStatsAPI.GetStatCoefficients += new RecalculateStatsAPI.StatHookEventHandler(VideogameWon);
        }
        public static void VideogameWon(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.inventory)
            {
                var stack = body.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    float gamewon = body.healthComponent.fullHealth * 1.04f;
                    args.baseShieldAdd += gamewon * stack;
                }
            }
        }
    }
}
