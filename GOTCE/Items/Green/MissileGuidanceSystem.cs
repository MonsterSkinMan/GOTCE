using RoR2;
using R2API;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class MissileGuidanceSystem : ItemBase<MissileGuidanceSystem>
    {
        public override string ConfigName => "Missile Guidance System";

        public override string ItemName => "Missile Guidance System";

        public override string ItemLangTokenName => "GOTCE_MissileFovCrit";

        public override string ItemPickupDesc => "On 'Critical FOV Strike' fire a homing missile.";

        public override string ItemFullDescription => "Gain <style=cIsUtility>5% FOV crit chance</style>. On '<style=cIsUtility>Critical FOV Strike</style>', fire <style=cIsDamage>1</style> <style=cStack>(+1 per stack)</style> missiles for <style=cIsDamage>300%</style> damage.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.Utility, GOTCETags.Crit, GOTCETags.FovRelated };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/MissileGuidanceSystem.png");

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
            CriticalTypes.OnFovCrit += Missile;
            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory)
                    {
                        args.Stats.FovCritChanceAdd += GetCount(args.Stats.body) > 0 ? 5 : 0;
                    }
                }
            };
        }

        public void Missile(object sender, FovCritEventArgs args)
        {
            // Debug.Log("fov crit happened");
            if (NetworkServer.active)
            {
                // Debug.Log("network active");
                if (args.Body)
                {
                    // Debug.Log("body check passed");
                    GameObject projectilePrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MissileProjectile");
                    int stack = args.Body.inventory.GetItemCount(ItemDef);
                    // Debug.Log(stack);
                    for (int i = 0; i < stack; i++)
                    {
                        MissileUtils.FireMissile(args.Body.transform.position, args.Body, new ProcChainMask(), null, args.Body.damage * 3f, Util.CheckRoll(args.Body.crit, args.Body.master), projectilePrefab, DamageColorIndex.Item, false);
                    }
                }
            }
        }
    }
}