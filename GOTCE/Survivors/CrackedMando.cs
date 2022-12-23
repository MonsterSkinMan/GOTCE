using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using System;
using System.Collections.Generic;
using EntityStates;
using GOTCE.Skills;
using RoR2.ExpansionManagement;
using GOTCE.Achievements.CrackedCommando;
using GOTCE.EntityStatesCustom;

namespace GOTCE.Survivors
{
    public class CrackedMando : SurvivorBase<CrackedMando>
    {
        public override string bodypath => "Assets/Prefabs/Survivors/Crackmando/CrackmandoBody.prefab";
        public override string name => "CrackedCommandoBody";
        public override bool clone => false;

        public override void Modify()
        {
            base.Modify();
            CharacterBody body = prefab.GetComponent<CharacterBody>();
            body.preferredPodPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/RoboCratePod.prefab").WaitForCompletion();
            body._defaultCrosshairPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/SimpleDotCrosshair.prefab").WaitForCompletion();
            prefab.GetComponent<CameraTargetParams>().cameraParams = Addressables.LoadAssetAsync<CharacterCameraParams>("RoR2/Base/Common/ccpStandard.asset").WaitForCompletion();

            // prefab.transform.Find("Model Base").Find("CrackModel").GetComponentInChildren<CharacterModel>().itemDisplayRuleSet = Addressables.LoadAssetAsync<ItemDisplayRuleSet>("RoR2/Base/Commando/idrsCommando.asset").WaitForCompletion();

            EntityStateMachine esm = AddESM(prefab, "Flight", new SerializableEntityStateType(typeof(Idle)));
            
            SkillLocator sl = prefab.GetComponent<SkillLocator>();
            ReplaceSkill(sl.special, SuppressiveNader.Instance.SkillDef);
            ReplaceSkill(sl.primary, DoubleDoubleTap.Instance.SkillDef);
            ReplaceSkill(sl.secondary, PhaseRounder.Instance.SkillDef);
            ReplaceSkill(sl.utility, VeryTactical.Instance.SkillDef);

            LanguageAPI.Add("GOTCE_CRACKMANDO_NAME", "Cracked Commando");
            LanguageAPI.Add("GOTCE_CRACKMANDO_DESC", "Cracked Commando is a jack of all trades survivor who can do a bit of everything, whether that be destroying multiplayer lobbies with Suppressive Nader or outdamaging railgunner with his Double Double Double Double Tap.");
            LanguageAPI.Add("GOTCE_CRACKMANDO_SUBTITLE", "Delirious Dragoon");
            LanguageAPI.Add("GOTCE_CRACKMANDO_WIN", "And so he left, the cacophony of voices ringing throughout his skull.");
            LanguageAPI.Add("GOTCE_CRACKMANDO_FAIL", "And so he vanished, with terminal lucidity.");
            LanguageAPI.Add("GOTCE_CRACKMANDO_LORE", "A bloodbath.\nThat's all he could describe it as.\nJust five minutes earlier, the soldier and his squadron were ambushed by some mysterious orange being, and proceeded to wipe out all of the Commando's comrades. He was left there, in a sea of their remains, intestines strewn across his limbs, as the entity approached him.\n...\n...\nIt stared at him, curiously. Perhaps this one had potential?\nShrugging, the orange cat... thing split open its face, revealing the true form underneath, firing a wave of raw data and code at the lone survivor. The Commando screamed out in pain as he was bombarded with corruption, and he began to absorb the carnage around him.\nWhen it finally stopped, the Commando had been transformed into a 32-armed monster, and a monstrous face had opened up in his chest. He could hear the voices of the tortured souls of his allies whispering and screaming in his head, and his body began to shatter.\nNoticing this, Cracked Emoji put a pair of sunglasses onto the Commando's helmet, and his mouth curled into a satisfied smirk. This one would do.\nWith his first harbinger of destruction created, he disappeared, leaving the delirious Cracked Commando to his own devices.");

            SkillFamily skillFamily;
            skillFamily = sl.primary.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = Skills.SuppressiveBarrage.Instance.SkillDef,
                unlockableDef = SuppressiveBarrageUnlock.Instance.enabled ? SuppressiveBarrageUnlock.Instance.def : null,
                viewableNode = new ViewablesCatalog.Node(Skills.SuppressiveBarrage.Instance.SkillDef.skillNameToken, false, null)
            };

            GameObject umbra = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoMonsterMaster.prefab").WaitForCompletion().InstantiateClone("CrackmandoMonsterMaster");
            umbra.GetComponent<CharacterMaster>().bodyPrefab = prefab;
            ContentAddition.AddMaster(umbra);
            ContentAddition.AddEntityState<EntityStatesCustom.CrackedMando.DoubleDoubleTap>(out bool _);
            ContentAddition.AddEntityState<EntityStatesCustom.CrackedMando.SuppressiveBarrage>(out bool _);
            ContentAddition.AddEntityState<EntityStatesCustom.CrackedMando.PhaseRounder>(out bool _);
            ContentAddition.AddEntityState<EntityStatesCustom.CrackedMando.VeryTactical>(out bool _);
            ContentAddition.AddEntityState<EntityStatesCustom.CrackedMando.SuppressiveNader>(out bool _);
        }

        public override void PostCreation()
        {
            base.PostCreation();
            SurvivorDef surv = new SurvivorDef
            {
                bodyPrefab = prefab,
                descriptionToken = "GOTCE_CRACKMANDO_DESC",
                displayPrefab = Main.SecondaryAssets.LoadAsset<GameObject>("Assets/Prefabs/Survivors/Crackmando/Model/CrackMandoDisplay.prefab"),
                primaryColor = Color.yellow,
                cachedName = "GOTCE_CRACKMANDO_NAME",
                unlockableDef = SurvivorUnlock.Instance.enabled ? SurvivorUnlock.Instance.def : null,
                desiredSortPosition = 16,
                mainEndingEscapeFailureFlavorToken = "GOTCE_CRACKMANDO_FAIL",
                outroFlavorToken = "GOTCE_CRACKMANDO_WIN",
            };

            ContentAddition.AddBody(prefab);
            ContentAddition.AddSurvivorDef(surv);
        }
    }
} 