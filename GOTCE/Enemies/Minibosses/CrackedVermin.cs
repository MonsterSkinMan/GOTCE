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
        public override string CloneName => "Cracked Vermin";
        public override string PathToClone => "RoR2/DLC1/Vermin/VerminBody.prefab";
        public override string PathToCloneMaster => "RoR2/DLC1/Vermin/VerminMaster.prefab";
        public CharacterBody body;
        public CharacterMaster master;

        public override void CreatePrefab()
        {
            base.CreatePrefab();
            body = prefab.GetComponent<CharacterBody>();
            body.baseArmor = 0;
            body.attackSpeed = 0.3f;
            body.damage = 0.7f;
            body.levelDamage = 0.06f;
            body.baseMaxHealth = 180f;
            body.levelMaxHealth = 54f;
            body.baseMoveSpeed = 45f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_CRACKEDVERMIN_NAME";
            body.subtitleNameToken = "GOTCE_CRACKEDVERMIN_SUBTITLE";
            body.baseRegen = 0f;
            body.portraitIcon = Main.MainAssets.LoadAsset<Texture2D>("Assets/Textures/Icons/Enemies/CrackedVermin.png");
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

            model.GetComponent<HurtBoxGroup>().hurtBoxes = new HurtBox[]
            {
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
            LanguageAPI.Add("GOTCE_CRACKEDVERMIN_LORE", "Cracked Emoji watched on at the blind vermin running about in the clearing below. How boring they are, he thought. Considering how he couldn't begin his invasion in proper for a good bit since he couldn't create a link back home to ferry his army to the RoR universe, he needed an alternative. That usually involved corrupting some of the local enemies to assist him in spreading instability. Plus, this increase in firepower would help him corrupt the universe enough to start bringing in his coalition on a large scale. So, he created boxes full of swag-ass shoes and threw them down to the stupid rats below.\nThe blind vermin immediately investigated the noise and found the boxes full of shoes. While they couldn't see how dope they were, they knew that they were made of high quality materials thanks to how they felt and smelled. Due to the fact that the blind vermin loved expensive fashion, they put the shoes on and were immediately transformed into nothing more than beings of pure chaos and crackedness. They began zooming around, stomping all who got in their way.\nSatisfied, Cracked Emoji left to further sow the seeds of disarray, resting assured that the Cracked Vermin would help his cause. He just hopes they don't realize that their shoes are, in fact, fake. He would very much appreciate if you didn't type the phrase \"Your jordans are fake.\" in the chat whenever you see a Cracked Vermin spawn.");
            LanguageAPI.Add("GOTCE_CRACKEDVERMIN_SUBTITLE", "Horde of Many");

            On.RoR2.Chat.AddMessage_string += (orig, str) =>
            {
                orig(str);
                // Debug.Log(str);
                if (str.ToLower().Contains("your jordans are fake"))
                {
                    ReadOnlyCollection<TeamComponent> team = TeamComponent.GetTeamMembers(TeamIndex.Monster);
                    foreach (TeamComponent com in team)
                    {
                        if (com.body && com.body.healthComponent)
                        {
                            if (com.body.baseNameToken == "GOTCE_CRACKEDVERMIN_NAME")
                            {
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
                DirectorAPI.Stage.SulfurPools,
                DirectorAPI.Stage.RallypointDeltaSimulacrum,
                DirectorAPI.Stage.AphelianSanctuarySimulacrum
            };

            RegisterEnemy(prefab, prefabMaster, stages, DirectorAPI.MonsterCategory.Minibosses, false);
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 60;
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