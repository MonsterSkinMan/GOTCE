using BepInEx.Configuration;
using R2API;
using RoR2;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using RoR2.Projectile;

namespace GOTCE.Items.Lunar
{
    public class EntropicEmblem : ItemBase<EntropicEmblem>
    {
        public override string ConfigName => "Entropic Emblem";

        public override string ItemName => "Entropic Emblem";

        public override string ItemLangTokenName => "GOTCE_EntropicEmblem";

        public override string ItemPickupDesc => "Nearby enemies and projectiles are instantly deleted, but interactables and allies are aswell...";

        public override string ItemFullDescription => "Instantly delete all enemies, allies, projectiles, and interactables within 30m of you (+30m per stack)";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { GOTCETags.Unstable, GOTCETags.Bullshit};

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
            On.RoR2.CharacterBody.OnInventoryChanged += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    self.AddItemBehavior<Emblem>(GetCount(self));
                }
            };
        }

        private class Emblem : CharacterBody.ItemBehavior {
            private float stopwatch = 0f;
            private float delay = 0.05f;
            private float radius = 30f;
            private GameObject voidVfx = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/CritGlassesVoid/CritGlassesVoidExecuteEffect.prefab").WaitForCompletion();
            private GameObject indicator;

            private void Start() {
                indicator = GameObject.CreatePrimitive(PrimitiveType.Sphere).InstantiateClone("EntropicIndicator");
                indicator.transform.SetParent(gameObject.transform);
                indicator.layer = LayerIndex.projectile.intVal;
                indicator.GetComponent<MeshRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Teleporters/matTeleporterRangeIndicator.mat").WaitForCompletion();
            }
        
            private void FixedUpdate() {
                indicator.transform.position = body.corePosition;
                indicator.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
                if (NetworkServer.active) {
                    stopwatch += Time.fixedDeltaTime;

                    radius = 30f * stack;
                    
                    if (stopwatch >= delay) {
                        stopwatch = 0f;
                        Collider[] colliders = Physics.OverlapSphere(body.corePosition, radius, ~0 , QueryTriggerInteraction.Collide);

                        foreach (Collider col in colliders) {
                            if (col.gameObject == gameObject) {
                                // this is the player holding the item, pass
                            }
                            else {
                                if (col.GetComponent<EntityLocator>()) {
                                    if (col.GetComponent<EntityLocator>().entity.GetComponent<PurchaseInteraction>() || col.GetComponent<EntityLocator>().entity.GetComponent<BarrelInteraction>() || col.GetComponent<EntityLocator>().entity.GetComponent<ScrapperController>()) {
                                        EffectManager.SpawnEffect(voidVfx, new EffectData
                                        {
                                            origin = col.transform.position,
                                            scale = 0.5f
                                        }, true);
                                        Destroy(col.GetComponent<EntityLocator>().entity);
                                        Destroy(col.gameObject);
                                    }
                                }

                                else if (col.GetComponent<CharacterBody>()) { // dont check parent for body since a body will always have it's own collider
                                    EffectManager.SpawnEffect(voidVfx, new EffectData
                                    {
                                        origin = col.transform.position,
                                        scale = 0.5f
                                    }, true);
                                    Destroy(col.gameObject);
                                }

                                else if (col.GetComponent<ProjectileController>() || col.GetComponentInParent<ProjectileController>()) {
                                    EffectManager.SpawnEffect(voidVfx, new EffectData
                                    {
                                        origin = col.transform.position,
                                        scale = 0.5f
                                    }, true);
                                    if (col.transform.parent) {
                                        Destroy(col.transform.parent.gameObject);
                                    }
                                    else {
                                        Destroy(col.gameObject);
                                    }
                                }

                                else if (col.GetComponent<GenericPickupController>()) {
                                    EffectManager.SpawnEffect(voidVfx, new EffectData
                                    {
                                        origin = col.transform.position,
                                        scale = 0.5f
                                    }, true);
                                    Destroy(col.gameObject);
                                }

                                else if (col.GetComponent<TeleporterInteraction>()) {
                                    EffectManager.SpawnEffect(voidVfx, new EffectData
                                    {
                                        origin = col.transform.position,
                                        scale = 0.5f
                                    }, true);
                                    Destroy(col.gameObject);
                                }

                                else if (col.gameObject.name == "TeleporterBaseMesh") {
                                    Destroy(GameObject.Find("Teleporter1(Clone)"));
                                    EffectManager.SpawnEffect(voidVfx, new EffectData
                                    {
                                        origin = col.transform.position,
                                        scale = 0.5f
                                    }, true);
                                    Destroy(col.gameObject);
                                }
                            }
                        }
                    }
                }
            }

            private void OnDestroy() {
                Destroy(indicator);
            }
        }

    }
}