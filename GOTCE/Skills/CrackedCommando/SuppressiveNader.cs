using UnityEngine;
using EntityStates;
using GOTCE.EntityStatesCustom;

namespace GOTCE.Skills
{
    public class SuppressiveNader : SkillBase<SuppressiveNader>
    {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStatesCustom.CrackedMando.SuppressiveNader));

        // public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStates.Commando.CommandoWeapon.FireBarrage));
        public override string NameToken => "GOTCE_NADER_NAME";

        public override string DescToken => "GOTCE_NADER_DESC";
        public override string ActivationStateMachineName => "Weapon";
        public override int BaseMaxStock => 2;
        public override float BaseRechargeInterval => 4f;
        public override bool BeginSkillCooldownOnSkillEnd => false;
        public override bool CancledFromSprinting => false;
        public override bool CancelSprintingOnActivation => false;
        public override bool FullRestockOnAssign => true;
        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;
        public override bool IsCombatSkill => true;
        public override bool MustKeyPress => false;
        public override int RechargeStock => 1;
        public override int StockToConsume => 1;
        public override Sprite Icon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/nader.png");
    }
}