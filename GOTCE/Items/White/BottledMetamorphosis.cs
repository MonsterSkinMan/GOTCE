using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using static GOTCE.Main;

// using UnityEngine.AddressableAssets;

namespace GOTCE.Items.White
{
    public class BottledMetamorphosis : ItemBase<BottledMetamorphosis>
    {
        public override string ItemName => "Bottled Metamorphosis";

        public override string ItemLangTokenName => "GOTCE_BottledMetamorphosis";

        public override string ItemPickupDesc => "Periodically transform into a random survivor.";

        public override string ItemFullDescription => "Every <style=cIsUtility>60</style> <style=cStack>(-10% per stack)</style> seconds, turn into a <style=cIsUtility>random survivor</style>.";

        public override string ItemLore => "The world inhabited by life is a nonsensical place. Imparting any sort of rules towards nature or general logic on the way the world behaves can only confuse you. The best way to integrate yourself into the animalistic side of our world is to embrace it. Let the chaos of life itself flow around you, rather than being destroyed by its torrential force. Many benefits can be absorbed from the disorder of life.";
        public override string ConfigName => ItemName;

        public override ItemTier Tier => ItemTier.NoTier;

        public override GameObject ItemModel => null;
        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.AIBlacklist, GOTCETags.Bullshit };

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/BottledMetamorphosis.png");

        private static readonly System.Random random = new System.Random();
        private static GameObject heretic;

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateItem();
            Hooks();
            heretic = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Heretic/HereticBody.prefab").WaitForCompletion();
        }

        public override void CreateConfig(ConfigFile config)
        {
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        public override void Hooks()
        {
            On.RoR2.CharacterBody.OnInventoryChanged += AttachController;
            On.RoR2.Inventory.GiveItem_ItemDef_int += Inventory_GiveItem_ItemDef_int;
        }

        private void Inventory_GiveItem_ItemDef_int(On.RoR2.Inventory.orig_GiveItem_ItemDef_int orig, Inventory self, ItemDef itemDef, int count)
        {
            orig(self, itemDef, count);
            if (NetworkServer.active && itemDef == Instance.ItemDef)
            {
                var stack = self.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    self.gameObject.GetComponent<MetaController>().interval = 60 * Mathf.Pow(0.9f, stack - 1);
                    // diminishing stacking like safer spaces
                }
            }
        }

        public static GameObject GetRandomSurvivorBodyPrefab()
        {
            List<GameObject> bodies = new();
            foreach (SurvivorDef def in SurvivorCatalog.allSurvivorDefs)
            {
                if (def.bodyPrefab.name != heretic.name)
                {
                    bodies.Add(def.bodyPrefab);
                }
            }
            return bodies[random.Next(0, bodies.Count)];
        }

        public void AttachController(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            if (self.inventory)
            {
                bool flag = self.inventory.GetItemCount(ItemDef) > 0;
                MetaController controller = self.gameObject.GetComponent<MetaController>();

                if (flag != controller)
                {
                    if (flag)
                    {
                        self.gameObject.AddComponent<MetaController>();
                    }
                    else
                    {
                        UnityEngine.Object.Destroy(controller);
                    }
                }
            }
            orig(self);
        }
    }

    public class MetaController : MonoBehaviour
    {
        private CharacterBody body;
        public float interval;
        private float stopwatch;

        public void Start()
        {
            body = gameObject.GetComponent<CharacterBody>();
            interval = 60f;
            stopwatch = interval;
        }

        public void FixedUpdate()
        {
            stopwatch -= Time.fixedDeltaTime;
            if (body.inventory)
            {
                if (body.inventory.GetItemCount(BottledMetamorphosis.Instance.ItemDef) > 0)
                {
                    if (stopwatch <= 0)
                    {
                        body.master.bodyPrefab = BottledMetamorphosis.GetRandomSurvivorBodyPrefab();
                        body.master.Respawn(body.master.GetBody().transform.position + new Vector3(0f, 5f, 0f), body.master.GetBody().transform.rotation);
                    }
                    // respawn slightly off the ground to prevent weird teleports
                    // had it happen once on commencement, where i encountered mithrix and suddenly got teleported to soul pillars lmao
                }
            }
        }
    }
}