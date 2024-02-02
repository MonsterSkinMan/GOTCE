using System;
using UnityEngine;
using RoR2;
using RoR2.Skills;
using Unity;
using RoR2.Orbs;
using EntityStates.BrotherMonster;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Utilities;
using HarmonyLib;
using GOTCE.Gamemodes.Crackclipse;
using static R2API.SoundAPI.Music;
using IL.RoR2.ContentManagement;

namespace GOTCE.Enemies.Superbosses {
    public class Glassthrix : EnemyBase<Glassthrix> {
        public override string CloneName => "Glassthrix";
        public override string PathToClone => Utils.Paths.GameObject.BrotherBody;
        public override string PathToCloneMaster => Utils.Paths.GameObject.BrotherMaster;
        public override bool local => false;
        public override bool localMaster => false;
        private CharacterBody body;
        private static int P1WavesCount = 10;
        private static float WavesInterval = 0.5f;
        private static int P3WavesCount = 50;
        public static bool shouldProceedGlassthrixConversion = true;
        public static CharacterSpawnCard glassthrixCard;
        public static CharacterSpawnCard moonDetonation;
        private static int glassthrixWaveCount = 20;
        private static int glassthrixTotalWaves = 20;
        private static int vanillaTotalWaves;
        private static int vanillaWaveCount;
        private static Vector3 atlasSpawnPos;
        //
        public const uint THEME_GROUP = 978650076U;
        public const uint THEME_INTRO = 111873711U;
        public const uint THEME_P2_LOOP = 308672640U;
        public const uint THEME_P4_OUTRO = 335467517U;
        public const uint THEME_34_TRANSITION = 672545105U;
        public const uint THEME_P1_LOOP = 1011564647U;
        public const uint THEME_P3_LOOP = 1721060345U;
        public const uint THEME_12_TRANSITION = 2367825945U;
        public const uint THEME_P4_LOOP = 2507775610U;
        public const uint THEME_23_TRANSITION = 2839510093U;
        //
        public static CustomMusicTrackDef muGlassthrix1;
        public static CustomMusicTrackDef muGlassthrix2;
        public static CustomMusicTrackDef muGlassthrix3;
        public static CustomMusicTrackDef muGlassthrix4;

