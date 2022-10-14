using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Collections.Generic;
using EntityStates;
using GOTCE.Skills;

// WIP

namespace GOTCE.Skills {
    public abstract class SkillBase<T> : SkillBase where T : SkillBase<T> {
        public static T Instance { get; private set; }

        public SkillBase()
        {
            if (Instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting ItemBase was instantiated twice");
            Instance = this as T;
        }
    } 

    public abstract class SkillBase {
        public abstract string NameToken { get; }
        public abstract string DescToken { get; }

        public abstract EntityStates.SerializableEntityStateType ActivationState { get;}
        public abstract string ActivationStateMachineName { get; }
        public virtual int BaseMaxStock { get; } = 1;
        public virtual float BaseRechargeInterval { get; } = 0f;
        public virtual bool BeginSkillCooldownOnSkillEnd { get; } = true;
        public virtual bool CancledFromSprinting { get; } = false;
        public virtual bool CancelSprintingOnActivation { get; } = true;
        public virtual bool FullRestockOnAssign { get; } = true;
        public virtual InterruptPriority SkillInterruptPriority { get; } = InterruptPriority.Any;
        public virtual bool IsCombatSkill { get; } = true;
        public virtual bool MustKeyPress { get; } = false;
        public virtual int RechargeStock { get; } = 1;
        public virtual Sprite Icon { get; } = null;
        public virtual int StockToConsume { get; } = 1;

        public SkillDef SkillDef;

        public virtual void Create() {
            SkillDef = ScriptableObject.CreateInstance<SkillDef>();
            SkillDef.activationState = ActivationState;
            SkillDef.activationStateMachineName = ActivationStateMachineName;
            SkillDef.baseMaxStock = BaseMaxStock;
            SkillDef.baseRechargeInterval = BaseRechargeInterval;
            SkillDef.beginSkillCooldownOnSkillEnd = BeginSkillCooldownOnSkillEnd;
            SkillDef.canceledFromSprinting = CancledFromSprinting;
            SkillDef.cancelSprintingOnActivation = CancelSprintingOnActivation;
            SkillDef.fullRestockOnAssign = FullRestockOnAssign;
            SkillDef.interruptPriority = SkillInterruptPriority;
            SkillDef.isCombatSkill = IsCombatSkill;
            SkillDef.mustKeyPress = MustKeyPress;
            SkillDef.rechargeStock = RechargeStock;
            SkillDef.icon = Icon;
            SkillDef.skillNameToken = NameToken;
            SkillDef.skillDescriptionToken = DescToken;
            SkillDef.stockToConsume = StockToConsume;

            ContentAddition.AddSkillDef(SkillDef);
        }

    }
} 