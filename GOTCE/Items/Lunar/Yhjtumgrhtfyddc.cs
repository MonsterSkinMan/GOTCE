using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using KinematicCharacterController;
using System.Linq;
using System.Collections.Generic;

namespace GOTCE.Items.Lunar
{
    public class Yhjtumgrhtfyddc : ItemBase<Yhjtumgrhtfyddc>
    {
        public override string ConfigName => "Yhjtumgrhtfyddc";

        public override string ItemName => "Yhjtumgrhtfyddc";

        public override string ItemLangTokenName => "GOTCE_Yhjtumgrhtfyddc";

        public override string ItemPickupDesc => "Gain every buff... <color=#FF7F7F>BUT gain every debuff.</color>\n";

        public override string ItemFullDescription => "Gain <style=cIsUtility>1</style> of every <style=cIsUtility>buff</style>. Gain <style=cIsHealth>1</style> of every <style=cIsHealth>debuff</style>.";

        public override string ItemLore => "Jimmy:\n\"Oweraugohuy ubyo uoyeriuhom i iumoh iuhodrsaiuw. Eriuogqeoii3h5hj,iub aejg? Pjisrb oinb rsui nbrnii. Fce uyw sagweie uihb! SDJDBNHBS BEUHWB BRYERBN R$HOIH%IJ! IUBHY! IUBHY! IUBHY! Erhkjdnbd brobo iojb oijre98h y374q h890.\"\nJommy:\n\"From my perspective on this topic and I think a lot of them are either of them or the fire one is not the same way that you are doing it for a while now and I think you are a good fit for the damage loss in the future to the fucking ground in a way they are very good and they are not the only ones that are not in a good condition or are there other things you can do to help them understand the problem.\"";

        public override ItemTier Tier => ItemTier.Lunar;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage, ItemTag.BrotherBlacklist, ItemTag.AIBlacklist };

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
            On.RoR2.Inventory.RemoveItem_ItemIndex_int += Inventory_RemoveItem_ItemIndex_int;
            RoR2.CharacterBody.onBodyStartGlobal += CharacterBody_onBodyStartGlobal;
            // TODO:

            // Figure out how to make the buffs persist per stage, or give them per stage
        }

        private void CharacterBody_onBodyStartGlobal(CharacterBody obj)
        {
            List<BuffDef> buffs = new()
                {
                    RoR2Content.Buffs.Immune, RoR2Content.Buffs.Intangible, RoR2Content.Buffs.Nullified, RoR2Content.Buffs.Entangle,
                    RoR2Content.Buffs.LunarSecondaryRoot, RoR2Content.Buffs.HiddenInvincibility, DLC1Content.Buffs.BearVoidReady, DLC1Content.Buffs.EliteVoid, RoR2Content.Buffs.LunarShell,
                    RoR2Content.Buffs.VoidFogMild, RoR2Content.Buffs.VoidFogStrong, DLC1Content.Buffs.ImmuneToDebuffReady
                };
            foreach (CharacterBody body in CharacterBody.readOnlyInstancesList)
            {
                if (body && body.inventory)
                {
                    var stack = body.inventory.GetItemCount(Instance.ItemDef);
                    if (stack > 0)
                    {
                        AddBuffs(buffs, body, false);
                    }
                }
            }
        }

        private void AddBuffs(List<BuffDef> blacklist, CharacterBody body, bool remove)
        {
            if (NetworkServer.active)
            {
                foreach (BuffDef buffDef in RoR2.ContentManagement.ContentManager._buffDefs)
                {
                    if (!blacklist.Contains(buffDef))
                    {
                        if (remove)
                        {
                            body.RemoveBuff(buffDef);
                        }
                        else
                        {
                            body.AddBuff(buffDef);
                        }
                    }
                }
            }
        }

        private void Inventory_RemoveItem_ItemIndex_int(On.RoR2.Inventory.orig_RemoveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            if (NetworkServer.active && itemIndex == Instance.ItemDef.itemIndex)
            {
                List<BuffDef> buffs = new()
                {
                    RoR2Content.Buffs.Immune, RoR2Content.Buffs.Intangible, RoR2Content.Buffs.Nullified, RoR2Content.Buffs.Entangle,
                    RoR2Content.Buffs.LunarSecondaryRoot, RoR2Content.Buffs.HiddenInvincibility, DLC1Content.Buffs.BearVoidReady, DLC1Content.Buffs.EliteVoid, RoR2Content.Buffs.LunarShell,
                    RoR2Content.Buffs.VoidFogMild, RoR2Content.Buffs.VoidFogStrong, DLC1Content.Buffs.ImmuneToDebuffReady
                };
                var body = self.gameObject.GetComponent<CharacterMaster>().GetBody();
                AddBuffs(buffs, body, true);
            }
        }

        private void Inventory_GiveItem_ItemIndex_int(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            if (NetworkServer.active && itemIndex == Instance.ItemDef.itemIndex)
            {
                List<BuffDef> buffs = new()
                {
                    RoR2Content.Buffs.Immune, RoR2Content.Buffs.Intangible, RoR2Content.Buffs.Nullified, RoR2Content.Buffs.Entangle,
                    RoR2Content.Buffs.LunarSecondaryRoot, RoR2Content.Buffs.HiddenInvincibility, DLC1Content.Buffs.BearVoidReady, DLC1Content.Buffs.EliteVoid, RoR2Content.Buffs.LunarShell,
                    RoR2Content.Buffs.VoidFogMild, RoR2Content.Buffs.VoidFogStrong, DLC1Content.Buffs.ImmuneToDebuffReady
                };

                var body = self.gameObject.GetComponent<CharacterMaster>().GetBody();
                AddBuffs(buffs, body, false);
            }
        }
    }
}