using UnityEngine;
using EntityStates;
using GOTCE.Enemies.EntityStatesCustom;

namespace GOTCE.Skills
{
    public class NaeNaeUtil : SkillBase<NaeNaeUtil>
    {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStatesCustom.NaeNaeLord.NaeNaeUtil));

        // public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStates.Commando.CommandoWeapon.FireBarrage));
        public override string NameToken => "GOTCE_NaeNaeUtil_NAME";

        public override string DescToken => "GOTCE_NaeNaeUtil_DESC";
        public override string ActivationStateMachineName => "NaeNae";
        public override int BaseMaxStock => 1;
        public override float BaseRechargeInterval => 20f;
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