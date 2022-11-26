using RoR2;
using RoR2.Achievements;
using System;
using Unity;
using UnityEngine;
using System.Text.RegularExpressions;
using R2API;
using BepInEx;

namespace GOTCE.Achievements
{
    public abstract class AchievementBase<T> : AchievementBase where T : AchievementBase<T>
    {
        public static T Instance { get; private set; }

        public AchievementBase()
        {
            if (Instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting AchievementBase was instantiated twice");
            Instance = this as T;
        }
    }

    public abstract class AchievementBase
    {
        public bool enabled;
        public UnlockableDef def;
        public abstract Sprite Icon { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string UnlockName { get; }
        public abstract string TokenName { get; }
        public virtual bool Hidden { get; } = false;

        public void Create(BepInEx.Configuration.ConfigFile config)
        {
            def = ScriptableObject.CreateInstance<UnlockableDef>();

            def.nameToken = $"ACHIEVEMENT_{TokenName}_NAME";
            def.cachedName = UnlockName;
            def.achievementIcon = Icon;
            def.hidden = Hidden;

            LanguageAPI.Add($"ACHIEVEMENT_{TokenName}_NAME", Name);
            LanguageAPI.Add($"ACHIEVEMENT_{TokenName}_DESCRIPTION", Description);

            enabled = config.Bind<bool>($"Unlock: {Name}", "Enable?", true, "Should this achievement be enabled? A value of false makes it always unlocked.").Value;

            if (enabled)
            {
                ContentAddition.AddUnlockableDef(def);
            }

            PostCreation();
        }

        public virtual void PostCreation()
        {
        }
    }
}