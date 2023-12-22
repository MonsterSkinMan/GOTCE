using System;
using UnityEngine.SceneManagement;
using RoR2;

namespace GOTCE.Stages {
    public class CommencementOld : StageBase<CommencementOld>
    {
        public override string LangTokenName => "GOTCE_Cessation";

        public override SceneType SceneType => SceneType.Stage;

        public override string SceneDisplayName => "Cessation";

        public override string SceneSubtitle => "!tag t";

        public override string SceneLore => "THE VOICES ARE SO FUCKING LOUD";

        public override string SceneName => "moon";

        public override SceneCollection DestinationGroup => null;

        public override void Modify()
        {
            base.Modify();
        }

        public override void ModifySceneInfo(ClassicStageInfo info)
        {
        
        }

        [ConCommand(commandName = "warpcessation", flags = ConVarFlags.None, helpText = "the voices")]
        public static void WarpToCessation(ConCommandArgs args) {
            if (Run.instance) {
                Run.instance.AdvanceStage(Instance.sceneDef);
            }
        }
    }
}