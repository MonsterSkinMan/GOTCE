using UnityEngine;
using EntityStates;
using GOTCE.Enemies.EntityStatesCustom;

namespace GOTCE.Skills
{
    public class NaeNaeSec : SkillBase<NaeNaeSec>
    {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStatesCustom.NaeNaeLord.NaeNaeBuff));

        // public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStates.Commando.CommandoWeapon.FireBarrage));
        public override string NameToken => "GOTCE_NaeNaeSec_NAME";

        public override string DescToken => "GOTCE_NaeNaeSec_DESC";
        public override string ActivationStateMachineName => "NaeNae";
        public override int BaseMaxStock => 1;
        public override float BaseRechargeInterval => 10f;
        public override bool BeginSkillCooldownOnSkillEnd => true;
        public override bool CancledFromSprinting => false;
        public override bool CancelSprintingOnActivation => true;
        public override bool FullRestockOnAssign => true;
        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;
        public override bool IsCombatSkill => true;
        public override bool MustKeyPress => false;
        public override int RechargeStock => 1;
        public override int StockToConsume => 1;
        public override Sprite Icon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/NEA.png");
    }
}