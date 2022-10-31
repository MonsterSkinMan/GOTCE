using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Items.VoidBoss
{
    public class CorruptsAllShatterspleens : ItemBase<CorruptsAllShatterspleens>
    {
        public override string ConfigName => "Corrupts all Shatterspleens";

        public override string ItemName => "<style=cIsVoid>Corrupts all Shatterspleens.<style>";

        public override string ItemLangTokenName => "GOTCE_CorruptsAllShatterspleens";

        public override string ItemPickupDesc => "<style=cIsVoid>Corrupts all Shatterspleens.<style>";

        public override string ItemFullDescription => "Gain <style=cIsDamage>5% critical chance</style>. Critical strikes collapse enemies for 400% base damage. <style=cIsVoid>Corrupts all Shatterspleens.<style>";

        public override string ItemLore => "<style=cIsVoid>Corrupts all Shatterspleens.<style>";

        public override ItemTier Tier => ItemTier.VoidBoss;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

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
            RecalculateStatsAPI.GetStatCoefficients += Crit;
            //On.RoR2.GlobalEventManager.ServerDamageDealt += NEEDLETICK;
        }

        public void Crit(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.inventory && body.inventory.GetItemCount(ItemDef) > 0)
            {
                args.critAdd += 5f;
            }
        }
        /*public void NEEDLETICK(On.RoR2.GlobalEventManager.orig_ServerDamageDealt orig, DamageReport report)
        {
            orig(report);
            if (report.attacker && report.attackerBody)
            {
                if (report.attackerBody.inventory)
                {
                    int count = report.attackerBody.inventory.GetItemCount(ItemDef);
                    float duration = 5f + (3f * (count - 1));
                    if (count > 0 && report.damageInfo.crit) idk what to do for stacking and I also don't know how to handle the duration for needletick
                    {
                        if (report.victim && report.victimBody)
                        {
                            report.victimBody.AddTimedBuff(DLC1Content.Buffs.Fracture, duration);
                            DotController.InflictDot(report.victim.gameObject, report.attacker, DotController.DotIndex.Fracture, duration);
                        }
                    }
                }
            }
        }*/
    }
}
