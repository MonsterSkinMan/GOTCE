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
    public class GlassthrixSlash1 : SkillBase<GlassthrixSlash1>
    {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStatesCustom.Glassthrix.P1.Slash));
        public override string NameToken => "GOTCE_GlassthrixSlash1_NAME";
        public override string DescToken => "GOTCE_GlassthrixSlash1_DESC";
        public override string ActivationStateMachineName => "Weapon";
        public override int BaseMaxStock => 5;
        public override float BaseRechargeInterval => 2f;
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