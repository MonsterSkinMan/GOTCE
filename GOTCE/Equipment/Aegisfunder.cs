using RoR2;
using R2API;
using UnityEngine;
using BepInEx.Configuration;
using System;
using RoR2.Projectile;
using UnityEngine.Diagnostics;
using R2API.Networking.Interfaces;
using R2API.Networking;
using System.Collections;

namespace GOTCE.Equipment
{
    public class Aegisfunder : EquipmentBase<Aegisfunder>
    {
        public override string EquipmentName => "The Aegisfunder";

        public override string EquipmentLangTokenName => "GOTCE_Aegisfunder";

        public override string EquipmentPickupDesc => "Unleash a rapid-fire barrage of Aegises that give barrier on hit. Consumes Aegises.";

        public override string EquipmentFullDescription => "Fire a rapid stream of Aegises for <style=cIsDamage>250% damage</style> that give 10% barrier on hit. Consumes <style=cIsUtility>1 Aegis</style> per second of firing.";

        public override string EquipmentLore => "";
        public override bool CanBeRandomlyTriggered => false;

        public override GameObject EquipmentModel => null;

        public override Sprite EquipmentIcon => null;

        public override float Cooldown => 0f;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateEquipment();
            Hooks();
        }

        public override void Hooks()
        {
            On.RoR2.EquipmentSlot.UpdateGoldGat += UpdateAegisGat;
            // On.RoR2.GlobalEventManager.OnHitEnemy += Barrier;
        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            return false;
        }

        public void UpdateAegisGat(On.RoR2.EquipmentSlot.orig_UpdateGoldGat orig, EquipmentSlot self)
        {
            bool flag = self.equipmentIndex == EquipmentDef._equipmentIndex;
            AegisGatController controller = self.characterBody.gameObject.GetComponent<AegisGatController>();
            if (flag != controller)
            {
                if (flag)
                {
                    // i did not nullcheck this so do not give aegisgat to any non-players
                    self.characterBody.gameObject.AddComponent<AegisGatController>();
                }
                else
                {
                    GameObject.Destroy(controller);
                }
            }
            orig(self);
        }
    }

    public class AegisGatController : MonoBehaviour
    {
        public CharacterBody body;
        private float timer = 0f;
        private float timerAegis = 0f;
        private float interval = 0.1f;
        private float intervalAegis = 1f;
        private GameObject prefab;
        private GameObject aegis;
        private CharacterMaster master;
        private InputBankTest input;
        private bool shouldFire = false;

        public void Awake()
        {
            prefab = Main.SecondaryAssets.LoadAsset<GameObject>("Assets/Prefabs/Projectiles/AegisFunder/AegisProjectile.prefab");
            body = gameObject.GetComponent<CharacterBody>();
            input = gameObject.GetComponent<InputBankTest>();
            master = body.master;
            PrefabAPI.RegisterNetworkPrefab(prefab);
        }

        public void FixedUpdate()
        {
            if (input.activateEquipment.justPressed)
            {
                shouldFire = !shouldFire;
            }

            if (master.inventory.GetItemCount(RoR2Content.Items.BarrierOnOverHeal) <= 0)
            {
                shouldFire = false;
            }

            timer += Time.fixedDeltaTime;
            if (timer >= interval && shouldFire)
            {
                timer = 0f;
                ProcChainMask mask = new ProcChainMask();
                GameObject proj = prefab.InstantiateClone("guh");
                Vector3 forward = body.transform.forward;
                float distance = 3;
                Vector3 pos = body.transform.position + forward*distance;
                FireProjectileInfo info = new()
                {
                    damage = body.damage * 2.5f,
                    projectilePrefab = prefab,
                    speedOverride = 350f,
                    crit = Util.CheckRoll(body.crit, body.master),
                    damageColorIndex = DamageColorIndex.WeakPoint,
                    position = pos,
                    rotation = Util.QuaternionSafeLookRotation(Util.ApplySpread(body.equipmentSlot.GetAimRay().direction, -1.5f, 1.5f, -1.5f, 1.5f)),
                    owner = body.gameObject,
                    procChainMask = mask
                };

                ProjectileManager.instance.FireProjectile(info);
            }

            timerAegis += Time.fixedDeltaTime;
            if (timerAegis >= intervalAegis && shouldFire)
            {
                if (NetworkServer.active)
                {
                    master.inventory.RemoveItem(RoR2Content.Items.BarrierOnOverHeal);
                }
                timerAegis = 0f;
            }
        }

    }
} 