using BepInEx.Configuration;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using R2API.Networking;
using R2API.Networking.Interfaces;

namespace GOTCE.Items.Red
{
    public class SpikestripChanNFT : ItemBase<SpikestripChanNFT>
    {
        public override string ConfigName => "Spikestrip Chan NFT";

        public override string ItemName => "Spikestrip Chan NFT";

        public override string ItemLangTokenName => "GOTCE_SpikestripChanNFT";

        public override string ItemPickupDesc => "Increases a random stat.\n";

        public override string ItemFullDescription => "Increases <style=cIsUtility>1</style> <style=cStack>(+1 per stack)</style> random stat(s) by <style=cIsDamage>100%</style>.";

        public override string ItemLore => "\"So, what've you been up to recently?\"\n\"Oh, I've been getting into these things called NFTs- have you heard of them?\"\n\"Are you getting scammed?\"\n\"No, I just bought one of this anime girl I like-\"\n\"It better not be that fucking Spikestrip Chan you always talk about.\"\n\"Oh umm... It miiight be?\"\n\"How much did you spend on it?\"\n\"Erm... four grand.\"\n\"...Why don't you try going outside for once? Meeting some real women?\"";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.Utility, ItemTag.Healing, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/sscnft.png");

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            NetworkingAPI.RegisterMessageType<GOTCENFT.SyncAddBuff>();
            NetworkingAPI.RegisterMessageType<GOTCENFT.SyncRemoveBuff>();
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            On.RoR2.Inventory.GiveItem_ItemIndex_int += Inventory_GiveItem_ItemIndex_int;
            On.RoR2.Inventory.RemoveItem_ItemIndex_int += Inventory_RemoveItem_ItemIndex_int;

            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;

