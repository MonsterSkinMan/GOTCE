using UnityEngine;
using RoR2.Orbs;

namespace GOTCE.Items.Green
{
    public class Lotus : ItemBase<Lotus>
    {
        public override string ItemName => "Lotus";

        public override string ConfigName => "Lotus";

        public override string ItemLangTokenName => "GOTCE_Lotus";

        public override string ItemPickupDesc => "Gain bonus max health upon getting hit.";

        public override string ItemFullDescription => "Upon getting hit, increase your <style=cIsHealing>maximum health</style> by <style=cIsHealing>1</style> <style=cStack>(+1 per stack)</style> permanently.";

        public override string ItemLore => "within destruction more like.. damn she is such a bad bitch though";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility };

        public override GameObject ItemModel => Main.MainAssets.LoadAsset<GameObject>("Assets/Models/Prefabs/Item/KirnTheItem/KirnTheItemNoOutline.prefab");

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/kirn.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            var lotussssssssy = sender.GetComponent<LotussyUgghhhhh>();
            if (sender && lotussssssssy)
            {
                var stack = GetCount(sender);
                args.baseHealthAdd += 1f * stack * lotussssssssy.hitCount;
            }
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            var stack = GetCount(self.body);
            if (stack > 0)
            {
                if (self.GetComponent<LotussyUgghhhhh>() == null)
                {
                    self.gameObject.AddComponent<LotussyUgghhhhh>();
                    self.GetComponent<LotussyUgghhhhh>().hitCount++;
                }
                else
                {
                    self.GetComponent<LotussyUgghhhhh>().hitCount++;
                }
            }
            orig(self, damageInfo);
        }
    }

    public class LotussyUgghhhhh : MonoBehaviour
    {
        public int hitCount = 0;
    }
}