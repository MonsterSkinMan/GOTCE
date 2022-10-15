using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using UnityEngine.Networking;

namespace GOTCE.Items.Green
{
    public class DecorativeDrill : ItemBase<DecorativeDrill>
    {
        public override string ConfigName => "Decorative Drill";

        public override string ItemName => "Decorative Drill";

        public override string ItemLangTokenName => "GOTCE_DecorativeDrill";

        public override string ItemPickupDesc => "'Critical Strikes' grant a temporary barrier.";

        public override string ItemFullDescription => "Gain <style=cIsDamage>5% critical chance</style>. Gain a <style=cIsHealing>temporary barrier</style> on '<style=cIsDamage>Critical Strike</style>' for <style=cIsHealing>30</style> <style=cStack>(+15 per stack)</style> health.";

        public override string ItemLore => "\"...That’s a drill. Why do you have that in your home?\"\n\"Why not? Any day now, there might be some sort of hostile rock monster that busts down my door and tries to kill me.\"\n\"Is that even a real drill? Mining drills are really expensive. I’ll go get a rock.\"\n\"It’ll work, I’m telling you.\"\nzzzzzzzzzzzzzz..zzz..zz...\n\"...That did absolutely nothing.\"\n\"But it might not do nothing! It could still be helpful!\"\n\"I think you just got a fake drill. Who sold you this?\"\n\"It’s gonna help!\"";

        public override ItemTier Tier => ItemTier.Tier2;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Healing, ItemTag.Utility };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/drill.png");

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
            RecalculateStatsAPI.GetStatCoefficients += new RecalculateStatsAPI.StatHookEventHandler(CritIncrease);
            On.RoR2.GlobalEventManager.OnCrit += GlobalEventManager_OnCrit;
        }

        private void GlobalEventManager_OnCrit(On.RoR2.GlobalEventManager.orig_OnCrit orig, GlobalEventManager self, CharacterBody body, DamageInfo damageInfo, CharacterMaster master, float procCoefficient, ProcChainMask procChainMask)
        {
            if (body && procCoefficient > 0f && master && master.inventory)
            {
                int itemCount = master.inventory.GetItemCount(Instance.ItemDef);
                if (itemCount > 0 && body.healthComponent)
                {
                    if (NetworkServer.active)
                    {
                        body.healthComponent.AddBarrier((15f + 15f * itemCount) * procCoefficient);
                    }
                }
            }
        }

        public static void CritIncrease(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.inventory)
            {
                var stack = body.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.critAdd += 5f;
                }
            }
        }
    }
}