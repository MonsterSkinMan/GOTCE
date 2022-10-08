/* using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Collections.Generic;
using RoR2.Skills;
using EntityStates;

// WIP

namespace GOTCE.Enemies.Skills {
    public abstract class SkillBase<T> : SkillBase where T : SkillBase<T> {
        public static T Instance { get; private set; }

        public SkillBase()
        {
            if (Instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting ItemBase was instantiated twice");
            Instance = this as T;
        }
    } 

    public abstract class SkillBase {
        public abstract GameObject ownerPrefab { get; }
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
        public virtual InterruptPriority InterruptPriority { get; } = InterruptPriority.Any;
        public virtual bool IsCombatSkill { get; } = true;
        public virtual bool MustKeyPress { get; } = false;
        public virtual int RechargeStock { get; } = 1;
        

    }
} */ 