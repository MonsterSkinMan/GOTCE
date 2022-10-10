using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;
using UnityEngine.Networking;
using GOTCE.Items.White;
using HarmonyLib;

namespace GOTCE.Items.Void_White
{
    public class BarrierStone : ItemBase<BarrierStone>
    {
        public override string ConfigName => "Barrier Stone";

        public override string ItemName => "Barrier Stone";

        public override string ItemLangTokenName => "GOTCE_BarrierStone";

        public override string ItemPickupDesc => "Gain a miniscule temporary barrier on hit. Does nothing on the first stack because it's <style=cHealth>total dogshit</color>. <style=cIsVoid>Corrupts all Barrierworks</style>.";

        public override string ItemFullDescription => "Gain a <style=cIsHealing>temporary barrier</style> on hit for <style=cIsHealing>0</style> <style=cStack>(+2 per stack)</style> <style=cIsHealing>maximum health</style>. <style=cIsVoid>Corrupts all Barrierworks</style>.";

        public override string ItemLore => "...But what if your environment is devoid of everything? Fighting in a barren wasteland? At that point, your final choice is to push your own body into the role of protection. Convert the strikes of your foe into a defensive option. Rush head on into combat and fear nothing, letting yourself absorb injury. You will emerge better because of it.";

        public override ItemTier Tier => ItemTier.VoidTier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Healing, ItemTag.Utility };

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/barrierstone.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.Items.ContagiousItemManager.Init += WoolieDimension;
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);
            if (NetworkServer.active && damageInfo.attacker != null && victim != null)
            {
                if (damageInfo.attacker.GetComponent<CharacterBody>() != null)
                {
                    CharacterBody component = damageInfo.attacker.GetComponent<CharacterBody>();
                    if (component.inventory != null)
                    {
                        int stack = component.inventory.GetItemCount(GOTCE.Items.Void_White.BarrierStone.Instance.ItemDef.itemIndex);
                        if (stack > 0)
                        {
                            CharacterBody body = PlayerCharacterMasterController.instances[0].master.GetBody();
                            body.healthComponent.AddBarrier((2f * stack) - 2f);
                        }
                    }
                }
            }
        }

        private void WoolieDimension(On.RoR2.Items.ContagiousItemManager.orig_Init orig)
        {
            ItemDef.Pair transformation = new()
            {
                itemDef1 = Barrierworks.Instance.ItemDef,
                itemDef2 = this.ItemDef
            };
            ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem] = ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem].AddToArray(transformation);
            orig();
        }
    }
}