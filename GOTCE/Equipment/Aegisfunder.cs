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

        public override string EquipmentFullDescription => "Fire a rapid stream of <style=cIsHealing>Aegises</style> for <style=cIsDamage>250% damage</style> that give <style=cIsHealing>10% barrier</style> on hit. Consumes <style=cIsHealing>1 Aegis</style> per second of firing.";

        public override string EquipmentLore => "The Aegisfunder. 100% Aegis per Aegis is EXTREMELY aegis, even if you <i>do</i> have a crazy Aegis of on-Aegis Aegises to scale Aegis with. Yes, the Aegis changes makes this a more lucrative Aegis <i>ON AEGIS</i>, but in Aegis, if you're already at the Aegis of applying a stupid amount of Aegis, you probably don't need the Aegisfunder to help you with that. Take something Aegis.";
        public override bool CanBeRandomlyTriggered => false;

        public override GameObject EquipmentModel => null;
        public static DamageAPI.ModdedDamageType AegisType = DamageAPI.ReserveDamageType();

        public override Sprite EquipmentIcon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Items/TheAegisfunder.png");

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
            On.RoR2.GlobalEventManager.OnHitEnemy += Barrier;
        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            return false;
        }

        private void Barrier(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo info, GameObject victim)
        {
            orig(self, info, victim);
            if (NetworkServer.active && info.attacker && info.attacker.GetComponent<CharacterBody>())
            {
                if (info.HasModdedDamageType(AegisType))
                {
                    CharacterBody body = info.attacker.GetComponent<CharacterBody>();
                    body.healthComponent.AddBarrier(body.healthComponent.fullHealth * 0.1f);
                }
            }
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
            prefab = Main.SecondaryAssets.LoadAsset<GameObject>("Assets/Prefabs/Projectiles/AegisFunder/AegisProjectile.prefab").InstantiateClone("guh");
            body = gameObject.GetComponent<CharacterBody>();
            input = gameObject.GetComponent<InputBankTest>();
            master = body.master;
            DamageAPI.ModdedDamageTypeHolderComponent holder = prefab.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            holder.Add(Aegisfunder.AegisType);
            PrefabAPI.RegisterNetworkPrefab(prefab);
        }

        public void FixedUpdate()
        {
            if (input.activateEquipment.justPressed)
            {
                shouldFire = !shouldFire;
            }

            if (master.inventory.GetItemCount(RoR2Content.Items.BarrierOnOverHeal) <= 0 || body.healthComponent.health <= 0)
            {
                shouldFire = false;
            }

            timer += Time.fixedDeltaTime;
            if (timer >= interval && shouldFire & body.hasAuthority)
            {
                timer = 0f;
                ProcChainMask mask = new ProcChainMask();
                GameObject proj = prefab.InstantiateClone("guh");
                FireProjectileInfo info = new()
                {
                    damage = body.damage * 250f,
                    projectilePrefab = prefab,
                    speedOverride = 350f,
                    crit = Util.CheckRoll(body.crit, body.master),
                    damageColorIndex = DamageColorIndex.WeakPoint,
                    position = body.corePosition + new Vector3(0f, 0.25f, 0f),
                    rotation = Util.QuaternionSafeLookRotation(Util.ApplySpread(body.equipmentSlot.GetAimRay().direction, -0.5f, 0.5f, -0.5f, 0.5f)),
                    owner = body.gameObject,
                    procChainMask = mask,
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