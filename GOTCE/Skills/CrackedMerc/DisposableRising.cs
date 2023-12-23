using UnityEngine;
using EntityStates;

namespace GOTCE.Skills
{
    public class DisposableRising : SkillBase<DisposableRising>
    {
        public override SerializableEntityStateType ActivationState => new(typeof(EntityStatesCustom.CrackedMerc.DisposableRising));

        // public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStates.Commando.CommandoWeapon.FireBarrage));
        public override string NameToken => "GOTCE_DisposableRising_NAME";

        public override string DescToken => "GOTCE_DisposableRising_DESC";
        public override string ActivationStateMachineName => "Weapon";
        public override int BaseMaxStock => 1;
        public override float BaseRechargeInterval => 2.5f;
        public override bool BeginSkillCooldownOnSkillEnd => true;
        public override bool CancledFromSprinting => false;
        public override bool CancelSprintingOnActivation => false;
        public override bool FullRestockOnAssign => true;
        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;
        public override bool IsCombatSkill => true;
        public override bool MustKeyPress => false;
        public override int RechargeStock => 1;
        public override int StockToConsume => 1;
        public override Sprite Icon => null;
    }
}