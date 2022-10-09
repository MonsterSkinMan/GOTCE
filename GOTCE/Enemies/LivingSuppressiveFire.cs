using BepInEx;
using R2API;
using RoR2;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using RoR2.Skills;
using GOTCE;
using UnityEngine.Scripting;

namespace GOTCE.Enemies {
    public class LivingSuppressiveFire {
        public static GameObject LivingSuppressiveFireObj;
        public static GameObject LivingSuppressiveFireMaster;
        public static CharacterDirection direction;

        public static void Create() {
            LivingSuppressiveFireObj = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Wisp/WispBody.prefab").WaitForCompletion(), "LivingSuppressiveFire");
            LivingSuppressiveFireMaster = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Wisp/WispMaster.prefab").WaitForCompletion(), "LivingSuppressiveFireMaster");
            // R2API.ContentAddition.AddBody(LivingSuppressiveFireObj);
            CharacterBody component = LivingSuppressiveFireObj.GetComponent<CharacterBody>();
            component.baseDamage = 0.05f;
            component.baseCrit = 0f;
            component.levelCrit = 0f;
            component.baseMaxHealth = 200f;
            component.levelMaxHealth = 15f;
            component.baseArmor = 25f;
            component.baseRegen = 0f;
            component.levelRegen = 0f;
            component.baseMoveSpeed = 11f;
            component.levelMoveSpeed = 0f;
            component.levelDamage = 0;
            component.baseAttackSpeed = 2f;
            // component.name = "LivingSuppressiveFire";
            component.baseNameToken = "LIVING_SUPPRESSIVE_FIRE_NAME";
            component.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            component.visionDistance = 20000f;

            CharacterModel model = LivingSuppressiveFireObj.GetComponentInChildren<CharacterModel>();
            Animator anim = LivingSuppressiveFireObj.GetComponentInChildren<Animator>();
            
            // Main.ModLogger.LogDebug(model.name);
            model.baseRendererInfos[0].defaultMaterial = Main.MainAssets.LoadAsset<Material>("Assets/Materials/Enemies/kirnMaterial.mat");
            model.baseRendererInfos[1].defaultMaterial = Main.MainAssets.LoadAsset<Material>("Assets/Materials/Enemies/kirnMaterial.mat");
            // model.baseRendererInfos[0].renderer.GetComponent<SkinnedMeshRenderer>().sharedMesh = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/frozenwall/HumanCrate2Mesh.prefab").WaitForCompletion().GetComponentInChildren<Mesh>();
            model.baseRendererInfos[1].renderer.gameObject.SetActive(false);
            // Main.ModLogger.LogDebug(model.mainSkinnedMeshRenderer.name);

            SkillLocator locator = LivingSuppressiveFireObj.GetComponent<SkillLocator>();
            SkillFamily family = ScriptableObject.CreateInstance<SkillFamily>();
            ((ScriptableObject)family).name = "the";
            // family.variants = new SkillFamily.Variant[1];
            locator.primary._skillFamily = family;
            locator.primary._skillFamily.variants = new SkillFamily.Variant[1];

            // RoR2.Skills.SkillDef supp = Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>("RoR2/Base/Commando/CommandoBodyBarrage.asset").WaitForCompletion();
            //supp.baseMaxStock = 9000;
            //supp.stockToConsume = 0;

            RoR2.Skills.SkillDef supp = Skills.Consistency.Instance.SkillDef;

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