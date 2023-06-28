using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items.White
{
    public class PSG : ItemBase<PSG>
    {
        public override string ConfigName => "PSG";

        public override string ItemName => "PSG";

        public override string ItemLangTokenName => "GOTCE_PSG";

        public override string ItemPickupDesc => "gain 8% of yrou max hp in shield";

        public override string ItemFullDescription => "gain 8% <style=cStack>(+8% per stack)</style> of yrou max hp in shield. recharges outside of danger. thirteen personal shield generators is a videogame won. ensures a good run.";

        public override string ItemLore => "Personal Shield Generator. An innocent-seeming device on its own, made by Hinon, the same company responsible for the shields that charge your ships. They realized the same electricity that covered the surface area of the ships could be reduced to fit a person, or any humanoid thing. Later on, every UES employee and the bots that work for them had one just in case they needed quick defense. Some people on Earth didn't like them, the concept of electricity covering their body didn't sound too appealing to them, I suppose. I call them fools. This shield has saved me more times than I can count.\nBut I've noticed something weird with this little thing. Every time I have it with me, my luck is always better. UES employees host gambling nights sometimes, and everytime I have it with me, I <i>always</i> end up winning more than I would without it. I started tallying up my earnings with and without the shield on me, and found that I won about 20% more often with the shield. Sometimes, someone might accidentally fling something off of a desk, and it would woosh right past me. Call it superstition, but this just seems too consistent to be a coincidence. Some of the boys have also tried putting on multiple at once. Hinon says it's a bad idea, that it's too much electricity, but I've seen some people who put on four, five, and even one guy who put on thirteen. I swear he was unkillable. Snatching the gens from his fallen comrades and using them himself, he was one of the few who made it out of that. It's almost like with thirteen, you can't lose.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, GOTCETags.Shield };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/PSG.png");

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
                    float gamewon = body.healthComponent.fullHealth * 0.08f;
                    args.baseShieldAdd += gamewon * stack;
                }
            }
        }
    }
}
