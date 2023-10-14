using RoR2;
using R2API;
using UnityEngine;
using BepInEx.Configuration;
using System.Collections;

namespace GOTCE.Items.Green
{
    public class OPMercBuild : ItemBase<OPMercBuild>
    {
        public override string ConfigName => "OP Merc Build";

        public override string ItemName => "OP Merc Build";

        public override string ItemLangTokenName => "GOTCE_OPMercBuild";

        public override string ItemPickupDesc => "Gain a miniscule amount of invincibilty each second.";

        public override string ItemFullDescription => "Gain <style=cIsHealing>1</style> <style=cStack>(+1 per stack)</style> <style=cIsHealing>invincibility frames</style> each second.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, GOTCETags.Bullshit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/MissileGuidanceSystem.png");

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
            On.RoR2.Inventory.RemoveItem_ItemIndex_int += Inventory_RemoveItem_ItemIndex_int;
        }

        private void Inventory_RemoveItem_ItemIndex_int(On.RoR2.Inventory.orig_RemoveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            if (itemIndex == Instance.ItemDef.itemIndex)
            {
                var master = self.GetComponent<CharacterMaster>();
                if (master)
                {
                    var body = master.GetBody();
                    if (body)
                    {
                        var stack = GetCount(body);
                        if (body.GetComponent<OPMercBuildController>() != null && count >= stack)
                        {
                            GameObject.Destroy(body.GetComponent<OPMercBuildController>());
                        }
                    }
                }
            }
        }

        private void Inventory_GiveItem_ItemIndex_int(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            if (itemIndex == Instance.ItemDef.itemIndex)
            {
                var master = self.GetComponent<CharacterMaster>();
                if (master)
                {
                    var body = master.GetBody();
                    if (body)
                    {
                        var stack = GetCount(body);
                        if (body.GetComponent<OPMercBuildController>() == null)
                        {
                            body.gameObject.AddComponent<OPMercBuildController>();
                            body.GetComponent<OPMercBuildController>().iframesDuration = 0.01f * stack;
                        }
                        else
                        {
                            body.GetComponent<OPMercBuildController>().iframesDuration = 0.01f * stack;
                        }
                    }
                }
            }
        }
    }

    public class OPMercBuildController : MonoBehaviour
    {
        public CharacterBody body;
        public float timer;
        public float interval = 1f;
        public float iframesDuration = 0f;

        public void Start()
        {
            body = GetComponent<CharacterBody>();
        }

        public void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;
            if (timer >= interval)
            {
                body.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, iframesDuration);
                timer = 0f;
            }
        }
    }
}