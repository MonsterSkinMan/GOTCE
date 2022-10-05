using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GOTCE.Items
{
    public class MonstersVisage : ItemBase<MonstersVisage>
    {
        public static GameObject HideAndShriek;

        public override string ConfigName => "Monsters Visage";

        public override string ItemName => "Monster's Visage";

        public override string ItemLangTokenName => "GOTCE_LunarNearbyDamageBonus";

        public override string ItemPickupDesc => "Deal double damage to nearby enemies... <color=#FF7F7F>BUT deal halved damage to faraway enemies.</color>\n";

        public override string ItemFullDescription => "Increase damage to enemies within <style=cIsDamage>10m</style> by <style=cIsDamage>100%</style> <style=cStack>(+100% per stack)</style>. Reduce damage to enemies not within <style=cIsDamage>10m</style> by <style=cIsDamage>50%</style> <style=cStack>(+50% per stack)</style>.";

        public override string ItemLore => "You have to push through in life. No matter what happens, you can’t give up. You can’t let yourself be defined by your own weaknesses and shortcomings.  It is okay to have flaws; perfection is overrated anyways. However, you can’t let yourself be consumed by your flaws. Instead, achieve greatness in spite of them. Time may be limited, but that’s all the more reason to make the most of it that you can. Never lose focus of your ultimate goal. The true meaning of life is to create your own destiny.";

        public override ItemTier Tier => ItemTier.Lunar;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/shittyvisage.png");

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }
        public void MonsterSkinMan()
        {
            var MonstersPissage = Addressables.LoadAssetAsync<GameObject>("Prefabs/NetworkedObjects/NearbyDamageBonusIndicator").WaitForCompletion();
            HideAndShriek = PrefabAPI.InstantiateClone(MonstersPissage, "ADHDEffect", true);
            Transform transform = HideAndShriek.transform.Find("Radius, Spherical");
            transform.localScale = Vector3.one * 10f * 2f;
        }

        public override void Hooks()
        {
            On.RoR2.HealthComponent.TakeDamage += On_HCTakeDamage;
        }

        private void On_HCTakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo)
        {
            if (damageInfo.attacker && damageInfo.attacker.GetComponent<CharacterBody>() && damageInfo.attacker.GetComponent<CharacterBody>().inventory)
            {
                var inv = damageInfo.attacker.GetComponent<CharacterBody>().inventory;

                Vector3 vector = Vector3.zero;
                if (damageInfo.attacker)
                {
                    CharacterBody characterBody = damageInfo.attacker.GetComponent<CharacterBody>();
                    if (characterBody)
                    {
                        vector = characterBody.corePosition - damageInfo.position;
                    }
                }

                if (inv)
                {
                    int monstersVisageCount = inv.GetItemCount(ItemDef);
                    if (monstersVisageCount > 0)
                    {
                        if (vector.sqrMagnitude <= 100f)
                        {
                            damageInfo.damage *= 2f + 1f * (monstersVisageCount - 1);
                        }
                        else
                        {
                            damageInfo.damage *= Mathf.Pow(0.5f, monstersVisageCount);
                        }
                    }
                }

                if (damageInfo == null || damageInfo.rejected || !damageInfo.attacker || damageInfo.attacker == self.gameObject || damageInfo.attacker.GetComponent<HealthComponent>() == null || damageInfo.attacker.GetComponent<HealthComponent>().body == null)
                {
                    orig(self, damageInfo);
                    return;
                }

                float num = damageInfo.damage;
            }
            orig(self, damageInfo);
        }
    }
}