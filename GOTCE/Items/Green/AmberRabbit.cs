using System.Collections.Generic;
using System.Data;
using BepInEx.Configuration;
using GOTCE.Items.White;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items.Green
{
    public class AmberRabbit : ItemBase<AmberRabbit>
    {
        public override string ConfigName => "Amber Rabbit";

        public override string ItemFullDescription => "Gain <style=cIsUtility>5% stage transition crit chance</style>. On '<style=cIsUtility>Stage Transition Crit</style>', <style=cIsUtility>double your item count</style>.";

        public override string ItemLore => "The old clock was just the beginning. Now that I've seen this little blip, this little tear in the fabric of reality, everything looks like a tool to widen it. My newest discovery is a small figurine of a rabbit that appears to be covered in some odd form of amber. On a subatomic scale, something incredibly odd appears to be happening. When I shine a light on it, not only is some absorbed, but an equal amount is emitted by the amber itself. Perfectly equal. So far, it's just light, but I believe this can go further. Much further.";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.AIBlacklist, ItemTag.OnStageBeginEffect, GOTCETags.Crit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/AmberRabbit.png");

        public override string ItemLangTokenName => "GOTCE_AmberRabbit";

        public override string ItemName => "Amber Rabbit";

        public override string ItemPickupDesc => "On Stage Transition Crit, double your item count.";

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            CriticalTypes.OnStageCrit += the;
            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory)
                    {
                        args.Stats.StageCritChanceAdd += GetCount(args.Stats.body) > 0 ? 5 : 0;
                    }
                }
            };
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public void the(object sender, StageCritEventArgs args)
        {
            if (NetworkServer.active && Run.instance.stageClearCount != 0)
            {
                var instances = PlayerCharacterMasterController.instances;
                foreach (PlayerCharacterMasterController controller in instances)
                {
                    if (controller.master && controller.master.GetBody())
                    {
                        Inventory inv = controller.master.GetBody().inventory;
                        if (inv.GetItemCount(ItemDef) > 0)
                        {
                            List<ItemIndex> items = inv.itemAcquisitionOrder;
                            foreach (ItemIndex itemIndex in items)
                            {
                                int toIncrease = inv.GetItemCount(itemIndex) * inv.GetItemCount(ItemDef);
                                inv.GiveItem(itemIndex, toIncrease);
                            }
                        }
                    }
                }
            }
        }
    }
}