using R2API;
using RoR2;
using UnityEngine;
using RoR2.Skills;
using GOTCE.Skills;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections.ObjectModel;

// DOES NOT WORK YET

namespace GOTCE.Enemies.Minibosses
{
    public class CrackedVermin : EnemyBase<CrackedVermin>
    {
        public override string CloneName => "CrackedVermin";
        public override string PathToClone => "RoR2/DLC1/Vermin/VerminBody.prefab";
        public override string PathToCloneMaster => "RoR2/DLC1/Vermin/VerminMaster.prefab";
        public CharacterBody body;
        public CharacterMaster master;

        public override void CreatePrefab()
        {
            base.CreatePrefab();
            body = prefab.GetComponent<CharacterBody>();
            body.baseArmor = 0;
            body.attackSpeed = 1f;
            body.damage = 20f;
            body.levelDamage = 4f;
            body.baseMaxHealth = 150f;
            body.levelMaxHealth = 150f;
            body.baseMoveSpeed = 30f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_CRACKEDVERMIN_NAME";
            body.subtitleNameToken = "GOTCE_CRACKEDVERMIN_SUBTITLE";
            body.baseRegen = 0f;
            body.portraitIcon = Main.MainAssets.LoadAsset<Texture2D>("Assets/Textures/Icons/Enemies/CrackedPest.png");
            
        }

        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = prefab;

            GameObject model = GameObject.Instantiate(Main.SecondaryAssets.LoadAsset<GameObject>("Assets/Prefabs/Enemies/Vermin/mdlVermin.prefab"));
            SphereCollider box1 = model.transform.Find("hitbox").GetComponent<SphereCollider>();
            SphereCollider box2 = model.transform.Find("mdlVerminMouth").GetComponent<SphereCollider>();
            CapsuleCollider box3 = model.transform.Find("mdlVerminFur").GetComponent<CapsuleCollider>();
            CapsuleCollider box4 = model.transform.Find("mdlVermin").GetComponent<CapsuleCollider>();
            CapsuleCollider shoe1 = model.transform.Find("VerminArmature")
            .transform.Find("ROOT")
            .transform.Find("base")
            .transform.Find("Thigh.r")
            .transform.Find("Leg1.r")
            .transform.Find("Leg2.r")
            .transform.Find("Foot.r")
            .transform.Find("shoe").GetComponentInChildren<CapsuleCollider>();
            CapsuleCollider shoe2 = model.transform.Find("VerminArmature")
            .transform.Find("ROOT")
            .transform.Find("base")
            .transform.Find("Thigh.l")
            .transform.Find("Leg1.l")
            .transform.Find("Leg2.l")
            .transform.Find("Foot.l")
            .transform.Find("shoe").GetComponentInChildren<CapsuleCollider>();
            
            DisableSkins(prefab);
            SetupModel(prefab, model);
            SetupHurtbox(prefab, model, box1, 0, true);
            SetupHurtbox(prefab, model, box2, 1);
            SetupHurtbox(prefab, model, box3, 2);
            SetupHurtbox(prefab, model, box4, 3);
            SetupHurtbox(prefab, model, shoe1, 4);
            SetupHurtbox(prefab, model, shoe2, 5);

            model.GetComponent<HurtBoxGroup>().hurtBoxes = new HurtBox[] {
                box1.gameObject.GetComponent<HurtBox>(),
                box2.gameObject.GetComponent<HurtBox>(),
                box3.gameObject.GetComponent<HurtBox>(),
                box4.gameObject.GetComponent<HurtBox>(),
                shoe1.gameObject.GetComponent<HurtBox>(),
                shoe2.gameObject.GetComponent<HurtBox>(),
            };
            model.GetComponent<HurtBoxGroup>().bullseyeCount = 1;
            model.GetComponent<HurtBoxGroup>().mainHurtBox = box1.gameObject.GetComponent<HurtBox>();

            SkillDef thunderslam = Skills.CrackedSlam.Instance.SkillDef;
            SkillLocator sl = prefab.GetComponent<SkillLocator>();

            prefab.GetComponent<CharacterDeathBehavior>().deathState = new EntityStates.SerializableEntityStateType(typeof(EntityStatesCustom.Providence.ProviDeath));

            ReplaceSkill(sl.primary, thunderslam);

            LanguageAPI.Add("GOTCE_CRACKEDVERMIN_NAME", "Cracked Vermin");
            LanguageAPI.Add("GOTCE_CRACKEDVERMIN_LORE", "");
            LanguageAPI.Add("GOTCE_CRACKEDVERMIN_SUBTITLE", "Horde of Many");
            
            On.RoR2.Chat.AddMessage_string += (orig, str) => {
                orig(str);
                // Debug.Log(str);
                if (str.ToLower().Contains("your jordans are fake")) {
                    ReadOnlyCollection<TeamComponent> team = TeamComponent.GetTeamMembers(TeamIndex.Monster);
                    foreach (TeamComponent com in team) {
                        if (com.body && com.body.healthComponent) {
                            if (com.body.baseNameToken == "GOTCE_CRACKEDVERMIN_NAME") {
                                com.body.healthComponent.Suicide();
                            }
                        }
                    }
                }
            };
        }

        public override void PostCreation()
        {
            base.PostCreation();
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
            isc.directorCreditCost = 50;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.TeleporterOK;
            isc.hullSize = HullClassification.Human;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            isc.sendOverNetwork = true;
            isc.prefab = prefabMaster;
            isc.name = "cscCrackedVermin";
        }

        public override void AddDirectorCard()
        {
            base.AddDirectorCard();
            card.minimumStageCompletions = 0;
            card.selectionWeight = 1;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
        }
    }
}