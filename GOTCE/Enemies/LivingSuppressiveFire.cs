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
    public class LivingSuppressiveFire
    {
        public static GameObject LivingSuppressiveFireObj;
        public static GameObject LivingSuppressiveFireMaster;
        public static CharacterDirection direction;
        public static GameObject bodyModel;

        public static void Create()
        {
            LivingSuppressiveFireObj = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Wisp/WispBody.prefab").WaitForCompletion(), "LivingSuppressiveFire");
            LivingSuppressiveFireMaster = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Wisp/WispMaster.prefab").WaitForCompletion(), "LivingSuppressiveFireMaster");
            bodyModel = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/frozenwall/HumanCrate2Mesh.prefab").WaitForCompletion();
            CharacterBody body = LivingSuppressiveFireObj.GetComponent<CharacterBody>();
            body.baseMaxHealth = 110f;
            body.levelMaxHealth = 33f;

            body.baseDamage = 5.5f;
            body.levelDamage = 1.1f;

            body.baseMoveSpeed = 16f;

            body.baseNameToken = "LIVING_SUPPRESSIVE_FIRE_NAME";
            body.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;

            body.name = "LivingSuppressiveFireBody";

            CharacterModel model = LivingSuppressiveFireObj.GetComponentInChildren<CharacterModel>();
            // Animator anim = LivingSuppressiveFireObj.GetComponentInChildren<Animator>();

            // Main.ModLogger.LogDebug(model.name);
            model.baseRendererInfos[0].defaultMaterial = Main.MainAssets.LoadAsset<Material>("Assets/Materials/Enemies/kirnMaterial.mat");
            model.baseRendererInfos[1].defaultMaterial = Main.MainAssets.LoadAsset<Material>("Assets/Materials/Enemies/kirnMaterial.mat");
            // Main.ModLogger.LogDebug(model.mainSkinnedMeshRenderer.name);\
            model.baseRendererInfos[1].renderer.gameObject.SetActive(false);
            MeshFilter mesh = GameObject.CreatePrimitive(PrimitiveType.Capsule).GetComponent<MeshFilter>();
            mesh.transform.localScale += new Vector3(3, 3, 3);
            model.baseRendererInfos[0].renderer.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = mesh.sharedMesh;
            

            SkillLocator locator = LivingSuppressiveFireObj.GetComponent<SkillLocator>();
            SkillFamily family = ScriptableObject.CreateInstance<SkillFamily>();
            ((ScriptableObject)family).name = "the";
            // family.variants = new SkillFamily.Variant[1];
            locator.primary._skillFamily = family;
            locator.primary._skillFamily.variants = new SkillFamily.Variant[1];

            SkillDef supp = Skills.Consistency.Instance.SkillDef;

            locator.primary._skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = supp
            };

            CharacterMaster master = LivingSuppressiveFireMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = LivingSuppressiveFireObj;
            master.name = "LivingSuppressiveFireMaster";

            AISkillDriver FleeAndAttack = (from x in master.GetComponents<AISkillDriver>()
                                           where x.maxDistance == 20
                                           select x).First();
            FleeAndAttack.maxDistance = 13f;

            AISkillDriver StrafeAndAttack = (from x in master.GetComponents<AISkillDriver>()
                                             where x.maxDistance == 30
                                             select x).First();
            StrafeAndAttack.maxDistance = 55f;

            AISkillDriver What = (from x in master.GetComponents<AISkillDriver>()
                                  where x.maxDistance == 40
                                  select x).First();
            What.maxDistance = 55f;

            LanguageAPI.Add("LIVING_SUPPRESSIVE_FIRE_NAME", "Living Suppressive Fire");
            LanguageAPI.Add("LIVING_SUPPRESSIVE_FIRE_LORE", "Even if frags did 2000% with no falloff...");
            LanguageAPI.Add("LIVING_SUPPRESSIVE_FIRE_SUBTITLE", "Overwhelmed with Consistency");

            ContentAddition.AddBody(LivingSuppressiveFireObj);
            ContentAddition.AddMaster(LivingSuppressiveFireMaster);
        }
    }
}