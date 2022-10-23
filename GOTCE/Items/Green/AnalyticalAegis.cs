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

        public override string ItemPickupDesc => "Gain a miniscule temporary barrier on 'Critcal FOV Strike'";

        public override string ItemFullDescription => "Gain 2 (+5 per stack) barrier on 'Critical FOV Strike'";

        public override string ItemLore => "w dniu dzisiejszym dniu wczorajszym czasie rzeczywistym sumieniem i nie ma w sobie i nie wiem co to jest w porządku i nie wiem czy nie ma co się dzieje się w nim nie jest w stanie w Polsce i na pewno będzie w dniu jutrzejszym czasie rzeczywistym się z nią nie jest to dla mnie nie ma problemu ze nie ma co do tej pory nie otrzymałem żadnej informacji w dniu wczorajszym czasie i miejscu na to pewno będzie to nie jest tak samo jak w w Polsce I'ts na to że w końcu się udało mi to na pewno się w dniu dzisiejszym świecie jest w stanie w Polsce w dniu jutrzejszym terminie do dnia dzisiejszego nie otrzymałem jeszcze w pracy i tak się składa się z nią nie klik\n\n@MonsterSkinMan make this the log for analytical aegis";

        public override ItemTier Tier => ItemTier.Tier2;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Healing };

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
            White.ZoomLenses.Instance.OnFovCrit += Aegis;
        }

        public void Aegis(object sender, White.FovCritEventArgs args) {
            if (args.Body) {
                if (args.Body.inventory) {
                    if (NetworkServer.active) {
                        Inventory inv = args.Body.inventory;
                        int count = inv.GetItemCount(ItemDef);
                        int barrier = 5 * (count-1);
                        if (count > 0) {
                            barrier += 2;
                            args.Body.healthComponent.AddBarrier(barrier);
                        }
                    }
                }
            }
        }
    }
}