using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using static GOTCE.Main;

namespace GOTCE.Items.White
{
    public class BottledMetamorphosis : ItemBase<BottledMetamorphosis>
    {
        public override string ItemName => "Bottled Metamorphosis";

        public override string ItemLangTokenName => "GOTCE_BottledMetamorphosis";

        public override string ItemPickupDesc => "Periodically transform into a random entity.";

        public override string ItemFullDescription => "Every <style=cIsUtility>5</style> <style=cStack>(-10% stack)</style> seconds, turn into a <style=cIsUtility>random entity</style>.";

        public override string ItemLore => "The world inhabited by life is a nonsensical place. Imparting any sort of rules towards nature or general logic on the way the world behaves can only confuse you. The best way to integrate yourself into the animalistic side of our world is to embrace it. Let the chaos of life itself flow around you, rather than being destroyed by its torrential force. Many benefits can be absorbed from the disorder of life.";
        public override string ConfigName => ItemName;

        public override ItemTier Tier => ItemTier.Tier1;

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/BottledMetamorphosis.png");



        private static readonly System.Random random = new System.Random();

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateItem();
            Hooks();
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
            // On.RoR2.CharacterBody.FixedUpdate += Transform;
            // On.RoR2.CharacterBody.OnInventoryChanged += StackTimer;
            // On.RoR2.Stage.FixedUpdate += UpdateTimer; 
            On.RoR2.CharacterBody.OnInventoryChanged += AttachController;
        }


        public static GameObject GetRandomCharacterBodyPrefab()
        {
            List<string> donot = new List<string>() {
                "BirdsharkBody", "ArtifactShellBody","AltarSkeletonBody", "BackupDroneOldBody", "BeetleCrystalBody", "BeetleGuardAllyBody", "BeetleGuardCrystalBody",
            "BeetleWard", "DeathProjectile", "ExplosivePotDestructibleBody", "FusionCellDestructibleBody", "GolemBodyInvincible",
            "GravekeeperTrackingFireball", "LemurianBruiserBody", "LunarWispTrackingBomb", "MinorConstructAttachableBody", "MinorConstructBody", "MinorConstructOnKillBody", "NullifierBody", "OilBeetle",
            "ParentPodBody", "SMInfiniteTowerMaulingRockLarge", "SMInfiniteTowerMaulingRockMedium", "SMInfiniteTowerMaulingRockSmall", "SMMaulingRockLarge", "SMMaulingRockMedium", "SMMaulingRockSmall", "ScavSackProjectile",
            "SpectatorBody", "SpectatorSlowBody", "SulfurPodBody", "TimeCrystalBody", "UrchinTurretBody", "VagrantTrackingBomb", "VoidBarnacleNoCastBody", "VoidRaidCrabJointBody",
            "VultureEggBody", "Pot2Body"
            };
            List<GameObject> bodies = new List<GameObject>();
            foreach (GameObject body in BodyCatalog.allBodyPrefabs)
            {
                bodies.Add(body);
            }
            foreach (string str in donot)
            {
                bodies.Remove(BodyCatalog.FindBodyPrefab(str));
            }
            return bodies[random.Next(0, bodies.Count)];
        }

        /* public void Transform(On.RoR2.CharacterBody.orig_FixedUpdate orig, CharacterBody self) {
            orig(self);
            if (self.inventory) {
                if (self.inventory.GetItemCount(ItemDef) > 0) {
                    // Main.ModLogger.LogDebug(stopwatch);
                    if (stopwatch <= 0) {
                        self.master.bodyPrefab = GetRandomCharacterBodyPrefab();
                        self.master.Respawn(self.master.GetBody().transform.position, self.master.GetBody().transform.rotation);
                        // self.AddTimedBuff(MetamorphoTimer.Buff, 5f);
                    }
                }
            }
        } */

        public void StackTimer(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);
            if (self.inventory && self.inventory.GetItemCount(ItemDef) > 0)
            {
                // interval = 5f * Mathf.Pow(0.9f, self.inventory.GetItemCount(ItemDef));
                if (self.gameObject.GetComponent<MetaController>())
                {
                    // self.gameObject.GetComponent<MetaController>().interval =  5f * Mathf.Pow(0.9f, self.inventory.GetItemCount(ItemDef));
                }
            }
        }

        public void AttachController(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);
            if (self.inventory)
            {
                bool flag = self.inventory.GetItemCount(ItemDef) > 0;
                MetaController controller = self.GetComponent<MetaController>();
                if (flag != controller)
                {
                    if (flag)
                    {
                        self.gameObject.AddComponent<MetaController>();
                    }
                    else
                    {
                        Object.Destroy(controller);
                    }
                }
            }
        }

        /* public void UpdateTimer(On.RoR2.Stage.orig_FixedUpdate orig, Stage self) {
            orig(self);
            stopwatch -= Time.fixedDeltaTime;
            if (stopwatch <= 0) {
                stopwatch = 5f;
            }
        } */

    }

    public class MetaController : MonoBehaviour
    {
        private CharacterBody body;
        private static float interval = 5f;
        private float stopwatch = interval;


        public void Start()
        {
            body = gameObject.GetComponent<CharacterBody>();
            interval = 5f * Mathf.Pow(0.9f, body.inventory.GetItemCount(BottledMetamorphosis.Instance.ItemDef));
        }

        public void FixedUpdate()
        {
            stopwatch -= Time.fixedDeltaTime;
            if (body.inventory)
            {
                if (body.inventory.GetItemCount(BottledMetamorphosis.Instance.ItemDef) > 0)
                {
                    // Main.ModLogger.LogDebug(stopwatch);
                    if (stopwatch <= 0)
                    {
                        body.master.bodyPrefab = BottledMetamorphosis.GetRandomCharacterBodyPrefab();
                        body.master.Respawn(body.master.GetBody().transform.position, body.master.GetBody().transform.rotation);
                        // self.AddTimedBuff(MetamorphoTimer.Buff, 5f);
                    }
                }
            }
        }
    }


}