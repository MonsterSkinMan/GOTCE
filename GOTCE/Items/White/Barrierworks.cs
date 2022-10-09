using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using R2API.Utils;

namespace GOTCE.Items.White
{
    public class Barrierworks : ItemBase<Barrierworks>
    {
        public override string ConfigName => "Barrierworks";

        public override string ItemName => "Barrierworks";

        public override string ItemLangTokenName => "GOTCE_Barrierworks";

        public override string ItemPickupDesc => "Activating an interactable grants a temporary barrier.";

        public override string ItemFullDescription => "Activating an interactable grants a <style=cIsHealing>temporary barrier</style> for <style=cIsHealing>5% maximum health</style> <style=cStack>(+5% per stack)</style>.";

        public override string ItemLore => "Do not let yourself waste anything. Utilize your surroundings and all available material to the greatest extent; waste is as grave a sin as failure itself. In a true situation of life and death, there is nothing that is useless. Everything can be used as a weapon or as protection. The job just falls on you to recognize these things and use them to the fullest extent. Even things like containers can be assimilated into your war machine to best ensure your survival.\n\nFailure is not an option, and with the right knowledge and materials, not a possibility.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Healing, ItemTag.Utility, ItemTag.InteractableRelated };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

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
            On.RoR2.GlobalEventManager.OnInteractionBegin += GlobalEventManager_OnInteractionBegin;
        }

        private void GlobalEventManager_OnInteractionBegin(On.RoR2.GlobalEventManager.orig_OnInteractionBegin orig, GlobalEventManager self, Interactor interactor, IInteractable interactable, GameObject interactableObject)
        {
            if (NetworkServer.active)
            {
                CharacterBody body = interactor.GetComponent<CharacterBody>();
                if (body)
                {
                    Inventory inv = body.inventory;
                    if (inv)
                    {
                        int itemCount = inv.GetItemCount(Instance.ItemDef.itemIndex);
                        if (itemCount > 0)
                        {
                            body.healthComponent.AddBarrier((body.healthComponent.fullHealth * 0.05f) * itemCount);
                        }
                    }
                }
            }
        }
    }
}