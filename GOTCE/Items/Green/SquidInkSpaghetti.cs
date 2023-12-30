using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class SquidInkSpaghetti : ItemBase<SquidInkSpaghetti>
    {
        public override string ConfigName => "Squid Ink Spaghetti";

        public override string ItemName => "Squid Ink Spaghetti";

        public override string ItemLangTokenName => "GOTCE_SquidInkSpaghetti";

        public override string ItemPickupDesc => "'Critical Stage Transitions' reveal nearby interactables";

        public override string ItemFullDescription => "Gain 10% <style=cStack>(+10% per stack)</style> stage crit chance. On stage crit, reveal all interactables within 500m for 10 seconds.";

        public override string ItemLore => "The centuries after the war were very unkind to the wildlife. We hunted many species to extinction. Some for sport, but the main culprit was for... food. Not just survival food, luxury. Squids were some of the most popular to hunt, and unfortunately, we killed them all for the fat cats. So what was left for us? Their ink sacs. So we do what we can: we fill a pot, throw some spaghetti in, cook, and then throw in squid ink.\n\nIt... works. I'd prefer the actual squid pieces, but this does decently well.";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.AIBlacklist, ItemTag.OnStageBeginEffect, GOTCETags.Crit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

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
                        args.Stats.StageCritChanceAdd += GetCount(args.Stats.body) * 10;
                    }
                }
            };
            CriticalTypes.OnStageCrit += splatoon;
        }

        private void splatoon(object sender, StageCritEventArgs args)
        {
            if (NetworkServer.active && Run.instance.stageClearCount != 0)
            {
                var instances = PlayerCharacterMasterController.instances;
                foreach (PlayerCharacterMasterController playerCharacterMaster in instances)
                {
                    if (playerCharacterMaster.master.inventory.GetItemCount(ItemDef) > 0)
                    {
                        CharacterMaster master = playerCharacterMaster.master;
                        NetworkServer.Spawn(UnityEngine.Object.Instantiate<GameObject>(LegacyResourcesAPI.Load<GameObject>("Prefab/NetworkedObjects/ChestScanner"), master.GetBody().transform.position, Quaternion.identity));
                    }
                }
            }
        }
    }
}
