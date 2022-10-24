using R2API;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using System.Linq;

namespace GOTCE.Enemies.Standard
{
    public class LivingSuppressiveFire : EnemyBase<LivingSuppressiveFire>
    {
        public override string PathToClone => "RoR2/Base/Wisp/WispBody.prefab";
        public override string CloneName => "LivingSuppressiveFire";
        public override string PathToCloneMaster => "RoR2/Base/Wisp/WispMaster.prefab";
        public CharacterBody body;
        public CharacterMaster master;

        public delegate Vector3 orig_aimOrigin(InputBankTest self);

        public override void CreatePrefab()
        {
            base.CreatePrefab();
            body = prefab.GetComponent<CharacterBody>();
            body.baseArmor = 0;
            body.attackSpeed = 1f;
            body.damage = 3.5f;
            body.baseMaxHealth = 110f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_LIVINGSUPPRESSIVEFIRE_NAME";
            body.baseRegen = 0f;
            body.portraitIcon = Main.MainAssets.LoadAsset<Texture2D>("Assets/Textures/Icons/Enemies/LivingSuppressiveFire.png");
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 40;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.None;
            isc.hullSize = HullClassification.Human;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Air;
            isc.sendOverNetwork = true;
            isc.prefab = prefabMaster;
            isc.name = "cscLivingSuppressiveFire";
        }

        public override void AddDirectorCard()
        {
            base.AddDirectorCard();
            card.minimumStageCompletions = 1;
            card.selectionWeight = 1;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
        }

        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();

            if (!prefab.GetComponent<TeamComponent>())
            {
                TeamComponent team = prefab.AddComponent<TeamComponent>();
                team.teamIndex = TeamIndex.Monster;
            }
            // prefab.GetComponent<TeamComponent>().teamIndex = TeamIndex.Monster;
            // SwapMaterials(prefab, Main.MainAssets.LoadAsset<Material>("Assets/Materials/Enemies/kirnMaterial.mat"), true, null);
            // DisableMeshes(prefab, new List<int> { 1 });
            // SwapMeshes(prefab, Utils.MiscUtils.GetMeshFromPrimitive(PrimitiveType.Sphere), true);

            body.GetComponent<SphereCollider>().radius = 3f;

            GameObject model = Main.MainAssets.LoadAsset<GameObject>("Assets/Models/Prefabs/Enemies/LivingSuppressiveFire/mdlKirn.prefab");
            CapsuleCollider box1 = model.transform.Find("BarrelHitbox").gameObject.GetComponent<CapsuleCollider>();
            CapsuleCollider box2 = model.transform.Find("HandleHitbox").gameObject.GetComponent<CapsuleCollider>();
            CapsuleCollider boxweak = model.transform.Find("WeakPointHitbox").gameObject.GetComponent<CapsuleCollider>();

            SetupModel(prefab, model);
            SetupHurtbox(prefab, model, box1, 0);
            SetupHurtbox(prefab, model, box2, 1);
            SetupHurtbox(prefab, model, boxweak, 2, true, HurtBox.DamageModifier.SniperTarget);

            model.GetComponent<HurtBoxGroup>().hurtBoxes = new HurtBox[] {
                box1.gameObject.GetComponent<HurtBox>(),
                box2.gameObject.GetComponent<HurtBox>(),
                boxweak.gameObject.GetComponent<HurtBox>()
            };

            box1.transform.localScale = new Vector3(0.2f, 0.7f, 0.3f);

            if (!model.GetComponent<ChildLocator>())
            {
                model.AddComponent<ChildLocator>();
            }

            ChildLocator locator = model.GetComponent<ChildLocator>();
            locator.transformPairs = new ChildLocator.NameTransformPair[1];
            locator.transformPairs[0].name = "Muzzle";
            locator.transformPairs[0].transform = model.transform.Find("BarrelHitbox");

            model.GetComponent<HurtBoxGroup>().bullseyeCount = 1;
            model.GetComponent<HurtBoxGroup>().mainHurtBox = box1.gameObject.GetComponent<HurtBox>();

            var livingSuppMesh = model.transform.GetChild(0).transform;
            Transform barrel = model.transform.GetChild(1).transform;
            var handle = model.transform.GetChild(2).transform;
            var weakpoint = model.transform.GetChild(3).transform;

            AISkillDriver FleeAndAttack = (from x in master.GetComponents<AISkillDriver>()
                                           where x.maxDistance == 20
                                           select x).First();
            FleeAndAttack.maxDistance = 25f;

            AISkillDriver StrafeAndAttack = (from x in master.GetComponents<AISkillDriver>()
                                             where x.maxDistance == 30
                                             select x).First();
            StrafeAndAttack.minDistance = 25f;
            StrafeAndAttack.maxDistance = 55f;

            AISkillDriver What = (from x in master.GetComponents<AISkillDriver>()
                                  where x.maxDistance == 40
                                  select x).First();
            What.maxDistance = 55f;

            SkillLocator sl = prefab.GetComponentInChildren<SkillLocator>();
            ReplaceSkill(sl.primary, Skills.Consistency.Instance.SkillDef);

            LanguageAPI.Add("GOTCE_LIVINGSUPPRESSIVEFIRE_NAME", "Living Suppressive Fire");
            LanguageAPI.Add("GOTCE_LIVINGSUPPRESSIVEFIRE_LORE", "\"I-is that a fucking floating gun?\"\n\"What are you talking abou- wait what the fuck how is this happening?\"\n\"I genuinely have no idea? Wh-what is it even doing there?\"\n\"It seems to be searching for something? Is it alive?\"\n\"I guess so? This planet is fucking crazy ma- oh shit I think it heard us.\"\n\"Oh fuck you're right it's coming this way.\"\n<i>I value survivability and consistency...</i>\n\"What the fuck? It can talk? And what's it saying about survivability and consist-\"\n<i>Too inconsistent!</i>\nThe two were then promplty shredded to bits as the gun unleashed a suppressing flood of bullets upon them.");
            LanguageAPI.Add("GOTCE_LIVINGSUPPRESSIVEFIRE_SUBTITLE", "Horde of Many");
        }

        public override void PostCreation()
        {
            base.PostCreation();
            RegisterEnemy(prefab, prefabMaster, null, DirectorAPI.MonsterCategory.BasicMonsters, true);
        }

        public static Vector3 InputBankTest_aimOrigin_Get(orig_aimOrigin orig, InputBankTest self)
        {
            if (self.characterBody)
            {
                if (self.characterBody.aimOriginTransform)
                {
                    return self.characterBody.aimOriginTransform.position;
                }
            }
            return self.transform.position;
        }
    }
}