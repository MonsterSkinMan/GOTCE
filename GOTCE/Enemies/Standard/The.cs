using R2API;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using System.Linq;

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

            On.RoR2.CharacterBody.Start += (orig, self) => {
                orig(self);
                if (self.baseNameToken == "GOTCE_THE_NAME") {
                    self.inventory.GiveItem(RoR2Content.Items.ExtraLife, 2);
                }
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

            prefab.GetComponent<CharacterDeathBehavior>().deathState = new(typeof(GOTCE.EntityStatesCustom.The.TheDeath));
            ReplaceSkill(sl.primary, Skills.Kick.Instance.SkillDef);

            master.bodyPrefab = prefab;

            LanguageAPI.Add("GOTCE_THE_NAME", "The");
            LanguageAPI.Add("GOTCE_THE_LORE", "You cannot run from The. You cannot hide from The. You cannot escape The. The is eternal...");
            LanguageAPI.Add("GOTCE_THE_SUBTITLE", "Horde of Many");
        }

        public override void PostCreation()
        {
            base.PostCreation();
            RegisterEnemy(prefab, prefabMaster, null, DirectorAPI.MonsterCategory.BasicMonsters, true);
        }
    }
}