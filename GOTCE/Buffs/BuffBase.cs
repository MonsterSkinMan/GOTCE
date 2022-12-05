using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Buffs
{
    public abstract class BuffBase<T> : BuffBase where T : BuffBase<T>
    {
        public static T instance { get; private set; }

        public BuffBase()
        {
            if (instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting BuffBase was instantiated twice");
            instance = this as T;
        }
    }

    public abstract class BuffBase
    {
        public abstract string BuffName { get; }
        public abstract Color Color { get; }
        public virtual bool CanStack { get; set; } = false;
        public virtual bool IsDebuff { get; set; } = false;
        public abstract Sprite BuffIcon { get; }
        public virtual bool Hidden { get; set; } = false;

        public BuffDef BuffDef;

        public virtual void Init(ConfigFile config) {

        }

        public void CreateBuff(ConfigFile config)
        {
            BuffDef = ScriptableObject.CreateInstance<BuffDef>();
            BuffDef.name = BuffName;
            BuffDef.buffColor = Color;
            BuffDef.canStack = CanStack;
            BuffDef.isDebuff = IsDebuff;
            BuffDef.iconSprite = BuffIcon;
            BuffDef.isHidden = Hidden;

            Init(config);

            ContentAddition.AddBuffDef(BuffDef);

            Hooks();
        }

        public virtual void Hooks() {

        }
    }
}