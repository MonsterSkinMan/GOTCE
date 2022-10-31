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

        public override string ItemLore => "Having trouble with committing murder using orbit-based weaponry? Struggling with controlling crowds from outside of normal time and space? With our revolutionary new product, you can get free wifi anywhere you go!\n<i>Note that FreeWifiCo is not responsible for any harm to you or your collaborators brought about with misuse of our product. The FreeWifiEmitter does emit harmful radiation which can be lethal in concentrations produced by multiple emitters. We are not responsible for any bodily injury, illness, or death brought about by this effect.</i>";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.AIBlacklist, GOTCETags.Unstable };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/FreeWifi.png");

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
                    if (self.inventory.GetItemCount(ItemDef) >= 2 && self.master && self.inventory)
                    {
                        self.healthComponent.Suicide(self.gameObject, self.gameObject, DamageType.Generic);
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