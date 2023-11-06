using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class ScathingEmbrace : ItemBase<ScathingEmbrace>
    {
        public override string ConfigName => "Scathing Embrace";

        public override string ItemName => "Scathing Embrace";

        public override string ItemLangTokenName => "GOTCE_ScathingEmbrace";

        public override string ItemPickupDesc => "On Camera Rotation Crit, gain bonus max armor and movement speed.";

        public override string ItemFullDescription => "Gain <style=cIsUtility>5% camera rotation crit chance</style>. On '<style=cIsUtility>Camera Rotation Crit</style>', increase <style=cIsHealing>armor</style> by <style=cIsHealing>0.5</style> <style=cStack>(+0.5 per stack)</style> and <style=cIsUtility>movement speed</style> by <style=cIsUtility>1%</style> <style=cStack>(+1% per stack)</style>";

        public override string ItemLore => "This all since thou\r\nLeft your light\r\nTo behold how\r\nI bleed";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, GOTCETags.Bullshit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/Retrobauble.png");

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
            CriticalTypes.OnRotationCrit += AndRotate;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory && GetCount(args.Stats.body) > 0)
                    {
                        args.Stats.RotationCritChanceAdd += 5f;
                    }
                }
            };
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (NetworkServer.active && body.masterObject)
            {
                var stats = body.masterObject.GetComponent<GOTCE_StatsComponent>();
                var stack = GetCount(body);
                if (stats && stack > 0)
                {
                    var armorAdd = 0.5f * stack * stats.total_camera_rotation_crits;
                    var speedAdd = 0.01f * stack * stats.total_camera_rotation_crits;
                    args.armorAdd += armorAdd;
                    args.moveSpeedMultAdd += speedAdd;
                }
            }
        }

        public void AndRotate(object sender, RotationCritEventArgs args)
        {
            if (args.Body && NetworkServer.active)
            {
                if (args.Body.masterObject && args.Body.masterObject.GetComponent<GOTCE_StatsComponent>())
                {
                    args.Body.masterObject.GetComponent<GOTCE_StatsComponent>().total_camera_rotation_crits++;
                }
            }
        }
    }
}