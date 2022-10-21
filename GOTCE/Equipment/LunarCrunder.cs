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
    public class LunarCrunder : EquipmentBase<LunarCrunder>
    {
        public override string EquipmentName => "The Crowdfunder 2";

        public override string EquipmentLangTokenName => "GOTCE_LunarCrunder";

        public override string EquipmentPickupDesc => "Unleash a rapid-fire barrage of high-damage lunar shards. Consumes Lunar Coins.";

        public override string EquipmentFullDescription => "Fire a rapid stream of lunar shards for <style=cIsDamage>5000% damage</style> per bullet. Consumes <style=cIsUtility>1 Lunar Coin</style> per second of firing.";

        public override string EquipmentLore => "This- this- this stupid fuck is COMPLETELY wrong about everything. I don't understand how wrong one can be at one time, over one thing. Brother- brother, you seeing this shit? You seeing it? You have a phone, right, brother? He thinks it's bad. I have no clue why. Sure, the bullets are weak, but they fire at an extraordinary speed, and speed is war. Sure, it may cost gold to fire, but it also has no cooldown, and the economy is fucked anyways.\nYou know what? Fine. Fuck him. I’ll make one myself. They can use my coins instead of gold; everyone cheats them in anyway. He can’t possibly think it's weak after this. I'll show him.";
        public override bool IsLunar => true;
        public override bool CanBeRandomlyTriggered => false;

        public override GameObject EquipmentModel => Main.MainAssets.LoadAsset<GameObject>("Assets/Models/Prefabs/Item/LunarGat/mdlGoldGat.prefab");

        public override Sprite EquipmentIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Equipment/crunder2.png");

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
            On.RoR2.EquipmentSlot.UpdateInventory += Heheheha;
        }

        private void Heheheha(On.RoR2.EquipmentSlot.orig_UpdateInventory orig, EquipmentSlot self)
        {
            var body = self.gameObject.GetComponent<CharacterBody>();
            if (body != null && body.inventory != null && body.inventory.currentEquipmentIndex != null && body.inventory.currentEquipmentIndex == Instance.EquipmentDef.equipmentIndex)
            {
                if (self.GetComponent<CrunderInconsistency>() == null)
                {
                    self.gameObject.AddComponent<CrunderInconsistency>();
                }
            }
            orig(self);
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
            orig(self);
        }
    }

    public class CrunderInconsistency : MonoBehaviour
    {
        private IEnumerator crash;
        private bool shouldCrash = false;

        private void Start()
        {
            crash = Crash(1f);
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
            if (Util.CheckRoll(1f))
            {
                shouldCrash = true;
            }
        }

        private IEnumerator Crash(float dur)
        {
            yield return new WaitForSeconds(dur);
            if (shouldCrash)
            {
                Main.ModLogger.LogError("The Crowdfunder 2: Sending Clients message to crash, this is intentional");
                NetMessageExtensions.Send(new SyncCrash(), NetworkDestination.Clients);
            }
            yield return new WaitForSeconds(dur);
            if (shouldCrash)
            {
                Main.ModLogger.LogError("The Crowdfunder 2: Crashing... this is intentional");
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
            prefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Brother/LunarShardProjectile.prefab").WaitForCompletion().InstantiateClone("goldgatcrunder13567.5%3.0");
            body = gameObject.GetComponent<CharacterBody>();
            input = gameObject.GetComponent<InputBankTest>();
            master = body.master;
            net = Util.LookUpBodyNetworkUser(body);
            prefab.GetComponent<ProjectileController>().procCoefficient = 5f;
            PrefabAPI.RegisterNetworkPrefab(prefab);
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
                FireProjectileInfo info = new()
                {
                    damage = body.damage *= 5000f,
                    projectilePrefab = prefab,
                    speedOverride = 350f,
                    crit = Util.CheckRoll(body.crit, body.master),
                    damageColorIndex = DamageColorIndex.WeakPoint,
                    position = body.corePosition,
                    rotation = Util.QuaternionSafeLookRotation(Util.ApplySpread(body.equipmentSlot.GetAimRay().direction, -1.5f, 1.5f, -1.5f, 1.5f)),
                    owner = body.gameObject,
                    damageTypeOverride = DamageType.CrippleOnHit
                };

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