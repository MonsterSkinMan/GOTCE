using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Collections.Generic;
using EntityStates;
using UnityEngine.Networking;

// WIP

namespace GOTCE.Enemies {
    public abstract class EnemyBase<T> : EnemyBase where T : EnemyBase<T> {
        public static T Instance { get; private set; }

        public EnemyBase()
        {
            if (Instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting ItemBase was instantiated twice");
            Instance = this as T;
        }
    } 

    public abstract class EnemyBase {
        public DirectorCard card;
        public CharacterSpawnCard isc;
        public virtual string PathToClone { get; } = null;
        public virtual string CloneName { get; } = null;
        public virtual string PathToCloneMaster { get; } = null;
        public GameObject prefab;
        public GameObject prefabMaster;

        public virtual void Create() {
            if (PathToClone != null && CloneName != null && PathToCloneMaster != null) {
                CreatePrefab();
            }
            AddSpawnCard();
            AddDirectorCard();
            Modify();
        }

        public virtual void Modify() {

        }
        public virtual void AddSpawnCard() {
            isc = new CharacterSpawnCard();
        }

        public virtual void AddDirectorCard() {
            card = new DirectorCard();
            card.spawnCard = isc;
        }

        public void RegisterEnemy(GameObject bodyPrefab, GameObject masterPrefab, List<DirectorAPI.Stage> stages = null, DirectorAPI.MonsterCategory category = DirectorAPI.MonsterCategory.BasicMonsters, bool all = false) {
            ContentAddition.AddBody(bodyPrefab);
            ContentAddition.AddMaster(masterPrefab);

            if (stages != null) {
                foreach (DirectorAPI.Stage stage in stages) {
                    DirectorAPI.Helpers.AddNewMonsterToStage(card, category, stage);
                }
            }

            if (all) {
                DirectorAPI.Helpers.AddNewMonster(card, category);
            }

        }

        public void DestroyModelLeftovers(GameObject prefab) {
            GameObject.Destroy(prefab.transform.Find("ModelBase").gameObject);
            GameObject.Destroy(prefab.transform.Find("CameraPivot").gameObject);
            GameObject.Destroy(prefab.transform.Find("AimOrigin").gameObject);
        }

        public virtual void CreatePrefab() {
            prefab = PrefabAPI.InstantiateClone(UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>(PathToClone).WaitForCompletion(), CloneName + "Body");
            prefab.GetComponent<NetworkIdentity>().localPlayerAuthority = false;
            prefabMaster = PrefabAPI.InstantiateClone(UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>(PathToCloneMaster).WaitForCompletion(), CloneName + "Master");
        }

        public void ReplaceSkill(GenericSkill slot, SkillDef replaceWith, string familyName = "temp") {
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

    }
}