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

        public override string ItemPickupDesc => "Gain increased attack speed... <color=#FF7F7F>BUT you transform into the struck enemy on hit</color>.";

        public override string ItemFullDescription => "Gain <style=cIsDamage>+100%</style> <style=cStack>(+100% per stack)</style> <style=cIsDamage>attack speed</style>. Transform into the struck enemy on hit. Decrease <style=cIsHealing>regeneration</style> by <style=cIsHealing>0%</style> <style=cStack>(-50% per stack)</style>.";

        public override string ItemLore => "Everybody may be different, but at the end of the day, we're all the same. At some point, there was a single-celled organism that gave rise to all modern life on the planet. Hell, all of humanity is descended from one person at this point if I remember correctly, so everything really comes back together in the end. Or, I guess this would be the beginning?";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.Utility, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/Conformity.png");

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
            On.RoR2.GlobalEventManager.OnHitEnemy += (orig, self, info, victim) =>
            {
                orig(self, info, victim);
                if (NetworkServer.active && info.attacker)
                {
                    CharacterBody attacker = info.attacker.GetComponent<CharacterBody>();
                    CharacterBody vBody = victim.GetComponent<CharacterBody>();
                    if (attacker && vBody && vBody.master && attacker.masterObject)
                    {
                        if (GetCount(attacker) > 0)
                        {
                            if (attacker.masterObject.GetComponent<Components.GOTCE_StatsComponent>())
                            {
                                attacker.master.bodyPrefab = vBody.master.bodyPrefab;
                                attacker.masterObject.GetComponent<Components.GOTCE_StatsComponent>().RespawnExtraLife();
                            }
                        }
                    }
                }
            };

            RecalculateStatsAPI.GetStatCoefficients += (body, args) =>
            {
                if (NetworkServer.active && GetCount(body) > 0)
                {
                    float increase = 1 * GetCount(body);
                    float decrease = 0.5f * (GetCount(body) - 1);

                    args.attackSpeedMultAdd += increase;
                    args.regenMultAdd -= decrease;
                }
            };
        }
    }
}