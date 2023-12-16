using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Buffs
{
    public class NoCrit : BuffBase<NoCrit>
    {
        public override Sprite BuffIcon => null;
        public override bool CanStack => false;
        public override Color Color => Color.red;
        public override string BuffName => "Crit Disabled";
        public override bool IsDebuff => true;
        public override bool Hidden => true;

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += (CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args) =>
            {
                if (NetworkServer.active)
                {
                    if (body.HasBuff(BuffDef))
                    {
                        args.critAdd -= 1000f;
                    }
                }
            };
        }
    }
}
