using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using BepInEx.Configuration;

namespace GOTCE.Items.Red
{
    public class ZaniestTree : ItemBase<ZaniestTree>
    {
        public override string ItemName => "Zaniest Tree";

        public override string ConfigName => ItemName;

        public override string ItemLangTokenName => "GOTCE_ZaniestTree";

        public override string ItemPickupDesc => "motivational poster.";

        public override string ItemFullDescription => "Gain a Zany Tree ally. The Zany Tree <style=cIsUtility>laughs</style> at targets, launching them back <style=cIsUtility>8m</style> <style=cStack>(+4m per stack)</style> and stuns them for 1 second.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/ZaniestTree.png");

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
            RecalculateStatsAPI.GetStatCoefficients += (body, args) => {
                body.AddItemBehavior<ZanyTreeController>(GetCount(body));
            };
        }

        public class ZanyTreeController : CharacterBody.ItemBehavior {
            public float teleportStopwatch = 0f;
            public float laughStopwatch;
            public static float teleportTimer = 10f;
            public static float laughTimer = 4f;
            GameObject tree;

            public void Start() {
                // tree = GameObject.Instantiate(models[Random.Range(0, models.Count)].Load<GameObject>());
            }

            public void FixedUpdate() {
                teleportStopwatch -= Time.fixedDeltaTime;
                laughStopwatch -= Time.fixedDeltaTime;

                if (teleportStopwatch <= 0f) {
                    teleportStopwatch = teleportTimer;

                    Vector3 position = Utils.MiscUtils.GetSafePositionsWithinDistance(body.corePosition, 30f).GetRandom();
                    tree.transform.position = position;
                }

                if (laughStopwatch <= 0f) {
                    laughStopwatch = laughTimer;

                    BlastAttack attack = new();
                    attack.baseDamage = 0f;
                    attack.baseForce = 4000f * stack;
                    attack.damageType = DamageType.Stun1s;
                    attack.radius = 30f;
                    attack.teamIndex = TeamIndex.Player;

                    AkSoundEngine.PostEvent(Events.Play_lunar_golem_death, base.gameObject);
                }
            }

            public void OnDestroy() {
                GameObject.Destroy(tree);
            }
        }
    }
}