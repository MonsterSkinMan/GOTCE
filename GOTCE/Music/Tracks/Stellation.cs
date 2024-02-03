using System;

namespace GOTCE.Music {
    public class Stellation : GOTCEMusicBank<Stellation>
    {
        public override string BankName => "MusicTracks";

        public override string InitBankName => "MusicTracks-Init";

        public override string MusicSystemName => "PlayStellation";

        public override uint GROUP => 978650076U;

        public override string PauseSystemName => "PauseStellation";

        public override string UnpauseSystemName => "UnpauseStellation";

        public const uint P1 = 1635194252U;
        public const uint P2 = 1635194255U;
        public const uint P3 = 1635194254U;
        public const uint P4 = 1635194249U;

        public override void CollectMusicTracks()
        {
            CreateMusicTrack("GlassthrixPhase1", GROUP, P1);
            CreateMusicTrack("GlassthrixPhase2", GROUP, P2);
            CreateMusicTrack("GlassthrixPhase3", GROUP, P3);
            CreateMusicTrack("GlassthrixPhase4", GROUP, P4);
        }
    }
}