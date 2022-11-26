using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using BepInEx.Configuration;
using System.Linq;
using GOTCE.Components;

namespace GOTCE.Items.Red
{
    public class PeerReviewedSource : ItemBase<PeerReviewedSource>
    {
        public override string ConfigName => "Peer Reviewed Source";

        public override string ItemName => "Peer Reviewed Source";

        public override string ItemLangTokenName => "GOTCE_PeerReviewedSource";

        public override string ItemPickupDesc => "I don't fucking think so.";

        public override string ItemFullDescription => "Identify <style=cIsUtility>1</style> <style=cStack>(+1 per stack)</style> enemy every <style=cIsUtility>15</style> seconds. Identified enemies are <style=cIsUtility>permanently stunned</style> and increase your <style=cIsDamage>base damage</style> by <style=cIsDamage>1</style> + <style=cIsDamage>2%</style> on kill.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.Utility, ItemTag.OnKillEffect };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
            Identified.Awake();
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += Barrier;
            On.RoR2.CharacterBody.OnInventoryChanged += UpdateBehavior;
        }

        public void Barrier(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (NetworkServer.active)
            {
                if (body.isPlayerControlled && body.masterObject)
                {
                    if (GetCount(body) > 0)
                    {
                        float inc = (0.01f + (0.02f * (GetCount(body) - 1)));
                        GOTCE_StatsComponent stats = body.masterObject.GetComponent<GOTCE_StatsComponent>();
                        if (stats)
                        {
                            args.damageMultAdd += stats.identifiedkillCount * inc;
                        }
                    }
                }
            }
        }

        public void UpdateBehavior(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);
            self.AddItemBehavior<SourceBehavior>(self.inventory.GetItemCount(Instance.ItemDef));
        }

        public void OnKill(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport report)
        {
            if (NetworkServer.active && report.victimBody && report.attackerBody)
            {
                if (GetCount(report.attackerBody) > 0)
                {
                    GOTCE_StatsComponent stats = report.attackerBody.masterObject.GetComponent<GOTCE_StatsComponent>();
                    if (stats)
                    {
                        if (report.victimBody.HasBuff(Identified.buff))
                        {
                            stats.identifiedkillCount += 1;
                        }
                    }
                }
            }
            orig(self, report);
        }
    }

    public class Identified
    {
        public static BuffDef buff;

        public static void Awake()
        {
            buff = ScriptableObject.CreateInstance<BuffDef>();
            buff.canStack = false;
            buff.isDebuff = true;
            buff.name = "indentified";
            buff.isHidden = true;

            R2API.ContentAddition.AddBuffDef(buff);

            /* On.RoR2.CharacterBody.FixedUpdate += (orig, self) => {
                orig(self);
                if (self.HasBuff(buff) && NetworkServer.active) {
                    if (self.gameObject.GetComponentInChildren<SetStateOnHurt>()) {
                        self.gameObject.GetComponentInChildren<SetStateOnHurt>().SetShock(10f);
                    }
                }
            }; */
            RecalculateStatsAPI.GetStatCoefficients += (self, args) =>
            {
                if (self.HasBuff(buff) && NetworkServer.active)
                {
                    if (self.gameObject.GetComponentInChildren<SetStateOnHurt>())
                    {
                        self.gameObject.GetComponentInChildren<SetStateOnHurt>().SetStun(10000f);
                    }
                }
            };
        }
    }

    public class SourceBehavior : CharacterBody.ItemBehavior
    {
        private float stopwatch = 0f;
        private float delay = 15f;

        public void FixedUpdate()
        {
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= delay)
            {
                stopwatch = 0;

                List<TeamComponent> enemies = TeamComponent.GetTeamMembers(TeamIndex.Monster).ToList();
                for (int i = 0; i < stack; i++)
                {
                    TeamComponent com = enemies[UnityEngine.Random.Range(0, enemies.Count - 1)];
                    if (com && NetworkServer.active)
                    {
                        if (com.body && !com.body.HasBuff(Identified.buff))
                        {
                            com.body.AddBuff(Identified.buff);
                            Light l = com.body.gameObject.AddComponent<Light>();
                            l.color = Color.red;
                            l.intensity = 50;
                            // identified enemies glow so the player can see which are identitifed
                            float explosionRadius = 5f;
                            GameObject explosionEffectPrefab = EntityStates.Destructible.TimeCrystalDeath.explosionEffectPrefab;
                            EffectManager.SpawnEffect(explosionEffectPrefab, new EffectData
                            {
                                origin = base.transform.position,
                                scale = explosionRadius,
                                rotation = Quaternion.identity
                            }, transmit: true);
                        }
                    }
                }
            }
        }
    }
}