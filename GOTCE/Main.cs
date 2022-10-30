using BepInEx;
using GOTCE.Artifact;
using GOTCE.Enemies.Changes;

//using GOTCE.Enemies.Normal_Enemies;
using GOTCE.Equipment;
using GOTCE.Equipment.EliteEquipment;
using GOTCE.Items;
using R2API;
using R2API.Networking;
using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using SearchableAttribute = HG.Reflection.SearchableAttribute;
using GOTCE.Skills;
using GOTCE.Interactables;
using GOTCE.Enemies;
using RoR2.ExpansionManagement;
using Object = UnityEngine.Object;
using GOTCE.Enemies.Standard;
using GOTCE.Enemies.Minibosses;
using GOTCE.Enemies.Bosses;
using MonoMod.RuntimeDetour;
using GOTCE.Based;
using GOTCE.Survivors;

[assembly: SearchableAttribute.OptIn]

namespace GOTCE
{
    [BepInPlugin(ModGuid, ModName, ModVer)]
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [BepInDependency("com.xoxfaby.BetterUI", BepInDependency.DependencyFlags.SoftDependency)] // soft dependency for compat
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [R2APISubmoduleDependency(nameof(DamageAPI), nameof(ItemAPI), nameof(LanguageAPI), nameof(EliteAPI), nameof(RecalculateStatsAPI), nameof(DirectorAPI), nameof(NetworkingAPI), nameof(PrefabAPI))]
    public class Main : BaseUnityPlugin
    {
        public const string ModGuid = "com.TheBestAssociatedLargelyLudicrousSillyheadGroup.GOTCE";
        public const string ModName = "Gamers of the Cracked Emoji";
        public const string ModVer = "1.1.1";

        public static AssetBundle MainAssets;
        public static AssetBundle SecondaryAssets;
        public static AssetBundle GOTCEModels;

        public List<ArtifactBase> Artifacts = new();
        public List<ItemBase> Items = new();
        public List<EquipmentBase> Equipments = new();
        public List<EliteEquipmentBase> EliteEquipments = new();

        public static Dictionary<ArtifactBase, bool> ArtifactStatusDictionary = new();
        public static Dictionary<ItemBase, bool> ItemStatusDictionary = new();
        public static Dictionary<EquipmentBase, bool> EquipmentStatusDictionary = new();
        public static Dictionary<EliteEquipmentBase, bool> EliteEquipmentStatusDictionary = new();

        public static ExpansionDef GOTCEExpansionDef;
        public static ExpansionDef SOTVExpansionDef;
        public static GameObject GOTCERunBehavior;

        //Provides a direct access to this plugin's logger for use in any of your other classes.
        public static BepInEx.Logging.ManualLogSource ModLogger;

        public static R2API.DamageAPI.ModdedDamageType nader = R2API.DamageAPI.ReserveDamageType();
        public static R2API.DamageAPI.ModdedDamageType rounder = R2API.DamageAPI.ReserveDamageType();
        public static R2API.DamageAPI.ModdedDamageType truekill = R2API.DamageAPI.ReserveDamageType();
        public static R2API.DamageAPI.ModdedDamageType root = R2API.DamageAPI.ReserveDamageType();

