using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class AnalyticalAegis : ItemBase<AnalyticalAegis>
    {
        public override string ConfigName => "Analytical Aegis";

        public override string ItemName => "Analytical Aegis";

        public override string ItemLangTokenName => "GOTCE_AnalyticalAegis";

        public override string ItemPickupDesc => "Gain a miniscule temporary barrier on 'FOV Crit'.";

        public override string ItemFullDescription => "Gain <style=cIsUtility>5% FOV crit chance</style>.  On '<style=cIsUtility>FOV Crit</style>', gain <style=cIsHealing>2</style> <style=cStack>(+5 per stack)</style> <style=cIsHealing>barrier</style>.";

        public override string ItemLore => "w dniu dzisiejszym dniu wczorajszym czasie rzeczywistym sumieniem i nie ma w sobie i nie wiem co to jest w porządku i nie wiem czy nie ma co się dzieje się w nim nie jest w stanie w Polsce i na pewno będzie w dniu jutrzejszym czasie rzeczywistym się z nią nie jest to dla mnie nie ma problemu ze nie ma co do tej pory nie otrzymałem żadnej informacji w dniu wczorajszym czasie i miejscu na to pewno będzie to nie jest tak samo jak w w Polsce I'ts na to że w końcu się udało mi to na pewno się w dniu dzisiejszym świecie jest w stanie w Polsce w dniu jutrzejszym terminie do dnia dzisiejszego nie otrzymałem jeszcze w pracy i tak się składa się z nią nie klik\n\n@MonsterSkinMan make this the log for analytical aegis";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Healing, ItemTag.AIBlacklist, GOTCETags.BarrierRelated, GOTCETags.Crit, GOTCETags.FovRelated };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/AnalyticalAegis.png");

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
            CriticalTypes.OnFovCrit += Aegis;
            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory)
                    {
                        args.Stats.FovCritChanceAdd += GetCount(args.Stats.body) > 0 ? 5 : 0;
                    }
                }
            };
        }

        public void Aegis(object sender, FovCritEventArgs args)
        {
            if (args.Body)
            {
                if (args.Body.inventory)
                {
                    if (NetworkServer.active)
                    {
                        Inventory inv = args.Body.inventory;
                        int count = inv.GetItemCount(ItemDef);
                        int barrier = 5 * (count - 1);
                        if (count > 0)
                        {
                            barrier += 2;
                            args.Body.healthComponent.AddBarrier(barrier);
                        }
                    }
                }
            }
        }
    }
}