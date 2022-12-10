using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Buffs
{
    public class ViendNoArmor : BuffBase<ViendNoArmor>
    {
        public override Sprite BuffIcon => null;
        public override bool CanStack => true;
        public override Color Color => Color.red;
        public override string BuffName => "The Only Thing They Fear Armor Decay";
        public override bool IsDebuff => true;
        public override bool Hidden => false;

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += (CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args) =>
            {
                if (NetworkServer.active)
                {
                    if (body.HasBuff(BuffDef))
                    {
                        args.armorAdd -= 6.25f * body.GetBuffCount(BuffDef);
                    }
                }
            };
        }
    }
}