using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using static GOTCE.Main;

namespace GOTCE.Items
{
    public class BottledMetamorphosis : ItemBase<BottledMetamorphosis>
    {
        public override string ItemName => "Bottled Metamorphosis";

        public override string ItemLangTokenName => "GOTCE_RandomBody";

        public override string ItemPickupDesc => "Periodically transform into a random CharacterBody.";

        public override string ItemFullDescription => "Every 5 seconds <style=cStack>(-10% stack)</style>, turn into a random CharacterBody.";

        public override string ItemLore => "The world inhabited by life is a nonsensical place. Imparting any sort of rules towards nature or general logic on the way the world behaves can only confuse you. The best way to integrate yourself into the animalistic side of our world is to embrace it. Let the chaos of life itself flow around you, rather than being destroyed by its torrential force. Many benefits can be absorbed from the disorder of life.";
        public override string ConfigName => ItemName;

        public override ItemTier Tier => ItemTier.Tier1;

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;


        private float stopwatch = 5f;
        private float interval = 5f;
        

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
            On.RoR2.CharacterBody.FixedUpdate += Transform;
            On.RoR2.CharacterBody.OnInventoryChanged += StackTimer;
        }
        

        public GameObject GetRandomCharacterBodyPrefab() {
            List<string> donot = new List<string>() {
                "BirdsharkBody", "ArtifactShellBody","AltarSkeletonBody", "BackupDroneOldBody", "BeetleCrystalBody", "BeetleGuardAllyBody", "BeetleGuardCrystalBody",
            "BeetleWard", "DeathProjectile", "ExplosivePotDestructibleBody", "FusionCellDestructibleBody", "GolemBodyInvincible",
            "GravekeeperTrackingFireball", "LemurianBruiserBody", "LunarWispTrackingBomb", "MinorConstructAttachableBody", "MinorConstructBody", "MinorConstructOnKillBody", "NullifierBody", "OilBeetle", 
            "ParentPodBody", "SMInfiniteTowerMaulingRockLarge", "SMInfiniteTowerMaulingRockMedium", "SMInfiniteTowerMaulingRockSmall", "SMMaulingRockLarge", "SMMaulingRockMedium", "SMMaulingRockSmall", "ScavSackProjectile",
            "SpectatorBody", "SpectatorSlowBody", "SulfurPodBody", "TimeCrystalBody", "UrchinTurretBody", "VagrantTrackingBomb", "VoidBarnacleNoCastBody", "VoidRaidCrabJointBody",
            "VultureEggBody", "Pot2Body"
            };
            List<GameObject> bodies = new List<GameObject>();
            foreach (GameObject body in BodyCatalog.allBodyPrefabs) {
                bodies.Add(body);
            }
            foreach (string str in donot) {
                bodies.Remove(BodyCatalog.FindBodyPrefab(str));
            }
            return bodies[random.Next(0, bodies.Count)];
        }

        public void Transform(On.RoR2.CharacterBody.orig_FixedUpdate orig, CharacterBody self) {
            orig(self);
            stopwatch -= Time.fixedDeltaTime;
            if (self.inventory) {
                if (self.inventory.GetItemCount(ItemDef) > 0) {
                    // Main.ModLogger.LogDebug(stopwatch);
                    if (stopwatch <= 0) {
                        self.master.bodyPrefab = GetRandomCharacterBodyPrefab();
                        // Main.ModLogger.LogDebug(self.master.bodyPrefab.name);
                        self.master.Respawn(self.master.GetBody().transform.position, self.master.GetBody().transform.rotation);
                        // self.AddTimedBuff(MetamorphoTimer.Buff, 5f);
                        stopwatch = interval;
                    }
                }
            }
        }

        public void StackTimer(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self) {
            if (self.inventory && self.inventory.GetItemCount(ItemDef) > 0) {
                interval = 5f * Mathf.Pow(0.9f, self.inventory.GetItemCount(ItemDef));
            }
        }

    }


}