using System;

namespace GOTCE.EntityStatesCustom.SolusScanner {
    public class BeamFriendly : BeamBase {
        public override TargetType targetType => TargetType.Friendly;
    }
}