using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.ExpansionManagement;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GOTCE.Items
{
    // The directly below is entirely from TILER2 API (by ThinkInvis) specifically the Item module. Utilized to implement instancing for classes.
    // TILER2 API can be found at the following places:
    // https://github.com/ThinkInvis/RoR2-TILER2
    // https://thunderstore.io/package/ThinkInvis/TILER2/

    public abstract class ItemBase<T> : ItemBase where T : ItemBase<T>
    {
        //This, which you will see on all the -base classes, will allow both you and other modders to enter through any class with this to access internal fields/properties/etc as if they were a member inheriting this -Base too from this class.
        public static T Instance { get; private set; }

        public ItemBase()
        {
            if (Instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting ItemBase was instantiated twice");
            Instance = this as T;
        }
    }

    public abstract class ItemBase
    {
        public abstract string ConfigName { get; }
        public abstract string ItemName { get; }
        public abstract string ItemLangTokenName { get; }
        public abstract string ItemPickupDesc { get; }
        public abstract string ItemFullDescription { get; }
        public abstract string ItemLore { get; }

        public abstract ItemTier Tier { get; }
        public virtual Enum[] ItemTags { get; set; } = new Enum[] { };

        public abstract GameObject ItemModel { get; }
        public abstract Sprite ItemIcon { get; }

        public ItemDef ItemDef;
        public GameObject prefab = null;

        public virtual bool CanRemove { get; } = true;

        public virtual bool Hidden { get; } = false;

        public virtual bool AIBlacklisted { get; set; } = false;

        public virtual ItemTierDef OverrideTierDef { get; } = null;
        public virtual ExpansionDef RequiredExpansionHolder { get; } = Main.SOTVExpansionDef;
        public static GameObject CubeModel;

        /// <summary>
        /// This method structures your code execution of this class. An example implementation inside of it would be:
        /// <para>CreateConfig(config);</para>
        /// <para>CreateLang();</para>
        /// <para>CreateItem();</para>
        /// <para>Hooks();</para>
        /// <para>This ensures that these execute in this order, one after another, and is useful for having things available to be used in later methods.</para>
        /// <para>P.S. CreateItemDisplayRules(); does not have to be called in this, as it already gets called in CreateItem();</para>
        /// </summary>
        /// <param name="config">The config file that will be passed into this from the main class.</param>
        ///

        private static GameObject emptyModel;

        public virtual void Init(ConfigFile config)
        {
            emptyModel = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();
            CubeModel = Main.MainAssets.LoadAsset<GameObject>("Assets/Models/Prefabs/Item/Drill/Cube.prefab");

            bool preset1 = config.Bind<bool>("Presets:", "NonLunarLunar", false, "Disable all items under the NonLunarLunar (non-lunar items with downsides) category.").Value;
            bool preset2 = config.Bind<bool>("Presets:", "Masochist", false, "Disable all items under the Masochist (self-damage or self-killing) category.").Value;
            bool preset3 = config.Bind<bool>("Presets:", "Unstable", false, "Disable all items under the Unstable (lagging and crashing) category.").Value;
            bool preset4 = config.Bind<bool>("Presets:", "Bullshit", false, "Disable all items under the Bullshit (really stupid) category.").Value;

            if (preset1 && ItemTags.Contains(GOTCETags.NonLunarLunar)) {
                // pass
            }
            else if (preset2 && ItemTags.Contains(GOTCETags.Masochist)) {
                // pass
            }
            else if (preset3 && ItemTags.Contains(GOTCETags.Unstable)) {
                // pass
            }
            else if (preset4 && ItemTags.Contains(GOTCETags.Bullshit)) {
                // pass
            }
            else {
                CreateItem();
                CreateLang();
                CreateConfig(config);
                Hooks();
            }
        }

        public virtual void CreateConfig(ConfigFile config)
        { }

        protected virtual void CreateLang()
        {
            LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_NAME", ItemName);
            LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_PICKUP", ItemPickupDesc);
            LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_DESCRIPTION", ItemFullDescription);
            LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_LORE", ItemLore);
        }

        public abstract ItemDisplayRuleDict CreateItemDisplayRules();

        protected void CreateItem()
        {
            if (AIBlacklisted)
            {
                List<Enum> tmp = ItemTags.ToList<Enum>();
                tmp.Add(ItemTag.AIBlacklist);
                ItemTags = tmp.ToArray<Enum>();
            }

            ItemDef = ScriptableObject.CreateInstance<ItemDef>();
            ItemDef.name = "ITEM_" + ItemLangTokenName;
            ItemDef.nameToken = "ITEM_" + ItemLangTokenName + "_NAME";
            ItemDef.pickupToken = "ITEM_" + ItemLangTokenName + "_PICKUP";
            ItemDef.descriptionToken = "ITEM_" + ItemLangTokenName + "_DESCRIPTION";
            ItemDef.loreToken = "ITEM_" + ItemLangTokenName + "_LORE";
            /*if (ItemDef.pickupModelPrefab == null)
            {
                GameObject prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Material mat = new(new Shader());
            }*/
            ItemDef.requiredExpansion = RequiredExpansionHolder;
            if (ItemModel == null)
            {
                // GameObject prefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                // doesnt work or too small
                // ItemDef.pickupModelPrefab = emptyModel;
                if (ItemIcon != null)
                {
                    // prefab = PrefabAPI.InstantiateClone(GameObject.CreatePrimitive(PrimitiveType.Cube), $"{ItemName}-model");
                    prefab = PrefabAPI.InstantiateClone(CubeModel, $"{ItemName}-model");
                    prefab.GetComponentInChildren<MeshRenderer>().transform.localScale = new(1.5f, 1.5f, 1.5f);
                    prefab.GetComponentInChildren<MeshRenderer>().material.mainTexture = ItemIcon.texture;
                    GameObject.DontDestroyOnLoad(prefab);
                    ItemDef.pickupModelPrefab = prefab; 
                    /*System.Random rand = new();
                    int objects = rand.Next(3, 9);
                    PrimitiveType[] prims = {
                        PrimitiveType.Sphere,
                        PrimitiveType.Capsule,
                        PrimitiveType.Cylinder,
                        PrimitiveType.Cube,
                    };
                    Color[] colors = {
                        Color.cyan,
                        Color.red,
                        Color.magenta,
                        Color.grey
                    };

                    GameObject first = GameObject.CreatePrimitive(prims[rand.Next(0, prims.Length)]);
                    first.GetComponent<MeshRenderer>().material.color = colors[rand.Next(0, colors.Length)];
                    for (int i = 0; i < objects; i++) {
                        GameObject prim = GameObject.CreatePrimitive(prims[rand.Next(0, prims.Length)]);
                        prim.GetComponent<MeshRenderer>().material.color = colors[rand.Next(0, colors.Length)];
                        prim.transform.SetParent(first.transform);
                        prim.transform.localPosition = new Vector3(rand.Next(-1, 1), rand.Next(-1, 1), rand.Next(-1, 1));
                        prim.transform.localRotation = Quaternion.Euler(new Vector3(rand.Next(-360, 360), rand.Next(-360, 360), rand.Next(-360, 360)));
                    }

                    GameObject prefab = PrefabAPI.InstantiateClone(first, $"{ItemName}-model");
                    GameObject.DontDestroyOnLoad(prefab);
                    ItemDef.pickupModelPrefab = prefab; */
                }
                else
                {
                    ItemDef.pickupModelPrefab = emptyModel;
                }
            }
            else
            {
                ItemDef.pickupModelPrefab = ItemModel;
            }
            ItemDef.pickupIconSprite = ItemIcon;
            ItemDef.hidden = Hidden;
            ItemDef.canRemove = CanRemove;
            if (OverrideTierDef)
            {
                ItemDef._itemTierDef = OverrideTierDef;
            }
            else
            {
                ItemDef.deprecatedTier = Tier;
            }

            if (ItemTags.Length > 0) { ItemDef.tags = ItemTags.Cast<ItemTag>().ToArray(); }
        }

        public virtual void Hooks()
        { }

        //Based on ThinkInvis' methods
        public int GetCount(CharacterBody body)
        {
            if (!body || !body.inventory) { return 0; }

            return body.inventory.GetItemCount(ItemDef);
        }

        public int GetCount(CharacterMaster master)
        {
            if (!master || !master.inventory) { return 0; }

            return master.inventory.GetItemCount(ItemDef);
        }

        public bool ContainsTag(ItemDef def, Enum tag) {
            return def.ContainsTag((ItemTag)tag);
        }

        public int GetCountSpecific(CharacterBody body, ItemDef itemDef)
        {
            if (!body || !body.inventory) { return 0; }

            return body.inventory.GetItemCount(itemDef);
        }

        public bool HasItem(CharacterBody body) {
            return GetCount(body) > 1;
        }
    }
}