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
    public class CrackedPest {
        public static GameObject CrackedPestObj;
        public static GameObject CrackedPestMaster;

        public static void Create() {
            CrackedPestObj = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/FlyingVermin/FlyingVerminBody.prefab").WaitForCompletion(), "CrackedPest");
            CrackedPestMaster = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/FlyingVermin/FlyingVerminMaster.prefab").WaitForCompletion(), "CrackedPestMaster");
            // R2API.ContentAddition.AddBody(CrackedPestObj);
            CharacterBody component = CrackedPestObj.GetComponent<CharacterBody>();
            component.baseDamage = 50f;
            component.baseCrit = 100f;
            component.levelCrit = 0f;
            component.baseMaxHealth = 500f;
            component.levelMaxHealth = 50f;
            component.baseArmor = 100f;
            component.baseRegen = 5f;
            component.levelRegen = 5f;
            component.baseMoveSpeed = 20f;
            component.levelMoveSpeed = 0f;
            component.levelDamage = 50;
            component.baseAttackSpeed = 50f;
            component.hasOneShotProtection = true;
            component.oneShotProtectionFraction = 0.1f;
            // component.name = "CrackedPest";
            component.baseNameToken = "CRACKED_PEST_NAME";
            component.bodyFlags |= CharacterBody.BodyFlags.HasBackstabImmunity | CharacterBody.BodyFlags.ImmuneToGoo | CharacterBody.BodyFlags.ImmuneToExecutes | CharacterBody.BodyFlags.ResistantToAOE | CharacterBody.BodyFlags.HasBackstabPassive;
            

            CharacterMaster master = CrackedPestMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = CrackedPestObj;
            
            LanguageAPI.Add("CRACKED_PEST_NAME", "Cracked Pest");
            LanguageAPI.Add("CRACKED_PEST_SUBTITLE", "cracked");
            


            R2API.ContentAddition.AddBody(CrackedPestObj);
            R2API.ContentAddition.AddMaster(CrackedPestMaster);
        }
    }
}