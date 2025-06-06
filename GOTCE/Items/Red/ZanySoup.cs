using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using BepInEx.Configuration;
using System.Reflection;

namespace GOTCE.Items.Red
{
    public class ZanySoup : ItemBase<ZanySoup>
    {
        public override string ItemName => "Zany Soup";

        public override string ConfigName => ItemName;

        public override string ItemLangTokenName => "GOTCE_ZanySoup";

        public override string ItemPickupDesc => "Triple the amount of food-related items you have.";

        public override string ItemFullDescription => "Gain <style=cIsUtility>+2</style> <style=cStack>(+1 per stack)</style> of every <style=cIsHealing>food-related item</style> you have.";

        public override string ItemLore => "Your soup, sir. We made sure to cook it using the <i>finest</i> banter and the <i>wackiest</i> takes, harvested from our server farms in the Alps. Garnished with a gentle portion of absolute degeneracy and sprinkled with a thin dusting of incompetent ownership, the Zany Soup is the pride of our dear establishment.";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/zanysoup.png");

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
            On.RoR2.Inventory.GiveItem_ItemIndex_int += Increase;
            On.RoR2.Inventory.RemoveItem_ItemIndex_int += Inventory_RemoveItem_ItemIndex_int;
        }

        private void Inventory_RemoveItem_ItemIndex_int(On.RoR2.Inventory.orig_RemoveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            if (NetworkServer.active && itemIndex == ItemDef.itemIndex)
            {
                // Main.ModLogger.LogDebug("network server is active");

                int toIncrease = self.GetItemCount(itemIndex) > 1 ? 1 : 2;

                List<ItemIndex> foodRelated = new()
                {
                    RoR2Content.Items.FlatHealth.itemIndex, RoR2Content.Items.ParentEgg.itemIndex,
                    White.MoldySteak.Instance.ItemDef.itemIndex, RoR2Content.Items.Mushroom.itemIndex,
                    DLC1Content.Items.MushroomVoid.itemIndex, DLC1Content.Items.AttackSpeedAndMoveSpeed.itemIndex,
                    RoR2Content.Items.SprintBonus.itemIndex, RoR2Content.Items.Plant.itemIndex,
                    Green.SpafnarsFries.Instance.ItemDef.itemIndex,

                    RoR2Content.Items.HealWhileSafe.itemIndex, RoR2Content.Items.IgniteOnKill.itemIndex,
                    DLC1Content.Items.HealingPotion.itemIndex, RoR2Content.Items.Seed.itemIndex,
                    RoR2Content.Items.TPHealingNova.itemIndex, RoR2Content.Items.Squid.itemIndex,
                    RoR2Content.Items.Clover.itemIndex, RoR2Content.Items.AlienHead.itemIndex,
                    DLC1Content.Items.RandomEquipmentTrigger.itemIndex, RoR2Content.Items.KillEliteFrenzy.itemIndex,
                    DLC1Content.Items.PermanentDebuffOnHit.itemIndex, RoR2Content.Items.NovaOnLowHealth.itemIndex,
                    RoR2Content.Items.SiphonOnLowHealth.itemIndex, RoR2Content.Items.BleedOnHitAndExplode.itemIndex,
                    DLC1Content.Items.CloverVoid.itemIndex, DLC1Content.Items.BleedOnHitVoid.itemIndex,
                    DLC1Content.Items.VoidMegaCrabItem.itemIndex, DLC1Content.Items.MissileVoid.itemIndex,
                    DLC1Content.Items.ExtraLifeVoid.itemIndex, DLC1Content.Items.BearVoid.itemIndex,
                    DLC1Content.Items.SlowOnHitVoid.itemIndex, White.AnimalHead.Instance.ItemDef.itemIndex,
                    BottledCommand.Instance.ItemDef.itemIndex, BottledEnigma.Instance.ItemDef.itemIndex,
                    Green.HeartyBreakfast.Instance.ItemDef.itemIndex, Lunar.PaleAle.Instance.ItemDef.itemIndex,
                    TubOfLard.Instance.ItemDef.itemIndex, VoidLunar.ExpressionOfTheImmolated.Instance.ItemDef.itemIndex,
                    VoidRed.BisonSkull.Instance.ItemDef.itemIndex, VoidRed.TubOfBart.Instance.ItemDef.itemIndex,
                    VoidWhite.FortifiedFlask.Instance.ItemDef.itemIndex, VoidWhite.PluripotentBisonSteak.Instance.ItemDef.itemIndex,
                    VoidYellow.CorruptsAllShatterspleens.Instance.ItemDef.itemIndex, White.Lunch.Instance.ItemDef.itemIndex,
                    White.GummyVitamins.Instance.ItemDef.itemIndex, Yellow.RightRingFingerOfProvidence.Instance.ItemDef.itemIndex,
                    White.Balls.Instance.ItemDef.itemIndex, Red.GalsoneSteak.Instance.ItemDef.itemIndex, Green.Gasogreen.Instance.ItemDef.itemIndex, 
                    White.SummertimeSoda.Instance.ItemDef.itemIndex, White.SkullReaction.Instance.ItemDef.itemIndex, NoTier.Parasite.Instance.ItemDef.itemIndex, Lunar.PebbsiZero.Instance.ItemDef.itemIndex, Green.SquidInkSpaghetti.Instance.ItemDef.itemIndex
                    // original list is
                    // bison steak, planula, moldy steak, bungus, wungus, mocha, energy drink, desk plant, spafnar's fries

                    // added
                    // cautious slug, gasoline for the funny, power elixir, leeching seed, lepton lily, squid polyp,
                    // 57 leaf clover, alien head, bottled chaos, brainstalks, symbiotic scorpion, genesis loop, mired urn,
                    // shatterspleen, benthic bloom, needletick, newly hatched zoea, plasma shrimp, pluripotent larva,
                    // safer spaces, tentabauble, animal head, bottled command, bottled enigma
                };

                foreach (ItemIndex item in self.itemAcquisitionOrder)
                {
                    if (foodRelated.Contains(item))
                    {
                        // Main.ModLogger.LogDebug("Food item found: " + item);
                        self.RemoveItem(item, toIncrease);
                    }
                }
            }
            orig(self, itemIndex, count);
        }

