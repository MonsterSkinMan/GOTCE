using UnityEngine;
using EntityStates;
using GOTCE.EntityStatesCustom;

namespace GOTCE.Skills
{
    public class Kick : SkillBase<Kick>
    {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStatesCustom.The.Kick));

        // public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStates.Commando.CommandoWeapon.FireBarrage));
        public override string NameToken => "GOTCE_KICK_NAME";

        public override string DescToken => "GOTCE_KICK_DESC";
        public override string ActivationStateMachineName => "Weapon";
        public override int BaseMaxStock => 1;
        public override float BaseRechargeInterval => 0f;
        public override bool BeginSkillCooldownOnSkillEnd => true;
        public override bool CancledFromSprinting => false;
        public override bool CancelSprintingOnActivation => false;
        public override bool FullRestockOnAssign => true;
        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;
        public override bool IsCombatSkill => true;
        public override bool MustKeyPress => false;
        public override int RechargeStock => 1;
        public override int StockToConsume => 0;
        public override Sprite Icon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/NEA.png");
    }
}