using RoR2;
using System;
using UnityEngine;

namespace GOTCE.Tiers
{
    public abstract class TierBase<T> : TierBase where T : TierBase<T>
    {
        public static T Instance { get; private set; }

        public TierBase()
        {
            if (Instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting ItemBase was instantiated twice");
            Instance = this as T;
        }
    }

    public abstract class TierBase
    {
        public abstract string TierName { get;}
        public abstract bool CanScrap { get;}
        public abstract bool IsDroppable { get;}
        public virtual ItemTier TierEnum { get;} // cast to itemtier before passing enum to this
        public virtual ItemTierDef.PickupRules PickupRules { get;}
        public abstract GameObject DropletDisplayPrefab { get;}
        public virtual ColorCatalog.ColorIndex ColorIndex { get;}
        public virtual ColorCatalog.ColorIndex DarkColorIndex { get;}
        public abstract GameObject HighlightPrefab {get; }

        public ItemTierDef tier;

        public virtual void Awake()
        {
            CreateTier();
        }

        protected void CreateTier()
        {
            tier = ScriptableObject.CreateInstance<ItemTierDef>();

            tier.name = TierName;
            tier.isDroppable = IsDroppable;
            tier.canScrap = CanScrap;
            tier.colorIndex = ColorIndex;
            tier.darkColorIndex = DarkColorIndex;
            tier.dropletDisplayPrefab = DropletDisplayPrefab;
            tier.highlightPrefab = HighlightPrefab;
            tier.pickupRules = PickupRules;
            tier._tier = TierEnum;

            R2API.ContentAddition.AddItemTierDef(tier);
        }
    }
}