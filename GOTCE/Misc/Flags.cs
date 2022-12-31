using RoR2;
using System;
using R2API;
using Unity;
using UnityEngine;
using Mono.Cecil;
using MonoMod.Cil;
using Mono.Cecil.Cil;

namespace GOTCE.Misc
{
    // custom item tags start at 90 to hopefully avoid mod conflict
    public enum GOTCETags : int
    {
        Shield = 90,
        AFKRelated = 91,
        Unstable = 92,
        StageDependant = 93,
        TimeDependant = 94,
        Crit = 95,
        BarrierRelated = 96,
        Consumable = 97,
        Masochist = 98,
        OnDeathEffect = 99,
        WhipAndNaeNae = 100,
        Galsone = 101,
        BackupMagSynergy = 102,
        Cracked = 103,
        Bullshit = 104,
        NonLunarLunar = 105,
        FovRelated = 106,
        Pauldron = 107,
    }

    public static class Flags
    {
        public static void Initialize()
        {
            // max index of itemtag is hardcoded to 21 because the hopoo james
            IL.RoR2.ItemCatalog.SetItemDefs += (il) =>
            {
                ILCursor c = new ILCursor(il);
                if (c.TryGotoNext(MoveType.After, x => x.MatchLdcI4((int)ItemTag.Count)))
                {
                    c.EmitDelegate<Func<int>>(() => 108);
                    c.Emit(OpCodes.Add);
                }
            };
        }
    }

    // reserve custom damage types
    public static class DamageTypes
    {
        public static DamageAPI.ModdedDamageType NaderEffect = DamageAPI.ReserveDamageType();
        public static DamageAPI.ModdedDamageType FullChainLightning = DamageAPI.ReserveDamageType();
        public static DamageAPI.ModdedDamageType Truekill = DamageAPI.ReserveDamageType();
        public static DamageAPI.ModdedDamageType Root = DamageAPI.ReserveDamageType();
        public static DamageAPI.ModdedDamageType Explosion = DamageAPI.ReserveDamageType();
        public static DamageAPI.ModdedDamageType Hooked = DamageAPI.ReserveDamageType();
        public static DamageAPI.ModdedDamageType Scorched = DamageAPI.ReserveDamageType();
    }
}