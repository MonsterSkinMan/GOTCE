using BepInEx.Configuration;
using GOTCE.Components;
using GOTCE.Items.Lunar;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace GOTCE.Items.White
{
    public class BentCrutch : ItemBase<BentCrutch>
    {
        public override string ConfigName => "Bent Crutch";

        public override string ItemName => "Bent Crutch";

        public override string ItemLangTokenName => "GOTCE_BentCrutch";

        public override string ItemPickupDesc => "Gain a small chance to cheat death. Increase all stats upon dying.";

        public override string ItemFullDescription => "Gain an <style=cIsHealing>5%</style> <style=cStack>(+5% per stack)</style> chance to <style=cIsHealing>revive</style>. Each death increases your <style=cIsDamage>stats</style> by <style=cIsDamage>10%</style> <style=cStack>(+10% per stack)</style>.";

        public override string ItemLore => "Kael used to be involved with some sort of gang. They were just mischief-makers, using some spray paint to draw on walls and causing as much noise as possible. One day Kael made a grave mistake, where he was shot for drawing on military property. As he was rushed to the hospital, the doctors cut him a deal that he couldn't even know of. No papers, no one to sign on his behalf, the doctors just did it. They saved his life, but he ended up with his leg muscles missing. He was trapped to use crutches for the rest of his life.\n\nMedical care became an experiment after the war. Doctors had no leashes to hold them back and they could do what they wanted.In the end, it did save people's lives... sometimes. How could a person not be grateful that they had their lives saved? Sure, they could lose everything they have, but at least they were alive!\n\nWhen Kael returned to his old group, they laughed at him, calling him a liability and a slow-poke now. When he wasn't looking, one kid bashed his crutch, causing it to be permanently bent. He didn't have anyone anymore.\n\nAnd after it all, Kael wondered if he even wanted his life saved in the end.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.Healing, GOTCETags.Crit, GOTCETags.FovRelated, ItemTag.OnStageBeginEffect };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/BentCrutch.png");

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
                    args.armorAdd += sender.armor * ((0.1f + 0.1f * (stack - 1)) * stats.deathCount);
                    args.attackSpeedMultAdd += (0.1f + 0.1f * (stack - 1)) * stats.deathCount;
                    args.moveSpeedMultAdd += (0.1f + 0.1f * (stack - 1)) * stats.deathCount;
                    args.damageMultAdd += (0.1f + 0.1f * (stack - 1)) * stats.deathCount;
                    args.moveSpeedMultAdd += (0.1f + 0.1f * (stack - 1)) * stats.deathCount;
                    args.cooldownMultAdd -= (0.1f + 0.1f * (stack - 1)) * stats.deathCount;
                    args.baseJumpPowerAdd += (0.1f + 0.1f * (stack - 1)) * stats.deathCount;
                    args.baseHealthAdd += (0.1f + 0.1f * (stack - 1)) * stats.deathCount;
                    args.shieldMultAdd += (0.1f + 0.1f * (stack - 1)) * stats.deathCount;
                    args.baseRegenAdd += sender.regen * ((0.1f + 0.1f * (stack - 1)) * stats.deathCount);
                    args.critDamageMultAdd += (0.1f + 0.1f * (stack - 1)) * stats.deathCount;
                    args.levelMultAdd += (0.1f + 0.1f * (stack - 1)) * stats.deathCount;
                }
            }
        }
    }
}
