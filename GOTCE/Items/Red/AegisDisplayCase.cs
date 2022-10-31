using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using BepInEx.Configuration;
using System.Linq;

namespace GOTCE.Items.Red
{
    public class AegisDisplayCase : ItemBase<AegisDisplayCase>
    {
        public override string ConfigName => "Aegis Display Case";

        public override string ItemName => "Aegis Display Case";

        public override string ItemLangTokenName => "GOTCE_AegisDisplayCase";

        public override string ItemPickupDesc => "Barrier decay rate is halved.";

        public override string ItemFullDescription => "Barrier decay rate is reduced by 50% (+50% per stack).";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Healing, ItemTag.Utility, GOTCETags.BarrierRelated };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += Barrier;
        }

        public void Barrier(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args) {
            if (NetworkServer.active) {
                if (GetCount(body) > 0) {
                    float decrease = Mathf.Pow(0.5f, GetCount(body));
                    body.barrierDecayRate *= 0.5f;
                }
            }
        }
    }
}