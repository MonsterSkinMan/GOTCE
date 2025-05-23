﻿using BepInEx.Configuration;
using EntityStates.GameOver;
using R2API;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Diagnostics;
using UnityEngine.Networking;

namespace GOTCE.Items.Lunar
{
    public class CorruptedShard : ItemBase<CorruptedShard>
    {
        public override string ConfigName => "Corrupted Shard";

        public override string ItemName => "Corrupted Shard";

        public override string ItemLangTokenName => "GOTCE_CorruptedShard";

        public override string ItemPickupDesc => "<color=#e64b13>Seems to do nothing...</color> <color=#FF7F7F>BUT seems to do nothing...</color>\n";

        public override string ItemFullDescription => "Your attacks have a <style=cIsDamage>50%</style> <style=cStack>(+50% per stack)</style> chance to <style=cIsDamage>instantly kill</style> an enemy. The game has a <style=cIsHealth>0.5%</style> chance to <style=cIsHealth>crash</style> every second.";

        public override string ItemLore => "This newest anomaly is... odd. More so than everything else I've investigated so far. Just looking at it makes me deeply uneasy. In-depth scrutiny of it shows nothing out of the ordinary compared to the rest of the artifacts, and its own origin came from the crack I found. I just never feel safe around it, but I don't know where to get rid of it, and I'm worried that destroying it will make everything worse. This unease is horrible, but I'll power through. I have work to do.";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.Utility, ItemTag.BrotherBlacklist, ItemTag.AIBlacklist, GOTCETags.Unstable, GOTCETags.Bullshit };

        public override GameObject ItemModel => null;
        private GameObject voidVFX;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/corruptedshard.png");

        public override void Init(ConfigFile config)
        {
            base.Init(config);
            NetworkingAPI.RegisterMessageType<SyncCrash>();
            voidVFX = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/CritGlassesVoid/CritGlassesVoidExecuteEffect.prefab").WaitForCompletion();
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            if (self.inventory.GetItemCount(Instance.ItemDef) > 0)
            {
                if (self.GetComponent<CorruptedShardComponent>() == null)
                {
                    self.gameObject.AddComponent<CorruptedShardComponent>();
                }
            }
            orig(self);
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (damageInfo.attacker)
            {
                if (damageInfo.attacker.GetComponent<CharacterBody>())
                {
                    CharacterBody body = damageInfo.attacker.GetComponent<CharacterBody>();

                    if (body.inventory)
                    {
                        if (Util.CheckRoll(body.inventory.GetItemCount(ItemDef) * 50f * damageInfo.procCoefficient, body.master))
                        {
                            damageInfo.damageType |= DamageType.VoidDeath;
                            EffectManager.SpawnEffect(voidVFX, new EffectData
                            {
                                origin = self.body.transform.position,
                                scale = 2f
                            }, true);
                        }
                    }
                }
            }
            orig(self, damageInfo);
        }

        // this doesnt work
    }

    public class CorruptedShardComponent : MonoBehaviour
    {
        private bool shouldCrash = false;

        private void Start()
        {
            for (int i = 0; i < NetworkUser.readOnlyInstancesList.Count; i++)
            {
                var users = NetworkUser.readOnlyInstancesList[i];
                if (users && users.isParticipating && users.localUser != null && users.localUser.userProfile != null)
                {
                    users.localUser.userProfile.RequestEventualSave();
                }
            }

            StartCoroutine(Crash(1f));
        }

        private void FixedUpdate()
        {
            if (Util.CheckRoll(0.005f * Time.fixedDeltaTime))
            {
                shouldCrash = true;
            }
        }

        private IEnumerator Crash(float dur)
        {
            yield return new WaitForSeconds(dur);
            if (shouldCrash)
            {
                Main.ModLogger.LogError("Corrupted Shard: Sending Clients message to crash, this is intentional");
                NetMessageExtensions.Send(new SyncCrash(), NetworkDestination.Clients);
            }
            yield return new WaitForSeconds(dur);
            if (shouldCrash)
            {
                Main.ModLogger.LogError("Corrupted Shard: Crashing... this is intentional");
                UnityEngine.Diagnostics.Utils.ForceCrash(ForcedCrashCategory.FatalError);
            }
            yield break;
        }
    }

    public class SyncCrash : INetMessage, ISerializableObject
    {
        public void Serialize(NetworkWriter writer)
        {
        }

        public void Deserialize(NetworkReader reader)
        {
        }

        public void OnReceived()
        {
            if (!NetworkServer.active)
            {
                UnityEngine.Diagnostics.Utils.ForceCrash(ForcedCrashCategory.FatalError);
            }
        }
    }
}