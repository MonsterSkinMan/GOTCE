using RoR2;
using R2API;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class MissileFovCrit : ItemBase<MissileFovCrit>
    {
        public override string ConfigName => "Missile Guidance System";

        public override string ItemName => "Missile Guidance System";

        public override string ItemLangTokenName => "GOTCE_MissileFovCrit";

        public override string ItemPickupDesc => "On 'Critcal FOV Strike' fire a homing missile.";

        public override string ItemFullDescription => "On 'Critical FOV Strike', fire 1 (+1 per stack) missiles for 300% damage. Gain 5% 'FOV Crit' chance";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier2;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage, ItemTag.Utility };

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
            Items.White.ZoomLenses.Instance.OnFovCrit += Missile;
        }

        public void Missile(object sender, Items.White.FovCritEventArgs args) {
            // Debug.Log("fov crit happened");
            if (NetworkServer.active) {
                // Debug.Log("network active");
                if (args.Body) {
                    // Debug.Log("body check passed");
                    GameObject projectilePrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MissileProjectile");
                    int stack = args.Body.inventory.GetItemCount(ItemDef);
                    // Debug.Log(stack);
                    for (int i = 0; i < stack; i++) {
                        MissileUtils.FireMissile(args.Body.transform.position, args.Body, new ProcChainMask(), null, args.Body.damage * 3f, Util.CheckRoll(args.Body.crit, args.Body.master), projectilePrefab, DamageColorIndex.Item, false);
                    }
                }
            }
        }
    }
}