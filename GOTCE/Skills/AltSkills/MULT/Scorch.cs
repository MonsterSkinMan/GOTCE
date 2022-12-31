using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using EntityStates;
using GOTCE.EntityStatesCustom;

namespace GOTCE.Skills
{
    public class Scorch : SkillBase<Scorch>
    {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStatesCustom.AltSkills.MULT.Scorch));

        public override string NameToken => "GOTCE_SCORCH_NAME";

        public override string DescToken => "GOTCE_SCORCH_DESC";
        public override string ActivationStateMachineName => "Scorch";
        public override int BaseMaxStock => 1;
        public override float BaseRechargeInterval => 8f;
        public override bool BeginSkillCooldownOnSkillEnd => true;
        public override bool CancledFromSprinting => false;
        public override bool CancelSprintingOnActivation => false;
        public override bool FullRestockOnAssign => true;
        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;
        public override bool IsCombatSkill => true;
        public override bool MustKeyPress => false;
        public override int RechargeStock => 1;
        public override int StockToConsume => 1;
        public override Sprite Icon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Skills/scorch.png");
        public override string[] KeywordTokens => new string[] {"GOTCE_INCINERATE_KEYWORD"};
    }
}
