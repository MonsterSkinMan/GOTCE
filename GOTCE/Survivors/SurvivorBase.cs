using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using System;
using System.Collections.Generic;
using EntityStates;
using RoR2.ExpansionManagement;

// WIP

namespace GOTCE.Survivors
{
    public abstract class SurvivorBase<T> : SurvivorBase where T : SurvivorBase<T>
    {
        public static T Instance { get; private set; }

        public SurvivorBase()
        {
            if (Instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting SurvivorBase was instantiated twice");
            Instance = this as T;
        }
    }

    public abstract class SurvivorBase
    {
        public virtual string bodypath{ get; } = null;
        public GameObject prefab;
        public virtual ExpansionDef RequiredExpansionHolder { get; } = Main.SOTVExpansionDef;
        public virtual SurvivorDef def {get; } = null;

        public virtual void Create()
        {
            if (bodypath != null)
            {
                CreatePrefab();
                var req = prefab.AddComponent<ExpansionRequirementComponent>();
                req.requiredExpansion = RequiredExpansionHolder;
            }
            Modify();
            PostCreation();
        }

        public virtual void Modify()
        {
        }

        public virtual void PostCreation()
        {
            if (def) {
                ContentAddition.AddBody(prefab);
                ContentAddition.AddSurvivorDef(def);
            }
        }

        public void DestroyModelLeftovers(GameObject prefab)
        {
            GameObject.Destroy(prefab.GetComponentInChildren<ModelLocator>().modelBaseTransform.gameObject);
        }

        public virtual void CreatePrefab()
        {
            
            
            prefab = PrefabAPI.InstantiateClone(Main.SecondaryAssets.LoadAsset<GameObject>(bodypath), "survivorbody");
        }

        public void ReplaceSkill(GenericSkill slot, SkillDef replaceWith, string familyName = "temp")
        {
            SkillFamily family = ScriptableObject.CreateInstance<SkillFamily>();
            ((ScriptableObject)family).name = familyName;
            // family.variants = new SkillFamily.Variant[1];
            slot._skillFamily = family;
            slot._skillFamily.variants = new SkillFamily.Variant[1];

            slot._skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = replaceWith
            };
        }

        public void DisableSkins(GameObject prefab)
        {
            CharacterModel model = prefab.GetComponentInChildren<CharacterModel>();
            if (prefab.GetComponentInChildren<ModelSkinController>())
            {
                ModelSkinController controller = prefab.GetComponentInChildren<ModelSkinController>();
                controller.enabled = false;
            }
        }

        public void RelocateMeshTransform(GameObject prefab, Transform transform, bool parent = false)
        {
            CharacterModel model = prefab.GetComponentInChildren<CharacterModel>();
            model.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            if (parent)
            {
                model.transform.SetParent(transform);
            }
        }

        public void SwapMeshes(GameObject prefab, Mesh mesh, bool all = false, List<int> renders = null)
        {
            CharacterModel model = prefab.GetComponentInChildren<CharacterModel>();
            if (all)
            {
                foreach (CharacterModel.RendererInfo info in model.baseRendererInfos)
                {
                    if (info.renderer.GetComponentInChildren<SkinnedMeshRenderer>())
                    {
                        info.renderer.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = mesh;
                    }
                }
            }

            if (renders != null)
            {
                foreach (int i in renders)
                {
                    if (model.baseRendererInfos[i].renderer.GetComponentInChildren<SkinnedMeshRenderer>())
                    {
                        model.baseRendererInfos[i].renderer.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = mesh;
                    }
                }
            }

            if (all)
            {
                foreach (CharacterModel.RendererInfo info in model.baseRendererInfos)
                {
                    if (info.renderer.GetComponentInChildren<MeshRenderer>())
                    {
                        info.renderer.GetComponentInChildren<MeshFilter>().sharedMesh = mesh;
                    }
                }
            }

            if (renders != null)
            {
                foreach (int i in renders)
                {
                    if (model.baseRendererInfos[i].renderer.GetComponentInChildren<MeshRenderer>())
                    {
                        model.baseRendererInfos[i].renderer.GetComponentInChildren<MeshFilter>().sharedMesh = mesh;
                    }
                }
            }
        }

