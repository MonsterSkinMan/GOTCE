using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Skills;
using UnityEngine.Networking;
using RoR2.CharacterAI;

namespace GOTCE.Enemies.Bosses
{
    public class Provi : EnemyBase<Provi>
    {
        public override string PathToClone => "Assets/Prefabs/Enemies/Provi/ProviBody.prefab";
        public override bool local => true;
        public override string CloneName => "Providence";
        public override string PathToCloneMaster => "RoR2/Base/Merc/MercMonsterMaster.prefab";
        public CharacterBody body;
        public CharacterMaster master;

        public override void CreatePrefab()
        {
            base.CreatePrefab();
            body = prefab.GetComponent<CharacterBody>();
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 800;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.TeleporterOK;
            isc.hullSize = HullClassification.Human;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            isc.sendOverNetwork = true;
            isc.prefab = prefabMaster;
            isc.name = "cscProvi";
        }

        public override void AddDirectorCard()
        {
            base.AddDirectorCard();
            card.minimumStageCompletions = 2;
            card.selectionWeight = 10;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
        }

        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = prefab;

            prefab.GetComponent<CharacterDeathBehavior>().deathState = new EntityStates.SerializableEntityStateType(typeof(EntityStatesCustom.Providence.ProviDeath));

            /*foreach (AISkillDriver driver in prefabMaster.GetComponents<AISkillDriver>()) {
                if (driver.skillSlot == SkillSlot.Primary) {
                    driver.minDistance = 0f;
                    driver.maxDistance = 5f;
                }

                driver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            }*/

            SkillLocator sl = prefab.GetComponent<SkillLocator>();
            ReplaceSkill(sl.primary, Skills.Slash.Instance.SkillDef);
            ReplaceSkill(sl.secondary, Skills.Downslash.Instance.SkillDef);
            ReplaceSkill(sl.utility, Skills.Shockwave.Instance.SkillDef);
            ReplaceSkill(sl.special, Skills.TrackingOrb.Instance.SkillDef);

            LanguageAPI.Add("GOTCE_PROVI_NAME", "Providence");
            LanguageAPI.Add("GOTCE_PROVI_LORE", "The young lemurian known as Zarold watched in awe. He couldn't believe he was attending a Lil' Murian concert live. The tickets were expensive as shit and were notorious for selling out almost immediately, meaning many had to burn even more money on tickets as scalpers resold them at exorbitant prices. Luckily, Zarold's friend, Drederick, had managed to snag a pair for them a few months back, and today was finally the day. As they watched the rapper/DJ bust some crazy-ass moves on stage, a loud shattering sound suddenly echoed throughout the room. Immediately, everything stopped as a strange, almost glitchy rift appeared in the sky. As panic began to ensue, none other than Providence tumbled out as the rift disappeared as suddenly as it appeared. Providence, the hero of the lemurians. Providence, who had saved them from their apparent doom on their home planet. Providence, who the lemurians built a great temple for in honor. Providence, who had been killed less than a year prior at the hands of a strange invader. Providence had finally returned, somehow.");
            LanguageAPI.Add("GOTCE_PROVI_SUBTITLE", "Bulwark of the Weak");

            DeathRewards deathRewards = prefab.GetComponent<DeathRewards>();
            ExplicitPickupDropTable dt = ScriptableObject.CreateInstance<ExplicitPickupDropTable>();
            if (Items.Yellow.RightRingFingerOfProvidence.Instance?.ItemDef) {
                dt.pickupEntries = new ExplicitPickupDropTable.PickupDefEntry[]
                {
                    new ExplicitPickupDropTable.PickupDefEntry {pickupDef = Items.Yellow.RightRingFingerOfProvidence.Instance.ItemDef, pickupWeight = 1f},
                };
                deathRewards.bossDropTable = dt;
            }
        }

        public override void PostCreation()
        {
            base.PostCreation();
            List<DirectorAPI.Stage> stages = new() {
                DirectorAPI.Stage.ScorchedAcres,
                DirectorAPI.Stage.SulfurPools,
                DirectorAPI.Stage.RallypointDelta,
                DirectorAPI.Stage.RallypointDeltaSimulacrum
            };
            RegisterEnemy(prefab, prefabMaster, stages, DirectorAPI.MonsterCategory.Champions, false);
        }
    }
}