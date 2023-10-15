using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class BillieJean : ItemBase<BillieJean>
    {
        public override string ConfigName => "Billie Jean";

        public override string ItemName => "Billie Jean";

        public override string ItemLangTokenName => "GOTCE_BillieJean";

        public override string ItemPickupDesc => "Who will dance on the floor in the round? YOU.";

        public override string ItemFullDescription => "Gain <style=cIsUtility>10%</style> <style=cStack>(+5% per stack)</style> <style=cIsUtility>FOV crit chance</style>. Gain <style=cIsUtility>30%</style> <style=cStack>(+20% per stack)</style> <style=cIsUtility>sprint speed</style>. <style=cIsUtility>Become Michael Jackson</style>.";

        public override string ItemLore => "\"Billie Jean\" is a song by American singer Michael Jackson, released by Epic Records on January 2, 1983, as the second single from Jackson's sixth studio album, Thriller (1982). It was written and composed by Jackson and produced by Jackson and Quincy Jones. \"Billie Jean\" blends post-disco, rhythm and blues, funk and dance-pop. The lyrics describe a woman, Billie Jean, who claims that the narrator is the father of her newborn son, which he denies. Jackson said the lyrics were based on groupies' claims about his older brothers when he toured with them as the Jackson 5.\r\n\r\n\"Billie Jean\" reached number one on the Billboard Hot 100, topped the Billboard Hot Black Singles chart within three weeks, and became Jackson's fastest-rising number one single since \"ABC\", \"The Love You Save\" and \"I'll Be There\" in 1970, all of which he recorded as a member of the Jackson 5. Billboard ranked it as the No. 2 song for 1983. \"Billie Jean\" is certified 6× Platinum by the Recording Industry Association of America (RIAA). The song has sold over 10 million copies worldwide, making it one of the best-selling singles of all time. It was also a number one hit in the UK, Canada, France, Switzerland and Belgium, and reached the top ten in many other countries. \"Billie Jean\" was one of the best-selling singles of 1983, helping Thriller become the best-selling album of all time, and became Jackson's best-selling solo single.";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/BillieJean.png");

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
            On.RoR2.Inventory.GiveItem_ItemIndex_int += Inventory_GiveItem_ItemIndex_int;
            On.RoR2.Inventory.RemoveItem_ItemIndex_int += Inventory_RemoveItem_ItemIndex_int;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory)
                    {
                        var stack = GetCount(args.Stats.body);
                        if (stack > 0)
                            args.Stats.FovCritChanceAdd += 10f + 5f * (stack - 1);
                    }
                }
            };
        }

        private void Inventory_RemoveItem_ItemIndex_int(On.RoR2.Inventory.orig_RemoveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            if (itemIndex == Instance.ItemDef.itemIndex)
            {
                var master = self.GetComponent<CharacterMaster>();
                if (master)
                {
                    var body = master.GetBody();
                    if (body)
                    {
                        var characterDirection = body.characterDirection;
                        if (characterDirection)
                            characterDirection.turnSpeed = 720f;
                    }
                }
            }
        }

        private void Inventory_GiveItem_ItemIndex_int(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            if (itemIndex == Instance.ItemDef.itemIndex)
            {
                var master = self.GetComponent<CharacterMaster>();
                if (master)
                {
                    var body = master.GetBody();
                    if (body)
                    {
                        var characterDirection = body.characterDirection;
                        if (characterDirection)
                            characterDirection.turnSpeed = 0f;
                    }
                }
            }
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            var stack = GetCount(sender);
            if (stack > 0)
            {
                args.sprintSpeedAdd += 0.3f + 0.2f * (stack - 1);
            }
        }
    }
}