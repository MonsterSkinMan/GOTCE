using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Buffs
{
    public class Scorched : BuffBase<Scorched>
    {
        public override Sprite BuffIcon => null;
        public override bool CanStack => false;
        public override Color Color => Color.red;
        public override string BuffName => "Incinerate";
        public override bool IsDebuff => true;
        public override bool Hidden => true;

        public override void Hooks()
        {
            On.RoR2.HealthComponent.TakeDamage += Damaged;
            On.RoR2.GlobalEventManager.OnCharacterDeath += Death;
            RecalculateStatsAPI.GetStatCoefficients += Recalc;
        }

        private void Damaged(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo info) {
            orig(self, info);
            if (NetworkServer.active && self.body.HasBuff(BuffDef)) {
                HealthComponent hc;
                if (info.attacker && (hc = info.attacker.GetComponent<HealthComponent>()) != null) {
                    hc.AddBarrier(info.damage * 0.2f);
                }
            }

            if (NetworkServer.active && info.HasModdedDamageType(DamageTypes.Scorched)) {
                self.body.AddTimedBuff(BuffDef, 3f);
            }
        }

        private void Death(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport report) {
            orig(self, report);
            if (!report.attackerBody || !report.victimBody) {
                return;
            }
            CharacterBody ab = report.attackerBody;
            CharacterBody vb = report.victimBody;
            DamageInfo info = report.damageInfo;
            if (NetworkServer.active && vb.HasBuff(BuffDef)) {
                HealthComponent hc;
                if ((hc = ab.healthComponent) != null) {
                    hc.AddBarrier(hc.fullCombinedHealth * 0.25f);
                }
            }
        }

        private void Recalc(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args) {
            if (NetworkServer.active) {
                if (body.HasBuff(BuffDef)) {
                    BurnEffectController controller = body.GetComponent<BurnEffectController>();
                    if (!controller) {
                        controller = body.gameObject.AddComponent<BurnEffectController>();
                        controller.effectType = BurnEffectController.normalEffect;
                        controller.target = body.modelLocator.modelTransform.gameObject;
                    }
                }
                else {
                    bool hasOtherFlame = body.HasBuff(RoR2Content.Buffs.OnFire) || body.HasBuff(DLC1Content.Buffs.StrongerBurn);
                    if (!hasOtherFlame) {
                        BurnEffectController controller = body.GetComponent<BurnEffectController>();
                        if (controller) {
                            GameObject.Destroy(controller);
                        }
                    }
                }
            }
        }
    }
}