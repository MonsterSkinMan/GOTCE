using RoR2;
using R2API;
using UnityEngine;
using BepInEx.Configuration;
using HarmonyLib;

namespace GOTCE.Items
{
    public class FormlessSand : ItemBase<FormlessSand>
    {
        public override string ConfigName => "Formless Sand";

        public override string ItemName => "Formless Sand";

        public override string ItemLangTokenName => "GOTCE_FormlessSand";

        public override string ItemPickupDesc => "Double your health... <color=#FF7F7F>BUT halve your damage.</color> <style=cIsVoid>Corrupts all Shaped Glasses</style>.";

        public override string ItemFullDescription => "<style=cIsHealing>Increase maximum health by 100%</style> <style=cStack>(+100% per stack)</style>. <style=cIsHealing>Reduce base damage by 50%</style> <style=cStack>(+50% per stack)</style>. <style=cIsVoid>Corrupts all Shaped Glasses</style>.";

        public override string ItemLore => "Gaze upon it. Does order need to permeate all things in order to keep it in this world? Feel it in your hand, flowing like water in an ocean. Grasp it firmly, and it hardens to fit, feeling like a solid stone. Do you think chaos is innately evil? Disorder leads only to ruin? No. Order and law do not guarantee peace and good, just as the sun shining does not guarantee a warm day. You have much to learn from this world.";

        public override ItemTier Tier => ItemTier.VoidTier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

        public override GameObject ItemModel => null;

        public ItemDef itemDef;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/FormlessSand.png");

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
            RecalculateStatsAPI.GetStatCoefficients += new RecalculateStatsAPI.StatHookEventHandler(StatChanges);
            On.RoR2.Items.ContagiousItemManager.Init += WoolieDimension;
        }

        public static void StatChanges(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.inventory)
            {
                var stack = body.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.damageMultAdd -= (Mathf.Pow(2f, stack) - 1) / Mathf.Pow(2f, stack);
                    args.healthMultAdd += Mathf.Pow(2f, stack) - 1f;
                }
            }
        }

        private void WoolieDimension(On.RoR2.Items.ContagiousItemManager.orig_Init orig)
        {
            ItemDef.Pair transformation = new ItemDef.Pair()
            {
                itemDef1 = RoR2Content.Items.LunarDagger,
                itemDef2 = this.ItemDef
            };
            ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem] = ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem].AddToArray(transformation);
            orig();
        }
    }
}
