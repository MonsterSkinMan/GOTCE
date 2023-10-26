using BepInEx.Configuration;
using UnityEngine;

namespace GOTCE.Items.White
{
    public class BarrierInsurance : ItemBase<BarrierInsurance>
    {
        public override bool CanRemove => true;
        public override string ConfigName => ItemName;
        public override string ItemFullDescription => "At the start of each stage, gain <style=cIsHealing>barrier</style> equal to <style=cIsHealing>7000%</style> of the amount <style=cStack>(+7000% per stack)</style> of <style=cIsHealing>barrier</style> items you have.";
        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/BarrierInsurance.png");
        public override string ItemLangTokenName => "GOTCE_BarrierInsurance";
        public override string ItemLore => "I don't legally need to disclose my name, but here at I. M. Leegull's Insurance, we cover everything from the normal accidents, to the... not so normal accidents. All the car crashes, the house fires, and even the godawful scaling of high-health barrier decay are covered by my plan. It's a cheap cost of 600 dollars per... week, and if you say that this just sounds like getting the shit scammed out of you, you would be closer to the answer than you think. I'm not scamming you, specifically, I'm just committing insurance fraud.";
        public override GameObject ItemModel => null;
        public override string ItemName => "Barrier Insurance";
        public override string ItemPickupDesc => "Gain an amount of barrier at the start of each stage based on how many barrier items you have.";
        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.Healing, GOTCETags.BarrierRelated, ItemTag.OnStageBeginEffect };
        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.CharacterBody.Start += Barrier;
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public void Barrier(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self)
        {
            if (NetworkServer.active)
            {
                if (GetCount(self) > 0)
                {
                    if (self.inventory)
                    {
                        int total = 0;
                        foreach (ItemIndex index in self.inventory.itemAcquisitionOrder)
                        {
                            if (ContainsTag(ItemCatalog.GetItemDef(index), GOTCETags.BarrierRelated))
                            {
                                total += self.inventory.GetItemCount(index);
                            }
                        }
                        float scale = 70f * GetCount(self);
                        self.healthComponent.AddBarrier(total * scale);
                    }
                }
            }
            orig(self);
        }

        private void Stage_Start(On.RoR2.Stage.orig_Start orig, Stage self)
        {
            orig(self);
            if (CharacterMaster.instancesList != null)
            {
                foreach (CharacterMaster cm in CharacterMaster.instancesList)
                {
                    if (cm.inventory)
                    {
                        var stack = GetCount(cm);
                        if (stack > 0)
                        {
                            var body = cm.GetBody();
                            if (body)
                            {
                                int total = 0;
                                foreach (ItemIndex index in cm.inventory.itemAcquisitionOrder)
                                {
                                    var itemDef = ItemCatalog.GetItemDef(index);

                                    var hasBarrierInName = Language.GetString(itemDef.nameToken).ToLower().Contains("barrier");
                                    var hasBarrierInPickup = Language.GetString(itemDef.pickupToken).ToLower().Contains("barrier");
                                    var hasBarrierInDescription = Language.GetString(itemDef.descriptionToken).ToLower().Contains("barrier");

                                    if (ContainsTag(itemDef, GOTCETags.BarrierRelated) || hasBarrierInName || hasBarrierInPickup || hasBarrierInDescription)
                                    {
                                        total += cm.inventory.GetItemCount(index);
                                    }
                                }
                                float scale = 70f * stack;
                                body.healthComponent.AddBarrier(total * scale);
                            }
                        }
                    }
                }
            }
        }
    }
}