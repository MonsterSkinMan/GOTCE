using R2API;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using System.Linq;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System;
using MonoMod.RuntimeDetour;

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
            body.damage = 5.5f;
            body.levelDamage = 1.1f;
            body.baseMaxHealth = 110f;
            body.levelMaxHealth = 33f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_LIVINGSUPPRESSIVEFIRE_NAME";
            body.baseRegen = 0f;
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
            card.minimumStageCompletions = 1;
            card.selectionWeight = 1;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
        }

        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = prefab;
            prefab.GetComponent<TeamComponent>().teamIndex = TeamIndex.Monster;
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

            model.GetComponent<HurtBoxGroup>().bullseyeCount = 1;
            model.GetComponent<HurtBoxGroup>().mainHurtBox = box1.gameObject.GetComponent<HurtBox>();

            var livingSuppMesh = model.transform.GetChild(0).transform;
            var barrel = model.transform.GetChild(1).transform;
            var handle = model.transform.GetChild(2).transform;
            var weakpoint = model.transform.GetChild(3).transform;
            /* livingSuppMesh.localPosition = new Vector3(0.15f, 0.05f, 0f);
            barrel.localPosition = new Vector3(0.5f, 0.39f, 0.24f);
            barrel.localScale = new Vector3(0.5f, 0.7f, 0.19f); */
            // handle.gameObject.SetActive(false);
            // position is absolute position
            // localPosition is parent position + local position

            // make an empty in the model hierarchy, place it in the barrel and make it the model transform in consistency state so it actually fires from the barrel

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
            LanguageAPI.Add("GOTCE_LIVINGSUPPRESSIVEFIRE_LORE", "Even if frags did 2000% with no falloff...");
            LanguageAPI.Add("GOTCE_LIVINGSUPPRESSIVEFIRE_SUBTITLE", "Horde of Many");
            RegisterEnemy(prefab, prefabMaster, null, DirectorAPI.MonsterCategory.BasicMonsters, true);

            /* IL.RoR2.InputBankTest.GetAimRay += (il) => {
                ILCursor c = new ILCursor(il);
                c.GotoNext(
                    x => x.MatchLdarg(0),
                    x => x.MatchCallOrCallvirt(typeof(Vector3), "get_aimOrigin")
                );
                c.Index += 1;
                c.Remove();
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Ldfld, typeof(InputBankTest).GetField("characterBody"));
                c.EmitDelegate<Func<CharacterBody, Vector3>>((cb) => {
                    return cb.transform.position;
                });
            }; */
        }

        /* private Ray the(On.EntityStates.BaseState.orig_GetAimRay orig, EntityStates.BaseState self) {
            return new Ray(self.transform.position, self.transform.forward);
        } */

        public static Vector3 InputBankTest_aimOrigin_Get(orig_aimOrigin orig, InputBankTest self) {
            return self.transform.position;
        }
    }
}