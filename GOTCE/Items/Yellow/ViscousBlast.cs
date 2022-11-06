using System.Linq;
using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items.Yellow
{
    public class ViscousBlast : ItemBase<ViscousBlast>
    {
        public ItemDef itemDef;
        public override string ConfigName => "Viscous Blast";

        public override string ItemFullDescription => "On death, explode in a <style=cIsUtility>1000m radius</style> for <style=cIsDamage>20000% damage</style> <style=cStack>(+10000% per stack)</style>.";

        public override string ItemLore => "I really don't like this stuff. I know it's just moisturizing jellyfish goop, but something about it makes me deeply uncomfortable. The thing we scooped it from- I don't like how that exploded, either. Exploding jellyfish guts can't be good for you.";

        public override GameObject ItemModel => null;
        public override string ItemLangTokenName => "GOTCE_ViscousBlast";
        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/ViscousBlast.png");
        public override string ItemName => "Viscous Blast";
        public override string ItemPickupDesc => "Emit a devastating explosion on death.";

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.AIBlacklist, GOTCETags.OnDeathEffect };

        public override ItemTier Tier => ItemTier.Boss;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.GlobalEventManager.OnCharacterDeath += boom;
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public void boom(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport report)
        {
            if (report.victim && report.victimBody)
            {
                CharacterBody body = report.victimBody;
                if (body.inventory)
                {
                    int count = body.inventory.GetItemCount(ItemDef);
                    float novaDamageCoefficient = (20000f + 10000 * (count - 1)) / 100f;
                    if (count > 0)
                    {
                        if (NetworkServer.active)
                        {
                            EffectManager.SimpleMuzzleFlash(EntityStates.VagrantMonster.FireMegaNova.novaEffectPrefab, body.gameObject, "Body", transmit: true);
                            BlastAttack blastAttack = new()
                            {
                                attacker = body.gameObject,
                                baseDamage = body.damage * novaDamageCoefficient,
                                baseForce = 10002,
                                bonusForce = Vector3.zero,
                                attackerFiltering = AttackerFiltering.NeverHitSelf,
                                crit = body.RollCrit(),
                                damageColorIndex = DamageColorIndex.Default,
                                damageType = DamageType.Generic,
                                falloffModel = BlastAttack.FalloffModel.None,
                                inflictor = body.gameObject,
                                position = body.transform.position,
                                procChainMask = default(ProcChainMask),
                                procCoefficient = 3f,
                                radius = 1000,
                                losType = BlastAttack.LoSType.NearestHit,
                                teamIndex = body.teamComponent.teamIndex,
                                impactEffect = EffectCatalog.FindEffectIndexFromPrefab(EntityStates.VagrantMonster.FireMegaNova.novaImpactEffectPrefab)
                            };
                            blastAttack.Fire();
                        }
                    }
                }
                orig(self, report);
            }
        }
    }
}