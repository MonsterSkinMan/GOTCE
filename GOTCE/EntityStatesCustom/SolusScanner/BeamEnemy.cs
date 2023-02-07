using System;

namespace GOTCE.EntityStatesCustom.SolusScanner {
    public class BeamEnemy : BeamBase {
        public override float damageCoefficient => 1f;
        public override TargetType targetType => TargetType.Enemy;
    }
}