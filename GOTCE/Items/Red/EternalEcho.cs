using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using HarmonyLib;

namespace GOTCE.Items.Red
{
    public class EternalEcho : ItemBase<EternalEcho>
    {
        public override string ItemName => "Eternal Echo";

        public override string ConfigName => ItemName;

        public override string ItemLangTokenName => "GOTCE_EternalEcho";

        public override string ItemPickupDesc => "Attacks hit an additional time for slightly less damage.";

        public override string ItemFullDescription => "Your attacks hit 1 (+1 per stack) additional time(s) for 75% total damage.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.HealthComponent.TakeDamage += (orig, self, info) => {
                orig(self, info);
                if (NetworkServer.active && info.attacker && info.attacker.GetComponent<CharacterBody>()) {
                    CharacterBody cb = info.attacker.GetComponent<CharacterBody>();
                    int c = GetCount(cb);
                    if (c > 0) {
                        info.damage *= 0.75f;
                        for (int i = 0; i < c; i++) {
                            orig(self, info);
                        }
                    }
                }
            };
        }
    }
}