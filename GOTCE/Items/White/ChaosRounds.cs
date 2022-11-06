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

        public override string ItemPickupDesc => "Chance to fire a random projectile when attacking.";

        public override string ItemFullDescription => "Gain a <style=cIsDamage>10%</style> <style=cStack>(+10% per stack)</style> chance to fire a <style=cIsUtility>random projectile</style> for <style=cIsDamage>120% base damage</style> on skill use.";

        public override string ItemLore => "<color=#e64b13>That D. Furthen idiot is dead now, I suppose. I guess it's time I started introducing some more chaos. How about... this item?</color>";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.Unstable };

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

        public void Chaos(On.RoR2.CharacterBody.orig_OnSkillActivated orig, CharacterBody self, GenericSkill skill)
        {
            orig(self, skill);
            if (self.hasAuthority)
            {
                if (self.inventory)
                {
                    Inventory inv = self.inventory;
                    float chance = 10f * inv.GetItemCount(ItemDef);
                    if (Util.CheckRoll(chance, self.master))
                    {
                        List<GameObject> prefabs = ProjectileCatalog.projectilePrefabs.ToList();
                        for (int i = 0; i < prefabs.Count; i++)
                        {
                            if (prefabs[i].name == EngiMineName || prefabs[i].name == SpiderMineName || prefabs[i].GetComponent<ProjectileSimple>() == null || prefabs[i].GetComponent<ProjectileCharacterController>() == null || prefabs[i].GetComponent<ProjectileController>() == null || prefabs[i].GetComponent<ProjectileDamage>() == null || prefabs[i].GetComponent<ProjectileDotZone>() == null || prefabs[i].GetComponent<ProjectileExplosion>() == null || prefabs[i].GetComponent<ProjectileImpactExplosion>() == null)
                            {
                                prefabs.RemoveAt(i);
                            }
                        }
                        GameObject prefab = prefabs[UnityEngine.Random.Range(0, prefabs.Count - 1)];

                        FireProjectileInfo info = default(FireProjectileInfo);
                        info.crit = Util.CheckRoll(self.crit, self.master);
                        info.damage = self.damage * 1.2f;
                        info.projectilePrefab = prefab;
                        info.procChainMask = default(ProcChainMask);
                        info.damageColorIndex = DamageColorIndex.Item;
                        info.position = self.corePosition;
                        info.rotation = Util.QuaternionSafeLookRotation(self.inputBank.aimDirection);
                        info.owner = self.gameObject;
                        ProjectileManager.instance.FireProjectile(info);

                    }
                }
            }
        }
    }
}