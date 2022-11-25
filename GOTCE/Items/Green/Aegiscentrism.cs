using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using RoR2.Projectile;
using System;

namespace GOTCE.Items.Green
{
    public class Aegiscentrism : ItemBase<Aegiscentrism>
    {
        public override string ConfigName => "Aegiscentrism";

        public override string ItemName => "Aegiscentrism";

        public override string ItemLangTokenName => "GOTCE_Aegiscentrism";

        public override string ItemPickupDesc => "Gain multiple orbital aegises. Every minute, assimilate another item into Aegicentrism.";

        public override string ItemFullDescription => "Every second, gain up to 6 (+2 per stack) orbiting aegises that give you 5% barrier after 10 seconds. Every 60 seconds, a random item is converted into this item.";

        public override string ItemLore => "i love aegis";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Healing, ItemTag.Utility, ItemTag.AIBlacklist, GOTCETags.BarrierRelated};

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

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
           RecalculateStatsAPI.GetStatCoefficients += Aegis;
        }

        public void Aegis(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args) {
            if (body && NetworkServer.active && body.inventory) {
                bool flag = body.inventory.GetItemCount(ItemDef) > 0;
                OrbitalAegisBehavior controller = body.gameObject.GetComponent<OrbitalAegisBehavior>();
                if (flag != controller) {
                    if (flag) {
                        body.gameObject.AddComponent<OrbitalAegisBehavior>();
                    }
                    if (!flag) {
                        GameObject.DestroyImmediate(controller);
                    }
                }
            }
        }

        
    }

    public class OrbitalAegisBehavior : MonoBehaviour {
        private int max = 6;
        public int current = 0;
        private float delay = 3f;
        private float stopwatch = 0f;
        private float transformTimer = 0f;
        private float transformDelay = 60f;
        private int maxTmp;
        private CharacterBody body;

        public event Action<OrbitalAegisBehavior> onDisabled;

        public void Start() {
            body = gameObject.GetComponent<CharacterBody>();
        }
        public void FixedUpdate() {
            stopwatch += Time.fixedDeltaTime;
            max = 6 + 2*(body.inventory.GetItemCount(Aegiscentrism.Instance.ItemDef) - 1);
            transformTimer += Time.fixedDeltaTime;
            if (stopwatch >= delay) {
                stopwatch = 0f;
                if (current <= max) {
                    GameObject prefab = PrefabAPI.InstantiateClone(Main.SecondaryAssets.LoadAsset<GameObject>("Assets/Prefabs/Projectiles/Aegiscentrism/OrbitalAegis.prefab"), "AegisClone");
                    prefab.AddComponent<OrbitalAegisController>();
                    // prefab.AddComponent<ProjectileGhostCluster>();

                    FireProjectileInfo fireProjectileInfo = default(FireProjectileInfo);
                    fireProjectileInfo.projectilePrefab = prefab;
                    fireProjectileInfo.crit = body.RollCrit();
                    fireProjectileInfo.damage = body.damage * 0;
                    fireProjectileInfo.damageColorIndex = DamageColorIndex.Item;
                    fireProjectileInfo.force = 0f;
                    fireProjectileInfo.owner = gameObject;
                    fireProjectileInfo.position = body.transform.position;
                    fireProjectileInfo.rotation = Quaternion.identity;
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                    current++;
                }
            }

            if (transformTimer >= transformDelay) {
                transformTimer = 0f;
                if (NetworkServer.active) {
                    List<ItemIndex> list = new List<ItemIndex>(body.inventory.itemAcquisitionOrder);
                    ItemIndex itemIndex = ItemIndex.None;
                    Util.ShuffleList(list, Run.instance.treasureRng);
                    foreach (ItemIndex item in list)
                    {
                        if (item != Aegiscentrism.Instance.ItemDef.itemIndex)
                        {
                            ItemDef itemDef = ItemCatalog.GetItemDef(item);
                            if ((bool)itemDef && itemDef.tier != ItemTier.NoTier)
                            {
                                itemIndex = item;
                                break;
                            }
                        }
                    }
                    if (itemIndex != ItemIndex.None)
                    {
                        body.inventory.RemoveItem(itemIndex);
                        body.inventory.GiveItem(Aegiscentrism.Instance.ItemDef);
                        CharacterMasterNotificationQueue.SendTransformNotification(body.master, itemIndex, Aegiscentrism.Instance.ItemDef.itemIndex, CharacterMasterNotificationQueue.TransformationType.Suppressed);
                    }
                }
            }
        }

        public void InitializeOrbiter(ProjectileOwnerOrbiter orbiter, OrbitalAegisController controller)
        {
            float num = body.radius + 2f + UnityEngine.Random.Range(0.25f, 0.25f * 5f);
            float num2 = num / 2f;
            num2 *= num2;
            float degreesPerSecond = 180f * Mathf.Pow(0.9f, num2);
            Quaternion quaternion = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), Vector3.up);
            Quaternion quaternion2 = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 0f), Vector3.forward);
            Vector3 planeNormal = quaternion * quaternion2 * Vector3.up;
            float initialDegreesFromOwnerForward = UnityEngine.Random.Range(0f, 360f);
            orbiter.Initialize(planeNormal, num, degreesPerSecond, initialDegreesFromOwnerForward);
            onDisabled += DestroyOrbiter;
            void DestroyOrbiter(OrbitalAegisBehavior orbitalAegisBehavior)
            {
                if ((bool)controller)
                {
                    controller.Kill();
                }
            }
        }

        private void OnDisable()
        {
            this.onDisabled?.Invoke(this);
            this.onDisabled = null;
        }
    }

    public class OrbitalAegisController : MonoBehaviour {
        private float stopwatch = 0f;
        private float delay = 10f;
        private CharacterBody body;

        public void OnEnable() {
            if (NetworkServer.active)
            {
                ProjectileController component = GetComponent<ProjectileController>();
                if ((bool)component.owner)
                {
                    AcquireOwner(component);
                }
                else
                {
                    component.onInitialized += AcquireOwner;
                }
            }
        }

        private void AcquireOwner(ProjectileController controller)
        {
            controller.onInitialized -= AcquireOwner;
            CharacterBody component = controller.owner.GetComponent<CharacterBody>();
            if (component)
            {
                ProjectileOwnerOrbiter component2 = GetComponent<ProjectileOwnerOrbiter>();
                component.GetComponent<OrbitalAegisBehavior>().InitializeOrbiter(component2, this);
                body = component;
                // delay = delay / Mathf.Pow(2f, (body.inventory.GetItemCount(Aegiscentrism.Instance.ItemDef)));
            }
        }

        public void FixedUpdate() {
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= delay) {
                stopwatch = 0f;
                Aegis();
            }
        }

        public void Aegis() {
            if (body) {
                body.healthComponent.AddBarrier(body.maxHealth * 0.05f);
            }
        }

        public void Kill() {
            DestroyImmediate(gameObject);
        }
    }
}