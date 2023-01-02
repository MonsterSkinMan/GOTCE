using System;
using Unity;
using UnityEngine;
using RoR2;

namespace GOTCE.Mechanics
{
    public class CriticalTypes
    {
        public static EventHandler<SprintCritEventArgs> OnSprintCrit;

        public static EventHandler<StageCritEventArgs> OnStageCrit;

        public static EventHandler<FovCritEventArgs> OnFovCrit;
        public static EventHandler<DeathCritEventArgs> OnDeathCrit;
        private static bool lastStageWasCrit = true;

        public static void Hooks() {
            On.RoR2.CharacterBody.Start += CharacterBody_Start_FOV;
            CharacterBody.onBodyStartGlobal += CharacterBody_OnStart_Crit;
            On.RoR2.CharacterBody.OnSprintStart += CharacterBody_OnSprintStart_Crit;
            R2API.RecalculateStatsAPI.GetStatCoefficients += CriticalRecalcSprint;
            On.RoR2.GlobalEventManager.OnCharacterDeath += CriticalDeath;

            RoR2.Run.onRunStartGlobal += (run) =>
            {
                lastStageWasCrit = false;
            };
        }  

        private static void CriticalDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport report) {
            orig(self, report);
            if (NetworkServer.active) {
                if (report.victimMaster && report.victimMaster.GetStatsComponent(out GOTCE_StatsComponent stats)) {
                    Debug.Log("stats comp found");
                    if (Util.CheckRoll(stats.deathCritChance, report.victimMaster) && !stats.isOnCritDeathCooldown) {
                        Debug.Log("critically dying");
                        stats.isOnCritDeathCooldown = true;
                        stats.deathPos = report.damageInfo.position;
                        stats.CriticallyDie();
                        OnDeathCrit?.Invoke(report.victimMaster, new(report.victimMaster, report));
                    }
                }
            }
        }

        private static void CharacterBody_Start_FOV(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self) {
            orig(self);
            if (self.isPlayerControlled)
            {
                self.gameObject.AddComponent<GOTCE_FovComponent>();
            }
        }

        private static void CharacterBody_OnStart_Crit(CharacterBody body)
        {   // there is a 99% chance this code is horrible
            if (body.masterObject && !body.masterObject.GetComponent<GOTCE_StatsComponent>())
            {
                body.masterObject.AddComponent<GOTCE_StatsComponent>();
            }
            if (NetworkServer.active && body.isPlayerControlled && Stage.instance.entryTime.timeSince <= 3f && body.masterObject.GetComponent<GOTCE_StatsComponent>())
            {
                bool lastStageWasCritPrev = lastStageWasCrit;

                if (lastStageWasCrit)
                {
                    EventHandler<StageCritEventArgs> raiseEvent = CriticalTypes.OnStageCrit;

                    if (raiseEvent != null)
                    {
                        raiseEvent(body.gameObject, new StageCritEventArgs());
                    }
                    lastStageWasCrit = false;
                }
                float totalChance = 0f;

                foreach (PlayerCharacterMasterController masterController in PlayerCharacterMasterController.instances)
                {
                    CharacterMaster master = masterController.master;
                    if (master.gameObject.GetComponent<GOTCE_StatsComponent>())
                    {
                        GOTCE_StatsComponent vars = master.gameObject.GetComponent<GOTCE_StatsComponent>();
                        vars.DetermineStageCrit();
                        totalChance += vars.stageCritChance;
                    }
                }

                if (Util.CheckRoll(totalChance) && !lastStageWasCritPrev)
                {
                    lastStageWasCrit = true;
                    Run.instance.AdvanceStage(Run.instance.nextStageScene);
                }
            }
        }

        private static void CharacterBody_OnSprintStart_Crit(On.RoR2.CharacterBody.orig_OnSprintStart orig, CharacterBody self) {
            orig(self);
            if (NetworkServer.active && self.isPlayerControlled) {
                if (self.masterObject && self.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                    GOTCE_StatsComponent stats = self.masterObject.GetComponent<GOTCE_StatsComponent>();
                    if (Util.CheckRoll(stats.sprintCritChance, self.master)) {
                        stats.isCriticallySprinting = true;
                        self.RecalculateStats();
                        OnSprintCrit?.Invoke(self.gameObject, new(self));
                    }
                }
            }
        }

        private static void CriticalRecalcSprint(CharacterBody self, R2API.RecalculateStatsAPI.StatHookEventArgs args) {
            if (NetworkServer.active) {
                if (self.masterObject && self.masterObject.GetComponent<GOTCE_StatsComponent>()) {
                    GOTCE_StatsComponent stats = self.masterObject.GetComponent<GOTCE_StatsComponent>();
                    if (!self.isSprinting) {
                        stats.isCriticallySprinting = false;
                    }
                    if (stats.isCriticallySprinting) {
                        args.moveSpeedMultAdd += self.sprintingSpeedMultiplier;
                    }
                }
            }
        }
        

        private class GOTCE_FovComponent : MonoBehaviour
        {
            public CharacterBody body;
            public Components.GOTCE_StatsComponent stats;
            private int baseFov;
            public bool critting = false;
            public float interval = 1f;
            public float stopwatch = 0f;
            private CameraTargetParams.CameraParamsOverrideHandle handle;

            private void Start()
            {
                body = gameObject.GetComponent<CharacterBody>();
                stats = body.masterObject.GetComponent<Components.GOTCE_StatsComponent>();
            }

            private void FixedUpdate()
            {
                stopwatch += Time.fixedDeltaTime;
                // Debug.Log(stopwatch);
                if (stopwatch >= interval)
                {
                    stopwatch = 0f;

                    if (!Util.CheckRoll(stats.fovCritChance, body.master) && critting)
                    {
                        critting = false;

                        gameObject.GetComponent<CameraTargetParams>().RemoveParamsOverride(handle, 0.5f);
                    }
                    else if (Util.CheckRoll(stats.fovCritChance, body.master) && critting) {
                        OnFovCrit?.Invoke(body.gameObject, new FovCritEventArgs(body));
                    }
                    else if (Util.CheckRoll(stats.fovCritChance, body.master) && !critting)
                    {
                        critting = true;

                        handle = gameObject.GetComponent<CameraTargetParams>().AddParamsOverride(new CameraTargetParams.CameraParamsOverrideRequest
                        {
                            cameraParamsData = new()
                            {
                                fov = new()
                                {
                                    value = 5,
                                    alpha = 0.7f
                                }
                            }
                        }, 0.5f);

                        EventHandler<FovCritEventArgs> raiseEvent = CriticalTypes.OnFovCrit;

                        // Event will be null if there are no subscribers
                        if (raiseEvent != null)
                        {
                            FovCritEventArgs args = new(body);

                            // Call to raise the event.
                            raiseEvent(this, args);
                        }
                        // Debug.Log("starting crit");
                    }
                }
            }
        }
    }

    public class SprintCritEventArgs
    {
        public CharacterBody Body;

        public SprintCritEventArgs(CharacterBody body)
        {
            Body = body;
        }
    }

    public class StageCritEventArgs : EventArgs
    {
        public StageCritEventArgs()
        {
        }
    }

    public class FovCritEventArgs : EventArgs
    {
        public CharacterBody Body;

        public FovCritEventArgs(CharacterBody body)
        {
            Body = body;
        }
    }

    public class DeathCritEventArgs : EventArgs {
        public CharacterMaster Master;
        public DamageReport Report;
        public DeathCritEventArgs(CharacterMaster _master, DamageReport _report) {
            Master = _master;
            Report = _report;
        }
    }

}