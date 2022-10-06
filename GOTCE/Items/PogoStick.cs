using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items
{
    public class PogoStick : ItemBase<PogoStick>
    {
        public override string ConfigName => "Pogo Stick";

        public override string ItemName => "Pogo Stick";

        public override string ItemLangTokenName => "GOTCE_JumpBoost";

        public override string ItemPickupDesc => "Increase jump height.";

        public override string ItemFullDescription => "Increases <style=cIsUtility>jump height</style> by <style=cIsUtility>30%</style> <style=cStack>(+30% per stack)</style>.";

        public override string ItemLore => "\"Against all the evil that plants can conjure, all the wickedness that Wall-Nuts can produce, we will send unto them... only you. Boing and sproing, until it is done.\"\n-Dr. Zomboss in 4-8";

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/pogo.png");

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
            RecalculateStatsAPI.GetStatCoefficients += new RecalculateStatsAPI.StatHookEventHandler(PogoStick.SproingSproing);
        }

        public static void SproingSproing(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.inventory)
            {
                var stack = body.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.jumpPowerMultAdd += 0.3f * stack;
                }
            }
        }
    }
}
