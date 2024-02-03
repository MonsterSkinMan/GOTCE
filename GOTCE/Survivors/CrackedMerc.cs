using System;
using RoR2;
using UnityEngine;

namespace GOTCE.Survivors {
    public class CrackedMerc : SurvivorBase<CrackedMerc> {
        public override string bodypath => Utils.Paths.GameObject.MercBody;
        public override string name => "CrackedMercBody";
        public override bool clone => true;
        public static GameObject DisplayPrefab;
        public static Texture2D texCrecDiffuse;
        public static Material matCrecDiffuse;

        public override void Modify()
        {
            base.Modify();

            Material mercMat = Utils.Paths.Material.matMerc.Load<Material>();

            texCrecDiffuse = Main.SecondaryAssets.LoadAsset<Texture2D>("crecDiffuse.png");
            matCrecDiffuse = DuplicateMat(mercMat, "matCrec", texCrecDiffuse);

            GameObject dml = Utils.Paths.GameObject.DisplayMissileRack.Load<GameObject>();

            CharacterBody body = prefab.GetComponent<CharacterBody>();
            body.baseNameToken = "GOTCE_CRACKEDMERC_NAME".Add("Cracked Mercenary");
            body.bodyColor = Color.gray;
            body.baseDamage = 14f;
            body.baseJumpCount = 1;

            SkillLocator locator = prefab.GetComponent<SkillLocator>();
            locator.passiveSkill.skillNameToken = "GOTCE_CRACKEDMERC_PASSIVE_NAME".Add("The OP Build");
            locator.passiveSkill.skillDescriptionToken = "GOTCE_CRACKEDMERC_PASSIVE_DESC".Add("Your <style=cIsDamage>base damage</style> is increased by <style=cIsUtility>10%</style> for every <style=cIsDamage>Berserker's Pauldron</style> you have.");
            locator.passiveSkill.icon = null;

            LanguageAPI.Add("GOTCE_CRACKEDMERC_SUBTITLE", "Ballistic Brute");
            LanguageAPI.Add("GOTCE_CRACKEDMERC_WIN", "And so he left, fuse still ticking.");
            LanguageAPI.Add("GOTCE_CRACKEDMERC_FAIL", "And so he vanished, disposed once and for all.");
            LanguageAPI.Add("GOTCE_CRACKEDMERC_DESC", "TBD");
            LanguageAPI.Add("GOTCE_CRACKEDMERC_LORE", "His friends always told him his greed would one day be his downfall. He would usually brush them off as ignorant, lazy, or stubborn. He deserved happiness, after all, and what's wrong with getting things to make you happy? But it was never enough. He didn't care what he got, whether it be conceptual, physical, emotional. He just wanted... <b>more</b>.\nHis obsession eventually turned him ballistic.He turned on his few remaining friends, taking everything they had for himself.He decorated himself in gaudy trash and worthless junk, putting the rotten fruits of his endeavors on full display.\nThat was why, when he saw that strange orange cat, he accepted its offer. He needed to know. He needed its power.\nSeeing him for the trash he was, the cat fused him with the garbage lining his form as an ironic punishment. And as for his desires for more, he was cursed with knowledge about everything and everywhere in the entire universe. By knowing everywhere he wasn't, he would know his place in this world at all times.\nThe only thing he didn't know was for what purpose he had been left so broken. You see, the cat had cursed him for more than just spite. The cat broke him for it wanted to break the universe. And those with broken wills were the easiest to coerce to do his bidding.");


            LanguageAPI.Add("GOTCE_DisposableVisions_NAME", "Visions of Disposability");
            LanguageAPI.Add("GOTCE_DisposableVisions_DESC", "Fire a <style=cIsUtility>missile</style> for <style=cIsDamage>300% damage</style>. <style=cIsUtility>Hold up to 12</style>.");

            LanguageAPI.Add("GOTCE_DisposableRising_NAME", "Rising Disposable Thunder Surge");
            LanguageAPI.Add("GOTCE_DisposableRising_DESC", "Propel yourself into the air, <style=cIsDamage>slashing for 300% damage</style> and <style=cIsDamage>firing a missile for 300% damage</style>.");

            LanguageAPI.Add("GOTCE_DisposableEgg_NAME", "Volcanic Disposable Missile Launcher Assault");
            LanguageAPI.Add("GOTCE_DisposableEgg_DESC", "Transform into a blazing fast missile, <style=cIsDamage>dealing 100,000% damage</style> on detonation.");

            LanguageAPI.Add("GOTCE_DisposableEvis_NAME", "Slicing Eviscerating Unstable Strides of Windy Disposability");
            LanguageAPI.Add("GOTCE_DisposableEvis_DESC", "Transform into a <style=cIsUtility>storm of blades</style>, repeatedly <style=cIsDamage>striking for 300%</style> damage and <style=cIsUtility>firing missiles</style>.");

            ModelLocator model = prefab.GetComponent<ModelLocator>();
            SwapMaterials(prefab, mercMat, matCrecDiffuse);
            SwapMaterials(prefab, matCrecDiffuse, Utils.Paths.Material.matGupBody.Load<Material>(), true);

            ChildLocator loc = model.modelTransform.GetComponent<ChildLocator>();

            Transform head = loc.FindChild("Head");
            head.gameObject.AddComponent<ScaleNullifer>();

            GameObject headDml = GameObject.Instantiate<GameObject>(dml, head);
            headDml.transform.localPosition = new Vector3(0, -20f, 0);
            headDml.transform.localRotation = Quaternion.Euler(-20, 180, 0);
            headDml.transform.localScale = 100f * Vector3.one;
            headDml.transform.Find("mdlMissileRack").Find("MissileRackArmature").Find("Base").Find("Arm1.l").localScale = new Vector3(0.01f, 0.01f, 0.01f);
            headDml.transform.Find("mdlMissileRack").Find("MissileRackArmature").Find("Base").Find("Arm1.r").localScale = new Vector3(0.01f, 0.01f, 0.01f);

            Transform chest = loc.FindChild("Chest");

            GameObject chestDml = GameObject.Instantiate<GameObject>(dml, chest);
            chestDml.transform.localPosition = new Vector3(0, 0.2f, 0);
            chestDml.transform.localRotation = Quaternion.Euler(90, 180, 0);
            chestDml.transform.Find("mdlMissileRack").Find("MissileRackArmature").Find("Base").Find("Arm1.l").localScale = new Vector3(0.01f, 0.01f, 0.01f);
            chestDml.transform.Find("mdlMissileRack").Find("MissileRackArmature").Find("Base").Find("Arm1.r").localScale = new Vector3(0.01f, 0.01f, 0.01f);

            Transform sword = chest.Find("SwingCenter").Find("SwordBase");

            GameObject swordDml = GameObject.Instantiate<GameObject>(dml, sword);
            swordDml.transform.localPosition = new Vector3(0, -0.1f, -0.1f);
            swordDml.transform.Find("mdlMissileRack").Find("MissileRackArmature").Find("Base").Find("Arm1.l").localScale = new Vector3(0.01f, 0.01f, 0.01f);
            swordDml.transform.Find("mdlMissileRack").Find("MissileRackArmature").Find("Base").Find("Arm1.r").localScale = new Vector3(0.01f, 0.01f, 0.01f);
            

            sword.Find("SwordLength.1").gameObject.AddComponent<ScaleNullifer>();
            sword.Find("Point Light").gameObject.SetActive(false);

            GameObject.Destroy(model.modelTransform.GetComponent<ModelSkinController>());

            ReplaceSkill(locator.primary, Skills.DisposableVisions.Instance.SkillDef);
            ReplaceSkill(locator.secondary, Skills.DisposableRising.Instance.SkillDef);
            ReplaceSkill(locator.utility, Skills.DisposableEgg.Instance.SkillDef);
            ReplaceSkill(locator.special, Skills.DisposableEvis.Instance.SkillDef);

            RecalculateStatsAPI.GetStatCoefficients += (self, args) =>
            {
                if (self.baseNameToken != "GOTCE_CRACKEDMERC_NAME") return;

                args.damageMultAdd += 0.1f * self.inventory.GetItemCount(RoR2Content.Items.WarCryOnMultiKill);
            };

            DisplayPrefab = PrefabAPI.InstantiateClone(model.modelTransform.gameObject, "CrackedMercDisplay", false);
        }

        public class ScaleNullifer : MonoBehaviour
        {
            public void Update()
            {
                base.transform.localScale = 0.01f * Vector3.one;
            }

            public void LateUpdate()
            {
                base.transform.localScale = 0.01f * Vector3.one;
            }
        }

        public override void PostCreation()
        {
            base.PostCreation();
            SurvivorDef surv = new SurvivorDef
            {
                bodyPrefab = prefab,
                descriptionToken = "GOTCE_CRACKEDMERC_DESC",
                displayPrefab = DisplayPrefab,
                primaryColor = Color.yellow,
                cachedName = "GOTCE_CRACKEDMERC_NAME",
                unlockableDef = null,
                desiredSortPosition = 16,
                mainEndingEscapeFailureFlavorToken = "GOTCE_CRACKEDMERC_FAIL",
                outroFlavorToken = "GOTCE_CRACKEDMERC_WIN",
            };

            ContentAddition.AddBody(prefab);
            ContentAddition.AddSurvivorDef(surv);
        }
    }
}