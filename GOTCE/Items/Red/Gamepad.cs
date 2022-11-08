using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using BepInEx.Configuration;
using UnityEngine.Profiling.Memory.Experimental;
using System.Reflection;
using System.Diagnostics;

namespace GOTCE.Items.Red
{
    public class Gamepad : ItemBase<Gamepad>
    {
        public override string ItemName => "Gamepad";

        public override string ConfigName => ItemName;

        public override string ItemLangTokenName => "GOTCE_Gamepad";

        public override string ItemPickupDesc => "Gain an increase to FOV crit chance based on your inputs per second";

        public override string ItemFullDescription => "Gain a 2% (+2% per stack) increase to FOV crit chance multiplied by your inputs per second.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.AIBlacklist, GOTCETags.Crit, GOTCETags.Masochist };

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
                self.AddItemBehavior<InputStacking>(GetCount(self));
            };
        }
    }

    public class InputStacking : CharacterBody.ItemBehavior {
        Components.GOTCE_StatsComponent stats;
        public void Start() {
            if (!body.masterObject.GetComponent<Components.GOTCE_StatsComponent>() && NetworkServer.active) {
                GameObject.DestroyImmediate(gameObject.GetComponent<InputStacking>());
            }
            else {
                stats = body.masterObject.GetComponent<Components.GOTCE_StatsComponent>();
            }
        }
        public void FixedUpdate() {
            if (body.hasAuthority) {
                if (Input.anyKey) {
                    stats.inputs++;
                }
            }
        }
    }
}