using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Collections.Generic;
using EntityStates;
using GOTCE.Enemies.EntityStatesCustom;

namespace GOTCE.Enemies.Skills {
    public class IonSurgerRigid : SkillBase<IonSurgerRigid> {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(IonSurgerRigidState));
        public override string NameToken => "GOTCE_IonSurgerRigid_NAME";
        public override string DescToken => "GOTCE_IonSurgerRigid_DESC";
        public override string ActivationStateMachineName => "Weapon";
        public override int BaseMaxStock => 1;
        public override float BaseRechargeInterval => 8f;
        public override bool BeginSkillCooldownOnSkillEnd => true;
        public override bool CancledFromSprinting => false;
        public override bool CancelSprintingOnActivation => true;
        public override bool FullRestockOnAssign => true;
        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;
        public override bool IsCombatSkill => true;
        public override bool MustKeyPress => false;
        public override int RechargeStock => 1;
        public override int StockToConsume => 1;
        public override Sprite Icon => null;
        
    }
}