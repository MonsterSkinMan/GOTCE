using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using System;
using System.Collections.Generic;
using EntityStates;
using GOTCE.Skills;
using RoR2.ExpansionManagement;

namespace GOTCE.Survivors {
    public class CrackedMando : SurvivorBase<CrackedMando> {
        public override string bodypath => "Assets/Prefabs/Survivors/Crackmando/CrackmandoBody.prefab";
        public override void Modify()
        {
            base.Modify();
            SkillLocator sl = prefab.GetComponent<SkillLocator>();
            ReplaceSkill(sl.special, SuppressiveNader.Instance.SkillDef);
            ReplaceSkill(sl.primary, DoubleDoubleTap.Instance.SkillDef);
            ReplaceSkill(sl.secondary, PhaseRounder.Instance.SkillDef);
            ReplaceSkill(sl.utility, VeryTactical.Instance.SkillDef);
            
            prefab.transform.Find("Model Base").transform.Find("CrackModel").transform.Find("hurtbox").GetComponent<HurtBox>().gameObject.layer = LayerIndex.entityPrecise.intVal;

            LanguageAPI.Add("GOTCE_CRACKMANDO_NAME", "Cracked Commando");
            LanguageAPI.Add("GOTCE_CRACKMANDO_DESC", "the");
            LanguageAPI.Add("GOTCE_CRACKMANDO_SUBTITLE", "Harbinger of the Cracked Emoji");
            LanguageAPI.Add("GOTCE_CRACKMANDO_WIN", "so cracked");
            LanguageAPI.Add("GOTCE_CRACKMANDO_FAIL", "so cracked");
        }

        public override void PostCreation()
        {
            base.PostCreation();
            SurvivorDef surv = new SurvivorDef
            {
                bodyPrefab = prefab,
                descriptionToken = "GOTCE_CRACKMANDO_DESC",
                displayPrefab = prefab.transform.Find("Model Base").transform.Find("CrackModel").gameObject,
                primaryColor = Color.yellow,
                cachedName = "GOTCE_CRACKMANDO_NAME",
                unlockableName = "Logs.Stages.limbo",
                desiredSortPosition = 12
            };

            ContentAddition.AddBody(prefab);
            ContentAddition.AddSurvivorDef(surv);
        }
    }
}