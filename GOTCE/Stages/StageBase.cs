using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace GOTCE.Stages {
    public abstract class StageBase<T> : StageBase where T : StageBase<T>
    {
        public static T Instance { get; private set; }

        public StageBase()
        {
            if (Instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting ItemBase was instantiated twice");
            Instance = this as T;
        }
    }

    public abstract class StageBase {
        public abstract string LangTokenName { get; }
        public abstract SceneType SceneType { get; }
        public abstract string SceneDisplayName { get; }
        public abstract string SceneSubtitle { get; }
        public virtual GameObject DioramaPrefab { get; } = null;
        public abstract string SceneLore { get; }
        public abstract string SceneName { get; }
        public virtual bool BlockOrbitalSkills { get; } = false;
        public abstract SceneCollection DestinationGroup { get; }
        public virtual MusicTrackDef MainTrack { get; }
        public virtual MusicTrackDef BossTrack { get; }
        public virtual RoR2.ExpansionManagement.ExpansionDef RequiredExpansion { get;} = null;
        public virtual bool ShouldIncludeInLogbook { get; } = true;
        public virtual Texture PreviewTexture { get; } = Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/MoldySteak.png").texture;
        public SceneDef sceneDef;
        public virtual List<string> interactableCardStrings { get; } = null;
        public virtual List<string> monsterCardStrings { get; } = null;
        public virtual string dccsInteractableClone { get; } = null;
        public virtual string dccsMonsterClone { get; } = null;
        public virtual void CreateConfig(ConfigFile config) {

        }
        public void Create(ConfigFile config) {
            CreateConfig(config);

            sceneDef = ScriptableObject.CreateInstance<SceneDef>();
            sceneDef.cachedName = SceneName;
            sceneDef.baseSceneNameOverride = SceneName;
            sceneDef.blockOrbitalSkills = BlockOrbitalSkills;
            sceneDef.destinationsGroup = DestinationGroup;
            sceneDef.dioramaPrefab = DioramaPrefab;
            sceneDef.shouldIncludeInLogbook = ShouldIncludeInLogbook;
            sceneDef.previewTexture = PreviewTexture;

            string baseToken = $"GOTCE_STAGE_{LangTokenName}";
            string loreToken = baseToken + "_LORE";
            string nameToken = baseToken + "_NAME";
            string subtitleToken = baseToken + "_SUBTITLE";
            
            sceneDef.nameToken = nameToken;
            sceneDef.loreToken = loreToken;
            sceneDef.subtitleToken = subtitleToken;

            LanguageAPI.Add(nameToken, SceneDisplayName);
            LanguageAPI.Add(loreToken, SceneLore);
            LanguageAPI.Add(subtitleToken, SceneSubtitle);

            if (RequiredExpansion) {
                sceneDef.requiredExpansion = RequiredExpansion;
            }

            sceneDef.mainTrack = MainTrack;
            sceneDef.bossTrack = BossTrack;
            sceneDef.sceneType = SceneType;

            ContentAddition.AddSceneDef(sceneDef);

            Modify();
            Hooks();
        }

        public virtual void Modify() {

        }

        public virtual void Hooks() {
            On.RoR2.ClassicStageInfo.RebuildCards += (orig, self) => {
                if (SceneManager.GetActiveScene().name != SceneName) {
                    orig(self);
                    return;
                }
                ModifySceneInfo(self);
                if (dccsInteractableClone != null) {
                    self.interactableDccsPool = Addressables.LoadAssetAsync<DccsPool>(dccsInteractableClone).WaitForCompletion();
                }
                if (dccsMonsterClone != null) {
                    self.monsterDccsPool = Addressables.LoadAssetAsync<DccsPool>(dccsMonsterClone).WaitForCompletion();
                }
                if (monsterCardStrings != null || interactableCardStrings != null) {
                    DccsPool interactables = self.interactableDccsPool;
                    DirectorCardCategorySelection dccsIsc = interactables.poolCategories[0].alwaysIncluded[0].dccs;
                    DccsPool monsters = self.monsterDccsPool;
                    DirectorCardCategorySelection dccsCsc = monsters.poolCategories[0].alwaysIncluded[0].dccs;

                    foreach (string mCard in monsterCardStrings) {
                        DirectorCard card = new();
                        card.selectionWeight = 1;
                        card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
                        
                        CharacterSpawnCard csc = Addressables.LoadAssetAsync<CharacterSpawnCard>(mCard).WaitForCompletion();
                        card.spawnCard = csc;
                        dccsCsc.AddCard(0, card);
                    }

                    foreach (string iCard in interactableCardStrings) {
                        DirectorCard card = new();
                        card.selectionWeight = 1;
                        card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
                        
                        InteractableSpawnCard isc = Addressables.LoadAssetAsync<InteractableSpawnCard>(iCard).WaitForCompletion();
                        card.spawnCard = isc;
                        dccsIsc.AddCard(0, card);
                    }
                }
                orig(self);
            };

            On.RoR2.SceneDirector.Start += (orig, self) => {
                if (SceneManager.GetActiveScene().name == SceneName) {
                    self.teleporterSpawnCard = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/Base/Teleporters/iscTeleporter.asset").WaitForCompletion();
                    DirectorStart(self);
                }
                orig(self);
            };
        }

        public virtual void ModifySceneInfo(ClassicStageInfo info) {

        }

        public virtual void DirectorStart(SceneDirector self) {

        }
    }
}