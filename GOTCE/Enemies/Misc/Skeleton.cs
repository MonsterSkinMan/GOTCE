using R2API;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using System.Linq;
using GOTCE.Gamemodes.Crackclipse;

namespace GOTCE.Enemies.Misc
{
    public class Skeleton : EnemyBase<Skeleton>
    {
        public override string PathToClone => Utils.Paths.GameObject.MercBody;
        public override string CloneName => "Skeleton";
        public override string PathToCloneMaster => Utils.Paths.GameObject.MercMonsterMaster;
        public CharacterBody body;
        public CharacterMaster master;

        public override void CreatePrefab()
        {
            base.CreatePrefab();
            CharacterBody body = prefab.GetComponent<CharacterBody>();
            body.baseMoveSpeed = 7;
            body.baseMaxHealth = 10000000;
            body.baseArmor = 10000000;
            body.baseDamage = 100000000;
            body.baseNameToken = "GOTCE_SKELETON_NAME";
            LanguageAPI.Add("GOTCE_SKELETON_NAME", "The Skeleton");
            body.portraitIcon = Main.SecondaryAssets.LoadAsset<Sprite>("iconSkeleton.png").texture;
        }
        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();

            GameObject model = Main.SecondaryAssets.LoadAsset<GameObject>("mdlSkeleton.prefab").InstantiateClone("mdlSkele");
            SetupModel(prefab, model);

            SerializableEntityStateType idle = new(typeof(SkeleState)); 

            EntityStateMachine guh = AddESM(prefab, "guh", idle); 

            SkillLocator sl = prefab.GetComponent<SkillLocator>();

            SkillDef lockedDef = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Captain/CaptainSkillDisconnected.asset").WaitForCompletion();

            ReplaceSkill(sl.primary, lockedDef);
            ReplaceSkill(sl.secondary, lockedDef);
            ReplaceSkill(sl.utility, lockedDef);
            ReplaceSkill(sl.special, lockedDef);

            master.bodyPrefab = prefab;
        }

        public void skeletime(On.RoR2.Stage.orig_Start orig, Stage self) {
            orig(self);
            if (NetworkServer.active) {
                if (self) {
                    if (Difficulty.IsCurrentDifHigherOrEqual(Difficulty.c8, Run.instance)) {
                        DirectorPlacementRule rule = new();
                        rule.placementMode = DirectorPlacementRule.PlacementMode.Random;
                        DirectorSpawnRequest request = new(isc, rule, Run.instance.spawnRng);
                        request.teamIndexOverride = TeamIndex.Void;
                        DirectorCore.instance.TrySpawnObject(request);
                    }
                }
            }
        }

        public void reviveskele(On.RoR2.CharacterBody.orig_OnDeathStart orig, CharacterBody self) {
            if (NetworkServer.active) {
                if (self.baseNameToken == "GOTCE_SKELETON_NAME") {
                    Transform target = PlayerCharacterMasterController.instances[UnityEngine.Random.RandomRange(0, PlayerCharacterMasterController.instances.Count)].master.GetBody().transform;
                    if (Difficulty.IsCurrentDifHigherOrEqual(Difficulty.c8, Run.instance) && target) {
                        Debug.Log("SkeleArtifact: Reviving Skeleton");
                        DirectorPlacementRule rule = new();
                        rule.placementMode = DirectorPlacementRule.PlacementMode.Random;
                        DirectorSpawnRequest request = new(isc, rule, Run.instance.spawnRng);
                        request.teamIndexOverride = TeamIndex.Void;
                        DirectorCore.instance.TrySpawnObject(request);
                    }
                }
            }
            orig(self);
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.prefab = prefabMaster;
            isc.directorCreditCost = 100000000;
            isc.name = "cscSkele";
            isc.hullSize = HullClassification.Golem;
            isc.sendOverNetwork = true;
            isc.noElites = true;
        }

        public override void PostCreation()
        {
            base.PostCreation();
            RegisterEnemy(prefab, prefabMaster, null);
        }

        public class SkeleState : BaseState {
        private float stopwatch = 0f;
        private float delay = 1.5f;
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= delay && base.fixedAge >= 5f) {
                stopwatch = 0f;
                if (NetworkServer.active) {
                    EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXQuick"), new EffectData
                    {
                        origin = base.characterBody.corePosition,
                        scale = 8f,
                        rotation = Quaternion.identity
                    }, transmit: true);

                    BlastAttack blast = new()
                    {
                        attacker = base.gameObject,
                        baseDamage = 1,
                        radius = 8f,
                        inflictor = null,
                        falloffModel = BlastAttack.FalloffModel.None,
                        crit = true,
                        position = base.characterBody.corePosition,
                        procCoefficient = 0f,
                        damageColorIndex = DamageColorIndex.Item,
                        teamIndex = TeamIndex.Void,
                        damageType = DamageType.VoidDeath
                    };
                    blast.Fire();
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
    }
}