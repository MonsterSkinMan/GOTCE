using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Buffs {
    public class ValveBalance : BuffBase<ValveBalance> {
        public override Sprite BuffIcon => null;
        public override bool CanStack => false;
        public override Color Color => Color.red;
        public override string BuffName => "Entangled";
        public override bool IsDebuff => false;
        public override bool Hidden => true;

        public override void Hooks() {
            RecalculateStatsAPI.GetStatCoefficients += (CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args) => {
                if (NetworkServer.active) {
                    if (body.HasBuff(BuffDef)) {
                        args.attackSpeedMultAdd += 1f;
                        args.armorAdd += 200;
                        args.moveSpeedMultAdd += 0.3f;
                    }
                }
            };
        }
    }
}