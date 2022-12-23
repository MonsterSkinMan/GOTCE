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
            LanguageAPI.Add("GOTCE_PROVI_LORE", "lol, lmao");
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
                DirectorAPI.Stage.RallypointDelta
            };
            RegisterEnemy(prefab, prefabMaster, stages, DirectorAPI.MonsterCategory.Champions, false);
        }
    }
}