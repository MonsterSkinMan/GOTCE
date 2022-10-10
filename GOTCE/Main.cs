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
using BepInEx.Bootstrap;
using UnityEngine.AddressableAssets;
using GOTCE.Components;
using SearchableAttribute = HG.Reflection.SearchableAttribute;
using RoR2.Navigation;
using GOTCE.Enemies.Skills;
using GOTCE.Interactables;

[assembly: SearchableAttribute.OptIn]

namespace GOTCE
{
    [BepInPlugin(ModGuid, ModName, ModVer)]
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    // [BepInDependency("com.xoxfaby.BetterUI", BepInDependency.DependencyFlags.SoftDependency)] // soft dependency so betterui doesnt go insane when it sees a custom tier
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI), nameof(EliteAPI), nameof(RecalculateStatsAPI), nameof(DirectorAPI), nameof(NetworkingAPI))]
    public class Main : BaseUnityPlugin
    {
        public const string ModGuid = "com.TheBestAssociatedLargelyLudicrousSillyheadGroup.GOTCE";
        public const string ModName = "Gamers of the Cracked Emoji";
        public const string ModVer = "1.0.2";

        public static AssetBundle MainAssets;

        public List<ArtifactBase> Artifacts = new List<ArtifactBase>();
        public List<ItemBase> Items = new List<ItemBase>();
        public List<EquipmentBase> Equipments = new List<EquipmentBase>();
        public List<EliteEquipmentBase> EliteEquipments = new List<EliteEquipmentBase>();

        public static Dictionary<ArtifactBase, bool> ArtifactStatusDictionary = new Dictionary<ArtifactBase, bool>();
        public static Dictionary<ItemBase, bool> ItemStatusDictionary = new Dictionary<ItemBase, bool>();
        public static Dictionary<EquipmentBase, bool> EquipmentStatusDictionary = new Dictionary<EquipmentBase, bool>();
        public static Dictionary<EliteEquipmentBase, bool> EliteEquipmentStatusDictionary = new Dictionary<EliteEquipmentBase, bool>();

        //Provides a direct access to this plugin's logger for use in any of your other classes.
        public static BepInEx.Logging.ManualLogSource ModLogger;

        private void Awake()
        {
            MainAssets = AssetBundle.LoadFromFile(Assembly.GetExecutingAssembly().Location.Replace("GOTCE.dll", "macterabrundle"));
            Debug.Log("Trolling");
            Debug.Log("test");
            ModLogger = Logger;

            /* if (Chainloader.PluginInfos.ContainsKey("com.xoxfaby.BetterUI")) {
                ItemSorting.tierMap.Add(LunarVoid.Instance.TierEnum, 3);
            } */

            // Don't know how to create/use an asset bundle, or don't have a unity project set up?
            // Look here for info on how to set these up: https://github.com/KomradeSpectre/AetheriumMod/blob/rewrite-master/Tutorials/Item%20Mod%20Creation.md#unity-project
            // (This is a bit old now, but the information on setting the unity asset bundle should be the same.)

            //using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GOTCE.macterabrundle"))
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

            //This section automatically scans the project for all items
            var ItemTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ItemBase)));

            foreach (var itemType in ItemTypes)
            {
                Debug.Log("Woolie");
                ItemBase item = (ItemBase)System.Activator.CreateInstance(itemType);
                // Debug.Log(item.ConfigName);
                if (ValidateItem(item, Items))
                {
                    Debug.Log("RNDThursday");
                    item.Init(Config);
                }
            }
            [SystemInitializer(dependencies: typeof(ItemCatalog))]
            void the() {
                var interactableTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(InteractableBase)));

                foreach (var interactableType in interactableTypes)
                {
                    Debug.Log("Populating interactables...");
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
            Enemies.LivingSuppressiveFire.Create();
            Enemies.CrackedPest.Create();

 
            CharacterSpawnCard spawncardS = new CharacterSpawnCard()
            {
                name = "cscKirn",
                prefab = Enemies.LivingSuppressiveFire.LivingSuppressiveFireMaster,
                sendOverNetwork = true,
                nodeGraphType = MapNodeGroup.GraphType.Air,
                requiredFlags = NodeFlags.None,
                forbiddenFlags = NodeFlags.NoCharacterSpawn,
                directorCreditCost = 20,
                eliteRules = SpawnCard.EliteRules.Default
            };

            CharacterSpawnCard spawncardC = new CharacterSpawnCard()
            {
                name = "cscCracked",
                prefab = Enemies.CrackedPest.CrackedPestMaster,
                sendOverNetwork = true,
                nodeGraphType = MapNodeGroup.GraphType.Air,
                requiredFlags = NodeFlags.None,
                forbiddenFlags = NodeFlags.NoCharacterSpawn,
                directorCreditCost = 100,
                eliteRules = SpawnCard.EliteRules.ArtifactOnly
            };

            DirectorCard directorCardS = new DirectorCard()
            {
                spawnCard = spawncardS,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Standard,
                selectionWeight = 1,
                preventOverhead = false
            };
            DirectorCard directorCardC = new DirectorCard()
            {
                spawnCard = spawncardC,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Far,
                selectionWeight = 1,
                preventOverhead = false
            };

            R2API.DirectorAPI.Helpers.AddNewMonster(directorCardS, DirectorAPI.MonsterCategory.BasicMonsters);
            R2API.DirectorAPI.Helpers.RemoveExistingMonsterFromStage("cscKirn", DirectorAPI.Stage.Commencement);
            R2API.DirectorAPI.Helpers.AddNewMonsterToStage(directorCardC, DirectorAPI.MonsterCategory.Minibosses, DirectorAPI.Stage.SulfurPools);
            R2API.DirectorAPI.Helpers.AddNewMonsterToStage(directorCardC, DirectorAPI.MonsterCategory.Minibosses, DirectorAPI.Stage.RallypointDelta);
            R2API.DirectorAPI.Helpers.AddNewMonsterToStage(directorCardC, DirectorAPI.MonsterCategory.Minibosses, DirectorAPI.Stage.AphelianSanctuary);
            R2API.DirectorAPI.Helpers.AddNewMonsterToStage(directorCardC, DirectorAPI.MonsterCategory.Minibosses, DirectorAPI.Stage.SiphonedForest);
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
        public bool ValidateItem(ItemBase item, List<ItemBase> itemList)
        {
            var enabled = Config.Bind<bool>("Item: " + item.ConfigName, "Enable Item?", true, "Should this item appear in runs?").Value;
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
    }
}