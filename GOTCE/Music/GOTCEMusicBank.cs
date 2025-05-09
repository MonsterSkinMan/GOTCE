using System;
using RoR2;
using UnityEngine;

namespace GOTCE.Music {
    public abstract class GOTCEMusicBank<T> : GOTCEMusicBank where T : GOTCEMusicBank<T>
    {
        public static T Instance { get; private set; }

        public GOTCEMusicBank()
        {
            if (Instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting ItemBase was instantiated twice");
            Instance = this as T;
        }
    }
    public abstract class GOTCEMusicBank {
        public abstract string BankName { get; }
        public abstract string InitBankName { get; }
        public abstract string MusicSystemName { get; }
        public abstract string PauseSystemName { get; }
        public abstract string UnpauseSystemName { get; }
        public abstract uint GROUP { get; }
        public Dictionary<string, GOTCEMusicDef> tracks;
        private static List<string> loadedBanks = new();

        public void Setup() {

            if (!loadedBanks.Contains(InitBankName)) {
                AkSoundEngine.LoadBank(InitBankName, out _);
                loadedBanks.Add(InitBankName);
            }


            if (!loadedBanks.Contains(BankName)) {
                AkSoundEngine.LoadBank(BankName, out _);
                loadedBanks.Add(BankName);
            }

            On.RoR2.MusicController.StartIntroMusic += StartMusicSystem;
            On.RoR2.MusicController.LateUpdate += HandlePause;
            tracks = new();
            CollectMusicTracks();
        }

        public abstract void CollectMusicTracks();
        public void CreateMusicTrack(string name, uint group, params uint[] states) {
            GOTCEMusicDef def = ScriptableObject.CreateInstance<GOTCEMusicDef>();
            def._cachedName = name;
            (def as ScriptableObject).name = name;
            def.BankName = BankName;
            def.GroupID = group;
            def.States = states;
            
            tracks.Add(name, def);

            ContentAddition.AddMusicTrackDef(def);
        }

        private void StartMusicSystem(On.RoR2.MusicController.orig_StartIntroMusic orig, MusicController self) {
            orig(self);
            AkSoundEngine.PostEvent(MusicSystemName, self.gameObject);
        }

        private void HandlePause(On.RoR2.MusicController.orig_LateUpdate orig, MusicController self)
        {
            bool flag = Time.timeScale == 0;
            if (self.wasPaused != flag) {
                AkSoundEngine.PostEvent(flag ? PauseSystemName : UnpauseSystemName, self.gameObject);
            }

            orig(self);
        }
    }
}