using UnityEngine;
using BepInEx.Configuration;
using GOTCE.EntityStatesCustom.Cracktain;
using RoR2.Items;

namespace GOTCE.Items.VoidLunar
{
    public class UranialRaegis : ItemBase<UranialRaegis>
    {
        public override string ConfigName => "Uranial Raegis";

        public override string ItemName => "Uranial Raegis";

        public override string ItemLangTokenName => "GOTCE_UranialRaegis";

        public override string ItemPickupDesc => "Randomly create a Ward of Barrier. ALL characters gain barrier while in the Ward.";

        public override string ItemFullDescription => "Creates a <style=cIsHealing>Ward of Barrier</style> in a random location nearby that buffs both enemies and allies within <style=cIsHealing>16m</style> <style=cStack>(+50% per stack)</style>, causing them to gain <style=cIsHealing>7% barrier</style> every second.";

        public override string ItemLore => "\"The original information super-highway: the spine. A strait-laced path from the body to the brain, it is responsible for ensuring the survival of all life. However, its limitations involve being localized to its parent body. What if - just like the development of the inter-computer wireless network - one's nervous system could communicate with another? What if the spine traced a path not just from body to mind, but a path that intersects between other bodies and minds?\"\r\n\r\n- Notes of Bob Michales, Serial Shiller and Bad Scientist";

        public override ItemTier Tier => Tiers.LunarVoid.Instance.TierEnum;

        public override ItemTierDef OverrideTierDef => Tiers.LunarVoid.Instance.tier;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, GOTCETags.BarrierRelated };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/UranialRaegis.png");

        public static GameObject ward;
        public static BuffDef barrierBuff;

        public override void Init(ConfigFile config)
        {
            base.Init(config);

            barrierBuff = ScriptableObject.CreateInstance<BuffDef>();

            barrierBuff.isHidden = true;
            barrierBuff.isCooldown = false;
            barrierBuff.isDebuff = false;
            barrierBuff.canStack = false;

            ContentAddition.AddBuffDef(barrierBuff);

            ward = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.DamageZoneWard.Load<GameObject>(), "BarrierWard", true);

            var buffWard = ward.GetComponent<BuffWard>();
            buffWard.buffDef = barrierBuff;

