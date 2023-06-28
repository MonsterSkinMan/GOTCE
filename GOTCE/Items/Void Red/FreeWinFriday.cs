using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using GOTCE.Components;
using BepInEx.Configuration;
using HarmonyLib;

namespace GOTCE.Items.Void_Red
{
    public class FreeWinFriday : ItemBase<FreeWinFriday>
    {
        public override string ConfigName => "Free Win Friday";

        public override string ItemName => "Free Win Friday";

        public override string ItemLangTokenName => "GOTCE_FreeWinFriday";

        public override string ItemPickupDesc => "Win. <style=cIsVoid>Corrupts all Spare Drone Parts.</style>";

        public override string ItemFullDescription => "On pickup, <style=cIsUtility>instantly win the run</style>. <style=cIsVoid>Corrupts all Spare Drone Parts.</style>";

        public override string ItemLore => "Spare Drone Parts gets a C. Your drones are meant to draw aggro, not deal damage. This item doesn't change that. While it makes your drone's damage much, much higher, when compared to your overall damage it really is no competition. Plus, if Colonel Droneman dies he won't respawn until the next stage, making the overall usefulness of Spare Drone Parts pretty limited.";

        public override ItemTier Tier => ItemTier.VoidTier3;

        public override Enum[] ItemTags => new Enum[] { GOTCETags.Bullshit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/FreeWinFriday.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override void Hooks()
        {
            On.RoR2.Inventory.GiveItem_ItemIndex_int += PowerCreep;
            On.RoR2.Items.ContagiousItemManager.Init += ContagiousItemManager_Init;
        }

        private void PowerCreep(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex index, int c) {
            orig(self, index, c);
            if (index == ItemDef.itemIndex) {
                Run.instance.BeginGameOver(RoR2Content.GameEndings.PrismaticTrialEnding);
            }
        }

        private void ContagiousItemManager_Init(On.RoR2.Items.ContagiousItemManager.orig_Init orig)
        {
            ItemDef.Pair transformation = new()
            {
                itemDef1 = DLC1Content.Items.DroneWeapons,
                itemDef2 = this.ItemDef
            };
            ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem] = ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem].AddToArray(transformation);
            orig();
        }
    }
}