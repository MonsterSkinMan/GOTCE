using BepInEx.Configuration;
using R2API;
using RoR2;
using System.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace GOTCE.Items.White
{
    public class GameDiscussion : ItemBase<GameDiscussion>
    {
        public override string ConfigName => "#gd1-nullified";

        public override string ItemName => "#gd1-nullified";

        public override string ItemLangTokenName => "GOTCE_GD1";

        public override string ItemPickupDesc => "'Critical Strikes' poison enemies.";

        public override string ItemFullDescription => "Gain <style=cIsDamage>+5% critical strike chance</style>. On '<style=cIsDamage>Critical Strike</style>', <style=cIsDamage>poison</style> your target for <style=cIsDamage>5</style> <style=cStack>(+3 per stack)</style> seconds.";

        public override string ItemLore => "<b>Me when I instantly kill God with one shot as Railgunner</b>\nAs he falls to the ground, the lifeless ethereal body makes only a faint sound. Railgunner reflects on these actions before turning around to see a man standing at the pearly gates holding a tome.\n\"About time we balanced the scales, don't you think old friend?\"\nHe opens his tome and from it spews a load of C# lines across the arena.\n\"N-no!\" The Railgunner cries, \"We sealed you away! You're supposed to be gone!\"\nThe man simply smiles and nods at the Railgunner.\n\"Yes. But my people needed me: needed me to end your reign of terror!\"\nIn a flash Railgunner's numbers are heavily nerfed, perhaps even halved, and her proc coeff reduced to 1.\nIn a final act of desperation, she charges her gun and blasts Ghor, and his body too falls to the ground lifeless. Even half the damage was enough to instantly kill him because she had bands.\n...and so she left, with the gd1ers forever living under the tyrannical boot of power creep.\n<b>(BAD END)</b>";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.Crit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/GD1Nullified.png");

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
            On.RoR2.GlobalEventManager.ServerDamageDealt += Poison;
            RecalculateStatsAPI.GetStatCoefficients += Crit;
        }

        public void Poison(On.RoR2.GlobalEventManager.orig_ServerDamageDealt orig, DamageReport report)
        {
            orig(report);
            if (report.attacker && report.attackerBody)
            {
                if (report.attackerBody.inventory)
                {
                    int count = report.attackerBody.inventory.GetItemCount(ItemDef);
                    float duration = 5f + (3f * (count - 1));
                    if (count > 0 && report.damageInfo.crit)
                    {
                        if (report.victim && report.victimBody)
                        {
                            report.victimBody.AddTimedBuff(RoR2Content.Buffs.Poisoned, duration); // ror2 is not a spreadsheet
                            DotController.InflictDot(report.victim.gameObject, report.attacker, DotController.DotIndex.Poison, duration);
                        }
                    }
                }
            }
        }

        public void Crit(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.inventory && body.inventory.GetItemCount(ItemDef) > 0)
            {
                args.critAdd += 5f;
            }
        }
    }
}