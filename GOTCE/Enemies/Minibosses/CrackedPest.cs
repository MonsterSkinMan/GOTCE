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
    public class CrackedPest : EnemyBase<CrackedPest> {
        public override string CloneName => "CrackedPest";
        public override string PathToClone => "RoR2/DLC1/FlyingVermin/FlyingVerminBody.prefab";
        public override string PathToCloneMaster => "RoR2/DLC1/FlyingVermin/FlyingVerminMaster.prefab";
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
            body.damage = 20f;
            body.levelDamage = 3f;
            body.baseMaxHealth = 500f;
            body.levelMaxHealth = 50f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_CRACKEDPEST_NAME";
            body.subtitleNameToken = "GOTCE_CRACKEDPEST_SUBTITLE";
            body.baseRegen = 0f;
            body.levelRegen = 0f;
        }
        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = prefab;

            DisableSkins(prefab);
            // SwapMaterials(prefab, Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/VFX/matLightningSphere.mat").WaitForCompletion(), true);
            // DestroyModelLeftovers(prefab);
            GameObject model = GameObject.Instantiate(Main.MainAssets.LoadAsset<GameObject>("Assets/Models/Prefabs/Enemies/CrackedPest/mdlCrackedPest.prefab"));
            
            CapsuleCollider box1 = model.transform.Find("mdlFlyingVermin").gameObject.GetComponent<CapsuleCollider>();
            CapsuleCollider box2 = model.transform.Find("mdlFlyingVerminFur").gameObject.GetComponent<CapsuleCollider>();
            CapsuleCollider boxweak = model.transform.Find("mdlFlyingVerminMouth").gameObject.GetComponent<CapsuleCollider>();

            SetupModel(prefab, model);
            SetupHurtbox(prefab, model, box1, 0);
            SetupHurtbox(prefab, model, box2, 1);
            SetupHurtbox(prefab, model, boxweak, 2, true, HurtBox.DamageModifier.SniperTarget);

            model.GetComponent<HurtBoxGroup>().hurtBoxes = new HurtBox[] {
                box1.gameObject.GetComponent<HurtBox>(),
                box2.gameObject.GetComponent<HurtBox>(),
                boxweak.gameObject.GetComponent<HurtBox>()
            };
            model.GetComponent<HurtBoxGroup>().bullseyeCount = 1;
            model.GetComponent<HurtBoxGroup>().mainHurtBox = box1.gameObject.GetComponent<HurtBox>();

            

            SkillDef spit = CrackedVerminSpit.Instance.SkillDef;
            SkillLocator sl = prefab.GetComponent<SkillLocator>();


            ReplaceSkill(sl.primary, spit);

            LanguageAPI.Add("GOTCE_CRACKEDPEST_NAME", "Cracked Pest");
            LanguageAPI.Add("GOTCE_CRACKEDPEST_LORE", "temp");
            LanguageAPI.Add("GOTCE_CRACKEDPEST_SUBTITLE", "Horde of Many");

            List<DirectorAPI.Stage> stages = new() {
                DirectorAPI.Stage.RallypointDelta,
                DirectorAPI.Stage.AphelianSanctuary,
                DirectorAPI.Stage.SiphonedForest,
                DirectorAPI.Stage.SulfurPools
            };
            
            RegisterEnemy(prefab, prefabMaster, stages, DirectorAPI.MonsterCategory.Minibosses, false);
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 100;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.TeleporterOK;
            isc.hullSize = HullClassification.Human;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Air;
            isc.sendOverNetwork = true;
            isc.prefab = prefab;
            isc.name = "cscCrackedPest";
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