using UnityEngine;
using EntityStates;
using GOTCE.EntityStatesCustom.CrackedMando;

namespace GOTCE.Skills
{
    public class Collapse : SkillBase<Collapse>
    {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStatesCustom.NemCore.Collapse));

        // public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStates.Commando.CommandoWeapon.FireBarrage));
        public override string NameToken => "GOTCE_NEMCORE_PRIMARY_NAME".Add("Collapse");

        public override string DescToken => "GOTCE_NEMCORE_PRIMARY_DESC".Add("Implode, dealing <style=cIsDamage>80 flat damage to nearby enemies</style> and <style=cDeath>dying</style>.");
        public override string ActivationStateMachineName => "Weapon";
        public override int BaseMaxStock => 1;
        public override float BaseRechargeInterval => 5f;
        public override bool BeginSkillCooldownOnSkillEnd => true;
        public override bool CancledFromSprinting => false;
        public override bool CancelSprintingOnActivation => false;
        public override bool FullRestockOnAssign => true;
        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;
        public override bool IsCombatSkill => true;
        public override bool MustKeyPress => false;
        public override int RechargeStock => 1;
        public override int StockToConsume => 0;
        public override Sprite Icon => null;
    }
}