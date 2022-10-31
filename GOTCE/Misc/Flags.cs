using RoR2;
using System;
using R2API;
using Unity;
using UnityEngine;
using Mono.Cecil;
using MonoMod.Cil;
using Mono.Cecil.Cil;

namespace GOTCE.Misc {
    // custom item tags start at 90 to hopefully avoid mod conflict
    public enum GOTCETags : int {
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
        OnStageBeginEffect = 100,
        WhipAndNaeNae = 101,
        Galsone = 102,
        BackupMagSynergy = 103,
        Cracked = 104,
        Bullshit = 105,

    }
    public static class Flags {
        public static void Initialize() {
            IL.RoR2.ItemCatalog.SetItemDefs += (il) => {
                ILCursor c = new ILCursor(il);
                if(c.TryGotoNext(MoveType.After,x => x.MatchLdcI4((int)ItemTag.Count))){
                    c.EmitDelegate<Func<int>>(() => 106);
                    c.Emit(OpCodes.Add);
                }
            };
        }
    }
}