        public override void CreatePrefab()
        {
            base.CreatePrefab();
            SwapStats(prefab, 20, 0, 22, 2500, 0, 12, 25);
            body = prefab.GetComponent<CharacterBody>();
            body.baseNameToken = "GOTCE_GLASSTHRIX_NAME";
            body.subtitleNameToken = "GOTCE_GLASSTHRIX_SUBTITLE";
            body.gameObject.AddComponent<WaveController>();
            body.gameObject.AddComponent<CloneSpammer>();
        }
        public override void Modify()
        {
            base.Modify();
            SkillLocator locator = prefab.GetComponent<SkillLocator>();
            ReplaceSkill(locator.primary, Skills.Glassthrix.GlassthrixHammer1.Instance.SkillDef);
            ReplaceSkill(locator.secondary, Skills.Glassthrix.GlassthrixSlash1.Instance.SkillDef);
            ReplaceSkill(locator.utility, Skills.Glassthrix.GlassthrixDash1.Instance.SkillDef);

            ConditionalSkillOverride skillOverride = prefab.GetComponent<ConditionalSkillOverride>();
            GameObject.Destroy(skillOverride);

            string cessationname = "LANG_CESSATION_NAME";
            string cessationsub = "LANG_CESSATION_SUBTITLE";

            SceneDef moon = Utils.Paths.SceneDef.moon.Load<SceneDef>();
            moon.nameToken = cessationname;
            moon.subtitleToken = cessationsub;

            LanguageAPI.Add(cessationname, "Cessation");
            LanguageAPI.Add(cessationsub, "twitch.tv/wooliegaming");

            DeathRewards deathRewards = prefab.GetComponent<DeathRewards>();
            deathRewards = prefab.AddComponent<DeathRewards>();
            deathRewards.characterBody = body;
            deathRewards.logUnlockableDef = ScriptableObject.CreateInstance<UnlockableDef>();
            deathRewards.logUnlockableDef.nameToken = "GOTCE_GLASSTHRIX_NAME";

            SwapMaterials(prefab, Utils.Paths.Material.maBrotherGlassOverlay.Load<Material>(), true, null);

            LanguageAPI.Add("GOTCE_GLASSTHRIX_NAME", "Glassthrix");
            LanguageAPI.Add("GOTCE_GLASSTHRIX_SUBTITLE", "Cracked King");
            
            GlobalEventManager.onServerDamageDealt += StealItem;
            On.EntityStates.BrotherMonster.ExitSkyLeap.FireRingAuthority += GameDesign;
            On.RoR2.Stage.Start += HandleGlassthrixConversion;
            On.EntityStates.BrotherMonster.UltChannelState.OnEnter += GameDesign2;
            On.RoR2.Run.Start += ResetMoon;

            prefabMaster.GetComponent<CharacterMaster>().bodyPrefab = prefab;

            glassthrixCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            glassthrixCard.prefab = prefabMaster;
            glassthrixCard.nodeGraphType = MapNodeGroup.GraphType.Ground;
            glassthrixCard.sendOverNetwork = true;
            glassthrixCard.name = "glassthrix"; 
            glassthrixCard.forbiddenFlags = NodeFlags.NoCharacterSpawn;;
            glassthrixCard.hullSize = HullClassification.Golem;

            moonDetonation = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            moonDetonation.nodeGraphType = MapNodeGroup.GraphType.Ground;
            moonDetonation.prefab = Utils.Paths.GameObject.BrotherHauntMaster.Load<GameObject>();
            moonDetonation.name = "moon detonation characterbody";
            moonDetonation.forbiddenFlags = NodeFlags.NoCharacterSpawn;
            moonDetonation.hullSize = HullClassification.Golem;

            uint[][] p1 = {new uint[] {THEME_GROUP, THEME_INTRO}, new uint[] {THEME_GROUP, THEME_P1_LOOP}};
            uint[][] p2 = {new uint[] {THEME_GROUP, THEME_12_TRANSITION}, new uint[] {THEME_GROUP, THEME_P2_LOOP}};
            uint[][] p3 = {new uint[] {THEME_GROUP, THEME_23_TRANSITION}, new uint[] {THEME_GROUP, THEME_P3_LOOP}};
            uint[][] p4 = {new uint[] {THEME_GROUP, THEME_34_TRANSITION}, new uint[] {THEME_GROUP, THEME_P4_LOOP}};

            muGlassthrix1 = CreateDef("muGlassthrix1", p1);
            muGlassthrix2 = CreateDef("muGlassthrix2", p2);
            muGlassthrix3 = CreateDef("muGlassthrix3", p3);
            muGlassthrix4 = CreateDef("muGlassthrix4", p4);
        }

        private void ResetMoon(On.RoR2.Run.orig_Start orig, Run self)
        {
            orig(self);
            // shouldProceedGlassthrixConversion = false;
        }

        private CustomMusicTrackDef CreateDef(string name, params uint[][] ids) {
            CustomMusicTrackDef def = ScriptableObject.CreateInstance<CustomMusicTrackDef>();
            def.cachedName = name;
            def.SoundBankName = Main.BankData.SoundBankName;
            def.CustomStates = new();

            for (int i = 0; i < ids.Length; i++) {
                CustomMusicTrackDef.CustomState state = new();
                state.GroupId = ids[i][0];
                state.StateId = ids[i][1];
                def.CustomStates.Add(state);
            }

            return def;
        }

        private void GameDesign2(On.EntityStates.BrotherMonster.UltChannelState.orig_OnEnter orig, UltChannelState self)
        {
            orig(self);

            if (UltChannelState.waveProjectileCount != glassthrixWaveCount) {
                vanillaWaveCount = UltChannelState.waveProjectileCount;
            }

            if (UltChannelState.totalWaves != glassthrixTotalWaves) {
                vanillaTotalWaves = UltChannelState.totalWaves;
            }

            UltChannelState.totalWaves = vanillaTotalWaves;
            UltChannelState.waveProjectileCount = vanillaWaveCount;

            if (self.GetComponent<WaveController>()) {
                UltChannelState.totalWaves = glassthrixTotalWaves;
                UltChannelState.waveProjectileCount = glassthrixWaveCount;
            }
        }

