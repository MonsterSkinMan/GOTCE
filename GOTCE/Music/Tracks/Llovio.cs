using System;

namespace GOTCE.Music {
    public class Llovio : GOTCEMusicBank<Llovio>
    {
        public override string BankName => "MusicTracks";

        public override string InitBankName => "MusicTracks-Init";

        public override string MusicSystemName => "PlayLlovio";

        public override uint GROUP => 2775631278U;

        public override string PauseSystemName => "PauseLlovio";

        public override string UnpauseSystemName => "UnpauseLlovio";

        public const uint LOOP = 691006007U;

        public override void CollectMusicTracks()
        {
            CreateMusicTrack("muLlovio", GROUP, LOOP);

            SceneDef moon = Utils.Paths.SceneDef.moon.Load<SceneDef>();
            moon.mainTrack = tracks["muLlovio"];
        }
    }
}