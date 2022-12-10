using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items.White
{
    public class WhiteFragment : ItemBase<WhiteFragment>
    {
        public override string ConfigName => "WhiteFragment";

        public override string ItemName => "White Fragment";

        public override string ItemLangTokenName => "GOTCE_WhiteFragment";

        public override string ItemPickupDesc => "<style=cDeath>Does nothing</style>. <style=cIsUtility>2</style> White Fragments combine into <style=cIsUtility>4 random items</style>.";

        public override string ItemFullDescription => "<style=cDeath>Does nothing</style>. <style=cIsUtility>2</style> White Fragments combine into <style=cIsUtility>4 random items</style>";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/WhiteFragment.png");

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
            On.RoR2.CharacterBody.OnInventoryChanged += (orig, self) =>
            {
                orig(self);
                if (NetworkServer.active)
                {
                    if (self.inventory && GetCount(self) > 0)
                    {
                        int count = GetCount(self);
                        if (count % 2 == 0)
                        {
                            self.inventory.RemoveItem(ItemDef, 2);
                            WeightedSelection<List<PickupIndex>> weightedSelection = new WeightedSelection<List<PickupIndex>>();
                            weightedSelection.AddChoice(Run.instance.availableTier1DropList, 100f);
                            weightedSelection.AddChoice(Run.instance.availableTier2DropList, 60f);
                            weightedSelection.AddChoice(Run.instance.availableTier3DropList, 4f);
                            weightedSelection.AddChoice(Run.instance.availableVoidTier1DropList, 4f);
                            weightedSelection.AddChoice(Run.instance.availableVoidTier1DropList, 2.3999999f);
                            weightedSelection.AddChoice(Run.instance.availableVoidTier1DropList, 0.16f);

                            for (int i = 0; i < 4; i++)
                            {
                                List<PickupIndex> list = weightedSelection.Evaluate(UnityEngine.Random.value);
                                ItemIndex index = PickupCatalog.GetPickupDef(list[UnityEngine.Random.Range(0, list.Count)])?.itemIndex ?? ItemIndex.None;
                                self.inventory.GiveItem(index);

                                CharacterMasterNotificationQueue.SendTransformNotification(self.master, ItemDef.itemIndex, index, CharacterMasterNotificationQueue.TransformationType.Default);
                            }
                        }
                    }
                }
            };
        }
    }
}