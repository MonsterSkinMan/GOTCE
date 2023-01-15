using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using RoR2.Projectile;
using System;
using RoR2.Orbs;

namespace GOTCE.Items.Green
{
    public class ExtremelyUnstable : ItemBase<ExtremelyUnstable>
    {
        public override string ConfigName => "Extremely Unstable Tesla Coil";

        public override string ItemName => "Extremely Unstable Tesla Coil";

        public override string ItemLangTokenName => "GOTCE_ExtremelyUnstablel";

        public override string ItemPickupDesc => "Rapidly shocks all enemies. Super fucking laggy.";

        public override string ItemFullDescription => "Passively shocks all enemies on the stage with lightning that deals 1% damage 50 (+50 per stack) times per second with a 0.01 proc coefficient. The lightning passively has 50 luck. Stack count is doubled every 5 seconds.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.Bullshit, GOTCETags.Unstable };

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
            RecalculateStatsAPI.GetStatCoefficients += Coil;
        }

        public void Coil(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            body.AddItemBehavior<TeslaBehavior>(GetCount(body));
        }

        public class TeslaBehavior : CharacterBody.ItemBehavior {
            float delay => 1 / (50 * stack);
            float stopwatch = 0f;
            float doubleDelay = 5f;
            float doubleTimer = 0f;

            private void FixedUpdate() {
                stopwatch += Time.fixedDeltaTime;
                doubleTimer += Time.fixedDeltaTime;
                if (stopwatch >= delay) {
                    stopwatch = 0f;

                    List<TeamComponent> coms = GameObject.FindObjectsOfType<TeamComponent>().Where(x => x.teamIndex != body.teamComponent.teamIndex).ToList();
                    foreach (TeamComponent com in coms) {
                        if (com.body && com.body.mainHurtBox) {
                            LightningOrb orb = new();
                            orb.damageValue = body.damage * 0.01f;
                            orb.attacker = base.gameObject;
                            orb.origin = body.corePosition;
                            orb.bouncesRemaining = 0;
                            orb.canBounceOnSameTarget = false;
                            orb.procChainMask = new();
                            orb.procCoefficient = 0.01f;
                            orb.speed = 15f;
                            orb.lightningType = LightningOrb.LightningType.Tesla;
                            orb.range = float.PositiveInfinity;
                            orb.target = com.body.mainHurtBox;
                            orb.teamIndex = body.teamComponent.teamIndex;

                            OrbManager.instance.AddOrb(orb);
                        }
                    }
                }

                if (doubleTimer >= doubleDelay) {
                    doubleTimer = 0f;
                    if (stack * 2 < int.MaxValue) {
                        body.inventory.GiveItem(ExtremelyUnstable.Instance.ItemDef, stack);
                    }
                }
            }
        }
    }
}