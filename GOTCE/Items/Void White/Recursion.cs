using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using UnityEngine.Networking;
using GOTCE.Items.White;
using HarmonyLib;

namespace GOTCE.Items.VoidWhite
{
    public class Recursion : ItemBase<Recursion>
    {
        public override string ConfigName => "Recursion";

        public override string ItemName => "Recursion";

        public override string ItemLangTokenName => "GOTCE_Recursion";

        public override string ItemPickupDesc => "Does nothing. <style=cIsVoid>Corrupts all Recursion</style>.";

        public override string ItemFullDescription => "Does nothing. <style=cIsVoid>Corrupts all Recursion</style>.";

        public override string ItemLore => "<style=cIsVoid>while</style> <style=cLunarObjective>(<style=cIsUtility>true</style>)</style> <style=cIsDamage>{</style>\n\n<style=cIsDamage>}</style>";

        public override ItemTier Tier => ItemTier.VoidTier1;

        public override Enum[] ItemTags => new Enum[] { GOTCETags.Bullshit };

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Items/Recursion.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.Items.ContagiousItemManager.Init += WoolieDimension;
            On.RoR2.Items.ContagiousItemManager.StepInventoryInfection += Block;
        }

        private void WoolieDimension(On.RoR2.Items.ContagiousItemManager.orig_Init orig)
        {
            ItemHelpers.RegisterCorruptions(ItemDef, new() { ItemDef });
            orig();
        }

        // force this because recursion blocking corruptions was a very inconsistent
        private bool Block(On.RoR2.Items.ContagiousItemManager.orig_StepInventoryInfection orig, Inventory inv, ItemIndex origIndex, int limit, bool forced) {
            if (inv.GetItemCount(ItemDef) > 0 && origIndex != ItemDef.itemIndex) {
                return false;
            }
            else {
                return orig(inv, origIndex, limit, forced);
            }
        }
    }
}