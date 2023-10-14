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

        public override string ItemFullDescription => "Gain <style=cIsUtility>5% FOV crit chance</style>. On '<style=cIsUtility>Critical FOV Strike</style>', fire <style=cIsDamage>2</style> <style=cStack>(+2 per stack)</style> missiles for <style=cIsDamage>300%</style> base damage.";

        public override string ItemLore => "The missile knows where it is at all times. It knows this because it knows where it isn't. By subtracting where it is from where it isn't, or where it isn't from where it is (whichever is greater), it obtains a difference, or deviation. The guidance subsystem uses deviations to generate corrective commands to drive the missile from a position where it is to a position where it isn't, and arriving at a position where it wasn't, it now is. Consequently, the position where it is, is now the position that it wasn't, and it follows that the position that it was, is now the position that it isn't.\nIn the event that the position that it is in is not the position that it wasn't, the system has acquired a variation, the variation being the difference between where the missile is, and where it wasn't. If variation is considered to be a significant factor, it too may be corrected by the GEA. However, the missile must also know where it was.\nThe missile guidance computer scenario works as follows. Because a variation has modified some of the information the missile has obtained, it is not sure just where it is. However, it is sure where it isn't, within reason, and it knows where it was. It now subtracts where it should be from where it wasn't, or vice-versa, and by differentiating this from the algebraic sum of where it shouldn't be, and where it was, it is able to obtain the deviation and its variation, which is called error.";

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
                    for (int i = 0; i < stack * 2; i++)
                    {
                        MissileUtils.FireMissile(args.Body.transform.position, args.Body, new ProcChainMask(), null, args.Body.damage * 3f, Util.CheckRoll(args.Body.crit, args.Body.master), projectilePrefab, DamageColorIndex.Item, false);
                    }
                }
            }
        }
    }
}