        private void HandleGlassthrixConversion(On.RoR2.Stage.orig_Start orig, Stage self)
        {
            orig(self);

            if (SceneManager.GetActiveScene().name != "moon") return;

            if (shouldProceedGlassthrixConversion) {
                ScriptedCombatEncounter[] encounters = GameObject.FindObjectsOfType<ScriptedCombatEncounter>();

                foreach (ScriptedCombatEncounter encounter in encounters) {
                    switch (encounter.gameObject.name) {
                        case "BrotherEncounter, Phase 1":
                            encounter.spawns[0].spawnCard = glassthrixCard;
                            atlasSpawnPos = encounter.spawns[0].explicitSpawnPosition.transform.position;
                            SetupMusic(encounter.gameObject, muGlassthrix1);
                            break;
                        case "BrotherEncounter, Phase 2":
                            for (int i = 0; i < 5; i++) {
                                encounter.spawns.AddItem(new ScriptedCombatEncounter.SpawnInfo() {
                                    spawnCard = moonDetonation,
                                    explicitSpawnPosition = encounter.spawns[0].explicitSpawnPosition,
                                    cullChance = 0f
                                });
                            }
                            SetupMusic(encounter.gameObject, muGlassthrix2);
                            break;
                        case "BrotherEncounter, Phase 3":
                            encounter.spawns[0].spawnCard = glassthrixCard;
                            SetupMusic(encounter.gameObject, muGlassthrix3);
                            break;
                        case "BrotherEncounter, Phase 4":
                            GameObject go = new("spawnPointSafeTravels");
                            go.transform.position = encounter.spawns[0].explicitSpawnPosition.transform.position + (Vector3.up * 300);
                            encounter.spawns[0].explicitSpawnPosition = go.transform;
                            encounter.spawns[0].spawnCard = SafeTravels.safeTravelsCard;
                            Debug.Log("registering spawn for atlas cannon");
                            encounter.onBeginEncounter += SpawnAtlasCannon;
                            encounter.combatSquad.onMemberDefeatedServer += (c, r) => {
                                if (Difficulty.IsCurrentDifHigherOrEqual(Difficulty.c7, Run.instance) && Random.Range(0, 100f) > 50) {
                                    UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.FatalError);
                                }
                            };
                            SetupMusic(encounter.gameObject, muGlassthrix4);
                            break;
                    }
                }
            }
        }

        public static void SetupMusic(GameObject obj, CustomMusicTrackDef def) {
            MusicTrackOverride over = obj.GetComponentInChildren<MusicTrackOverride>(true);

            if (!over) {
                return;
            }

            over.track = def;
            
            AkState state = over.GetComponent<AkState>();

            if (state) {
                state.enabled = false;
            }

            OnEnableEvent enable = over.GetComponent<OnEnableEvent>();

            if (enable) {
                enable.enabled = false;
            }
        }

        public static void SpawnAtlasCannon(ScriptedCombatEncounter encounter) {
            Debug.Log("spawning atlas cannon!");
            if (NetworkServer.active) {
                GameObject atlas = GameObject.Instantiate(Interactables.AtlasCannon.Instance.prefab, atlasSpawnPos, Quaternion.identity);
                NetworkServer.Spawn(atlas);
                EffectManager.SimpleEffect(Utils.Paths.GameObject.OmniExplosionVFXEngiTurretDeath.Load<GameObject>(), atlasSpawnPos, Quaternion.identity, true);
            }
        }

        public override void PostCreation()
        {
            base.PostCreation();
            RegisterEnemy(prefab, prefabMaster, null);
        }

        private void GameDesign(On.EntityStates.BrotherMonster.ExitSkyLeap.orig_FireRingAuthority orig, ExitSkyLeap self) {
            orig(self);
            WaveController controller = self.GetComponent<WaveController>();
            if (!controller || controller.hasStarted) {
                return;
            }

            self.duration += PhaseCounter.instance.phase == 3 ? P3WavesCount * WavesInterval : P1WavesCount * WavesInterval;
            controller.hasStarted = true;
            controller.count = PhaseCounter.instance.phase == 3 ? P3WavesCount : P1WavesCount;
            controller.exit = self;
            controller.TheFunny();
        }

