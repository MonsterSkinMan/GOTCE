using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace GOTCE.Stages {
    public class FeedbackDiscussion : StageBase<FeedbackDiscussion> {
        public override string LangTokenName => "FeedbackDiscussion";
        public override string SceneName => "feedbackdiscussion";
        public override string SceneSubtitle => "100% is EXTREMELY LOW.";
        public override string SceneDisplayName => "Woolie Dimension";
        public override string SceneLore => "Directive: Inject does more dps than mando m1 at range";
        public override SceneType SceneType => SceneType.Stage;
        public override SceneCollection DestinationGroup => Addressables.LoadAssetAsync<SceneCollection>("RoR2/Base/SceneGroups/sgStage1.asset").WaitForCompletion();
        public override MusicTrackDef BossTrack => Addressables.LoadAssetAsync<MusicTrackDef>("RoR2/Base/Common/muSong04.asset").WaitForCompletion();
        public override MusicTrackDef MainTrack => Addressables.LoadAssetAsync<MusicTrackDef>("RoR2/Base/Common/muSong04.asset").WaitForCompletion();
        public override GameObject DioramaPrefab => Main.MainAssets.LoadAsset<GameObject>("Assets/Models/Prefabs/Item/Drill/Cube.prefab");
        public override string dccsInteractableClone => "RoR2/Base/arena/dpArenaInteractables.asset";
        // public override string dccsMonsterClone => "RoR2/Base/arena/dpArenaMonsters.asset";
        public override bool ShouldIncludeInLogbook => false;

        public override void Hooks()
        {
            base.Hooks();
            On.RoR2.Chat.AddMessage_string += (orig, str) => {
                orig(str);
                if (str.ToLower().Contains("100% is extremely low")) {
                    if (Stage.instance) {
                        Stage.instance.BeginAdvanceStage(sceneDef);
                    }
                }
            };
        }

        public override void ModifySceneInfo(ClassicStageInfo info)
        {
            DirectorCard card = new DirectorCard();
            card.minimumStageCompletions = 0;
            card.spawnCard = Enemies.Bosses.CrowdfunderWoolie.Instance.isc;
            card.selectionWeight = 1;
            info.monsterDccsPool.poolCategories[0].alwaysIncluded[0].dccs.AddCard(0, Enemies.Bosses.CrowdfunderWoolie.Instance.card);
            info.monsterDccsPool.poolCategories[0].alwaysIncluded[0].dccs.AddCard(0, Enemies.Standard.LivingSuppressiveFire.Instance.card);
        }
    }
}