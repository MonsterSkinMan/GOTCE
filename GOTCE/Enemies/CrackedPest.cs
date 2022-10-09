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
            CharacterBody body = CrackedPestObj.GetComponent<CharacterBody>();
            body.baseDamage = 12f;
            body.levelDamage = 2.4f;

            body.baseMaxHealth = 150f;
            body.levelMaxHealth = 30f;

            body.baseMoveSpeed = 35f;

            body.baseAttackSpeed = 1f;

            body.hasOneShotProtection = true;

            body.oneShotProtectionFraction = 0.5f;

            body.baseNameToken = "CRACKED_PEST_NAME";

            body.name = "CrackedPestBody";

            CharacterMaster master = CrackedPestMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = CrackedPestObj;
            master.name = "CrackedPestMaster";

            LanguageAPI.Add("CRACKED_PEST_NAME", "Cracked Pest");
            LanguageAPI.Add("CRACKED_PEST_SUBTITLE", "Emoji Crack");

            ContentAddition.AddBody(CrackedPestObj);
            ContentAddition.AddMaster(CrackedPestMaster);
        }
    }
}