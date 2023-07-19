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

        public override string ItemLore => "<style=cWorldEvent>[WARNING] Unstable Tesla Coil has been integrated into the cell...!</style>\n<style=cMono>1. 2. 3.</style>\nThe Mercenary's blade slashed against the surface of the boulder, as he flew up the surface of the hill. Three golem lasers intersected around him, one barely missing his thigh and eviscerating the red elixir bottle he had saved.\n<style=cMono>4. 5. 6.</style>\nThe tongue/goat leg abominations were approaching from all angles. He could hear the ticking of a dozen watches as he ran across a stone arch to another hill.<style=cMono>7. 8. 9.</style>\nWith the fog filling up his lungs and the monsters catching up to him, he ran to the edge of the hill, and saw the Beetles, Golems, and Bison in the canyon, all with unnatural augmentations. He jumped off the hill and dashed at the next, coming a foot short of the ledge, and falling into the crowd below.\n<style=cMono>10.</style>\nHis late friend Red'Dit had warned him of how terrible this pain would be, but the Mercenary didn't grasp the magnitude of it then. Perhaps they would meet in the afterlife, so they could complain about it together.";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.Bullshit, GOTCETags.Unstable };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/ExtremelyUnstableTeslaCoil.png");

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