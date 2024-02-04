using System;
using GOTCE.Gamemodes.Crackclipse;

namespace GOTCE.Music {
    public class Prelude : GOTCEMusicBank<Prelude>
    {
        public override string BankName => "MusicTracks";

        public override string InitBankName => "MusicTracks-Init";

        public override string MusicSystemName => "PlayPrelude";

        public override uint GROUP => 917443112U;

        public override string PauseSystemName => "PausePrelude";

        public override string UnpauseSystemName => "UnpausePrelude";

        public const uint LOOP = 691006007U;

        public override void CollectMusicTracks()
        {
            CreateMusicTrack("muPrelude", GROUP, LOOP);

            CrackclipseRun run = GameMode.CrackclipsePrefab.GetComponent<CrackclipseRun>();
            MusicTrackOverride over = run.lobbyBackgroundPrefab.AddComponent<MusicTrackOverride>();
            over.track = tracks["muPrelude"];
        }
    }
}