using BepInEx.Configuration;
using UnityEngine;

namespace GOTCE.Items.White
{
    public class BoneArmour : ItemBase<BoneArmour>
    {
        public override string ConfigName => "Bone Armour";

        public override string ItemName => "Bone Armour";

        public override string ItemLangTokenName => "GOTCE_BoneArmour";

        public override string ItemPickupDesc => "You left me but your smile will always be with me.";

        public override string ItemFullDescription => "Increase <style=cIsHealing>armour</style> by <style=cIsHealing>1</style> <style=cStack>(+1 per stack)</style>. Reduce <style=cIsHealing>maximum health</style> by <style=cIsHealing>1%</style> <style=cStack>(+1% per stack)</style>.";

        public override string ItemLore => "Turn the page and smell the pages (like you always do) that I’m writing with precious care, hoping your lips truly read a reason to stay\r\n\r\nAt night awake, wonder if it’s something you’d let go. At night awake, do you fight for those who you’d never let go?\r\n\r\nIf the path I walk is beside you, I don’t care if there are highs and lows as long as you’re there and show me you care with tonic affliction. Tonic affliction";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Healing, GOTCETags.Bullshit, GOTCETags.NonLunarLunar };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/Balls.png");

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
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender)
            {
                var inventory = sender.inventory;
                if (inventory)
                {
                    var stack = inventory.GetItemCount(Instance.ItemDef);
                    if (stack > 0)
                    {
                        args.armorAdd += stack;
                        args.healthMultAdd += stack * 0.01f;
                    }
                }
            }
        }
    }
}