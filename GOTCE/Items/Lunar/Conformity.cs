using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GOTCE.Items.Lunar
{
    public class Conformity : ItemBase<Conformity>
    {
        public override string ConfigName => "Conformity";

        public override string ItemName => "Conformity";

        public override string ItemLangTokenName => "GOTCE_Conformity";

        public override string ItemPickupDesc => "<style=cIsUtility>Gain</style> increased <style=cIsDamage>attack speed</style>... <style=cDeath>but</style> you <style=cDeath>transform into the struck enemy</style> on hit.";

        public override string ItemFullDescription => "<style=cIsUtility>Gain</style> <style=cIsDamage>+100%</style> <style=cStack>(+100% per stack)</style> <style=cIsDamage>attack speed</style>... <style=cDeath>but</style> you <style=cIsVoid>transform</style> into the <style=cDeath>struck enemy</style> on hit. <style=cDeath>-0%</style> <style=cStack>(-50% per stack)</style> <style=cIsHealth>regeneration</style>";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.Utility, ItemTag.AIBlacklist };

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
            On.RoR2.GlobalEventManager.OnHitEnemy += (orig, self, info, victim) => {
                orig(self, info, victim);
                if (NetworkServer.active) {
                    CharacterBody attacker = info.attacker.GetComponent<CharacterBody>();
                    CharacterBody vBody = victim.GetComponent<CharacterBody>();
                    if (attacker && vBody && vBody.master) {
                        if (GetCount(attacker) > 0) {
                            if (attacker.masterObject.GetComponent<Components.GOTCE_StatsComponent>()) {
                                attacker.master.bodyPrefab = vBody.master.bodyPrefab;
                                attacker.masterObject.GetComponent<Components.GOTCE_StatsComponent>().RespawnExtraLife();
                            }
                        }
                    }
                }
            };

            RecalculateStatsAPI.GetStatCoefficients += (body, args) => {
                if (NetworkServer.active && GetCount(body) > 0) {
                    float increase = 1*GetCount(body);
                    float decrease = 0.5f*GetCount(body);

                    args.attackSpeedMultAdd += increase;
                    args.regenMultAdd -= decrease;
                }
            };
        }

    }
}