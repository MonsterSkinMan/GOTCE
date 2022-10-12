using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Collections.Generic;
using EntityStates;
using RoR2.CharacterAI;
using System.Linq;

namespace GOTCE.Enemies.Standard {
    public class LivingSuppressiveFire : EnemyBase<LivingSuppressiveFire> {
        public override string PathToClone => "RoR2/Base/Wisp/WispBody.prefab";
        public override string CloneName => "LivingSuppressiveFire";
        public override string PathToCloneMaster => "RoR2/Base/Wisp/WispMaster.prefab";
        public CharacterBody body;
        public CharacterMaster master;

        public override void CreatePrefab()
        {
            base.CreatePrefab();
            body = prefab.GetComponent<CharacterBody>();
            body.baseArmor = 0;
            body.levelArmor = 0;
            body.attackSpeed = 1f;
            body.levelAttackSpeed = 0f;
            body.damage = 7.5f;
            body.levelDamage = 2f;
            body.baseMaxHealth = 25f;
            body.levelMaxHealth = 5f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_LIVINGSUPPRESSIVEFIRE_NAME";
            body.baseRegen = 0f;
            body.levelRegen = 0f;
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 20;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.None;
            isc.hullSize = HullClassification.Human;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Air;
            isc.sendOverNetwork = true;
            isc.prefab = prefab;
            isc.name = "cscLivingSuppressiveFire";
        }

        public override void AddDirectorCard()
        {
            base.AddDirectorCard();
            card.minimumStageCompletions = 5;
            card.selectionWeight = 1;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
        }

        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = prefab;

            SwapMaterials(prefab, Main.MainAssets.LoadAsset<Material>("Assets/Materials/Enemies/kirnMaterial.mat"), true, null);
            DisableMeshes(prefab, new List<int> { 1 });
            SwapMeshes(prefab, Utils.MiscUtils.GetMeshFromPrimitive(PrimitiveType.Sphere), true);

            Light light = prefab.AddComponent<Light>();
            light.color = Color.yellow;
            light.intensity = 100f;
            light.bounceIntensity = 50f;
            light.shadows = LightShadows.Hard;

            foreach(AISkillDriver driver in prefabMaster.GetComponentsInChildren<AISkillDriver>()) {
                driver.maxDistance = 120f;
                driver.minDistance = 30f;
                driver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
                driver.activationRequiresAimTargetLoS = true;
                driver.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;
                driver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
                driver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
                driver.skillSlot = SkillSlot.Primary;
            }

            RelocateMeshTransform(prefab, light.transform, true);
            
            SkillLocator sl = prefab.GetComponentInChildren<SkillLocator>();
            ReplaceSkill(sl.primary, Skills.Consistency.Instance.SkillDef);

            LanguageAPI.Add("GOTCE_LIVINGSUPPRESSIVEFIRE_NAME", "Living Suppressive Fire");
            LanguageAPI.Add("GOTCE_LIVINGSUPPRESSIVEFIRE_LORE", "Even if frags did 2000% with no falloff...");
            LanguageAPI.Add("GOTCE_LIVINGSUPPRESSIVEFIRE_SUBTITLE", "Horde of Many");
            RegisterEnemy(prefab, prefabMaster, null, DirectorAPI.MonsterCategory.BasicMonsters, true);
        }
    }
}