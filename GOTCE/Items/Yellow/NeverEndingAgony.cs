using RoR2;
using R2API;
using UnityEngine;
using BepInEx.Configuration;
using System.Linq;
using UnityEngine.AddressableAssets;

namespace GOTCE.Items.Yellow
{
    public class NeverEndingAgony : ItemBase<NeverEndingAgony>
    {
        public override string ConfigName => "Never-Ending Agony";

        public override string ItemName => "Never-Ending Agony";

        public override string ItemLangTokenName => "GOTCE_NeverEndingAgony";

        public override string ItemPickupDesc => "Gup is not a funny meme shut the fuck up.";

        public override string ItemFullDescription => "<style=cIsHealth>Die</style>. (Same goes for your allies, fucker ! !)";

        public override string ItemLore => "Fuck you. Can you imagine how much I fucking HATE you right now? I don’t think I could express that using human language. You chose that stupid fucking orange ball- why? WHY?!? What does that stupid fucking glob have that I don’t? I hate you. I hate you. You piece of shit. Fuck you.";

        public override ItemTier Tier => ItemTier.Boss;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.Utility, ItemTag.Healing, ItemTag.AIBlacklist, GOTCETags.Bullshit, GOTCETags.NonLunarLunar };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/NEA.png");

        public ItemDef itemDef;

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
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
        }

        public new int GetCount(CharacterBody body)
        {
            if (!body || !body.inventory) { return 0; }

            return body.inventory.GetItemCount(ItemDef);
        }

        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);
            var inventoryCount = GetCount(self);
            if (inventoryCount > 0 && self.master && self.inventory)
            {
                foreach (var bodies in CharacterBody.instancesList.Where(x => x.teamComponent.teamIndex == TeamIndex.Player))
                {
                    var gup = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Gup/GupBody.prefab").WaitForCompletion();
                    bodies.master.TrueKill(gup.gameObject, gup.gameObject, DamageType.Generic);
                }
            }
        }
    }
}
