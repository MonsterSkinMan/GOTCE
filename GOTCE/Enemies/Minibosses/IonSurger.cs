using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using RoR2.Skills;
using GOTCE;
using UnityEngine.Scripting;
using System.Linq;
using RoR2.CharacterAI;
using GOTCE.Skills;
using EntityStates;
using GOTCE.Utils;
using System.Collections.Generic;

namespace GOTCE.Enemies.Minibosses {
    public class IonSurger : EnemyBase<IonSurger> {
        public override string CloneName => "IonSurger";
        public override string PathToClone => "RoR2/Base/Mage/MageBody.prefab";
        public override string PathToCloneMaster => "RoR2/Base/Merc/MercMonsterMaster.prefab";
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
            body.damage = 15f;
            body.levelDamage = 3f;
            body.baseMaxHealth = 600f;
            body.levelMaxHealth = 180f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_IONSURGER_NAME";
            body.subtitleNameToken = "GOTCE_IONSURGER_SUBTITLE";
            body.baseRegen = 0f;
            body.levelRegen = 0f;
            body.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            body.visionDistance = 2000000f;
        }
        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = prefab;

            DisableSkins(prefab);
            SwapMaterials(prefab, Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/VFX/matLightningSphere.mat").WaitForCompletion(), true);

            Light light = prefab.AddComponent<Light>();
            light.color = Color.blue;
            light.intensity = 250f;

            SkillDef ion = IonSurgerSurge.Instance.SkillDef;
            SkillLocator sl = prefab.GetComponent<SkillLocator>();

            foreach (AISkillDriver ai in prefabMaster.GetComponents<AISkillDriver>()) {
                ai.aimType = AISkillDriver.AimType.AtCurrentEnemy;
                ai.minDistance = 2;
                ai.maxDistance = 1000;
                ai.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
                ai.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
                ai.shouldSprint = true;
                ai.skillSlot = SkillSlot.Primary;
            }

            BaseAI baseAI = prefabMaster.GetComponent<BaseAI>();
            baseAI.enemyAttentionDuration = 100000f;
            baseAI.fullVision = true;

            ReplaceSkill(sl.primary, ion);
            ReplaceSkill(sl.secondary, ion);
            ReplaceSkill(sl.utility, ion);
            ReplaceSkill(sl.special, ion);

            LanguageAPI.Add("GOTCE_IONSURGER_NAME", "Ion Surger");
            LanguageAPI.Add("GOTCE_IONSURGER_LORE", "Melee range isn't viable.");
            LanguageAPI.Add("GOTCE_IONSURGER_SUBTITLE", "Horde of Many");
            
            RegisterEnemy(prefab, prefabMaster, null, DirectorAPI.MonsterCategory.Minibosses, true);
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 50;
            isc.eliteRules = SpawnCard.EliteRules.ArtifactOnly;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.TeleporterOK;
            isc.hullSize = HullClassification.Human;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            isc.sendOverNetwork = true;
            isc.prefab = prefab;
            isc.name = "cscIonSurger";
        }

        public override void AddDirectorCard()
        {
            base.AddDirectorCard();
            card.minimumStageCompletions = 0;
            card.selectionWeight = 1;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Close;
        }
    }
}