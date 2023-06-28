using RoR2;
using UnityEngine;

namespace GOTCE.Buffs
{
    public class Frenching : BuffBase<Frenching>
    {
        public override Sprite BuffIcon => null;
        public override bool CanStack => false;
        public override Color Color => Color.yellow;
        public override string BuffName => "Frenching";
        public override bool IsDebuff => false;
        public override bool Hidden => true;

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += (CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args) =>
            {
                if (NetworkServer.active)
                {
                    if (body.HasBuff(BuffDef))
                    {
                        args.moveSpeedMultAdd += 0.7f;
                    }
                }
            };
        }
    }
}
