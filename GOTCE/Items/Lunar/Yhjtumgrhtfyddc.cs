using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using KinematicCharacterController;

namespace GOTCE.Items.Lunar
{
    public class Yhjtumgrhtfyddc : ItemBase<Yhjtumgrhtfyddc>
    {

        public override string ConfigName => "Yhjtumgrhtfyddc";

        public override string ItemName => "Yhjtumgrhtfyddc";

        public override string ItemLangTokenName => "GOTCE_Yhjtumgrhtfyddc";

        public override string ItemPickupDesc => "Gain every buff... <color=#FF7F7F>BUT gain every debuff.</color>\n";

        public override string ItemFullDescription => "Gain <style=cIsUtility>1</style> of every <style=cIsUtility>buff</style>. Gain <style=cIsHealth>1</style> of every <style=cIsHealth>debuff</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Lunar;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/Yhjtumgrhtfyddc.png");

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
            On.RoR2.Inventory.GiveItem_ItemIndex_int += Inventory_GiveItem_ItemIndex_int;
        }

        private void Inventory_GiveItem_ItemIndex_int(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            if (NetworkServer.active && itemIndex == Instance.ItemDef.itemIndex)
            {
                foreach (BuffDef buffDef in RoR2.ContentManagement.ContentManager._buffDefs)
                {
                    var body = self.gameObject.GetComponent<CharacterMaster>().GetBody();
                    body.AddBuff(buffDef);
                }
            }
        }
    }
}