            PrefabAPI.RegisterNetworkPrefab(ward);
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.BuffWard.BuffTeam += BuffWard_BuffTeam;
            CharacterBody.onBodyInventoryChangedGlobal += CharacterBody_onBodyInventoryChangedGlobal;
        }

        private void CharacterBody_onBodyInventoryChangedGlobal(CharacterBody body)
        {
            var stack = GetCount(body);
            if (stack > 0 && body.GetComponent<RaegisSpawner>() == null)
            {
                body.gameObject.AddComponent<RaegisSpawner>();
            }
            else
            {
                GameObject.Destroy(body.gameObject.GetComponent<RaegisSpawner>());
            }
        }

        private void BuffWard_BuffTeam(On.RoR2.BuffWard.orig_BuffTeam orig, BuffWard self, IEnumerable<TeamComponent> recipients, float radiusSqr, Vector3 currentPosition)
        {
            orig(self, recipients, radiusSqr, currentPosition);
            foreach (TeamComponent teamComponent in recipients)
            {
                var characterBody = teamComponent.GetComponent<CharacterBody>();
                if (characterBody.HasBuff(barrierBuff))
                {
                    var distance = teamComponent.transform.position - currentPosition;
                    if (distance.sqrMagnitude <= radiusSqr)
                    {
                        if (characterBody && characterBody.GetComponent<BarrierWardControllerItsAComponentCauseICantBeBotheredToCopyPasteHealingWardAndNetworkItProperlySoYeah>() == null && (!characterBody.characterMotor || characterBody.characterMotor.isGrounded))
                        {
                            characterBody.gameObject.AddComponent<BarrierWardControllerItsAComponentCauseICantBeBotheredToCopyPasteHealingWardAndNetworkItProperlySoYeah>();
                        }
                    }
                    else if (distance.sqrMagnitude > radiusSqr && characterBody.GetComponent<BarrierWardControllerItsAComponentCauseICantBeBotheredToCopyPasteHealingWardAndNetworkItProperlySoYeah>() != null)
                    {
                        GameObject.Destroy(characterBody.GetComponent<BarrierWardControllerItsAComponentCauseICantBeBotheredToCopyPasteHealingWardAndNetworkItProperlySoYeah>());
                    }
                }
            }
        }

        private Vector3? FindWardSpawnPosition(Vector3 ownerPosition)
        {
            if (!SceneInfo.instance)
            {
                return null;
            }
            NodeGraph groundNodes = SceneInfo.instance.groundNodes;
            if (!groundNodes)
            {
                return null;
            }
            List<NodeGraph.NodeIndex> list = groundNodes.FindNodesInRange(ownerPosition, 0f, 50f, HullMask.None);
            if (list.Count > 0 && groundNodes.GetNodePosition(list[(int)UnityEngine.Random.Range(0f, (float)list.Count)], out Vector3 vector))
            {
                return new Vector3?(vector);
            }
            return null;
        }
    }

    public class RaegisSpawner : MonoBehaviour
    {
        public CharacterBody body;
        public int stack = 0;
        public float timer;
        public float interval = 30f;
        public bool isSpawned = false;
        public GameObject wardObject;

        public void Start()
        {
            body = GetComponent<CharacterBody>();
            var inventory = body.inventory;
            if (inventory)
            {
                stack = inventory.GetItemCount(UranialRaegis.Instance.ItemDef);
            }
        }

        public void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;
            if (timer >= interval)
            {
                if (!isSpawned)
                {
                    Vector3? wardPosition = FindWardSpawnPosition(body.corePosition);
                    if (wardPosition != null)
                    {
                        wardObject = Instantiate<GameObject>(UranialRaegis.ward, wardPosition.Value, Quaternion.identity);
                        Util.PlaySound("Play_randomDamageZone_appear", wardObject.gameObject);

                        wardObject.GetComponent<TeamFilter>().teamIndex = TeamIndex.None;

                        var buffWard = wardObject.GetComponent<BuffWard>();
                        buffWard.Networkradius = 30f * Mathf.Pow(1.5f, stack - 1);
                        buffWard.expireDuration = interval;
                        NetworkServer.Spawn(wardObject);
                        isSpawned = true;
                    }
                }
                else
                {
                    NetworkServer.Destroy(wardObject);
                }

                timer = 0f;
            }
        }

        private Vector3? FindWardSpawnPosition(Vector3 ownerPosition)
        {
            if (!SceneInfo.instance)
            {
                return null;
            }
            NodeGraph groundNodes = SceneInfo.instance.groundNodes;
            if (!groundNodes)
            {
                return null;
            }
            List<NodeGraph.NodeIndex> list = groundNodes.FindNodesInRange(ownerPosition, 0f, 50f, HullMask.None);
            Vector3 vector;
            if (list.Count > 0 && groundNodes.GetNodePosition(list[(int)UnityEngine.Random.Range(0f, (float)list.Count)], out vector))
            {
                return new Vector3?(vector);
            }
            return null;
        }
    }

    public class BarrierWardControllerItsAComponentCauseICantBeBotheredToCopyPasteHealingWardAndNetworkItProperlySoYeah : MonoBehaviour
    {
        public HealthComponent healthComponent;
        public float interval = 0.5f;
        public float timer;

        public void Start()
        {
            healthComponent = GetComponent<HealthComponent>();
        }

        public void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;
            if (timer >= interval)
            {
                healthComponent?.AddBarrier(healthComponent.fullCombinedHealth * 0.035f);
                timer = 0f;
            }
        }
    }
}