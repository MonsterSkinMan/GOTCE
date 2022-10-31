using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using GOTCE.Components;

namespace GOTCE.Items.White
{
    public class GummyVitamins : ItemBase<GummyVitamins>
    {
        public override bool CanRemove => true;
        public override string ConfigName => ItemName;
        public override string ItemFullDescription => "Gain a 8% (+8% per stack) chance to 'critcally sprint', doubling your sprinting speed.";
        public override Sprite ItemIcon => null;
        public override string ItemLangTokenName => "GOTCE_GummyVitamins";
        public override string ItemLore => "";
        public override GameObject ItemModel => null;
        public override string ItemName => "Gummy Vitamins";
        public override string ItemPickupDesc => "Gain a chance to 'critcally sprint', doubling your speed.";
        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage };
        public override ItemTier Tier => ItemTier.Tier1;
        public EventHandler<SprintCritEventArgs> OnSprintCrit;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += SprintCrit;
        }

        public void SprintCrit(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args) {
            if (NetworkServer.active && body.inventory) {
                if (body.masterObject && body.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                    GOTCE_StatsComponent stats = body.masterObject.GetComponent<GOTCE_StatsComponent>();
                    if (Util.CheckRoll(stats.sprintCritChance, body.master) && body.isSprinting) {
                        args.moveSpeedMultAdd += (body.sprintingSpeedMultiplier * 2);

                        EventHandler<SprintCritEventArgs> raiseEvent = OnSprintCrit;

                        // Event will be null if there are no subscribers
                        if (raiseEvent != null)
                        {
                            SprintCritEventArgs sprArgs = new(body);

                            // Call to raise the event.
                            raiseEvent(this, sprArgs);
                        }
                        // Debug.Log("starting crit");
                    }
                }
            }
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

    }

    public class SprintCritEventArgs {
        public CharacterBody Body;
        public SprintCritEventArgs(CharacterBody body) {
            Body = body;
        }
    }
}