        private void Awake()
        {
            MainAssets = AssetBundle.LoadFromFile(Assembly.GetExecutingAssembly().Location.Replace("GOTCE.dll", "macterabrundle"));
            SecondaryAssets = AssetBundle.LoadFromFile(Assembly.GetExecutingAssembly().Location.Replace("GOTCE.dll", "secondarybundle"));
            GOTCEModels = AssetBundle.LoadFromFile(Assembly.GetExecutingAssembly().Location.Replace("GOTCE.dll", "gotcemodels"));
            ModLogger = Logger;
            SOTVExpansionDef = Addressables.LoadAssetAsync<ExpansionDef>("RoR2/DLC1/Common/DLC1.asset").WaitForCompletion();

            // please just fucking use hopoo shaders AAAAAA
            // https://drive.google.com/drive/folders/1ndCC4TiN06nVC4X_3HaZjFa5sN07Y14S

            var mat1 = MainAssets.LoadAllAssets<Material>();
            foreach (Material material in mat1)
            {
                if (material.shader.name.StartsWith("StubbedShader"))
                {
                    material.shader = Resources.Load<Shader>("shaders" + material.shader.name.Substring(13));
                }
            }

            var mat2 = SecondaryAssets.LoadAllAssets<Material>();
            foreach (Material material in mat2)
            {
                if (material.shader.name.StartsWith("StubbedShader"))
                {
                    material.shader = Resources.Load<Shader>("shaders" + material.shader.name.Substring(13));
                }
            }

            var mat3 = GOTCEModels.LoadAllAssets<Material>();
            foreach (Material material in mat3)
            {
                if (material.shader.name.StartsWith("StubbedShader"))
                {
                    material.shader = Resources.Load<Shader>("shaders" + material.shader.name.Substring(13));
                }
            }

            /* if (Chainloader.PluginInfos.ContainsKey("com.xoxfaby.BetterUI")) {
                ItemSorting.tierMap.Add(LunarVoid.Instance.TierEnum, 3);
            } */

            // Don't know how to create/use an asset bundle, or don't have a unity project set up?
            // Look here for info on how to set these up: https://github.com/KomradeSpectre/AetheriumMod/blob/rewrite-master/Tutorials/Item%20Mod%20Creation.md#unity-project
            // (This is a bit old now, but the information on setting the unity asset bFream("GOTCE.macterabrundle"))
            //{
            //MainAssets = AssetBundle.LoadFromStream(stream);
            //}

            //This section automatically scans the project for all artifacts
            var ArtifactTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ArtifactBase)));

            foreach (var artifactType in ArtifactTypes)
            {
                ArtifactBase artifact = (ArtifactBase)Activator.CreateInstance(artifactType);
                //ModLogger.LogInfo(artifact.ArtifactDescription);
                if (ValidateArtifact(artifact, Artifacts))
                {
                    artifact.Init(Config);
                }
            }

            // add spacetime clock before other items since others might depend on it's event
            ItemBase faulty = (ItemBase)System.Activator.CreateInstance(typeof(GOTCE.Items.White.FaultySpacetimeClock));
            if (ValidateItem(faulty, Items))
            {
                faulty.Init(Config);
            }

            ItemBase zoom = (ItemBase)System.Activator.CreateInstance(typeof(GOTCE.Items.White.ZoomLenses));
            if (ValidateItem(zoom, Items))
            {
                zoom.Init(Config);
            }

            ItemBase gummy = (ItemBase)System.Activator.CreateInstance(typeof(GOTCE.Items.White.GummyVitamins));
            if (ValidateItem(gummy, Items))
            {
                gummy.Init(Config);
            }

            //This section automatically scans the project for all items
            var ItemTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ItemBase)));

            foreach (var itemType in ItemTypes)
            {
                if (itemType != typeof(GOTCE.Items.White.FaultySpacetimeClock) && itemType != typeof(GOTCE.Items.White.GummyVitamins) && itemType != typeof(GOTCE.Items.White.ZoomLenses))
                {
                    ItemBase item = (ItemBase)System.Activator.CreateInstance(itemType);
                    // Debug.Log(item.ConfigName);
                    if (ValidateItem(item, Items))
                    {
                        item.Init(Config);
                    }
                }
            }
            [SystemInitializer(dependencies: typeof(ItemCatalog))] // wait until after the catalog initializes to add interactables
            void the()
            {
                var interactableTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(InteractableBase)));

                foreach (var interactableType in interactableTypes)
                {
                    InteractableBase inter = (InteractableBase)System.Activator.CreateInstance(interactableType);
                    inter.Create();
                }
            }
            //this section automatically scans the project for all equipment
            var EquipmentTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(EquipmentBase)));

            foreach (var equipmentType in EquipmentTypes)
            {
                EquipmentBase equipment = (EquipmentBase)System.Activator.CreateInstance(equipmentType);
                if (ValidateEquipment(equipment, Equipments))
                {
                    equipment.Init(Config);
                }
            }

            //this section automatically scans the project for all elite equipment
            var EliteEquipmentTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(EliteEquipmentBase)));

            foreach (var eliteEquipmentType in EliteEquipmentTypes)
            {
                EliteEquipmentBase eliteEquipment = (EliteEquipmentBase)System.Activator.CreateInstance(eliteEquipmentType);
                if (ValidateEliteEquipment(eliteEquipment, EliteEquipments))
                {
                    eliteEquipment.Init(Config);
                }
            }

            //this section automatically scans the project for all equipment
            var SkillTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(SkillBase)));

            foreach (var skillType in SkillTypes)
            {
                SkillBase skill = (SkillBase)System.Activator.CreateInstance(skillType);
                skill.Create();
            }

            Itsgup.OhTheMisery();
            // LivingSuppressiveFire.Create();
            // IonSurger.Create(); // ION SURGER IS BROKEN
            Itsgup.SoMyMainGoalIsToBlowUp();
            Zased.DoTheBased();
            Based.SuppressiveNader.Hook();

            var enemyTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(EnemyBase)));

            foreach (var enemyType in enemyTypes)
            {
                Debug.Log("Woolie");
                EnemyBase enemy = (EnemyBase)System.Activator.CreateInstance(enemyType);
                // Debug.Log(item.ConfigName);
                enemy.Create();
            }

            var survivorTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(SurvivorBase)));

            foreach (var survivorType in survivorTypes)
            {
                Debug.Log("Woolie");
                SurvivorBase survivor = (SurvivorBase)System.Activator.CreateInstance(survivorType);
                // Debug.Log(item.ConfigName);
                survivor.Create();
            }

            Hook aimHook = new Hook(
                typeof(InputBankTest).GetProperty("aimOrigin", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetGetMethod(),
                typeof(LivingSuppressiveFire).GetMethod("InputBankTest_aimOrigin_Get", System.Reflection.BindingFlags.Public | BindingFlags.Static)
            );

            //CreateExpansion();
            /* On.RoR2.Networking.NetworkManagerSystemSteam.OnClientConnect += (s, u, t) => { };
            local multiplayer hook
            run modded ror2 twice, create a multiplayer lobby in one, then do connect localhost:7777 in the other instance
            */

            var materials = MainAssets.LoadAllAssets<Material>();
            foreach (Material material in materials)
                if (material.shader.name.StartsWith("StubbedShader"))
                    material.shader = Resources.Load<Shader>("shaders" + material.shader.name.Substring(13));
            var materials2 = SecondaryAssets.LoadAllAssets<Material>();
            foreach (Material material in materials)
                if (material.shader.name.StartsWith("StubbedShader"))
                    material.shader = Resources.Load<Shader>("shaders" + material.shader.name.Substring(13));
        }

        /// <summary>
        /// A helper to easily set up and initialize an artifact from your artifact classes if the user has it enabled in their configuration files.
        /// </summary>
        /// <param name="artifact">A new instance of an ArtifactBase class."</param>
        /// <param name="artifactList">The list you would like to add this to if it passes the config check.</param>
        public bool ValidateArtifact(ArtifactBase artifact, List<ArtifactBase> artifactList)
        {
            var enabled = Config.Bind<bool>("Artifact: " + artifact.ArtifactName, "Enable Artifact?", true, "Should this artifact appear for selection?").Value;

            if (enabled)
            {
                artifactList.Add(artifact);
            }
            return enabled;
        }

        /// <summary>
        /// A helper to easily set up and initialize an item from your item classes if the user has it enabled in their configuration files.
        /// <para>Additionally, it generates a configuration for each item to allow blacklisting it from AI.</para>
        /// </summary>
        /// <param name="item">A new instance of an ItemBase class."</param>
        /// <param name="itemList">The list you would like to add this to if it passes the config check.</param>
        public bool ValidateItem(ItemBase item, List<ItemBase> itemList, bool faulty = false)
        {
            if (faulty)
            {
                var enabled = Config.Bind<bool>("Item: " + item.ConfigName, "Enable Item?", true, "Should this item appear in runs? Disable other stage transition crit items when disabling this item.").Value;
            }
            else
            {
                var enabled = Config.Bind<bool>("Item: " + item.ConfigName, "Enable Item?", true, "Should this item appear in runs?").Value;
            }
            var aiBlacklist = Config.Bind<bool>("Item: " + item.ConfigName, "Blacklist Item from AI Use?", false, "Should the AI not be able to obtain this item?").Value;
            if (enabled)
            {
                itemList.Add(item);
                if (aiBlacklist)
                {
                    item.AIBlacklisted = true;
                }
            }
            return enabled;
        }

        /// <summary>
        /// A helper to easily set up and initialize an equipment from your equipment classes if the user has it enabled in their configuration files.
        /// </summary>
        /// <param name="equipment">A new instance of an EquipmentBase class."</param>
        /// <param name="equipmentList">The list you would like to add this to if it passes the config check.</param>
        public bool ValidateEquipment(EquipmentBase equipment, List<EquipmentBase> equipmentList)
        {
            if (Config.Bind<bool>("Equipment: " + equipment.EquipmentName, "Enable Equipment?", true, "Should this equipment appear in runs?").Value)
            {
                equipmentList.Add(equipment);
                return true;
            }
            return false;
        }

        /// <summary>
        /// A helper to easily set up and initialize an elite equipment from your elite equipment classes if the user has it enabled in their configuration files.
        /// </summary>
        /// <param name="eliteEquipment">A new instance of an EliteEquipmentBase class.</param>
        /// <param name="eliteEquipmentList">The list you would like to add this to if it passes the config check.</param>
        /// <returns></returns>
        public bool ValidateEliteEquipment(EliteEquipmentBase eliteEquipment, List<EliteEquipmentBase> eliteEquipmentList)
        {
            var enabled = Config.Bind<bool>("Equipment: " + eliteEquipment.EliteEquipmentName, "Enable Elite Equipment?", true, "Should this elite equipment appear in runs? If disabled, the associated elite will not appear in runs either.").Value;

            if (enabled)
            {
                eliteEquipmentList.Add(eliteEquipment);
                return true;
            }
            return false;
        }

        public void CreateExpansion()
        {
            var sotv = LegacyResourcesAPI.Load<ExpansionDef>("ExpansionDefs/DLC1");

            GOTCEExpansionDef = ScriptableObject.CreateInstance<ExpansionDef>();
            //var what = Addressables.LoadAssetAsync<GameObject>("12bf89dabb4bb914382a0e31546446cc").WaitForCompletion();
            GOTCERunBehavior = PrefabAPI.InstantiateClone(gameObject, "GOTCERunBehavior", true);
            DestroyImmediate(GOTCERunBehavior.GetComponent<GlobalDeathRewards>());
            var expansionRequirement = GOTCERunBehavior.GetComponent<ExpansionRequirementComponent>();
            expansionRequirement.requiredExpansion = GOTCEExpansionDef;
            /*
            GOTCERunBehavior.AddComponent<GOTCEVisuals>();
            GOTCERunBehavior.AddComponent<GOTCEBuffs>();
            GOTCERunBehavior.AddComponent<GOTCEEliteRamps>();
            GOTCERunBehavior.AddComponent<GOTCEDamageColors>();
            */
            PrefabAPI.RegisterNetworkPrefab(GOTCERunBehavior);
            // SpikestripContentBase.networkedObjectContent.Add(GOTCERunBehavior);
            GOTCEExpansionDef.name = "GOTCECONTENT_EXPANSION";
            GOTCEExpansionDef.nameToken = "GOTCECONTENT_EXPANSION_NAME";
            GOTCEExpansionDef.descriptionToken = "GOTCECONTENT_EXPANSION_DESCRIPTION";
            GOTCEExpansionDef.iconSprite = MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/NEA.png");
            GOTCEExpansionDef.disabledIconSprite = sotv.disabledIconSprite;
            GOTCEExpansionDef.requiredEntitlement = sotv.requiredEntitlement;
            GOTCEExpansionDef.runBehaviorPrefab = GOTCERunBehavior;
            //SpikestripContentBase.expansionDefContent.Add(GOTCEExpansionDef);
            LanguageAPI.Add(GOTCEExpansionDef.nameToken, "Gamers of The Cracked Emoji");
            LanguageAPI.Add(GOTCEExpansionDef.descriptionToken, "Adds content from the 'GOTCE' mod to the game.");
        }
    }
}