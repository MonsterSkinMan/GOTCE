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
        public override string bodypath => "RoR2/Base/Commando/CommandoBody.prefab";
        public override string name => "CrackedCommando";
        public override bool clone => true;
        public override void Modify()
        {
            base.Modify();
            CharacterBody body = prefab.GetComponent<CharacterBody>();
            body.baseDamage = 12;
            body.baseMaxHealth = 150;
            body.baseMoveSpeed = 7;
            body.baseCrit = 1f;
            body.baseNameToken = "GOTCE_CRACKMANDO_NAME";
            body.bodyColor = Color.yellow;
            body.portraitIcon = SuppressiveNader.Instance.Icon.texture;

            EntityStateMachine esm = AddESM(prefab, "Flight", new SerializableEntityStateType(typeof(Idle)));
            SkillLocator sl = prefab.GetComponent<SkillLocator>();
            ReplaceSkill(sl.special, SuppressiveNader.Instance.SkillDef);
            ReplaceSkill(sl.primary, DoubleDoubleTap.Instance.SkillDef);
            ReplaceSkill(sl.secondary, PhaseRounder.Instance.SkillDef);
            ReplaceSkill(sl.utility, VeryTactical.Instance.SkillDef);

            GameObject model = prefab.transform.Find("ModelBase").transform.Find("mdlCommandoDualies").gameObject;
            SwapMaterials(model, Main.SecondaryAssets.LoadAsset<Material>("Assets/Prefabs/Survivors/Crackmando/Materials/body.mat"), true);
            GameObject.DestroyImmediate(model.GetComponentInChildren<ModelSkinController>());

            LanguageAPI.Add("GOTCE_CRACKMANDO_NAME", "Cracked Commando");
            LanguageAPI.Add("GOTCE_CRACKMANDO_DESC", "Cracked Commando is a jack of all trades survivor who can do a bit of everything, whether that be destroying multiplayer lobbies with Suppressive Nader or outdamaging railgunner with his Double Double Double Double Tap.");
            LanguageAPI.Add("GOTCE_CRACKMANDO_SUBTITLE", "Harbinger of the Cracked Emoji");
            LanguageAPI.Add("GOTCE_CRACKMANDO_WIN", "And so he left, securing his crackedness...");
            LanguageAPI.Add("GOTCE_CRACKMANDO_FAIL", "And so he vanished, going from cracked to shattered...");

            SkillFamily skillFamily;
            skillFamily = sl.primary.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.SuppressiveBarrage.Instance.SkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(Skills.SuppressiveBarrage.Instance.SkillDef.skillNameToken, false, null)
            };
        }

        public override void PostCreation()
        {
            base.PostCreation();
            SurvivorDef surv = new SurvivorDef
            {
                bodyPrefab = prefab,
                descriptionToken = "GOTCE_CRACKMANDO_DESC",
                displayPrefab = prefab.transform.Find("ModelBase").transform.Find("mdlCommandoDualies").gameObject,
                primaryColor = Color.yellow,
                cachedName = "GOTCE_CRACKMANDO_NAME",
                unlockableName = "Logs.Stages.limbo",
                desiredSortPosition = 14,
                mainEndingEscapeFailureFlavorToken = "GOTCE_CRACKMANDO_FAIL",
                outroFlavorToken = "GOTCE_CRACKMANDO_WIN"
            };

            ContentAddition.AddBody(prefab);
            ContentAddition.AddSurvivorDef(surv);
        }
    }
}