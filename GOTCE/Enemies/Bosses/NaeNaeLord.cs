using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using EntityStates;
using RoR2.Skills;
using RoR2.Navigation;
using RoR2.CharacterAI;
using System.Linq;

namespace GOTCE.Enemies.Bosses {
    public class NaeNaeLord : EnemyBase<NaeNaeLord> {
        public override string CloneName => "NaeNaeLord";
        public override string PathToClone => "RoR2/Base/Commando/CommandoBody.prefab";
        public override string PathToCloneMaster => "RoR2/Base/Commando/CommandoMonsterMaster.prefab";
        
        public override void Modify()
        {
            base.Modify();
            prefabMaster.GetComponent<CharacterMaster>().bodyPrefab = prefab;

            CharacterBody body = prefab.GetComponent<CharacterBody>();
            body.baseNameToken = "GOTCE_NAENAE_NAME";
            body.isChampion = true;
            body.subtitleNameToken = "GOTCE_NAENAE_SUBTITLE";
            body.baseMaxHealth = 2500;
            body.baseDamage = 15;
            body.baseArmor = 10;
            body.baseAttackSpeed = 1;
            body.baseMoveSpeed = 12;
            body.autoCalculateLevelStats = true;
            body.regen = 0;
            body.levelRegen = 0;

            SkillLocator sl = prefab.GetComponent<SkillLocator>();

            /* ClearESM(prefab, prefabMaster);
            SerializableEntityStateType generic = new(typeof(EntityStates.GenericCharacterMain)); */
            SerializableEntityStateType idle = new(typeof(EntityStates.Idle)); 

            EntityStateMachine naenae = AddESM(prefab, "NaeNae", idle); 

            List<EntityStateMachine> machines = prefab.GetComponent<NetworkStateMachine>().stateMachines.Cast<EntityStateMachine>().ToList();
            machines.Add(naenae);
            prefab.GetComponent<NetworkStateMachine>().stateMachines = machines.ToArray();

            SkillDef prim = Skills.NaeNaePrim.Instance.SkillDef;
            SkillDef sec = Skills.NaeNaeSec.Instance.SkillDef;
            SkillDef util = Skills.NaeNaeUtil.Instance.SkillDef;
            SkillDef ult = Skills.NaeNaeUlt.Instance.SkillDef;

            ReplaceSkill(sl.primary, prim);
            ReplaceSkill(sl.secondary, sec);
            ReplaceSkill(sl.utility, util);
            ReplaceSkill(sl.special, ult);

            /* BaseAI ai = prefabMaster.GetComponent<BaseAI>();
            ai.stateMachine = bodymachine; */
            
            // List<AISkillDriver> drivers = new();
        
            foreach (AISkillDriver driver in prefabMaster.GetComponents<AISkillDriver>()) {
                driver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
                driver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
                // driver.minDistance = 30f;
                // drivers.Add(driver);
            }

            AISkillDriver spec = (from x in prefabMaster.GetComponents<AISkillDriver>() where x.skillSlot == SkillSlot.Special select x).First();
            spec.minDistance = 15f;
            spec.movementType = AISkillDriver.MovementType.FleeMoveTarget;

            AISkillDriver secondary = (from x in prefabMaster.GetComponents<AISkillDriver>() where x.skillSlot == SkillSlot.Secondary select x).First();
            secondary.moveTargetType = AISkillDriver.TargetType.NearestFriendlyInSkillRange;
            secondary.movementType = AISkillDriver.MovementType.ChaseMoveTarget;

            // ai.skillDrivers = drivers.ToArray(); 

            On.RoR2.HealthComponent.TakeDamage += (orig, self, info) => {
                if (NetworkServer.active) {
                    if (info.HasModdedDamageType(Main.truekill)) {
                        if (self && self.body && self.body.master) {
                            self.body.master.TrueKill();
                        }
                    }

                    if (info.HasModdedDamageType(Main.root)) {
                        if (self.body) {
                            self.body.AddTimedBuff(RoR2Content.Buffs.Nullified, 2.5f);
                        }
                    }
                }

                orig(self, info);
            };
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 250;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.None;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.hullSize = HullClassification.BeetleQueen;
            isc.occupyPosition = true;
            isc.prefab = prefabMaster;
            isc.name = "cscNaeNae";
        }

        public override void PostCreation()
        {
            base.PostCreation();
            RegisterEnemy(prefab, prefabMaster, null, DirectorAPI.MonsterCategory.Champions, true);
        }
    }
}