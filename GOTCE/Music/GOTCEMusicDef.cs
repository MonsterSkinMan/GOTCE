using System;
using RoR2;
using UnityEngine;

namespace GOTCE.Music {
    public class GOTCEMusicDef : MusicTrackDef {
        public string BankName;
        public uint GroupID;
        public uint[] States;
        public override void Play()
        {
            Preload();
            for (int i = 0; i < States.Length; i++) {
                // Debug.Log("Setting state: " + states[i]);
                AkSoundEngine.SetState(GroupID, States[i]);
            }
        }

        public override void Preload()
        {
            AkSoundEngine.LoadBank(BankName, out _);
        }

        public override void Stop()
        {
            Preload();
            for (int i = 0; i < States.Length; i++) {
                // Debug.Log("Unsetting state: " + states[i]);
                AkSoundEngine.SetState(GroupID, 0U);
            }
        }
    }
}