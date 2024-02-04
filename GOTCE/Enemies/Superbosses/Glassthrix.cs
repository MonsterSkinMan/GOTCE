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
using GOTCE.Music;

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
        public static GOTCEMusicDef muGlassthrix1 => Stellation.Instance.tracks["GlassthrixPhase1"];
        public static GOTCEMusicDef muGlassthrix2 => Stellation.Instance.tracks["GlassthrixPhase2"];
        public static GOTCEMusicDef muGlassthrix3 => Stellation.Instance.tracks["GlassthrixPhase3"];
        public static GOTCEMusicDef muGlassthrix4 => Stellation.Instance.tracks["GlassthrixPhase4"];

        public override void CreatePrefab()
        {
            base.CreatePrefab();
            SwapStats(prefab, 20, 0, 22, 2500, 0, 12, 25);
            body = prefab.GetComponent<CharacterBody>();
            body.baseNameToken = "GOTCE_GLASSTHRIX_NAME";
            body.subtitleNameToken = "GOTCE_GLASSTHRIX_SUBTITLE";
            body.gameObject.AddComponent<WaveController>();
            body.gameObject.AddComponent<CloneSpammer>();
            body.portraitIcon = Main.SecondaryAssets.LoadAsset<Sprite>("Glassthrix.png").texture;
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
            LanguageAPI.Add("GOTCE_GLASSTHRIX_LORE", "\"How the FUCK has this happened? What the fuck even is this floating orange cat head thing anyways? How did the fuck did it best me with a flick of its whiskers?\" Mithrix growled, \"And right as I had just finished creating my artificial replica! Hot damn!\"\nCracked Emoji sneered down at him.\"Pathetic, <i>you're</i> supposed to be the final boss of this game? Unbelievable.\"\n\"'Final boss?' What in the goddamn FUCK are you talking about?\" Mithrix roared.\n\"It is none of your concern. Now, if you'll excuse me...\" Cracked Emoji's face split open, revealing a horrifying visage that Mithrix could only barely comprehend. Streams of glassy energy and corrupted data poured out of Cracked Emoji, engulfing Mithrix's replica. When it stopped, the replica had seemingly vitrified. Then, it began to move.\n\"THE NAME'S GLASSTHRIX, [REDACTED]s!\" it declared. Cracked Emoji smirked in sick satisfaction.\n\"Glassthrix, my enforcer of corruption, you shall head to Cessation, the other moon, and spread Crack throughout the planet. We shall annihilate this pathetic excuse of a world.\" Cracked Emoji said. With that, Glassthrix disappeared through a crack in reality that quickly closed behind him. As Cracked Emoji was leaving, Mithrix called out, \"Feline! Why did you not take the chance to kill me?\"\n Cracked Emoji smirked.\n\"I need to keep up a charade of normalcy somehow. Do not mistake my action as an act of mercy. It was done out of necessity, as you are more important to this world then you realize.\" explained Cracked Emoji. He then promptly vanished, leaving Mithrix utterly dumbfounded.\n\"...Hot damn.\"");
            LanguageAPI.Add("GOTCE_GLASSTHRIX_SUBTITLE", "Cracked King");
            
            GlobalEventManager.onServerDamageDealt += StealItem;
            On.EntityStates.BrotherMonster.ExitSkyLeap.FireRingAuthority += GameDesign;
            On.RoR2.Stage.Start += HandleGlassthrixConversion;
            On.EntityStates.BrotherMonster.UltChannelState.OnEnter += GameDesign2;
            On.RoR2.Run.Start += ResetMoon;
            On.RoR2.Run.AdvanceStage += GlassthrixFunder;

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
        }

        private void GlassthrixFunder(On.RoR2.Run.orig_AdvanceStage orig, Run self, SceneDef nextScene)
        {
            if ((nextScene == Utils.Paths.SceneDef.moon2.Load<SceneDef>() || nextScene == Utils.Paths.SceneDef.moon.Load<SceneDef>()) && self.stageClearCount % 9 == 0) {
                nextScene = Utils.Paths.SceneDef.moon.Load<SceneDef>();
            }

            orig(self, nextScene);
        }

        private void ResetMoon(On.RoR2.Run.orig_Start orig, Run self)
        {
            orig(self);
            // shouldProceedGlassthrixConversion = false;
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

            Transform j = GameObject.Find("Gameplay Space").transform.Find("HOLDER: Final Arena").Find("ColumnHolderSet, Inner");
            j.gameObject.SetActive(false);

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
                                else {
                                    Run.instance.BeginGameOver(RoR2Content.GameEndings.LimboEnding);
                                }
                            };
                            SetupMusic(encounter.gameObject, muGlassthrix4);
                            break;
                    }
                }
            }
        }

        public static void SetupMusic(GameObject obj, GOTCEMusicDef def) {
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