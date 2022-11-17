using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using EntityStates;
using GOTCE.EntityStatesCustom;

namespace GOTCE.Skills
{
    public class SuperShotgun : SkillBase<SuperShotgun>
    {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStatesCustom.AltSkills.Commando.SuperShotgun));

        public override string NameToken => "GOTCE_SUPERSHOTGUN_NAME";

        public override string DescToken => "GOTCE_SUPERSHOTGUN_DESC";
        public override string ActivationStateMachineName => "Weapon";
        public override int BaseMaxStock => 1;
        public override float BaseRechargeInterval => 0f;
        public override bool BeginSkillCooldownOnSkillEnd => true;
        public override bool CancledFromSprinting => false;
        public override bool CancelSprintingOnActivation => true;
        public override bool FullRestockOnAssign => true;
        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;
        public override bool IsCombatSkill => true;
        public override bool MustKeyPress => false;
        public override int RechargeStock => 1;
        public override int StockToConsume => 0;
        public override Sprite Icon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/NEA.png");
    }
}
