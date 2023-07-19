using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class Gasogreen : ItemBase<Gasogreen>
    {
        public override string ConfigName => "Gasogreen";

        public override string ItemName => "Gasogreen";

        public override string ItemLangTokenName => "GOTCE_Gasogreen";

        public override string ItemPickupDesc => "Killing an enemy damages and weakens other nearby enemies.";

        public override string ItemFullDescription => "On kill, deal 150% damage to all enemies within 24m <style=cStack>(+8m per stack)</style> and weaken them permanently.";

        public override string ItemLore => "Order: Single-Gallon Tank of Gasogreen\nTracking Number: 50********\nEstimated Delivery: 04/18/2056\nShipping Method: Standard\nShipping Address: 394, Facility X, Earth\nShipping Details:\n\nPeople are so gullible, wouldn't you agree? They'll just listen to whatever commercials and advertising tell them about products, and accept them without second thoughts. This new Gasogreen product that Carmen conceptualized has been one of our most successful yet! It preys on people who want to think of themselves as environmentally conscious, when in reality, Gasogreen creates terrible, neurotoxic pollution. It's also far less efficient than other fuel sources, even the classic gasoline, meaning people have to buy more of it to travel the same distance. And the green dye? Genius. Plus, it helps to mask the \"\"\"unethical\"\"\" method that we use to manufacture this shit. Average consumers probably wouldn't want to know that their vehicles are running on the processed and extracted minds of other people, would they?\n\nAnyways, this is a sample for the new line of Gasogreen products we've been working on. Let me know how it performs in the testing chambers and send me the reports by the end of the week.";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.OnKillEffect, ItemTag.AIBlacklist, GOTCETags.Galsone };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/Gasogreen.png");

        private static readonly SphereSearch gasogreenSphereSearch = new();
        private static readonly List<HurtBox> gasogreenHurtBoxBuffer = new();
        private GameObject explosionVFX;

        public override void Init(ConfigFile config)
        {
            base.Init(config);
            explosionVFX = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Treebot/TreebotShockwaveEffect.prefab").WaitForCompletion();
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        private void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
        {
            if (damageReport.attackerBody && damageReport.attackerBody.inventory)
            {
                var stack = damageReport.attackerBody.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    var attackerTeamIndex = damageReport.attackerBody.teamComponent.teamIndex;

                    var radius = 24f + 8f * (stack - 1);
                    var bodyRadius = damageReport.victimBody.radius;
                    var finalRadius = radius + bodyRadius;

                    var explosionDamage = 1.5f;
                    float finalDamage = damageReport.attackerBody.damage * explosionDamage;

                    Vector3 corePosition = damageReport.victimBody.corePosition;
                    gasogreenSphereSearch.origin = corePosition;
                    gasogreenSphereSearch.mask = LayerIndex.entityPrecise.mask;
                    gasogreenSphereSearch.radius = finalRadius;
                    gasogreenSphereSearch.RefreshCandidates();
                    gasogreenSphereSearch.FilterCandidatesByHurtBoxTeam(TeamMask.GetUnprotectedTeams(attackerTeamIndex));
                    gasogreenSphereSearch.FilterCandidatesByDistinctHurtBoxEntities();
                    gasogreenSphereSearch.OrderCandidatesByDistance();
                    gasogreenSphereSearch.GetHurtBoxes(gasogreenHurtBoxBuffer);
                    gasogreenSphereSearch.ClearCandidates();

                    for (int i = 0; i < gasogreenHurtBoxBuffer.Count; i++)
                    {
                        HurtBox hurtBox = gasogreenHurtBoxBuffer[i];

                        if (hurtBox.healthComponent && hurtBox.healthComponent.body)
                        {
                            hurtBox.healthComponent.body.AddBuff(RoR2Content.Buffs.Weak);
                        }
                    }

                    gasogreenHurtBoxBuffer.Clear();

                    new BlastAttack
                    {
                        radius = finalRadius,
                        baseDamage = finalDamage,
                        procCoefficient = 0f,
                        crit = Util.CheckRoll(damageReport.attackerBody.crit, damageReport.attackerMaster),
                        damageColorIndex = DamageColorIndex.Item,
                        attackerFiltering = AttackerFiltering.Default,
                        falloffModel = BlastAttack.FalloffModel.None,
                        attacker = damageReport.attacker,
                        teamIndex = attackerTeamIndex,
                        position = corePosition
                    }.Fire();

                    EffectManager.SpawnEffect(GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab, new EffectData
                    {
                        origin = corePosition,
                        scale = finalRadius,
                        rotation = Util.QuaternionSafeLookRotation(damageReport.damageInfo.force)
                    }, true);
                }
            }

            orig(self, damageReport);
        }
    }
}
