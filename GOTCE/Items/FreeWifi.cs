/*using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using BepInEx.Configuration;
using R2API.Utils;
using RoR2.Skills;
using System.Reflection;
using MonoMod.RuntimeDetour;
using BepInEx;
using UnityEngine.UI;
using RoR2.CharacterAI;

namespace GOTCE.Items
{
    public class FreeWifi : ItemBase<FreeWifi>
    {
        public override string ConfigName => "Free WiFi";

        public override string ItemName => "Free WiFi";

        public override string ItemLangTokenName => "GOTCE_AlwaysAllowOrbitalSkills";

        public override string ItemPickupDesc => "Get WiFi anywhere you go!";

        public override string ItemFullDescription => "Captain is able to use orbital skills in hidden realms. Subsequent stacks of this item will cause you to <style=cIsHealth>die</style>.";

        public override string ItemLore => "Having trouble with committing murder using orbit-based weaponry? Struggling with controlling crowds from outside of normal time and space? With our revolutionary new product, you can get free wifi anywhere you go!\nNote that FreeWifiCo is not responsible for any harm to you or your collaborators brought about with misuse of our product. The FreeWifiEmitter does emit harmful radiation which can be lethal in concentrations produced by multiple emitters. We are not responsible for any bodily injury, illness, or death brought about by this effect.";

        public override ItemTier Tier => RoR2.ItemTier.Tier3;

        public override ItemTag[] ItemTags => new ItemTag[] { RoR2.ItemTag.Damage };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }
        public override void Hooks()
        {
            RoR2.CharacterBody.onBodyInventoryChangedGlobal += CharacterBody_OnInventoryChanged;
        }

        public bool blockOrbitalSkills;

        private void CharacterBody_OnInventoryChanged(RoR2.CharacterBody self)
        {
            var inventoryCount = GetCount(self);
            if (inventoryCount > 0 && self.master && self.inventory)
            {
                for (int i = 0; i < RoR2.SceneCatalog.allSceneDefs.Length; i++)
                {
                    SceneCatalog.allSceneDefs[i]-->blockOrbitalSkills = false;
                }
            }
            if (inventoryCount > 1 && self.master && self.inventory)
            {
                self.healthComponent.Suicide(self.gameObject, self.gameObject, RoR2.DamageType.Generic);
            }
        }
    }
}*/
