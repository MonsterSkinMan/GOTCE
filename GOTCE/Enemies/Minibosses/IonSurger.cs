using R2API;
using RoR2;
using UnityEngine;
using RoR2.Skills;
using RoR2.CharacterAI;
using GOTCE.Skills;
using System.Linq;
using System.Collections.Generic;

namespace GOTCE.Enemies.Minibosses
{
    public class IonSurger : EnemyBase<IonSurger>
    {
        public override string CloneName => "IonSurger";
        public override string PathToClone => "RoR2/Base/Mage/MageBody.prefab";
        public override string PathToCloneMaster => "RoR2/Base/Brother/BrotherMaster.prefab";
        public CharacterBody body;
        public CharacterMaster master;

        public override void CreatePrefab()
        {
            base.CreatePrefab();
            body = prefab.GetComponent<CharacterBody>();
            body.baseArmor = 0;
            body.attackSpeed = 1f;
            body.damage = 1f;
            body.levelDamage = 0.2f;
            body.baseMaxHealth = 200f;
            body.moveSpeed = 14f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_IONSURGER_NAME";
            body.subtitleNameToken = "GOTCE_IONSURGER_SUBTITLE";
            body.baseRegen = 0f;
            body.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            body.portraitIcon = Main.MainAssets.LoadAsset<Texture2D>("Assets/Textures/Icons/Enemies/LivingSuppressiveFire.png");
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 90;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.TeleporterOK;
            isc.hullSize = HullClassification.Human;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            isc.sendOverNetwork = true;
            isc.prefab = prefabMaster;
            isc.name = "cscIonSurger";
        }

        public override void AddDirectorCard()
        {
            base.AddDirectorCard();
            card.selectionWeight = 1;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Close;
        }

        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = prefab;
            prefab.GetComponent<TeamComponent>().teamIndex = TeamIndex.Monster;
            DisableSkins(prefab);
            // SwapMaterials(prefab, Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/VFX/matLightningSphere.mat").WaitForCompletion(), true);
            // DestroyModelLeftovers(prefab);
            GameObject model = GameObject.Instantiate(Main.MainAssets.LoadAsset<GameObject>("Assets/Models/Prefabs/Enemies/IonSurger/mdlIonSurger.prefab"));
            SetupModel(prefab, model);

            CapsuleCollider box1 = model.transform.Find("RightHitbox").gameObject.GetComponent<CapsuleCollider>();
            CapsuleCollider boxweak = model.transform.Find("LeftHitbox").gameObject.GetComponent<CapsuleCollider>();

            SetupModel(prefab, model);
            SetupHurtbox(prefab, model, box1, 0, false, HurtBox.DamageModifier.Normal, true);
            SetupHurtbox(prefab, model, boxweak, 1, true, HurtBox.DamageModifier.SniperTarget);

            model.GetComponent<HurtBoxGroup>().hurtBoxes = new HurtBox[] {
                box1.gameObject.GetComponent<HurtBox>(),
                boxweak.gameObject.GetComponent<HurtBox>()
            };
            model.GetComponent<HurtBoxGroup>().bullseyeCount = 1;
            model.GetComponent<HurtBoxGroup>().mainHurtBox = box1.gameObject.GetComponent<HurtBox>();

            SkillDef ion = IonSurgerSurge.Instance.SkillDef;
            SkillLocator sl = prefab.GetComponent<SkillLocator>();

            BaseAI baseAI = prefabMaster.GetComponent<BaseAI>();
            baseAI.enemyAttentionDuration = 5f;
            baseAI.fullVision = true;

            ReplaceSkill(sl.primary, ion);
            ReplaceSkill(sl.secondary, ion);
            ReplaceSkill(sl.utility, ion);
            ReplaceSkill(sl.special, ion);

            DeathRewards deathRewards = prefab.GetComponent<DeathRewards>();
            if (deathRewards)
            {
            }
            else
            {
                deathRewards = prefab.AddComponent<DeathRewards>();
                deathRewards.characterBody = body;
                deathRewards.logUnlockableDef = ScriptableObject.CreateInstance<UnlockableDef>();
                deathRewards.logUnlockableDef.nameToken = "Ion Surger";
            }

            LanguageAPI.Add("GOTCE_IONSURGER_NAME", "Ion Surger");
            LanguageAPI.Add("GOTCE_IONSURGER_LORE", "Melee range isn't viable.");
            LanguageAPI.Add("GOTCE_IONSURGER_SUBTITLE", "Horde of Many");

        }

        public override void PostCreation()
        {
            base.PostCreation();
            List<DirectorAPI.Stage> stages = new()
            {
                DirectorAPI.Stage.TitanicPlains, DirectorAPI.Stage.TitanicPlainsSimulacrum,
                DirectorAPI.Stage.RallypointDelta, DirectorAPI.Stage.RallypointDeltaSimulacrum,
                DirectorAPI.Stage.WetlandAspect, DirectorAPI.Stage.SirensCall
            };
            RegisterEnemy(prefab, prefabMaster, stages, DirectorAPI.MonsterCategory.Minibosses, true);
        }
    }
} 