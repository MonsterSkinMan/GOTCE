using BepInEx.Configuration;
using GOTCE.Components;
using GOTCE.Items.Lunar;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace GOTCE.Items.White
{
    public class TurbulentDefibrillator : ItemBase<TurbulentDefibrillator>
    {
        public override string ConfigName => "Turbulent Defibrillator";

        public override string ItemName => "Turbulent Defibrillator";

        public override string ItemLangTokenName => "GOTCE_TurbulentDefibrillator";

        public override string ItemPickupDesc => "Gain a small chance to cheat death. Increase attack speed upon dying.";

        public override string ItemFullDescription => "Gain an <style=cIsHealing>8%</style> <style=cStack>(+8% per stack)</style> chance to <style=cIsHealing>revive</style>. Each death increases your <style=cIsDamage>attack speed</style> by <style=cIsDamage>40%</style> <style=cStack>(+30% per stack)</style>.";

        public override string ItemLore => "To: k.ray@*******\nFrom: j.bar@******\nSubject: Research Concerns\n\nDoctor Ray,\nAfter reviewing your proposed developments myself and with the administration I am afraid to inform you that we cannot put your suggestions into place.After reading this letter, I am concerned that you may need to review your studies from medical school. Your proposed device has striking similarities to our standard defibrillators and the changes in its output are worryingly specific and unconnected to real research. I hope you remember that it is not the job of our healthcare center to resurrect people whose hearts have stopped, and you certainly would not be able to do it with a defibrillator. Furthermore, the proposed increases in electrical output, while they are spaced out at arbitrary intervals, would still be enough to apply a lethal shock to most patients, so they should not be applied to our current defibrillators. The attached video you sent us is not convincing evidence that this device works, and is a worrying sign of medical malpractice on your part. I have also requested an investigation into your research, which will be starting next week.\n\nTake care,\n[REDACTED]";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.Damage, ItemTag.AIBlacklist };
        // ai blacklist cause too much effort to make it work for mobs for now lol
        // hook CharacterMaster.IsDeadAndOutOfLivesServer for that

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/TurbulentDefibrillator.png");

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
            On.RoR2.CharacterMaster.OnBodyDeath += CharacterMaster_OnBodyDeath;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;

            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && args.Stats.master && args.Stats.master.inventory)
                {
                    if (GetCount(args.Stats.master) > 0)
                    {
                        args.Stats.reviveChanceAdd += 8f * GetCount(args.Stats.body);
                    }
                }
            };
        }

        private void CharacterMaster_OnBodyDeath(On.RoR2.CharacterMaster.orig_OnBodyDeath orig, CharacterMaster self, CharacterBody body)
        {
            orig(self, body);
            if (NetworkServer.active)
            {
                if (self.GetComponent<GOTCE_StatsComponent>())
                {
                    var stats = self.GetComponent<GOTCE_StatsComponent>();
                    var stack = body.inventory.GetItemCount(Instance.ItemDef);
                    if (stack > 0 && Util.CheckRoll(stats.reviveChance))
                    {
                        self.preventGameOver = true;
                        stats.Invoke(nameof(stats.RespawnExtraLife), 1f);
                        stats.deathCount++;
                    }
                }
            }
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                var stats = sender.gameObject.GetComponent<GOTCE_StatsComponent>();
                if (stack > 0 && stats)
                {
                    args.baseAttackSpeedAdd += (0.4f + 0.3f * (stack - 1)) * stats.deathCount;
                }
            }
        }
    }
}