        private class WaveController : MonoBehaviour {
            public bool hasStarted;
            public int count;
            public ExitSkyLeap exit;

            public void TheFunny() {
                if (count <= 0) {
                    hasStarted = false;
                    return;
                }

                exit.FireRingAuthority();
                count--;
                Invoke(nameof(TheFunny), WavesInterval);
            }
        }

        private class CloneSpammer : MonoBehaviour {
            public float cloneCd = 4f;
            public float cloneTimer;
            private int totalSpawned = 0;

            public void Start() {
                if (!PhaseCounter.instance || PhaseCounter.instance.phase != 3) {
                    GameObject.Destroy(this);
                }
                else {
                    return;
                    MasterSummon summon = new();
                    summon.masterPrefab = Utils.Paths.GameObject.BrotherGlassMaster.Load<GameObject>();
                    summon.ignoreTeamMemberLimit = true;
                    summon.position = base.transform.position + (Random.onUnitSphere * 10f) + (Vector3.up * 10f);
                    summon.useAmbientLevel = true;
                    summon.teamIndexOverride = TeamIndex.Monster;
                    summon.summonerBodyObject = base.gameObject;
                    summon.preSpawnSetupCallback += (CharacterMaster master) => {
                        if (master) {
                            master.onBodyStart += (CharacterBody body) => {
                                SkillLocator loc = body.skillLocator;
                                loc.primary.SetSkillOverride(body, Skills.Glassthrix.GlassthrixHammer1.Instance.SkillDef, GenericSkill.SkillOverridePriority.Replacement);
                                // loc.utility.SetSkillOverride(body, Skills.Glassthrix.GlassthrixDash1.Instance.SkillDef, GenericSkill.SkillOverridePriority.Replacement);
                                body.baseMoveSpeed *= 2f;
                            };
                        }
                    };
                    summon.Perform();
                }
            }
            public void FixedUpdate() {
                cloneTimer += Time.fixedDeltaTime;

                if (totalSpawned > 20) {
                    return;
                }

                if (cloneTimer > cloneCd) {
                    cloneTimer = 0;

                    MasterSummon summon = new();
                    summon.masterPrefab = Utils.Paths.GameObject.BrotherGlassMaster.Load<GameObject>();
                    summon.ignoreTeamMemberLimit = true;
                    summon.position = base.transform.position + (Random.onUnitSphere * 10f) + (Vector3.up * 10f);
                    summon.useAmbientLevel = true;
                    summon.teamIndexOverride = TeamIndex.Monster;
                    summon.summonerBodyObject = base.gameObject;
                    summon.preSpawnSetupCallback += (CharacterMaster master) => {
                        return; if (master) {
                            master.onBodyStart += (CharacterBody body) => {
                                body.transform.localScale *= 0.5f;
                                body.baseMaxHealth *= 0.3f;
                                SkillLocator loc = body.skillLocator;
                                loc.primary.SetSkillOverride(body.gameObject, Utils.Paths.SkillDef.CommandoBodyBarrage.Load<SkillDef>(), GenericSkill.SkillOverridePriority.Contextual);
                            };
                        }
                    };
                    summon.Perform();

                    totalSpawned++;
                }
            }
        }

        private void StealItem(DamageReport report) {
            if (NetworkServer.active && report.damageInfo.HasModdedDamageType(DamageTypes.StealItem)) {
                if (report.victimBody && report.victimBody.inventory) {
                    ItemIndex index;
                    try {
                        index = report.victimBody.inventory.itemAcquisitionOrder.First();
                    } catch {
                        return;
                    }
                    int c = report.victimBody.inventory.GetItemCount(index);
                    report.victimBody.inventory.RemoveItem(index, c);
                    ItemTransferOrb orb = new();
                    orb.inventoryToGrantTo = report.attackerBody.inventory;
                    orb.itemIndex = index;
                    orb.stack = c;
                    orb.origin = report.damageInfo.position;
                    orb.target = report.attackerBody.mainHurtBox;
                    OrbManager.instance.AddOrb(orb);
                }
            }
        }
    }
}