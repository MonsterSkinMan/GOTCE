using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using RoR2.Projectile;
using System.Linq;
using System;

namespace GOTCE.Items.White
{
    public class ChaosRounds : ItemBase<ChaosRounds>
    {
        public override string ConfigName => "Chaos Rounds";

        public override string ItemName => "Chaos Rounds";

        public override string ItemLangTokenName => "GOTCE_ChaosRounds";

        public override string ItemPickupDesc => "Chance to fire a <style=cIsUtility>random projectile</style> when attacking.";

        public override string ItemFullDescription => "Gain a <style=cIsDamage>10%</style> <style=cStack>(+10% per stack)</style> chance to fire a <style=cIsUtility>random projectile</style> on skill use.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/ChaosRounds.png");
        private string EngiMineName = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/EngiMine.prefab").WaitForCompletion().name;
        private string SpiderMineName = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/SpiderMine.prefab").WaitForCompletion().name;

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
            On.RoR2.CharacterBody.OnSkillActivated += Chaos;
        }

        public void Chaos(On.RoR2.CharacterBody.orig_OnSkillActivated orig, CharacterBody self, GenericSkill skill) {
            orig(self, skill);
            if (self.hasAuthority) {
                if (self.inventory) {
                    Inventory inv = self.inventory;
                    float chance = 10f*inv.GetItemCount(ItemDef);
                    if (Util.CheckRoll(chance, self.master)) {
                        List<GameObject> prefabs = ProjectileCatalog.projectilePrefabs.ToList();
                        for (int i = 0; i < prefabs.Count; i++) {
                            if (prefabs[i].name == EngiMineName || prefabs[i].name == SpiderMineName) {
                                prefabs.RemoveAt(i);
                            }
                            
                        }
                        GameObject prefab = prefabs[UnityEngine.Random.RandomRange(0, prefabs.Count - 1)];

                        FireProjectileInfo info = default(FireProjectileInfo);
                        info.crit = Util.CheckRoll(self.crit, self.master);
                        info.damage = self.damage * 1.2f;
                        info.projectilePrefab = prefab;
                        info.procChainMask = default(ProcChainMask);
                        info.damageColorIndex = DamageColorIndex.Item;
                        info.position = self.corePosition;
                        info.rotation = Util.QuaternionSafeLookRotation(self.equipmentSlot.GetAimRay().direction);
                        info.owner = self.gameObject;
                        ProjectileManager.instance.FireProjectileServer(info);
                    }
                }
            }
        }

    }
}