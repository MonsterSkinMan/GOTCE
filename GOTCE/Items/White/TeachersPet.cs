using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.White
{
    public class TeachersPet : ItemBase<TeachersPet>
    {
        public override string ConfigName => "Teachers Pet";

        public override string ItemName => "Teacher's Pet";

        public override string ItemLangTokenName => "GOTCE_TeachersPet";

        public override string ItemPickupDesc => "Deal extra damage if your survivor has the letter A in their name.";

        public override string ItemFullDescription => "You gain a 20% (+20% per stack) damage boost if the character you are playing as has the letter \"A\" in their name.";

        public override string ItemLore => "TBA";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] {ItemTag.Damage };

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
            RecalculateStatsAPI.GetStatCoefficients += A;
        }

        public void A(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args) {
            int c = GetCount(body);
            if (c > 0) {
                if (Language.GetString(body.baseNameToken).ToLower().Contains("a")) {
                    args.damageMultAdd += 0.2f * c;
                }
            }
        }
    }
}