        public void Increase(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex index, int count)
        {
            orig(self, index, count);
            if (NetworkServer.active && index == ItemDef.itemIndex)
            {
                // Main.ModLogger.LogDebug("network server is active");

                int toIncrease = self.GetItemCount(index) > 1 ? 1 : 2;

                List<ItemIndex> foodRelated = new()
                {
                    RoR2Content.Items.FlatHealth.itemIndex, RoR2Content.Items.ParentEgg.itemIndex,
                    White.MoldySteak.Instance.ItemDef.itemIndex, RoR2Content.Items.Mushroom.itemIndex,
                    DLC1Content.Items.MushroomVoid.itemIndex, DLC1Content.Items.AttackSpeedAndMoveSpeed.itemIndex,
                    RoR2Content.Items.SprintBonus.itemIndex, RoR2Content.Items.Plant.itemIndex,
                    Green.SpafnarsFries.Instance.ItemDef.itemIndex,

                    RoR2Content.Items.HealWhileSafe.itemIndex, RoR2Content.Items.IgniteOnKill.itemIndex,
                    DLC1Content.Items.HealingPotion.itemIndex, RoR2Content.Items.Seed.itemIndex,
                    RoR2Content.Items.TPHealingNova.itemIndex, RoR2Content.Items.Squid.itemIndex,
                    RoR2Content.Items.Clover.itemIndex, RoR2Content.Items.AlienHead.itemIndex,
                    DLC1Content.Items.RandomEquipmentTrigger.itemIndex, RoR2Content.Items.KillEliteFrenzy.itemIndex,
                    DLC1Content.Items.PermanentDebuffOnHit.itemIndex, RoR2Content.Items.NovaOnLowHealth.itemIndex,
                    RoR2Content.Items.SiphonOnLowHealth.itemIndex, RoR2Content.Items.BleedOnHitAndExplode.itemIndex,
                    DLC1Content.Items.CloverVoid.itemIndex, DLC1Content.Items.BleedOnHitVoid.itemIndex,
                    DLC1Content.Items.VoidMegaCrabItem.itemIndex, DLC1Content.Items.MissileVoid.itemIndex,
                    DLC1Content.Items.ExtraLifeVoid.itemIndex, DLC1Content.Items.BearVoid.itemIndex,
                    DLC1Content.Items.SlowOnHitVoid.itemIndex, White.AnimalHead.Instance.ItemDef.itemIndex,
                    BottledCommand.Instance.ItemDef.itemIndex, BottledEnigma.Instance.ItemDef.itemIndex
                    // original list is
                    // bison steak, planula, moldy steak, bungus, wungus, mocha, energy drink, desk plant, spafnar's fries

                    // added
                    // cautious slug, gasoline for the funny, power elixir, leeching seed, lepton lily, squid polyp,
                    // 57 leaf clover, alien head, bottled chaos, brainstalks, symbiotic scorpion, genesis loop, mired urn,
                    // shatterspleen, benthic bloom, needletick, newly hatched zoea, plasma shrimp, pluripotent larva,
                    // safer spaces, tentabauble, animal head, bottled command, bottled enigma
                    // maybe we could nerf it to 2x items later down the line
                };

                foreach (ItemIndex item in self.itemAcquisitionOrder)
                {
                    if (foodRelated.Contains(item))
                    {
                        // Main.ModLogger.LogDebug("Food item found: " + item);
                        self.GiveItem(item, toIncrease);
                    }
                }
            }
        }
    }
}