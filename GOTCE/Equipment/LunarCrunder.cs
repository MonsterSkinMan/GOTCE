using RoR2;
using R2API;
using UnityEngine;
using BepInEx.Configuration;
using System;
using RoR2.Projectile;
using UnityEngine.Networking;

namespace GOTCE.Equipment
{
    public class LunarCrunder : EquipmentBase<LunarCrunder>
    {
        public override string EquipmentName => "The Crowdfunder 2";

        public override string EquipmentLangTokenName => "GOTCE_LunarCrunder";

        public override string EquipmentPickupDesc => "Unleash a rapid-fire barrage of high-damage lunar shards. Consumes Lunar Coins.";

        public override string EquipmentFullDescription => "Fire a rapid stream of lunar shards for <style=cIsDamage>5000% damage</style> per bullet. Consumes <style=cIsUtility>1 Lunar Coin</style> per second of firing.";

        public override string EquipmentLore => "";
        public override bool IsLunar => true;
        public override bool CanBeRandomlyTriggered => false;

        public override GameObject EquipmentModel => Main.MainAssets.LoadAsset<GameObject>("Assets/Models/Prefabs/Item/LunarGat/mdlGoldGat.prefab");

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
            On.RoR2.EquipmentSlot.UpdateGoldGat += UpdateLunarGat;
        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            return false;
        }

        public void UpdateLunarGat(On.RoR2.EquipmentSlot.orig_UpdateGoldGat orig, EquipmentSlot self)
        {
            bool flag = self.equipmentIndex == EquipmentDef._equipmentIndex;
            LunarGatController controller = self.characterBody.gameObject.GetComponent<LunarGatController>();
            if (flag != controller)
            {
                if (flag)
                {
                    // i did not nullcheck this so do not give lunargat to any non-players
                    self.characterBody.gameObject.AddComponent<LunarGatController>();
                }
                else
                {
                    GameObject.Destroy(controller);
                }
            }
        }
    }

    public class LunarGatController : MonoBehaviour
    {
        public CharacterBody body;
        private float timer = 0f;
        private float timerCoins;
        private float interval = 0.1f;
        private float intervalCoins = 1f;
        private GameObject prefab;
        private CharacterMaster master;
        private InputBankTest input;
        private NetworkUser net;
        private bool shouldFire = false;

        public void Awake()
        {
            prefab = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Brother/LunarShardProjectile.prefab").WaitForCompletion();
            body = gameObject.GetComponent<CharacterBody>();
            input = gameObject.GetComponent<InputBankTest>();
            master = body.master;
            net = Util.LookUpBodyNetworkUser(body);
        }

        public void FixedUpdate()
        {
            if (input.activateEquipment.justPressed)
            {
                shouldFire = !shouldFire;
            }

            if (net.lunarCoins <= 0)
            {
                shouldFire = false;
            }

            timer += Time.fixedDeltaTime;
            if (timer >= interval && shouldFire)
            {
                timer = 0f;
                FireProjectileInfo info = new FireProjectileInfo();
                info.damage = body.damage *= 5000f;
                info.projectilePrefab = prefab;
                info.speedOverride = 350f;
                info.crit = Util.CheckRoll(body.crit, body.master);
                info.damageColorIndex = DamageColorIndex.WeakPoint;
                info.position = body.corePosition;
                info.rotation = Util.QuaternionSafeLookRotation(Util.ApplySpread(body.equipmentSlot.GetAimRay().direction, -1.5f, 1.5f, -1.5f, 1.5f));
                info.owner = body.gameObject;
                info.damageTypeOverride = DamageType.CrippleOnHit;

                ProjectileManager.instance.FireProjectileServer(info);
            }

            timerCoins += Time.fixedDeltaTime;
            if (timerCoins >= intervalCoins && shouldFire)
            {
                if (NetworkServer.active)
                {
                    net.DeductLunarCoins(1);
                }
                else
                {
                    net.CallRpcDeductLunarCoins(1);
                }
                timerCoins = 0f;
            }
        }
    }
}