            GOTCENFT.Init();
        }

        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);
            if (!self.inventory.GetComponent<GOTCENFT>() && self.isPlayerControlled)
            {
                self.inventory.gameObject.AddComponent<GOTCENFT>();
            }
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            Inventory inventory = sender.inventory;
            if (inventory)
            {
                GOTCENFT component = inventory.GetComponent<GOTCENFT>();
                if (component)
                {
                    args.healthMultAdd += 1f * component.buffStacks[GOTCENFT.BuffType.MaxHealth];
                    args.damageMultAdd += 1f * component.buffStacks[GOTCENFT.BuffType.Damage];
                    args.regenMultAdd += 1f * component.buffStacks[GOTCENFT.BuffType.Regen];
                    args.jumpPowerMultAdd += 1f * component.buffStacks[GOTCENFT.BuffType.JumpPower];
                    args.moveSpeedMultAdd += 1f * component.buffStacks[GOTCENFT.BuffType.MoveSpeed];
                    args.attackSpeedMultAdd += 1f * component.buffStacks[GOTCENFT.BuffType.AttackSpeed];
                    args.critAdd += 100f * component.buffStacks[GOTCENFT.BuffType.Crit];
                    args.armorAdd += 100f * component.buffStacks[GOTCENFT.BuffType.Armor];
                    args.critDamageMultAdd += 1f * component.buffStacks[GOTCENFT.BuffType.CritDamage];
                    args.shieldMultAdd += 1f * component.buffStacks[GOTCENFT.BuffType.Shield];
                }
            }
        }

        public void Inventory_GiveItem_ItemIndex_int(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            GOTCENFT component = self.GetComponent<GOTCENFT>();
            if (!component) component = self.gameObject.AddComponent<GOTCENFT>();
            orig(self, itemIndex, count);
            if (NetworkServer.active && itemIndex == ItemDef.itemIndex) for (var i = 0; i < count; i++) component.AddBuff();
        }

        public void Inventory_RemoveItem_ItemIndex_int(On.RoR2.Inventory.orig_RemoveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            GOTCENFT component = self.GetComponent<GOTCENFT>();
            if (!component) component = self.gameObject.AddComponent<GOTCENFT>();
            orig(self, itemIndex, count);
            if (NetworkServer.active && itemIndex == ItemDef.itemIndex) for (var i = 0; i < count; i++) component.RemoveBuff();
        }

        public class GOTCENFT : MonoBehaviour
        {
            public enum BuffType
            {
                MaxHealth,
                Damage,
                Regen,
                JumpPower,
                MoveSpeed,
                AttackSpeed,
                Crit,
                Armor,
                CritDamage,
                Shield
            }

            public static List<BuffType> buffTypes;
            public List<BuffType> buffOrder;
            public Dictionary<BuffType, int> buffStacks;

            internal static void Init()
            {
                buffTypes = ((BuffType[])System.Enum.GetValues(typeof(BuffType))).ToList();
            }

            public void Awake()
            {
                buffStacks = new Dictionary<BuffType, int>();
                for (var i = 0; i < buffTypes.Count; i++) buffStacks.Add(buffTypes[i], 0);
                buffOrder = new List<BuffType>();
            }

            public void AddBuff()
            {
                AddBuff(RoR2Application.rng.NextElementUniform(buffTypes));
            }

            public void AddBuff(BuffType chosenBuffType)
            {
                if (NetworkServer.active)
                    new SyncAddBuff(gameObject.GetComponent<NetworkIdentity>().netId, (int)chosenBuffType).Send(NetworkDestination.Clients);
                if (!buffOrder.Contains(chosenBuffType)) buffOrder.Add(chosenBuffType);
                buffStacks[chosenBuffType]++;
            }

            public void RemoveBuff()
            {
                if (NetworkServer.active)
                    new SyncRemoveBuff(gameObject.GetComponent<NetworkIdentity>().netId).Send(NetworkDestination.Clients);
                if (buffOrder.Count > 0)
                {
                    buffStacks[buffOrder[buffOrder.Count - 1]]--;
                    if (buffStacks[buffOrder[buffOrder.Count - 1]] <= 0) buffOrder.RemoveAt(buffOrder.Count - 1);
                }
            }

            public class SyncAddBuff : INetMessage
            {
                private NetworkInstanceId objID;
                private int chosenBuffType;

                public SyncAddBuff()
                {
                }

                public SyncAddBuff(NetworkInstanceId objID, int chosenBuffType)
                {
                    this.objID = objID;
                    this.chosenBuffType = chosenBuffType;
                }

                public void Deserialize(NetworkReader reader)
                {
                    objID = reader.ReadNetworkId();
                    chosenBuffType = reader.ReadInt32();
                }

                public void OnReceived()
                {
                    if (NetworkServer.active) return;
                    GameObject obj = Util.FindNetworkObject(objID);
                    if (obj)
                    {
                        GOTCENFT controller = obj.GetComponent<GOTCENFT>();
                        if (controller)
                        {
                            controller.AddBuff((BuffType)chosenBuffType);
                        }
                    }
                }

                public void Serialize(NetworkWriter writer)
                {
                    writer.Write(objID);
                    writer.Write(chosenBuffType);
                }
            }

            public class SyncRemoveBuff : INetMessage
            {
                private NetworkInstanceId objID;

                public SyncRemoveBuff()
                {
                }

                public SyncRemoveBuff(NetworkInstanceId objID)
                {
                    this.objID = objID;
                }

                public void Deserialize(NetworkReader reader)
                {
                    objID = reader.ReadNetworkId();
                }

                public void OnReceived()
                {
                    if (NetworkServer.active) return;
                    GameObject obj = Util.FindNetworkObject(objID);
                    if (obj)
                    {
                        GOTCENFT controller = obj.GetComponent<GOTCENFT>();
                        if (controller)
                        {
                            controller.RemoveBuff();
                        }
                    }
                }

                public void Serialize(NetworkWriter writer)
                {
                    writer.Write(objID);
                }
            }
        }
    }
}