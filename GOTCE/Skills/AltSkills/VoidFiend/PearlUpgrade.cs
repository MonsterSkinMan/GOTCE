using UnityEngine;
using EntityStates;
using GOTCE.EntityStatesCustom;

namespace GOTCE.Skills
{
    public class PearlUpgrade : SkillBase<PearlUpgrade>
    {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStatesCustom.AltSkills.VoidFiend.PearlUpgrade));

        // public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStates.Commando.CommandoWeapon.FireBarrage));
        public override string NameToken => "GOTCE_PEARLUPGRADE_NAME";

        public override string DescToken => "GOTCE_PEARLUPGRADE_DESC";
        public override string ActivationStateMachineName => "Weapon";
        public override int BaseMaxStock => 1;
        public override float BaseRechargeInterval => 5f;
        public override bool BeginSkillCooldownOnSkillEnd => true;
        public override bool CancledFromSprinting => false;
        public override bool CancelSprintingOnActivation => false;
        public override bool FullRestockOnAssign => true;
        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;
        public override bool IsCombatSkill => false;
        public override bool MustKeyPress => true;
        public override int RechargeStock => 1;
        public override int StockToConsume => 1;
        // public override string[] KeywordTokens => new string[] {"GOTCE_CORRUPTIONSPECIALUPGRADE_KEYWORD"};
        public override Sprite Icon => null;
    }
}