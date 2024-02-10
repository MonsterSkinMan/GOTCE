using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using System;
using System.Collections.Generic;
using EntityStates;
using GOTCE.Skills;
using RoR2.ExpansionManagement;
using GOTCE.EntityStatesCustom;

namespace GOTCE.Survivors {
    public class NemesisHealingCore : SurvivorBase<NemesisHealingCore> {
        public override string bodypath => Utils.Paths.GameObject.AffixEarthHealerBody;
        public override string name => "NemesisHealingCoreBody";
        public override bool clone => true;
        public static GameObject affixEarthHealerModel;

        public override void Modify()
        {
            base.Modify();
            CharacterBody body = prefab.GetComponent<CharacterBody>();
            body.baseNameToken = "GOTCE_NEMCORE_NAME".Add("Nemesis Healing Core");
            body.bodyColor = Color.red;
            body.portraitIcon = Main.SecondaryAssets.LoadAsset<Sprite>("NemesisHealingCore.png").texture;
            
            CameraTargetParams ctp = prefab.AddComponent<CameraTargetParams>();
            ctp.cameraParams = Utils.Paths.CharacterCameraParams.ccpStandard.Load<CharacterCameraParams>();

            SkillLocator locator = prefab.GetComponent<SkillLocator>();

            EntityStateMachine bodyEsm = prefab.GetComponent<EntityStateMachine>();
            bodyEsm.initialStateType = new(typeof(EntityStates.GenericCharacterMain));
            bodyEsm.mainStateType = new(typeof(EntityStates.GenericCharacterMain));

            AddESM(prefab, "Weapon", new(typeof(EntityStates.Idle)));

            prefab.AddComponent<EquipmentSlot>();

            GenericSkill slot = prefab.AddComponent<GenericSkill>();
            slot.skillName = "NemPrimary";

            locator.primary = slot;

            SwapMaterials(prefab, Utils.Paths.Material.matVoidBlinkBodyOverlayCorrupted.Load<Material>(), true);

            ReplaceSkill(slot, Skills.Collapse.Instance.SkillDef);

            affixEarthHealerModel = PrefabAPI.InstantiateClone(prefab.GetComponent<ModelLocator>().modelBaseTransform.gameObject, "nemcore");
            affixEarthHealerModel.transform.GetChild(0).localPosition = new(0, 1.5f, 0);
        }

        public override void PostCreation()
        {
            base.PostCreation();
            SurvivorDef surv = new SurvivorDef
            {
                bodyPrefab = prefab,
                descriptionToken = "GOTCE_NEMCORE_DESC".Add("what did you expect"),
                displayPrefab = affixEarthHealerModel,
                primaryColor = Color.red,
                cachedName = "GOTCE_NEMCORE_NAME",
                unlockableDef = null,
                desiredSortPosition = 16,
                mainEndingEscapeFailureFlavorToken = "GOTCE_NEMCORE_FAIL".Add("nor this"),
                outroFlavorToken = "GOTCE_NEMCORE_WIN".Add("you werent supposed to see this"),
            };

            ContentAddition.AddBody(prefab);
            ContentAddition.AddSurvivorDef(surv);
        }
    }
}