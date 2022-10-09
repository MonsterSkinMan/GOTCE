using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GOTCE.Enemies
{
    public class CrackedPest
    {
        public static GameObject CrackedPestObj;
        public static GameObject CrackedPestMaster;

        public static void Create()
        {
            CrackedPestObj = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/FlyingVermin/FlyingVerminBody.prefab").WaitForCompletion(), "CrackedPest");
            CrackedPestMaster = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/FlyingVermin/FlyingVerminMaster.prefab").WaitForCompletion(), "CrackedPestMaster");
            // R2API.ContentAddition.AddBody(CrackedPestObj);
            CharacterBody component = CrackedPestObj.GetComponent<CharacterBody>();
            component.baseDamage = 50f;
            component.levelDamage = 15f;

            component.baseMaxHealth = 150f;
            component.levelMaxHealth = 30f;

            component.baseMoveSpeed = 20f;

            component.baseAttackSpeed = 1f;

            component.hasOneShotProtection = true;

            component.oneShotProtectionFraction = 0.5f;

            component.baseNameToken = "CRACKED_PEST_NAME";
            component.bodyFlags |= CharacterBody.BodyFlags.HasBackstabImmunity | CharacterBody.BodyFlags.ImmuneToGoo | CharacterBody.BodyFlags.ImmuneToExecutes | CharacterBody.BodyFlags.ResistantToAOE | CharacterBody.BodyFlags.HasBackstabPassive;

            CharacterMaster master = CrackedPestMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = CrackedPestObj;

            LanguageAPI.Add("CRACKED_PEST_NAME", "Cracked Pest");
            LanguageAPI.Add("CRACKED_PEST_SUBTITLE", "Emoji Crack");

            ContentAddition.AddBody(CrackedPestObj);
            ContentAddition.AddMaster(CrackedPestMaster);
        }
    }
}