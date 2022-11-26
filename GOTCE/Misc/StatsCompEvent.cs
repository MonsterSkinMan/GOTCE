using System;
using RoR2;
using UnityEngine;

namespace GOTCE.Misc
{
    public static class StatsCompEvent
    {
        public static EventHandler<StatsCompRecalcArgs> StatsCompRecalc;
    }

    public class StatsCompRecalcArgs
    {
        public GOTCE_StatsComponent Stats;

        public StatsCompRecalcArgs(GOTCE_StatsComponent stats)
        {
            Stats = stats;
        }
    }
}