using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;

namespace GOTCE.Enemies.Bosses
{
    public class Voidlingling : EnemyBase<Voidlingling>
    {
        public override string PathToClone => "RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyBase.prefab";
        public override string CloneName => "Voidlingling";
        public override string PathToCloneMaster => "RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabMasterBase.prefab";
        public CharacterBody body;
        public CharacterMaster master;

        public override void CreatePrefab()
        {
            base.CreatePrefab();
            body = prefab.GetComponent<CharacterBody>();
            body.baseArmor = 0;
            body.levelArmor = 0;
            body.attackSpeed = 1f;
            body.levelAttackSpeed = 0f;
            body.damage = 21f;
            body.levelDamage = 4.2f;
            body.baseMaxHealth = 2400f;
            body.levelMaxHealth = 720f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_VOIDLINGLING_NAME";
            body.baseRegen = 0f;
            body.levelRegen = 0f;
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 600;
            isc.eliteRules = SpawnCard.EliteRules.ArtifactOnly;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.TeleporterOK;
            isc.hullSize = HullClassification.BeetleQueen;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Air;
            isc.sendOverNetwork = true;
            isc.prefab = prefabMaster;
            isc.name = "cscVoidlingling";
        }

        public override void AddDirectorCard()
        {
            base.AddDirectorCard();
            card.minimumStageCompletions = 1;
            card.selectionWeight = 1;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
        }

        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = prefab;

            prefab.transform.Find("Model Base").gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            DeathRewards deathRewards = prefab.GetComponent<DeathRewards>();
            if (deathRewards)
            {
            }
            else
            {
                deathRewards = prefab.AddComponent<DeathRewards>();
                deathRewards.characterBody = body;
            }
            ExplicitPickupDropTable dt = ScriptableObject.CreateInstance<ExplicitPickupDropTable>();
            dt.pickupEntries = new ExplicitPickupDropTable.PickupDefEntry[]
            {
                new ExplicitPickupDropTable.PickupDefEntry {pickupDef = Equipment.VoidDonut.Instance.EquipmentDef, pickupWeight = 1f},
            };
            deathRewards.bossDropTable = dt;

            LanguageAPI.Add("GOTCE_VOIDLINGLING_NAME", "Voidlingling");
            LanguageAPI.Add("GOTCE_VOIDLINGLING_LORE", "\"I'm... not sure I'm allowed to help you.\"\n\"LOOK! I just need some more Jailers and Devastators in order to capture Petrichor V, alright? I have sufficient money to hire some more.\"\n\"Aren't they technically higher ranked than you now? I know you’re a Voidborne and all, but you've been demoted pretty substantially since you disappointed [THE COUNCIL] so many times.\"\n\"I don't care! Screw [THE COUNCIL]! I need stronger troops if they want Petrichor so badly.\"\n\"Look, <i>Voidling</i>, I can sell you some more Reavers and Imposters if you so desperately need more firepower.\"\n\"I DON'T WANT YOUR STUPID REAVERS AND IMPOSTERS AND BARNACLES! THEY'RE THE REASON I'VE MADE NO PROGRESS ON THIS TAKEOVER FOR THE LAST FEW MILLENNIA! AND STOP CALLING ME VOIDLING!\"\n\"Well, <i>Voidling</i>, Reavers and Imposters are pretty useful in planetary takeover. Maybe if you actually utilized the functionality of the Reavers to create spatial barriers, you'd find more success. Or maybe, just maybe, you could actually learn how to fight instead of just taking potshots at creatures from across your stupid donut-shaped arena?\"\n\"Don’t insult my ancient fighting technique, Mr. I-Run-Away-From-Literally-Every-Fight-I-Get-Into-And-Then-Delete-My-Enemies-With-My-Whip! And that spatial barrier technique isn’t useful to me anyways! It's too much work to learn how to better utilize my Reavers! I am willing to pay triple wages for some Jailers and Devastators.\"\n\"<i>Sighs</i> Fine... but if [THE COUNCIL] gets on my case for selling you troops of a higher rank than you, you’re taking all of the blame.\"\n\"Yeah, sure, I'll just pretend I kidnapped them from you or something. Now give 'em here!\"\n\"Okay, whatever.\"\n\"Oh, and I have one more request. Could you figure out if you could make mass-produced clones of me to employ in my army?\"\n\"Um... sure, I guess.\"");
            LanguageAPI.Add("GOTCE_VOIDLINGLING_SUBTITLE", "A Fun and Engaging Boss");
        }

        public override void PostCreation()
        {
            base.PostCreation();
            List<DirectorAPI.Stage> stages = new() {
                DirectorAPI.Stage.DistantRoost,
                DirectorAPI.Stage.AphelianSanctuary,
                DirectorAPI.Stage.VoidLocus,
                DirectorAPI.Stage.VoidCell,
                DirectorAPI.Stage.RallypointDelta,
                DirectorAPI.Stage.SirensCall,
                DirectorAPI.Stage.TitanicPlains,
                DirectorAPI.Stage.AbyssalDepths
                
            };
            RegisterEnemy(prefab, prefabMaster, stages, DirectorAPI.MonsterCategory.Champions);
        }
    }

    // TODO:

    // remove spawn vfx (probably in an entitystate)
    // remove footstep vfx
    // make the spawn entitystate much faster
}