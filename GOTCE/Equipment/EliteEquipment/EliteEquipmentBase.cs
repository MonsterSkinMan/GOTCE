﻿using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static RoR2.CombatDirector;

namespace GOTCE.Equipment.EliteEquipment
{
    public abstract class EliteEquipmentBase<T> : EliteEquipmentBase where T : EliteEquipmentBase<T>
    {
        public static T Instance { get; private set; }

        public EliteEquipmentBase()
        {
            if (Instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting EliteEquipmentBase was instantiated twice");
            Instance = this as T;
        }
    }

    public abstract class EliteEquipmentBase
    {
        public abstract string EliteEquipmentName { get; }

        /// <summary>
        /// The lang token that will be used in registering most of your strings.
        /// <para>E.g.: AFFIX_HYPERCHARGED</para>
        /// </summary>
        public abstract string EliteAffixToken { get; }

        public abstract string EliteEquipmentPickupDesc { get; }
        public abstract string EliteEquipmentFullDescription { get; }
        public abstract string EliteEquipmentLore { get; }

        /// <summary>
        /// This is what appears before the name of the creature that has this elite status.
        /// <para>E.g.: "Hypercharged Beetle" where Hypercharged is the modifier.</para>
        /// </summary>
        public abstract string EliteModifier { get; }

        public virtual bool AppearsInSinglePlayer { get; } = true;

        public virtual bool AppearsInMultiPlayer { get; } = true;

        public virtual bool Hidden { get; } = true;

        public virtual bool CanDrop { get; } = false;

        public virtual float Cooldown { get; } = 60f;

        public abstract GameObject EliteEquipmentModel { get; }
        public abstract Sprite EliteEquipmentIcon { get; }

        public EquipmentDef EliteEquipmentDef;

        public BuffDef EliteBuffDef;

        /// <summary>
        /// Implement before calling CreateEliteEquipment.
        /// </summary>
        public abstract Sprite EliteBuffIcon { get; }

        public virtual Color EliteBuffColor { get; set; } = new Color32(255, 255, 255, byte.MaxValue);

        /// <summary>
        /// If not overriden, the elite can spawn in all tiers defined.
        /// </summary>
        public virtual EliteTierDef[] CanAppearInEliteTiers { get; set; } = EliteAPI.GetCombatDirectorEliteTiers();

        /// <summary>
        /// If you want the elite to have an overlay with your custom material.
        /// </summary>
        public abstract Texture2D EliteRampTexture { get; }

        public EliteDef EliteDef;

        /// <summary>
        /// How many more/less times the health should the elite have compared to its standard form?
        /// </summary>
        public virtual float HealthMultiplier { get; set; } = 1;

        /// <summary>
        /// How many more/less times the damage should the elite have compared to its standard form?
        /// </summary>
        public virtual float DamageMultiplier { get; set; } = 1;

        /// <summary>
        /// This method structures your code execution of this class. An example implementation inside of it would be:
        /// <para>CreateConfig(config);</para>
        /// <para>CreateLang();</para>
        /// <para>CreateBuff();</para>
        /// <para>CreateEquipment();</para>
        /// <para>CreateElite();</para>
        /// <para>Hooks();</para>
        /// <para>This ensures that these execute in this order, one after another, and is useful for having things available to be used in later methods.</para>
        /// <para>P.S. CreateItemDisplayRules(); does not have to be called in this, as it already gets called in CreateEquipment();</para>
        /// </summary>
        /// <param name="config">The config file that will be passed into this from the main class.</param>
        public abstract void Init(ConfigFile config);

        public abstract ItemDisplayRuleDict CreateItemDisplayRules();

        protected void CreateLang()
        {
            LanguageAPI.Add("ELITE_EQUIPMENT_" + EliteAffixToken + "_NAME", EliteEquipmentName);
            LanguageAPI.Add("ELITE_EQUIPMENT_" + EliteAffixToken + "_PICKUP", EliteEquipmentPickupDesc);
            LanguageAPI.Add("ELITE_EQUIPMENT_" + EliteAffixToken + "_DESCRIPTION", EliteEquipmentFullDescription);
            LanguageAPI.Add("ELITE_EQUIPMENT_" + EliteAffixToken + "_LORE", EliteEquipmentLore);
            LanguageAPI.Add("ELITE_" + EliteAffixToken + "_MODIFIER", EliteModifier + " {0}");
        }

