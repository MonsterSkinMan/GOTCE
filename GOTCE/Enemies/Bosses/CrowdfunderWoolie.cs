using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Skills;
using UnityEngine.Networking;
using RoR2.CharacterAI;

namespace GOTCE.Enemies.Bosses
{
    public class CrowdfunderWoolie : EnemyBase<CrowdfunderWoolie>
    {
        public override string PathToClone => "Assets/Prefabs/Enemies/Woolie/Woolie.prefab";
        public override bool local => true;
        public override string CloneName => "Crowdfunder Woolie";
        public override string PathToCloneMaster => "RoR2/Base/ClayBruiser/ClayBruiserMaster.prefab";
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
            isc.directorCreditCost = 900;
            isc.eliteRules = SpawnCard.EliteRules.Default;
            isc.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            isc.requiredFlags = RoR2.Navigation.NodeFlags.TeleporterOK;
            isc.hullSize = HullClassification.Human;
            isc.occupyPosition = true;
            isc.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            isc.sendOverNetwork = true;
            isc.prefab = prefabMaster;
            isc.eliteRules = SpawnCard.EliteRules.ArtifactOnly;
            isc.name = "cscCrowdfunderWoolie";
        }

        public override void AddDirectorCard()
        {
            base.AddDirectorCard();
            card.minimumStageCompletions = 3;
            card.selectionWeight = 1;
            card.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
        }

        public override void Modify()
        {
            base.Modify();
            master = prefabMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = prefab;

            SkillLocator sl = prefab.GetComponent<SkillLocator>();
            ReplaceSkill(sl.primary, Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Captain/CaptainSkillDisconnected.asset").WaitForCompletion());

            foreach (EntityStateMachine esm in prefab.GetComponents<EntityStateMachine>())
            {
                if (esm.customName == "Weapon")
                {
                    esm.initialStateType = new EntityStates.SerializableEntityStateType(typeof(EntityStatesCustom.Woolie.Crowdfunder));
                    esm.mainStateType = new EntityStates.SerializableEntityStateType(typeof(EntityStatesCustom.Woolie.Crowdfunder));
                }
            }

            LanguageAPI.Add("GOTCE_WOOLIE_NAME", "Crowdfunder Woolie");
            LanguageAPI.Add("GOTCE_WOOLIE_LORE", "\"It costs $180 to shoot one of these for 12 seconds.\"\n\n\"It costs $1080 to fire all 6 for 12 seconds.\"\n\n\"To kill you? Probably only one second. It's a shame I won't make the money back when we're done.\"\n\n<i>STOMPING SOUNDS, FOLLOWED BY THE SOUND OF GOLD BEING FIRED BY SIX CROWDFUNDERS</i>\n\n\"NONE LEAVE THE SLAUGHTERHOUSE! NOT ALIVE!\"\n\n[TRANSMISSION ERROR]");
            LanguageAPI.Add("GOTCE_WOOLIE_SUBTITLE", "THE Mr. Streamer");

            On.RoR2.CharacterBody.Start += (orig, self) =>
            {
                if (NetworkServer.active && self.baseNameToken == "GOTCE_WOOLIE_NAME" && self.equipmentSlot)
                {
                    self.inventory.SetEquipmentIndex(RoR2Content.Equipment.AffixRed.equipmentIndex);
                }
                orig(self);
            };

            DeathRewards deathRewards = prefab.GetComponent<DeathRewards>();
            ExplicitPickupDropTable dt = ScriptableObject.CreateInstance<ExplicitPickupDropTable>();
            if (Items.Yellow.Wooler.Instance?.ItemDef) {
                dt.pickupEntries = new ExplicitPickupDropTable.PickupDefEntry[]
                {
                    new ExplicitPickupDropTable.PickupDefEntry {pickupDef = Items.Yellow.Wooler.Instance.ItemDef, pickupWeight = 1f},
                };
                deathRewards.bossDropTable = dt;
            }
        }

        public override void PostCreation()
        {
            base.PostCreation();
            List<DirectorAPI.Stage> stages = new() {
                DirectorAPI.Stage.SulfurPools,
                DirectorAPI.Stage.WetlandAspect,
                DirectorAPI.Stage.DistantRoost,
                DirectorAPI.Stage.SkyMeadow
            };
            RegisterEnemy(prefab, prefabMaster, stages, DirectorAPI.MonsterCategory.Champions, false);
        }
    }
}