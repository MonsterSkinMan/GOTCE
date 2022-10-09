using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using BepInEx.Configuration;

namespace GOTCE.Items.Red
{
    public class BottledEnigma : ItemBase<BottledEnigma>
    {
        public override string ConfigName => "Bottled Enigma";

        public override string ItemName => "Bottled Enigma";

        public override string ItemLangTokenName => "GOTCE_BottledEnigma";

        public override string ItemPickupDesc => "Rapidly triggers random Equipment effects. Gain 26 max health.";

        public override string ItemFullDescription => "Activates <style=cIsUtility>1</style> random <style=cIsUtility>equipment</style> every frame. Increases <style=cIsHealing>maximum health</style> by <style=cIsHealing>26</style> <style=cStack>(+26 per stack)</style>.";

        public override string ItemLore => "The world is a nonsensical place. Imparting any sort of universal truth or one-size-fits-all logic can only confuse you. The best way to adapt to the world is to embrace it. Let the cacophony of existence itself flow around you, rather than being destroyed by its torrential force. Power can be absorbed from insanity.";

        public override ItemTier Tier => ItemTier.Tier3;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage, ItemTag.Healing, ItemTag.Utility, ItemTag.EquipmentRelated };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/Bottled_Enigma.png");

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        private static readonly System.Random random = new();

        public override void Hooks()
        {
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            RecalculateStatsAPI.GetStatCoefficients += new RecalculateStatsAPI.StatHookEventHandler(OverpoweredHealthUp);
            On.RoR2.Inventory.CalculateEquipmentCooldownScale += Inventory_CalculateEquipmentCooldownScale;
            On.RoR2.EquipmentSlot.FixedUpdate += EquipmentSlot_FixedUpdate;
        }

        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);
            var inventoryCount = GetCount(self);
            if (inventoryCount > 0 && self.master && self.inventory)
            {
                switch (random.Next(1, 12))
                {
                    case 1:
                        self.master.inventory.SetEquipmentIndex(RoR2Content.Equipment.Blackhole.equipmentIndex);
                        break;

                    case 2:
                        self.master.inventory.SetEquipmentIndex(RoR2Content.Equipment.BurnNearby.equipmentIndex);
                        break;

                    case 3:
                        self.master.inventory.SetEquipmentIndex(RoR2Content.Equipment.CommandMissile.equipmentIndex);
                        break;

                    case 4:
                        self.master.inventory.SetEquipmentIndex(RoR2Content.Equipment.CrippleWard.equipmentIndex);
                        break;

                    case 5:
                        self.master.inventory.SetEquipmentIndex(RoR2Content.Equipment.Saw.equipmentIndex);
                        break;

                    case 6:
                        self.master.inventory.SetEquipmentIndex(RoR2Content.Equipment.DeathProjectile.equipmentIndex);
                        break;

                    case 7:
                        self.master.inventory.SetEquipmentIndex(RoR2Content.Equipment.DroneBackup.equipmentIndex);
                        break;

                    case 8:
                        self.master.inventory.SetEquipmentIndex(RoR2Content.Equipment.Scanner.equipmentIndex);
                        break;

                    case 9:
                        self.master.inventory.SetEquipmentIndex(RoR2Content.Equipment.Meteor.equipmentIndex);
                        break;

                    case 10:
                        self.master.inventory.SetEquipmentIndex(RoR2Content.Equipment.Gateway.equipmentIndex);
                        break;

                    case 11:
                        self.master.inventory.SetEquipmentIndex(DLC1Content.Equipment.VendingMachine.equipmentIndex);
                        break;

                    case 12:
                        self.master.inventory.SetEquipmentIndex(DLC1Content.Equipment.Molotov.equipmentIndex);
                        break;
                }
            }
        }

        public static void OverpoweredHealthUp(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.inventory)
            {
                var stack = body.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.baseHealthAdd += Instance.GetCount(body) * 26f;
                }
            }
        }

        private float Inventory_CalculateEquipmentCooldownScale(On.RoR2.Inventory.orig_CalculateEquipmentCooldownScale orig, Inventory self)
        {
            var stack = self.GetItemCount(Instance.ItemDef.itemIndex);
            float thing = 0.00001f;
            if (stack > 0)
            {
                return thing;
            }
            else
            {
                return orig(self);
            }
        }

        private void EquipmentSlot_FixedUpdate(On.RoR2.EquipmentSlot.orig_FixedUpdate orig, EquipmentSlot self)
        {
            var body = self.GetComponent<CharacterBody>();
            orig(self);
            if (GetCount(body) > 0)
            {
                bool shouldRun = false;
                if (self.equipmentIndex != RoR2Content.Equipment.GoldGat.equipmentIndex)
                {
                    if (!self.inputBank.activateEquipment.justPressed)
                    {
                        shouldRun = true;
                    }
                }
                if (shouldRun && self.characterBody.isEquipmentActivationAllowed && self.hasEffectiveAuthority)
                {
                    if (NetworkServer.active)
                    {
                        self.ExecuteIfReady();
                    }
                    else
                    {
                        self.CallCmdExecuteIfReady();
                    }
                }
            }
        }
    }
}