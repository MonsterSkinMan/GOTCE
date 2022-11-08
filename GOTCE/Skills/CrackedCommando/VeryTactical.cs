using UnityEngine;
using EntityStates;
using GOTCE.EntityStatesCustom.CrackedMando;

namespace GOTCE.Skills
{
    public class VeryTactical : SkillBase<VeryTactical>
    {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStatesCustom.CrackedMando.VeryTactical));

        // public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStates.Commando.CommandoWeapon.FireBarrage));
        public override string NameToken => "GOTCE_VERYTACTICAL_NAME";

        public override string DescToken => "GOTCE_VERYTACTICAL_DESC";
        public override string ActivationStateMachineName => "Flight";
        public override int BaseMaxStock => 1;
        public override float BaseRechargeInterval => 5f;
        public override bool BeginSkillCooldownOnSkillEnd => true;
        public override bool CancledFromSprinting => false;
        public override bool CancelSprintingOnActivation => false;
        public override bool FullRestockOnAssign => true;
        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;
        public override bool IsCombatSkill => false;
        public override bool MustKeyPress => false;
        public override int RechargeStock => 1;
        public override int StockToConsume => 1;
        public override Sprite Icon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Prefabs/Survivors/Crackmando/SkillFamilies/slide.png");
    }
}