        protected void CreateEquipment()
        {
            EliteBuffDef = ScriptableObject.CreateInstance<BuffDef>();
            EliteBuffDef.name = EliteAffixToken;
            EliteBuffDef.buffColor = EliteBuffColor;
            EliteBuffDef.canStack = false;
            EliteBuffDef.iconSprite = EliteBuffIcon;

            EliteEquipmentDef = ScriptableObject.CreateInstance<EquipmentDef>();
            EliteEquipmentDef.name = "ELITE_EQUIPMENT_" + EliteAffixToken;
            EliteEquipmentDef.nameToken = "ELITE_EQUIPMENT_" + EliteAffixToken + "_NAME";
            EliteEquipmentDef.pickupToken = "ELITE_EQUIPMENT_" + EliteAffixToken + "_PICKUP";
            EliteEquipmentDef.descriptionToken = "ELITE_EQUIPMENT_" + EliteAffixToken + "_DESCRIPTION";
            EliteEquipmentDef.loreToken = "ELITE_EQUIPMENT_" + EliteAffixToken + "_LORE";
            EliteEquipmentDef.pickupModelPrefab = EliteEquipmentModel;
            EliteEquipmentDef.pickupIconSprite = EliteEquipmentIcon;
            EliteEquipmentDef.appearsInSinglePlayer = AppearsInSinglePlayer;
            EliteEquipmentDef.appearsInMultiPlayer = AppearsInMultiPlayer;
            EliteEquipmentDef.canDrop = CanDrop;
            EliteEquipmentDef.cooldown = Cooldown;
            EliteEquipmentDef.passiveBuffDef = EliteBuffDef;

            ItemAPI.Add(new CustomEquipment(EliteEquipmentDef, CreateItemDisplayRules()));

            On.RoR2.EquipmentSlot.PerformEquipmentAction += PerformEquipmentAction;

            if (UseTargeting && TargetingIndicatorPrefabBase)
            {
                On.RoR2.EquipmentSlot.Update += UpdateTargeting;
            }
        }


        protected void CreateElite()
        {
            EliteDef = ScriptableObject.CreateInstance<EliteDef>();
            EliteDef.name = "ELITE_" + EliteAffixToken;
            EliteDef.modifierToken = "ELITE_" + EliteAffixToken + "_MODIFIER";
            EliteDef.eliteEquipmentDef = EliteEquipmentDef;
            EliteDef.healthBoostCoefficient = HealthMultiplier;
            EliteDef.damageBoostCoefficient = DamageMultiplier;
            EliteDef.shaderEliteRampIndex = 0;

            var baseEliteTierDefs = EliteAPI.GetCombatDirectorEliteTiers();
            if (!CanAppearInEliteTiers.All(x => baseEliteTierDefs.Contains(x)))
            {
                var distinctEliteTierDefs = CanAppearInEliteTiers.Except(baseEliteTierDefs);

                foreach (EliteTierDef eliteTierDef in distinctEliteTierDefs)
                {
                    var indexToInsertAt = Array.FindIndex(baseEliteTierDefs, x => x.costMultiplier >= eliteTierDef.costMultiplier);
                    if (indexToInsertAt >= 0)
                    {
                        EliteAPI.AddCustomEliteTier(eliteTierDef, indexToInsertAt);
                    }
                    else
                    {
                        EliteAPI.AddCustomEliteTier(eliteTierDef);
                    }
                    baseEliteTierDefs = EliteAPI.GetCombatDirectorEliteTiers();
                }
            }

            EliteAPI.Add(new CustomElite(EliteDef, CanAppearInEliteTiers, EliteRampTexture));

            EliteBuffDef.eliteDef = EliteDef;
            ContentAddition.AddBuffDef(EliteBuffDef);
        }

        protected bool PerformEquipmentAction(On.RoR2.EquipmentSlot.orig_PerformEquipmentAction orig, RoR2.EquipmentSlot self, EquipmentDef equipmentDef)
        {
            if (equipmentDef == EliteEquipmentDef)
            {
                return ActivateEquipment(self);
            }
            else
            {
                return orig(self, equipmentDef);
            }
        }

        /// <summary>
        /// Must be implemented, but you can just return false if you don't want an On Use effect for your elite equipment.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        protected abstract bool ActivateEquipment(EquipmentSlot slot);

        public abstract void Hooks();

        #region Targeting Setup

