using RoR2;
using UnityEngine;
using GOTCE.Components;

namespace GOTCE.Buffs {
    public class Magnetized : BuffBase<Magnetized> {
        public override string BuffName => "Magnetized";
        public override bool IsDebuff => true;
        public override Color Color => Color.blue;
        public override Sprite BuffIcon => null;

        public override void Hooks()
        {
            base.Hooks();
            RecalculateStatsAPI.GetStatCoefficients += (body, args) => {
                if (NetworkServer.active) {
                    bool hasBuff = body.HasBuff(BuffDef);
                    bool hasComponent = body.GetComponent<MagnetController>();
                    if (hasBuff && !hasComponent) body.gameObject.AddComponent<MagnetController>();
                    if (!hasBuff && hasComponent) GameObject.Destroy(body.GetComponent<MagnetController>());
                }
            };
        }
    }
}