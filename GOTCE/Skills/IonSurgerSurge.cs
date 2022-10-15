/*
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

namespace GOTCE.Skills
{
    public class IonSurgerSurge : SkillBase<IonSurgerSurge>
    {
        // private EntityStateConfiguration state = Addressables.LoadAssetAsync<EntityStateConfiguration>("RoR2/Base/Mage/EntityStates.Mage.FlyUpState.asset").WaitForCompletion();

        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStates.Mage.FlyUpState));
        public override string NameToken => "GOTCE_IonSurgerSurge_NAME";
        public override string DescToken => "GOTCE_IonSurgerSurge_DESC";
        public override string ActivationStateMachineName => "Weapon";
        public override int BaseMaxStock => 1;
        public override float BaseRechargeInterval => 4f;
        public override bool BeginSkillCooldownOnSkillEnd => true;
        public override bool CancledFromSprinting => false;
        public override bool CancelSprintingOnActivation => true;
        public override bool FullRestockOnAssign => true;
        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;
        public override bool IsCombatSkill => true;
        public override bool MustKeyPress => true;
        public override int RechargeStock => 1;
        public override int StockToConsume => 1;
        public override Sprite Icon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/NEA.png");
    }
}
*/