using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using HarmonyLib;

namespace GOTCE.Items.Red
{
    public class FreeWifi : ItemBase<FreeWifi>
    {
        public override string ItemName => "Free WiFi";

        public override string ConfigName => ItemName;

        public override string ItemLangTokenName => "GOTCE_FreeWifi";

        public override string ItemPickupDesc => "Unlock orbital skills in hidden realms. Instantly die if picked up again.";

        public override string ItemFullDescription => "Captain can use his <style=cIsUtility>orbital skills</style> anywhere. <style=cIsHealth>Die</style> 0 <style=cStack>(+1 per stack)</style> times.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Healing };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.CharacterBody.OnInventoryChanged += Hopoo;
        }

        public void Hopoo(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            if (NetworkServer.active)
            {
                if (self.isPlayerControlled)
                {
                    // SkillLocator sl = self.skillLocator;
                    if (self.inventory.GetItemCount(ItemDef) > 0)
                    {
                        SceneCatalog.mostRecentSceneDef.blockOrbitalSkills = false;
                    }
                    if (self.inventory.GetItemCount(ItemDef) > 1)
                    {
                        self.healthComponent.Suicide();
                        self.master.inventory.RemoveItem(ItemDef, 1);
                    }
                    if (self.inventory.GetItemCount(ItemDef) <= 0)
                    {
                        List<SceneDef> scenes = new() {
                            SceneCatalog.GetSceneDefFromSceneName("bazaar"),
                            SceneCatalog.GetSceneDefFromSceneName("arena"),
                            SceneCatalog.GetSceneDefFromSceneName("voidstage"),
                            SceneCatalog.GetSceneDefFromSceneName("voidraid"),
                            SceneCatalog.GetSceneDefFromSceneName("goldshores"),
                            SceneCatalog.GetSceneDefFromSceneName("artifactworld"),
                            SceneCatalog.GetSceneDefFromSceneName("limbo"),
                            SceneCatalog.GetSceneDefFromSceneName("mysteryspace"),
                            SceneCatalog.GetSceneDefFromSceneName("testscene")
                        };

                        foreach (SceneDef scene in scenes)
                        {
                            scene.blockOrbitalSkills = true;
                            if (SceneCatalog.mostRecentSceneDef.cachedName == scene.cachedName)
                            {
                                SceneCatalog.mostRecentSceneDef.blockOrbitalSkills = true;
                            }
                        }
                    }
                }
            }
            orig(self);
        }
    }
}