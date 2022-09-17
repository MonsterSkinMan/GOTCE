using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using static GOTCE.Main;

namespace GOTCE.Items
{
    public class ExampleItem : ItemBase<ExampleItem>
    {
        public override string ItemName => "Deprecate Me Item";

        public override string ItemLangTokenName => "DEPRECATE_ME_ITEM";

        public override string ItemPickupDesc => "";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier1;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("ExampleItemPrefab.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("ExampleItemIcon.png");

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateItem();
            Hooks();
        }

        public override void CreateConfig(ConfigFile config)
        {

        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        public override void Hooks()
        {

        }

    }
}
