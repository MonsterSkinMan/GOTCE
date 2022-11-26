using System;
using Unity;
using UnityEngine;
using RoR2;

namespace GOTCE.Mechanics
{
    public class CriticalTypes
    {
        public static EventHandler<SprintCritEventArgs> OnSprintCrit;

        public static EventHandler<StageCritEventArgs> OnStageCrit;

        public static EventHandler<FovCritEventArgs> OnFovCrit;
    }

    public class SprintCritEventArgs
    {
        public CharacterBody Body;

        public SprintCritEventArgs(CharacterBody body)
        {
            Body = body;
        }
    }

    public class StageCritEventArgs : EventArgs
    {
        public StageCritEventArgs()
        {
        }
    }

    public class FovCritEventArgs : EventArgs
    {
        public CharacterBody Body;

        public FovCritEventArgs(CharacterBody body)
        {
            Body = body;
        }
    }
}