using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items.White
{
    public class Balls : ItemBase<Balls>
    {
        public override string ConfigName => "Balls";

        public override string ItemName => "Balls";

        public override string ItemLangTokenName => "GOTCE_Balls";

        public override string ItemPickupDesc => "They will stay.";

        public override string ItemFullDescription => "Increase <style=cIsUtility>'Critical FOV Strike'</style> chance by <style=cIsUtility>2%</style>. Gain <style=cIsHealing>8%</style> <style=cStack>(+4% per stack)</style> of your <style=cIsHealing>maximum health</style> as regenerating <style=cIsUtility>shields</style> for every '<style=cIsUtility>FOV Crit</style>' item you have.";

        public override string ItemLore => "Balls will stay - everything below THIS message will be deleted (this time its personal!!)";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.Healing, GOTCETags.FovRelated, ItemTag.CannotSteal };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/Balls.png");

        public override bool CanRemove => false;

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
            RecalculateStatsAPI.GetStatCoefficients += Synergy;
            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory)
                    {
                        args.Stats.FovCritChanceAdd += GetCount(args.Stats.body) > 0 ? 2f : 0f;
                    }
                }
            };
        }

        public void Synergy(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            int total = 0;
            if (!body.inventory)
            {
                return;
            }
            foreach (ItemIndex index in body.inventory.itemAcquisitionOrder)
            {
                if (ContainsTag(ItemCatalog.GetItemDef(index), GOTCETags.FovRelated))
                {
                    total += body.inventory.GetItemCount(index);
                }
            }

            if (GetCount(body) > 0)
            {
                float stackMult = 0.08f + (0.04f * (GetCount(body) - 1));
                float shields = (body.healthComponent.fullHealth * stackMult) * total;

                args.baseShieldAdd += shields;
            }
        }
    }
}