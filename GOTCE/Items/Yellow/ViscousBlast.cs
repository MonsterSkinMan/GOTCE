using System.Linq;
using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace GOTCE.Items.Yellow
{
    public class ViscousBlast : ItemBase<ViscousBlast>
    {

        public ItemDef itemDef;
        public override string ConfigName => "Viscous Blast";

        public override string ItemFullDescription => "On death, explode in a 1000m radius for 20,000% damage (+10,000% per stack)";

        public override string ItemLore => "I really don't like this stuff. I know it's just moisturizing jellyfish goop, but something about it makes me deeply uncomfortable. The thing we scooped it from- I don't like how that exploded, either. Exploding jellyfish guts can't be good for you.";

        public override GameObject ItemModel => null;
        public override string ItemLangTokenName => "GOTCE_ViscousBlast";
        public override Sprite ItemIcon => null;

        public override string ItemName => "Viscous Blast";

        public override string ItemPickupDesc => "Emit a devastating explosion on death..";

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };

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
                    float novaDamageCoefficient = 20000f + 10000 * (count - 1);
                    if (count > 0)
                    {
                        if (NetworkServer.active)
                        {
                            EffectManager.SimpleMuzzleFlash(EntityStates.VagrantMonster.FireMegaNova.novaEffectPrefab, body.gameObject, "Body", transmit: true);
                            BlastAttack blastAttack = new BlastAttack();
                            blastAttack.attacker = body.gameObject;
                            blastAttack.baseDamage = body.damage * novaDamageCoefficient;
                            blastAttack.baseForce = 10002;
                            blastAttack.bonusForce = Vector3.zero;
                            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                            blastAttack.crit = body.RollCrit();
                            blastAttack.damageColorIndex = DamageColorIndex.Default;
                            blastAttack.damageType = DamageType.Generic;
                            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                            blastAttack.inflictor = body.gameObject;
                            blastAttack.position = body.transform.position;
                            blastAttack.procChainMask = default(ProcChainMask);
                            blastAttack.procCoefficient = 3f;
                            blastAttack.radius = 1000;
                            blastAttack.losType = BlastAttack.LoSType.NearestHit;
                            blastAttack.teamIndex = body.teamComponent.teamIndex;
                            blastAttack.impactEffect = EffectCatalog.FindEffectIndexFromPrefab(EntityStates.VagrantMonster.FireMegaNova.novaImpactEffectPrefab);
                            blastAttack.Fire();
                        }
                    }
                }
            }
        }

    }
}