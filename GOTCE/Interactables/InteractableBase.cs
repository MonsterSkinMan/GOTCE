using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Collections.Generic;
using EntityStates;

// WIP

namespace GOTCE.Interactables {
    public abstract class InteractableBase<T> : InteractableBase where T : InteractableBase<T> {
        public static T Instance { get; private set; }

        public InteractableBase()
        {
            if (Instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting ItemBase was instantiated twice");
            Instance = this as T;
        }
    } 

    public abstract class InteractableBase {
        public DirectorCard card;
        public InteractableSpawnCard isc;
        public abstract DirectorAPI.InteractableCategory category { get; }
        public virtual DirectorAPI.Stage[] stages { get; } = null;

        public void Create() {
            Modify();
            MakeSpawnCard();
            MakeDirectorCard();
            if (stages != null) {
                foreach (DirectorAPI.Stage stage in stages) {
                    DirectorAPI.Helpers.AddNewInteractableToStage(card, category, stage);
                }
            }
            DirectorAPI.DirectorCardHolder holder = new DirectorAPI.DirectorCardHolder();
            holder.Card = card;
            DirectorAPI.Helpers.AddNewInteractableToStage(card, category, DirectorAPI.Stage.Custom, "moon1");
        }

        public virtual void MakeSpawnCard() {
            isc = new InteractableSpawnCard();
        }

        public virtual void MakeDirectorCard() {
            card = new DirectorCard();
            card.spawnCard = isc;
        }

        public virtual void Modify() {

        }
    }
}