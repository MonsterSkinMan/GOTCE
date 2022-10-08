using BepInEx.Configuration;
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

        public override string ItemPickupDesc => "Seems to do nothing... <color=#FF7F7F>BUT seems to do nothing...</color>\n";

        public override string ItemFullDescription => "Your attacks have a <style=cIsDamage>50%</style> <style=cStack>(+50% per stack)</style> chance to <style=cIsDamage>instantly kill</style> an enemy. The game has a <style=cIsHealth>0.1%</style> <style=cStack>(+0.1% per stack)</style> chance to <style=cIsHealth>crash</style> every second.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Lunar;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage, ItemTag.Utility };

        public override GameObject ItemModel => null;
        private GameObject voidVFX;

        public override Sprite ItemIcon => null;/* Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/shittyvisage.png"); */ // replace with corrupted shard icon

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
            if (self.body.inventory && Util.CheckRoll(self.body.inventory.GetItemCount(Instance.ItemDef) * 50f * damageInfo.procCoefficient, self.body.master))
            {
                damageInfo.damageType |= DamageType.VoidDeath;
                if (self.health > 0f)
                {
                    self.Networkhealth = 0f;
                }
                if (self.shield > 0f)
                {
                    self.Networkshield = 0f;
                }
                if (self.barrier > 0f)
                {
                    self.Networkbarrier = 0f;
                }
                EffectManager.SpawnEffect(voidVFX, new EffectData
                {
                    origin = self.transform.position,
                    scale = 1f
                }, true);
            }
            orig(self, damageInfo);
        }

        // this doesnt work
    }

    public class CorruptedShardComponent : MonoBehaviour
    {
        private IEnumerator crash;
        private bool shouldCrash = false;

        private void Start()
        {
            crash = Crash(0.5f);
            for (int i = 0; i < NetworkUser.readOnlyInstancesList.Count; i++)
            {
                var users = NetworkUser.readOnlyInstancesList[i];
                if (users && users.isParticipating && users.localUser != null && users.localUser.userProfile != null)
                {
                    users.localUser.userProfile.RequestEventualSave();
                }
            }

            StartCoroutine(crash);
        }

        private void FixedUpdate()
        {
            if (Util.CheckRoll(0.1f))
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