        public void DisableMeshes(GameObject prefab, List<int> renders)
        {
            CharacterModel model = prefab.GetComponentInChildren<CharacterModel>();
            foreach (int i in renders)
            {
                model.baseRendererInfos[i].renderer.gameObject.SetActive(false);
            }
        }

        public void SwapMaterials(GameObject prefab, Material mat, bool all = false, List<int> renders = null)
        {
            CharacterModel model = prefab.GetComponentInChildren<CharacterModel>();
            if (all)
            {
                for (int i = 0; i < model.baseRendererInfos.Length; i++)
                {
                    model.baseRendererInfos[i].defaultMaterial = mat;
                }
            }

            if (renders != null)
            {
                foreach (int i in renders)
                {
                    model.baseRendererInfos[i].defaultMaterial = mat;
                }
            }
        }

        public void SetupModel(GameObject prefab, GameObject model, float turnSpeed = 1200f)
        {
            DestroyModelLeftovers(prefab);
            foreach (HurtBoxGroup hurtboxes in prefab.GetComponentsInChildren<HurtBoxGroup>())
            {
                GameObject.Destroy(hurtboxes);
            }
            foreach (HurtBox hurtbox in prefab.GetComponentsInChildren<HurtBox>())
            {
                GameObject.Destroy(hurtbox);
            }

            GameObject modelbase = new("Model Base");
            modelbase.transform.SetParent(prefab.transform);
            modelbase.transform.SetPositionAndRotation(prefab.transform.position, prefab.transform.rotation);

            model.transform.SetParent(modelbase.transform);
            model.transform.SetPositionAndRotation(modelbase.transform.position, modelbase.transform.rotation);

            ModelLocator modelLocator = prefab.GetComponentInChildren<ModelLocator>();
            modelLocator.modelTransform = model.transform;
            modelLocator.modelBaseTransform = modelbase.transform;
            modelLocator.dontReleaseModelOnDeath = false;
            modelLocator.dontDetatchFromParent = false;
            modelLocator.noCorpse = true;
            modelLocator.preserveModel = false;
            modelLocator.autoUpdateModelTransform = true;

            CharacterDirection characterDirection = prefab.GetComponent<CharacterDirection>();
            if (characterDirection)
            {
                characterDirection.targetTransform = modelbase.transform;
                characterDirection.turnSpeed = turnSpeed;
            }

            CharacterDeathBehavior characterDeathBehavior = prefab.GetComponent<CharacterDeathBehavior>();
            characterDeathBehavior.deathStateMachine = prefab.GetComponent<EntityStateMachine>();
            characterDeathBehavior.deathState = new SerializableEntityStateType(typeof(GenericCharacterDeath));

            GameObject.Destroy(prefab.GetComponentInChildren<Animator>());

            model.AddComponent<HurtBoxGroup>();
        }

        public void SetupHurtbox(GameObject prefab, GameObject model, Collider collidier, short index, bool weakPoint = false, HurtBox.DamageModifier damageModifier = HurtBox.DamageModifier.Normal)
        {
            HurtBoxGroup hurtBoxGroup = model.GetComponent<HurtBoxGroup>();

            HurtBox componentInChildren = collidier.gameObject.AddComponent<HurtBox>();
            componentInChildren.gameObject.layer = LayerIndex.entityPrecise.intVal;
            componentInChildren.healthComponent = prefab.GetComponent<HealthComponent>();
            componentInChildren.isBullseye = weakPoint;
            componentInChildren.damageModifier = damageModifier;
            componentInChildren.hurtBoxGroup = hurtBoxGroup;
            componentInChildren.indexInGroup = index;
        }
    }
}