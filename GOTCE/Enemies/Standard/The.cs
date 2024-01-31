using R2API;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using System.Linq;
using EntityStates;

namespace GOTCE.Enemies.Standard
{
    public class The : EnemyBase<The>
    {
        public override string PathToClone => "Assets/Prefabs/Enemies/The/TheBody.prefab";
        public override string CloneName => "The";
        public override string PathToCloneMaster => "RoR2/DLC1/Vermin/VerminMaster.prefab";

        public override bool local => true;
        public CharacterBody body;
        public CharacterMaster master;

        public override void CreatePrefab()
        {
            base.CreatePrefab();

            On.RoR2.CharacterBody.Start += (orig, self) =>
            {
                if (self.baseNameToken == "GOTCE_THE_NAME")
                {
                    self.inventory.GiveItem(RoR2Content.Items.ExtraLife, 2);
                    Util.PlaySound("The", self.gameObject);
                }
                orig(self);
            };

            On.RoR2.HealthComponent.TakeDamage += (orig, self, info) => {
                if (NetworkServer.active && self.body.baseNameToken == "GOTCE_THE_NAME") {
                    self.Heal(self.fullCombinedHealth, new());

                    Vector3 guh = (info.attacker.transform.position - self.transform.position).normalized * -1300f;
                    info.force = guh;
                    self.TakeDamageForce(guh, true, true);
                        
                    EntityStatesCustom.The.TheHurtState state = new();
                    self.gameObject.GetComponent<SetStateOnHurt>().targetStateMachine.SetInterruptState(state, InterruptPriority.Death);
                    Util.PlaySound("The", self.gameObject);

                }
                orig(self, info);
            };
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 90;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.None;
            isc.hullSize = HullClassification.Human;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            isc.sendOverNetwork = true;
            isc.prefab = prefabMaster;
            isc.name = "cscThe";
            isc.forbiddenAsBoss = true;
        }

        public override void AddDirectorCard()
        {
            base.AddDirectorCard();
            card.minimumStageCompletions = 0;
            card.selectionWeight = 1;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
        }

        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();

            SkillLocator sl = prefab.GetComponentInChildren<SkillLocator>();

            prefab.GetComponent<CharacterDeathBehavior>().deathState = new(typeof(EntityStates.GenericCharacterDeath));
            prefab.GetComponent<SetStateOnHurt>().hurtState = new(typeof(EntityStatesCustom.The.TheHurtState));
            ReplaceSkill(sl.primary, Skills.Kick.Instance.SkillDef);

            foreach (AISkillDriver driver in prefabMaster.GetComponents<AISkillDriver>()) {
                if (driver.skillSlot == SkillSlot.Primary) {
                    driver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
                    driver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
                    driver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
                }
            }

            master.bodyPrefab = prefab;

            LanguageAPI.Add("GOTCE_THE_NAME", "The");
            LanguageAPI.Add("GOTCE_THE_LORE", "I have made a very fascinating, yet equally terrifying, discovery in regard to the Crack. It would seem that there are... living creatures beyond it. Er, well, I don't really know if they could be considered living, for various reasons, but... let's continue.\nI have dubbed this creature as simply The. Just The. This is because every time they do anything, they emit a sound that sounds like the word The. I can't even say that they say the word The, because their mouths don't move when they do it.\nThes seem to resemble the smirking cat emoji, except they have fairly long legs and... \"generous\" backsides, to say the least. I don't have any clue as to why, as they never sit down or create waste of any sort, nor do they seem to mate or reproduce. They are completely androgynous from what I've observed of them.\nThes do not seem to be capable of complex thought. They can react to visual and sensory stimuli just fine, but they seem almost like mindless computer viruses or something. They run around and... attempt to kick creatures in the testicles, regardless of if they have testicles or not. While their kicks don't hurt very much, Thes are capable of kicking at blinding speeds, which can make them somewhat menacing in certain circumstances.\nHowever, the most notable trait of Thes is that, from all of what we've observed, they are COMPLETELY impervious to all harm. Damage just seems to disable them for a few seconds, and all that happens is that their facial features disappear before regrowing. Even when a The was bombarded with antimatter artillery, it came out of it completely unharmed. You cannot run from The. You cannot hide from The. You cannot escape The. The is eternal.\nSo, why do I consider this creature to be terrifying? Well... when enough of them are gathered in one place, it almost seems like reality itself is struggling to keep up. It seemed like time itself slowed down, forcing us to reopen the Crack and send the Thes back through. I am sending this specimen to you in a specialized container in order for you to study it some more. Please write back to me as soon as possible.");
            LanguageAPI.Add("GOTCE_THE_SUBTITLE", "Horde of Many");
        }

        public override void PostCreation()
        {
            base.PostCreation();
            List<DirectorAPI.Stage> stages = new() {
                DirectorAPI.Stage.SulfurPools,
                DirectorAPI.Stage.AbandonedAqueduct,
                DirectorAPI.Stage.GildedCoast,
                DirectorAPI.Stage.RallypointDelta,
                DirectorAPI.Stage.ScorchedAcres,
                DirectorAPI.Stage.TitanicPlains,
                DirectorAPI.Stage.AphelianSanctuary,
                DirectorAPI.Stage.DistantRoost
            };
            RegisterEnemy(prefab, prefabMaster, stages, DirectorAPI.MonsterCategory.BasicMonsters, false);
        }
    }
}