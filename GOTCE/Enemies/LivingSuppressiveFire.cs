using BepInEx;
using R2API;
using RoR2;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using RoR2.Skills;

namespace GOTCE.Enemies {
    public class LivingSuppressiveFire {
        public static GameObject LivingSuppressiveFireObj;
        public static GameObject LivingSuppressiveFireMaster;

        public static void Create() {
            LivingSuppressiveFireObj = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Wisp/WispBody.prefab").WaitForCompletion(), "LivingSuppressiveFire");
            LivingSuppressiveFireMaster = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Wisp/WispMaster.prefab").WaitForCompletion(), "LivingSuppressiveFireMaster");
            // R2API.ContentAddition.AddBody(LivingSuppressiveFireObj);
            CharacterBody component = LivingSuppressiveFireObj.GetComponent<CharacterBody>();
            component.baseDamage = 1f;
            component.baseCrit = 0f;
            component.levelCrit = 0f;
            component.baseMaxHealth = 400f;
            component.levelMaxHealth = 200f;
            component.baseArmor = -36f;
            component.baseRegen = 0f;
            component.levelRegen = 0f;
            component.baseMoveSpeed = 27f;
            component.levelMoveSpeed = 0f;
            component.levelDamage = 0;
            component.baseAttackSpeed = 1f;
            // component.name = "LivingSuppressiveFire";
            component.baseNameToken = "LIVING_SUPPRESSIVE_FIRE_NAME";
            component.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            // UnityEngine.Object.Destroy(component.master);
            // component.gameObject.AddComponent<CharacterMaster>(LivingSuppressiveFireMaster);

            // SkillLocator sl = LivingSuppressiveFireObj.GetComponent<SkillLocator>();
            // RoR2.Skills.SkillDef supp = Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>("RoR2/Base/Commando/CommandoBodyBarrage.asset").WaitForCompletion();
            // sl.primary.defaultSkillDef = supp;

            /*Array.Resize(ref sl.primary.skillFamily.variants, sl.primary.skillFamily.variants.Length + 1);
            sl.primary.skillFamily.variants[sl.primary.skillFamily.variants.Length - 1] = new RoR2.Skills.SkillFamily.Variant
            {
                skillDef = ionsurge,
            }; */
            /* sl.primary.skillFamily.variants[0] = new RoR2.Skills.SkillFamily.Variant 
            {
                skillDef = supp
            }; */
            
            
            /* foreach (GenericSkill obj in LivingSuppressiveFireObj.GetComponentsInChildren<GenericSkill>())
            {
                UnityEngine.Object.DestroyImmediate(obj);
            } */

            SkillLocator locator = LivingSuppressiveFireObj.GetComponent<SkillLocator>();
            SkillFamily family = ScriptableObject.CreateInstance<SkillFamily>();
            ((ScriptableObject)family).name = "the";
            // family.variants = new SkillFamily.Variant[1];
            locator.primary._skillFamily = family;
            locator.primary._skillFamily.variants = new SkillFamily.Variant[1];

            RoR2.Skills.SkillDef supp = Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>("RoR2/Base/Commando/CommandoBodyBarrage.asset").WaitForCompletion();

            locator.primary._skillFamily.variants[0] = new RoR2.Skills.SkillFamily.Variant 
            {
                skillDef = supp
            }; 

            CharacterMaster master = LivingSuppressiveFireMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = LivingSuppressiveFireObj;
            
            LanguageAPI.Add("LIVING_SUPPRESSIVE_FIRE_NAME", "Living Suppressive Fire");
            LanguageAPI.Add("LIVING_SUPPRESSIVE_FIRE_LORE", "Even if frags did 2000% with no falloff...");
            LanguageAPI.Add("LIVING_SUPPRESSIVE_FIRE_SUBTITLE", "Overwhelmed with Consistency");
            


            R2API.ContentAddition.AddBody(LivingSuppressiveFireObj);
            R2API.ContentAddition.AddMaster(LivingSuppressiveFireMaster);
        }
    }
}