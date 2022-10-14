using R2API;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;

namespace GOTCE.Enemies.Standard
{
    public class LivingSuppressiveFire : EnemyBase<LivingSuppressiveFire>
    {
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
            body.damage = 5.5f;
            body.levelDamage = 1.1f;
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
            card.minimumStageCompletions = 2;
            card.selectionWeight = 1;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
        }

        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = prefab;

            // SwapMaterials(prefab, Main.MainAssets.LoadAsset<Material>("Assets/Materials/Enemies/kirnMaterial.mat"), true, null);
            // DisableMeshes(prefab, new List<int> { 1 });
            // SwapMeshes(prefab, Utils.MiscUtils.GetMeshFromPrimitive(PrimitiveType.Sphere), true);

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

            model.GetComponent<HurtBoxGroup>().bullseyeCount = 1;
            model.GetComponent<HurtBoxGroup>().mainHurtBox = box1.gameObject.GetComponent<HurtBox>();

            foreach (AISkillDriver driver in prefabMaster.GetComponentsInChildren<AISkillDriver>())
            {
                driver.maxDistance = 120f;
                driver.minDistance = 30f;
                driver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
                driver.activationRequiresAimTargetLoS = true;
                driver.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;
                driver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
                driver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
                driver.skillSlot = SkillSlot.Primary;
            }

            SkillLocator sl = prefab.GetComponentInChildren<SkillLocator>();
            ReplaceSkill(sl.primary, Skills.Consistency.Instance.SkillDef);

            LanguageAPI.Add("GOTCE_LIVINGSUPPRESSIVEFIRE_NAME", "Living Suppressive Fire");
            LanguageAPI.Add("GOTCE_LIVINGSUPPRESSIVEFIRE_LORE", "Even if frags did 2000% with no falloff...");
            LanguageAPI.Add("GOTCE_LIVINGSUPPRESSIVEFIRE_SUBTITLE", "Horde of Many");
            RegisterEnemy(prefab, prefabMaster, null, DirectorAPI.MonsterCategory.BasicMonsters, true);
        }
    }
}