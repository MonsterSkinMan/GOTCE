using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class HIFUsRacecar : ItemBase<HIFUsRacecar>
    {
        public override string ConfigName => "HIFUs Racecar";

        public override string ItemName => "HIFU's Racecar";

        public override string ItemLangTokenName => "GOTCE_Racecar";

        public override string ItemPickupDesc => "On ‘Sprint Crit’ instantly recharge all charges of your Secondary skill. Reduce Secondary skill cooldown.";

        public override string ItemFullDescription => "Gain <style=cIsUtility>5% sprint crit chance</style>. On sprint crit, recharge all of your m2 charges. Gain 10% (+15% per stack) cooldown reduction for your m2.";

        public override string ItemLore => "Freedom beyond our control\r\nPartition, they break out in endless stride\r\nI'm on the floor in travail\r\nFaithful days fall\r\nMemories hold\r\nFree my soul\r\nSeeking within\r\nThe curtain call\r\nFleeing our fate\r\nEvery step now recalled\r\nDown the drive\r\nStaying so close\r\nRetraction weighs my soul\r\nWe are thunder\r\nWe are all the space in tow\r\nNow again\r\nThis happening\r\nMy handle is honest calculation\r\nFree of the pain\r\nSeeking the glow\r\nWe both need it so\r\nTime will give way\r\nTime will formulate\r\nSeeking within\r\nThe curtain call\r\nFleeing our fate\r\nEvery step evolved\r\nDon't deny\r\nStaying so close\r\nCreation is our goal\r\nDiscretion, the only thing that made me incomplete\r\nCast the sail, just to spin this fate\r\nCross the empty sect in flight\r\nPleasant memories\r\nRedefine all that's left\r\nThe sound it makes me scream\r\nI can hear your voice at night (Your light lost in a waking dream within)\r\nFreedom is not of control\r\nEndless the chase, pushing through I'm so engulfed\r\nFalling to strain, but I still persist\r\nI'd like to paint this tapestry\r\nWith our blood\r\nTo represent the symmetry\r\nFinding a sickened result of possibility\r\nRefuse the violent source and pain that made you\r\nLeft to deny that it will change with no solitude\r\nLet us take this train\r\nLeaving the form, stretching far beyond what I can see\r\nChasing the race\r\nLeft to complete\r\nMy revelation\r\nReaction creates the fall\r\nRealize\r\nThese are broken words\r\nShattered thoughts amend\r\nSee beyond the only present road\r\nJust let go\r\nCounsel to ascend\r\nSee beyond the only present hold\r\nHe only present hold\r\nLet go\r\nCannot comprehend\r\nLeave it all to find a moment's love I am\r\nWorking to be complete\r\nNo more adversity\r\nGrowing consciously\r\nMy revelation\r\nReaction is not the call\r\nRely on my new self\r\nRising on my own\r\nSage advice, embrace wisdom\r\nHang the past\r\nTake the helm\r\nFashion a new future to live\r\nIn your flame\r\nI see regret\r\nI've awakened\r\nTo feel a piece of distant harmony\r\nHow could I not see\r\nPossibilities and unlimited passion\r\nHello luminescent being\r\nWalking outside of my last identity\r\nCelebrate\r\nFor in the end\r\nWe'll meet inequality\r\nI see light\r\nI see light in your eyes";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.AIBlacklist, GOTCETags.Crit, GOTCETags.BackupMagSynergy };

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
            CriticalTypes.OnSprintCrit += Vroom;
            RecalculateStatsAPI.GetStatCoefficients += CDR;
            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory)
                    {
                        args.Stats.SprintCritChanceAdd += GetCount(args.Stats.body) > 0 ? 5 : 0;
                    }
                }
            };
        }

        public void Vroom(object sender, SprintCritEventArgs args)
        {
            if (NetworkServer.active && args.Body)
            {
                if (GetCount(args.Body) > 0)
                {
                    args.Body.skillLocator.secondary.stock = args.Body.skillLocator.secondary.maxStock;
                }
            }
        }

        public void CDR(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (NetworkServer.active && body.inventory)
            {
                if (GetCount(body) > 0)
                {
                    args.secondaryCooldownMultAdd -= Utils.MathHelpers.InverseHyperbolicScaling(0.1f, 0.15f, 1f, GetCount(body));
                }
            }
        }
    }
}