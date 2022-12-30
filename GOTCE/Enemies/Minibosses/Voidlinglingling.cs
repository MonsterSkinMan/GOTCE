using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace GOTCE.Enemies.Minibosses
{
    public class Voidlinglingling : EnemyBase<Voidlinglingling>
    {
        public override string PathToClone => "RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyBase.prefab";
        public override string CloneName => "Voidlinglingling";
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
            body.damage = 17f;
            body.baseMaxHealth = 1200f;
            body.baseMoveSpeed = 65f;
            body.autoCalculateLevelStats = true;
            body.baseNameToken = "GOTCE_VOIDLINGLINGLING_NAME";
            body.subtitleNameToken = "GOTCE_VOIDLINGLINGLING_SUBTITLE";
            body.baseRegen = 0f;
            body.levelRegen = 0f;
            body.portraitIcon = Main.MainAssets.LoadAsset<Texture2D>("Assets/Textures/Icons/Enemies/Voidlingling.png");
            body.preferredInitialStateType = new EntityStates.SerializableEntityStateType(typeof(EntityStates.Uninitialized));
        }

        public override void AddSpawnCard()
        {
            base.AddSpawnCard();
            isc.directorCreditCost = 430;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.TeleporterOK;
            isc.hullSize = HullClassification.BeetleQueen;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Air;
            isc.sendOverNetwork = true;
            isc.prefab = prefabMaster;
            isc.name = "cscVoidlinglingling";
        }

        public override void AddDirectorCard()
        {
            base.AddDirectorCard();
            card.minimumStageCompletions = 5;
            card.selectionWeight = 1;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
        }

        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = prefab;

            prefab.GetComponent<CharacterDeathBehavior>().deathState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.GenericCharacterDeath));

            prefab.transform.Find("Model Base").gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            DeathRewards deathRewards = prefab.GetComponent<DeathRewards>();
            if (deathRewards)
            {
            }
            else
            {
                deathRewards = prefab.AddComponent<DeathRewards>();
                deathRewards.characterBody = body;
                deathRewards.logUnlockableDef = ScriptableObject.CreateInstance<UnlockableDef>();
                deathRewards.logUnlockableDef.nameToken = "Voidlinglingling";
            }

            LanguageAPI.Add("GOTCE_VOIDLINGLINGLING_NAME", "Voidlinglingling");
            LanguageAPI.Add("GOTCE_VOIDLINGLINGLING_LORE", "\"Greed. It's what dictates all of the Void culture. Everybody in it knows that, and yet they hardly ever realize it at all. Voidling thought he was different. But in the end, he's just a dumb kid who doesn't know anything. He thinks he's so much better than he actually is. He fails to realize that he's the reason he can't capture Petrichor V. He's completely incompetent. So what in the Hyperworld were you thinking, selling him specialized units and giving him access to LITERAL FUCKING CLONING TECHNOLOGY?\"\n\"Hey, money talks. I meant no disrespect to you or [THE COUNCIL]. Voidling simply had the money to buy them, so I gave it to him. Plus, I felt kinda hypocritical after insulting his battle tactics considering who I am and decided to have a bit of pity on him. Oh it's also pretty funny. Like giving an infant access to nuclear weaponry, ya feel me?\"\n\"Nae Nae Lord, we would [decimate] you if we could financially or logistically afford to, which is a fact we hope you're aware of. However, if you choose to circumvent our rules again, we will not hesitate to find a replacement supplier. Do you understand?\"\n\"<i>Sigh...</i> Yes, [REDACTED].\"\n\"Excellent. Now leave.\"\nEND OF RECORDING");
            LanguageAPI.Add("GOTCE_VOIDLINGLINGLING_SUBTITLE", "Horde of Many");
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
                DirectorAPI.Stage.AbyssalDepths,
                DirectorAPI.Stage.AbandonedAqueductSimulacrum,
                DirectorAPI.Stage.AbyssalDepthsSimulacrum,
                DirectorAPI.Stage.AphelianSanctuarySimulacrum,
                DirectorAPI.Stage.CommencementSimulacrum,
                DirectorAPI.Stage.RallypointDeltaSimulacrum,
                DirectorAPI.Stage.SkyMeadowSimulacrum,
                DirectorAPI.Stage.TitanicPlainsSimulacrum
            };
            RegisterEnemy(prefab, prefabMaster, stages, DirectorAPI.MonsterCategory.Champions);
        }
    }
}