using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using RoR2.Skills;
using GOTCE;
using UnityEngine.Scripting;
using System.Linq;
using RoR2.CharacterAI;

namespace GOTCE.Enemies
{
    public class IonSurger
    {
        public static GameObject IonSurgerObj;
        public static GameObject IonSurgerMaster;
        public static CharacterDirection direction;
        public static GameObject bodyModel;

        public static void Create()
        {
            IonSurgerObj = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Beetle/BeetleBody.prefab").WaitForCompletion(), "IonSurger");
            IonSurgerMaster = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Beetle/BeetleMaster.prefab").WaitForCompletion(), "IonSurgerMaster");
            // bodyModel = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/frozenwall/HumanCrate2Mesh.prefab").WaitForCompletion();
            CharacterBody body = IonSurgerObj.GetComponent<CharacterBody>();
            body.baseMaxHealth = 110f;
            body.levelMaxHealth = 33f;

            body.baseDamage = 10f;
            body.levelDamage = 4.5f;

            body.baseMoveSpeed = 8f;

            body.baseNameToken = "ION_SURGER_NAME";
            body.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;

            body.name = "IonSurgerBody";

            CharacterModel model = IonSurgerObj.GetComponentInChildren<CharacterModel>();
            // Animator anim = IonSurgerObj.GetComponentInChildren<Animator>();

            // Main.ModLogger.LogDebug(model.name);
            /* model.baseRendererInfos[0].defaultMaterial = Main.MainAssets.LoadAsset<Material>("Assets/Materials/Enemies/kirnMaterial.mat");
            model.baseRendererInfos[1].defaultMaterial = Main.MainAssets.LoadAsset<Material>("Assets/Materials/Enemies/kirnMaterial.mat");
            // Main.ModLogger.LogDebug(model.mainSkinnedMeshRenderer.name);\
            model.baseRendererInfos[1].renderer.gameObject.SetActive(false);
            MeshFilter mesh = GameObject.CreatePrimitive(PrimitiveType.Capsule).GetComponent<MeshFilter>();
            mesh.transform.localScale += new Vector3(3, 3, 3);
            model.baseRendererInfos[0].renderer.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = mesh.sharedMesh; */
            

            SkillLocator locator = IonSurgerObj.GetComponent<SkillLocator>();
            SkillFamily family = ScriptableObject.CreateInstance<SkillFamily>();
            ((ScriptableObject)family).name = "the";
            // family.variants = new SkillFamily.Variant[1];
            locator.primary._skillFamily = family;
            locator.primary._skillFamily.variants = new SkillFamily.Variant[1];

            SkillDef ion = Skills.IonSurgerRigid.Instance.SkillDef;

            locator.primary._skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = ion
            };

            locator.secondary._skillFamily = family;
            locator.secondary._skillFamily.variants = new SkillFamily.Variant[1];

            locator.secondary._skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = ion
            };
            



            CharacterMaster master = IonSurgerMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = IonSurgerObj;
            master.name = "IonSurgerMaster";

            

            LanguageAPI.Add("ION_SURGER_NAME", "Living Suppressive Fire");
            LanguageAPI.Add("ION_SURGER_LORE", "Even if frags did 2000% with no falloff...");
            LanguageAPI.Add("ION_SURGER_SUBTITLE", "Overwhelmed with Consistency");

            ContentAddition.AddBody(IonSurgerObj);
            ContentAddition.AddMaster(IonSurgerMaster);
        }
    }
}