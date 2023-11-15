using UnityEngine;

namespace GOTCE.Items.NoTier
{
    public class Parasite : ItemBase<Parasite>
    {
        public override string ConfigName => "Parasite";

        public override string ItemName => "ᅠParasite";

        public override string ItemLangTokenName => "Parasite";

        public override string ItemPickupDesc => "ᅠᅠᅠᅠParasite";

        public override string ItemFullDescription => "ᅠᅠParasite";

        public override string ItemLore => "ᅠᅠᅠᅠParasite";

        public override ItemTier Tier => ItemTier.NoTier;

        public override Enum[] ItemTags => new Enum[] { ItemTag.CannotSteal };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/Parasite.png");

        public override bool CanRemove => false;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            GlobalEventManager.OnInteractionsGlobal += GlobalEventManager_OnInteractionsGlobal;
            GlobalEventManager.onCharacterLevelUp += GlobalEventManager_onCharacterLevelUp;
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            GlobalEventManager.onServerCharacterExecuted += GlobalEventManager_onServerCharacterExecuted;
            HealthComponent.onCharacterHealServer += HealthComponent_onCharacterHealServer;
            On.RoR2.HealthComponent.AddBarrier += HealthComponent_AddBarrier;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.CharacterBody.FixedUpdate += CharacterBody_FixedUpdate;
        }

        private void CharacterBody_FixedUpdate(On.RoR2.CharacterBody.orig_FixedUpdate orig, CharacterBody self)
        {
            orig(self);
            RollAndAdd(0.000005f, self);
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            orig(self, damageInfo);
            RollAndAdd(0.0002f, self.body);
        }

        private void HealthComponent_AddBarrier(On.RoR2.HealthComponent.orig_AddBarrier orig, HealthComponent self, float value)
        {
            orig(self, value);
            RollAndAdd(0.001f, self.body);
        }

        private void HealthComponent_onCharacterHealServer(HealthComponent hc, float idc, ProcChainMask idc2)
        {
            RollAndAdd(0.0001f, hc.body);
        }

        private void GlobalEventManager_onServerCharacterExecuted(DamageReport report, float idk)
        {
            RollAndAdd(0.005f, report.attackerBody);
        }

        private void GlobalEventManager_onServerDamageDealt(DamageReport report)
        {
            RollAndAdd(0.00009f, report.attackerBody);
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport report)
        {
            RollAndAdd(0.0009f, report.attackerBody);
        }

        private void GlobalEventManager_onCharacterLevelUp(CharacterBody body)
        {
            RollAndAdd(0.009f, body);
        }

        private void GlobalEventManager_OnInteractionsGlobal(Interactor interactor, IInteractable interactable, GameObject interactableObject)
        {
            var body = interactor.GetComponent<CharacterBody>();
            RollAndAdd(0.002f, body);
        }

        private void RollAndAdd(float chance, CharacterBody body = null, Inventory inventory = null, CharacterMaster master = null)
        {
            if (Run.instance)
            {
                if (Run.instance.runRNG != null && Run.instance.runRNG.RangeFloat(0f, 1f) < chance)
                {
                    if (inventory)
                    {
                        inventory.GiveItem(Instance.ItemDef);
                    }

                    if (body)
                    {
                        var bodyInventory = body.inventory;
                        if (bodyInventory)
                        {
                            bodyInventory.GiveItem(Instance.ItemDef);
                        }
                    }

                    if (master)
                    {
                        var bodyFromMaster = master.GetBody();
                        if (bodyFromMaster)
                        {
                            var bodyInventory = bodyFromMaster.inventory;
                            if (bodyInventory)
                            {
                                bodyInventory.GiveItem(Instance.ItemDef);
                            }
                        }
                    }

                    var toLog = (Run.instance.runRNG.RangeFloat(0f, 1f)) switch
                    {
                        > 0.9f => ":3",
                        < 0.1f => "OwO",
                        _ => "UwU"
                    };

                    Debug.Log(toLog);
                }
            }
        }
    }
}