        //Targeting Support
        public virtual bool UseTargeting { get; } = false;

        public GameObject TargetingIndicatorPrefabBase = null;

        public enum TargetingType
        {
            Enemies,
            Friendlies,
        }

        public virtual TargetingType TargetingTypeEnum { get; } = TargetingType.Enemies;

        //Based on MysticItem's targeting code.
        protected void UpdateTargeting(On.RoR2.EquipmentSlot.orig_Update orig, EquipmentSlot self)
        {
            orig(self);

            if (self.equipmentIndex == EliteEquipmentDef.equipmentIndex)
            {
                var targetingComponent = self.GetComponent<TargetingControllerComponent>();
                if (!targetingComponent)
                {
                    targetingComponent = self.gameObject.AddComponent<TargetingControllerComponent>();
                    targetingComponent.VisualizerPrefab = TargetingIndicatorPrefabBase;
                }

                if (self.stock > 0)
                {
                    switch (TargetingTypeEnum)
                    {
                        case (TargetingType.Enemies):
                            targetingComponent.ConfigureTargetFinderForEnemies(self);
                            break;

                        case (TargetingType.Friendlies):
                            targetingComponent.ConfigureTargetFinderForFriendlies(self);
                            break;
                    }
                }
                else
                {
                    targetingComponent.Invalidate();
                    targetingComponent.Indicator.active = false;
                }
            }
        }

        public class TargetingControllerComponent : MonoBehaviour
        {
            public GameObject TargetObject;
            public GameObject VisualizerPrefab;
            public Indicator Indicator;
            public BullseyeSearch TargetFinder;
            public Action<BullseyeSearch> AdditionalBullseyeFunctionality = (search) => { };

            public void Awake()
            {
                Indicator = new Indicator(gameObject, null);
                Invalidate();
            }

            public void Invalidate()
            {
                TargetObject = null;
                Indicator.targetTransform = null;
            }

            public void ConfigureTargetFinderBase(EquipmentSlot self)
            {
                if (TargetFinder == null) TargetFinder = new BullseyeSearch();
                TargetFinder.teamMaskFilter = TeamMask.allButNeutral;
                TargetFinder.teamMaskFilter.RemoveTeam(self.characterBody.teamComponent.teamIndex);
                TargetFinder.sortMode = BullseyeSearch.SortMode.Angle;
                TargetFinder.filterByLoS = true;
                float num;
                Ray ray = CameraRigController.ModifyAimRayIfApplicable(self.GetAimRay(), self.gameObject, out num);
                TargetFinder.searchOrigin = ray.origin;
                TargetFinder.searchDirection = ray.direction;
                TargetFinder.maxAngleFilter = 10f;
                TargetFinder.viewer = self.characterBody;
            }

            public void ConfigureTargetFinderForEnemies(EquipmentSlot self)
            {
                ConfigureTargetFinderBase(self);
                TargetFinder.teamMaskFilter = TeamMask.GetUnprotectedTeams(self.characterBody.teamComponent.teamIndex);
                TargetFinder.RefreshCandidates();
                TargetFinder.FilterOutGameObject(self.gameObject);
                AdditionalBullseyeFunctionality(TargetFinder);
                PlaceTargetingIndicator(TargetFinder.GetResults());
            }

            public void ConfigureTargetFinderForFriendlies(EquipmentSlot self)
            {
                ConfigureTargetFinderBase(self);
                TargetFinder.teamMaskFilter = TeamMask.none;
                TargetFinder.teamMaskFilter.AddTeam(self.characterBody.teamComponent.teamIndex);
                TargetFinder.RefreshCandidates();
                TargetFinder.FilterOutGameObject(self.gameObject);
                AdditionalBullseyeFunctionality(TargetFinder);
                PlaceTargetingIndicator(TargetFinder.GetResults());
            }

            public void PlaceTargetingIndicator(IEnumerable<HurtBox> TargetFinderResults)
            {
                HurtBox hurtbox = TargetFinderResults.Any() ? TargetFinderResults.First() : null;

                if (hurtbox)
                {
                    TargetObject = hurtbox.healthComponent.gameObject;
                    Indicator.visualizerPrefab = VisualizerPrefab;
                    Indicator.targetTransform = hurtbox.transform;
                }
                else
                {
                    Invalidate();
                }
                Indicator.active = hurtbox;
            }
        }

        #endregion Targeting Setup
    }
}