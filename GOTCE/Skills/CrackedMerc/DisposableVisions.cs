using UnityEngine;
using EntityStates;

namespace GOTCE.Skills
{
    public class DisposableVisions : SkillBase<DisposableVisions>
    {
        public override SerializableEntityStateType ActivationState => new(typeof(EntityStatesCustom.CrackedMerc.DisposableVisions));

        // public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStates.Commando.CommandoWeapon.FireBarrage));
        public override string NameToken => "GOTCE_DisposableVisions_NAME";

        public override string DescToken => "GOTCE_DisposableVisions_DESC";
        public override string ActivationStateMachineName => "Weapon";
        public override int BaseMaxStock => 12;
        public override float BaseRechargeInterval => 2f;
        public override bool BeginSkillCooldownOnSkillEnd => true;
        public override bool CancledFromSprinting => false;
        public override bool CancelSprintingOnActivation => false;
        public override bool FullRestockOnAssign => true;
        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;
        public override bool IsCombatSkill => true;
        public override bool MustKeyPress => false;
        public override int RechargeStock => 12;
        public override int StockToConsume => 1;
        public override Sprite Icon => Main.SecondaryAssets.LoadAsset<Sprite>("crercPrimary.png");
    }
}