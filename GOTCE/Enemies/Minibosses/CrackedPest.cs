using R2API;
using RoR2;
using UnityEngine;
using RoR2.Skills;
using GOTCE.Skills;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace GOTCE.Enemies.Minibosses
{
    public class CrackedPest : EnemyBase<CrackedPest>
    {
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
            body.attackSpeed = 1f;
            body.damage = 20f;
            body.levelDamage = 4f;
            body.baseMaxHealth = 500f;
            body.levelMaxHealth = 150f;
            body.baseMoveSpeed = 30f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_CRACKEDPEST_NAME";
            body.subtitleNameToken = "GOTCE_CRACKEDPEST_SUBTITLE";
            body.baseRegen = 0f;
            body.portraitIcon = Main.MainAssets.LoadAsset<Texture2D>("Assets/Textures/Icons/Enemies/CrackedPest.png");
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
            CapsuleCollider glasses1 = model.transform.Find("FlyingVerminArmature")
            .transform.Find("ROOT")
            .transform.Find("Body")
            .transform.Find("MouthTop").transform.Find("cracked").transform.Find("Cone").GetComponentInChildren<CapsuleCollider>();
            CapsuleCollider glasses2 = model.transform.Find("FlyingVerminArmature")
            .transform.Find("ROOT")
            .transform.Find("Body")
            .transform.Find("MouthTop").transform.Find("cracked").transform.Find("Cone.001").GetComponentInChildren<CapsuleCollider>();

            SetupModel(prefab, model);
            SetupHurtbox(prefab, model, box1, 0);
            SetupHurtbox(prefab, model, box2, 1);
            SetupHurtbox(prefab, model, glasses1, 3);
            SetupHurtbox(prefab, model, glasses2, 4);
            SetupHurtbox(prefab, model, boxweak, 2, true, HurtBox.DamageModifier.SniperTarget);

            model.GetComponent<HurtBoxGroup>().hurtBoxes = new HurtBox[] {
                box1.gameObject.GetComponent<HurtBox>(),
                box2.gameObject.GetComponent<HurtBox>(),
                boxweak.gameObject.GetComponent<HurtBox>(),
                glasses1.gameObject.GetComponent<HurtBox>(),
                glasses2.gameObject.GetComponent<HurtBox>()
            };
            model.GetComponent<HurtBoxGroup>().bullseyeCount = 1;
            model.GetComponent<HurtBoxGroup>().mainHurtBox = box1.gameObject.GetComponent<HurtBox>();

            SkillDef spit = CrackedVerminSpit.Instance.SkillDef;
            SkillLocator sl = prefab.GetComponent<SkillLocator>();

            ReplaceSkill(sl.primary, spit);

            LanguageAPI.Add("GOTCE_CRACKEDPEST_NAME", "Cracked Pest");
            LanguageAPI.Add("GOTCE_CRACKEDPEST_LORE", "What's popping D. Furthen gang it's D. Furthen back with another vlog. So this morning, we ran into a group of Blind Pests, except we noticed that one of them was orange and was wearing some really epic sunglasses so I sent Tharson to check it out because I'm a terrible fucking person and enjoy making him do all of my dirty work and suffering for it. Anyways, he kept screaming \"HOLY SHIT IT'S SO FUCKING CRACKED\" over and over, and I guess he was too much of a baby to handle it, because it battered him with its explosive spit and he fucking died lol. I guess Tharson ISN'T okay from this encounter. What a funny joke, I love Tharson memes and I hope that the RoR2 community- I mean my fanbase doesn't run the joke into the center of the Earth. I don't even know what a RoR2 is lol wait yeah I do since RoR1 at least is canon to the Risk of Rain canon. Eh, whatever. The so-called Cracked Pest did seem a bit... odd, however. I'm worried about the existence of more Cracked entities and what that could mean for the stability of the universe.\n<color=#e64b13>Heheheha! Well, you won�t be able to worry about it once I'm done with you.</color>");
            LanguageAPI.Add("GOTCE_CRACKEDPEST_SUBTITLE", "Horde of Many");

            On.RoR2.GlobalEventManager.OnHitAll += vineboom;
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
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Air;
            isc.sendOverNetwork = true;
            isc.prefab = prefabMaster;
            isc.name = "cscCrackedPest";
        }

        public override void AddDirectorCard()
        {
            base.AddDirectorCard();
            card.minimumStageCompletions = 0;
            card.selectionWeight = 1;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
        }

        public static void vineboom(On.RoR2.GlobalEventManager.orig_OnHitAll orig, GlobalEventManager self, DamageInfo info, GameObject target)
        {
            orig(self, info, target);
            if (NetworkServer.active)
            {
                if (info.procChainMask.HasProc(ProcType.AACannon) && !info.procChainMask.HasProc(ProcType.PlasmaCore))
                {
                    float damage = info.damage * 0.15f;
                    float num = 8f;

                    EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXQuick"), new EffectData
                    {
                        origin = info.position,
                        scale = num,
                        rotation = Util.QuaternionSafeLookRotation(info.force)
                    }, transmit: true);

                    BlastAttack blast = new()
                    {
                        attacker = info.attacker,
                        baseDamage = damage,
                        radius = num,
                        inflictor = null,
                        falloffModel = BlastAttack.FalloffModel.None,
                        crit = info.crit,
                        position = info.position,
                        procCoefficient = 0f,
                        damageColorIndex = DamageColorIndex.Item,
                        teamIndex = TeamComponent.GetObjectTeam(info.attacker),
                        damageType = info.damageType
                    };
                    blast.Fire();
                }
            }
        }
    }
} 