using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Collections.Generic;
using EntityStates;
using GOTCE;
using GOTCE.EntityStatesCustom.Glassthrix;

namespace GOTCE.Skills.Glassthrix
{
    public class GlassthrixDash1 : SkillBase<GlassthrixDash1>
    {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStates.BrotherMonster.SlideIntroState));
        public override string NameToken => "GOTCE_GlassthrixDash1_NAME";
        public override string DescToken => "GOTCE_GlassthrixDash1_DESC";
        public override string ActivationStateMachineName => "Weapon";
        public override int BaseMaxStock => 1;
        public override float BaseRechargeInterval => 0.5f;
        public override bool BeginSkillCooldownOnSkillEnd => true;
        public override bool CancledFromSprinting => false;
        public override bool CancelSprintingOnActivation => false;
        public override bool FullRestockOnAssign => true;
        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;
        public override bool IsCombatSkill => true;
        public override bool MustKeyPress => true;
        public override int RechargeStock => 1;
        public override int StockToConsume => 1;
        public override Sprite Icon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/NEA.png");
    }
}