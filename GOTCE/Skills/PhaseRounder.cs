using UnityEngine;
using EntityStates;
using GOTCE.EntityStatesCustom.CrackedMando;

namespace GOTCE.Skills
{
    public class PhaseRounder : SkillBase<PhaseRounder>
    {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStatesCustom.CrackedMando.PhaseRounder));

        // public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStates.Commando.CommandoWeapon.FireBarrage));
        public override string NameToken => "GOTCE_PHASEROUNDER_NAME";

        public override string DescToken => "GOTCE_PHASEROUNDER_DESC";
        public override string ActivationStateMachineName => "Weapon";
        public override int BaseMaxStock => 1;
        public override float BaseRechargeInterval => 4f;
        public override bool BeginSkillCooldownOnSkillEnd => true;
        public override bool CancledFromSprinting => false;
        public override bool CancelSprintingOnActivation => true;
        public override bool FullRestockOnAssign => true;
        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;
        public override bool IsCombatSkill => true;
        public override bool MustKeyPress => false;
        public override int RechargeStock => 1;
        public override int StockToConsume => 1;
        public override Sprite Icon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Prefabs/Survivors/Crackmando/SkillFamilies/rounder.png");
    }
}