using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using System;
using System.Collections.Generic;
using EntityStates;
using RoR2.ExpansionManagement;

// WIP

namespace GOTCE.Interactables
{
    public abstract class InteractableBase<T> : InteractableBase where T : InteractableBase<T>
    {
        public static T Instance { get; private set; }

        public InteractableBase()
        {
            if (Instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting ItemBase was instantiated twice");
            Instance = this as T;
        }
    }

    public abstract class InteractableBase
    {
        public DirectorCard card;
        public InteractableSpawnCard isc;
        public abstract string Name { get; }

        private static readonly DirectorAPI.Stage[] staegis =
        {
            DirectorAPI.Stage.AbandonedAqueduct, DirectorAPI.Stage.AbandonedAqueductSimulacrum, DirectorAPI.Stage.AbyssalDepths,
            DirectorAPI.Stage.AbyssalDepthsSimulacrum, DirectorAPI.Stage.AphelianSanctuary, DirectorAPI.Stage.AphelianSanctuarySimulacrum,
            DirectorAPI.Stage.CommencementSimulacrum, DirectorAPI.Stage.DistantRoost, DirectorAPI.Stage.RallypointDelta,
            DirectorAPI.Stage.RallypointDeltaSimulacrum, DirectorAPI.Stage.ScorchedAcres, DirectorAPI.Stage.SiphonedForest,
            DirectorAPI.Stage.SirensCall, DirectorAPI.Stage.SkyMeadow, DirectorAPI.Stage.SkyMeadowSimulacrum, DirectorAPI.Stage.SulfurPools,
            DirectorAPI.Stage.SulfurPools, DirectorAPI.Stage.SunderedGrove, DirectorAPI.Stage.TitanicPlains, DirectorAPI.Stage.TitanicPlainsSimulacrum,
            DirectorAPI.Stage.WetlandAspect
            // default stage list, doesnt include hidden realms or commencement
        };

        public abstract DirectorAPI.InteractableCategory category { get; }
        public virtual DirectorAPI.Stage[] stages { get; } = staegis;
        public virtual ExpansionDef RequiredExpansionHolder { get; } = Main.SOTVExpansionDef;

        public void Create()
        {
            Modify();
            MakeSpawnCard();
            MakeDirectorCard();
            if (stages != null)
            {
                foreach (DirectorAPI.Stage stage in stages)
                {
                    DirectorAPI.Helpers.AddNewInteractableToStage(card, category, stage);
                }
            }
            DirectorAPI.DirectorCardHolder holder = new();
            holder.Card = card;
            // DirectorAPI.Helpers.AddNewInteractableToStage(card, category, DirectorAPI.Stage.Custom, "moon1");
        }

        public virtual void MakeSpawnCard()
        {
            isc = ScriptableObject.CreateInstance<InteractableSpawnCard>();
        }

        public virtual void MakeDirectorCard()
        {
            card = new() { spawnCard = isc };
        }

        public virtual void Modify()
        {
        }
    }
}