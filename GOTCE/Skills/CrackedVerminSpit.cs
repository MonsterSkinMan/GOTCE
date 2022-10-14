using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Collections.Generic;
using EntityStates;
using GOTCE.EntityStatesCustom.CrackedPest;
using GOTCE.Skills;

namespace GOTCE.Skills {
    public class CrackedVerminSpit : SkillBase<CrackedVerminSpit> {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(CrackedVerminSpitState));
        public override string NameToken => "GOTCE_CrackedVerminSpit_NAME";
        public override string DescToken => "GOTCE_CrackedVerminSpit_DESC";
        public override string ActivationStateMachineName => "Weapon";
        public override int BaseMaxStock => 1;
        public override float BaseRechargeInterval => 1f;
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