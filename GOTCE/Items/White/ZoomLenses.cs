using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using System;

namespace GOTCE.Items.White
{
    public class ZoomLenses : ItemBase<ZoomLenses>
    {
        public override string ConfigName => "Zoom Lenses";

        public override string ItemName => "Zoom Lenses";

        public override string ItemLangTokenName => "GOTCE_ZoomLenses";

        public override string ItemPickupDesc => "Gain a chance to periodically 'FOV Crit', zooming in your vision.";

        public override string ItemFullDescription => "Every second, you have a <style=cIsUtility>10%</style> <style=cStack>(+10% chance per stack)</style> to <style=cIsUtility>'FOV Crit'</style>, zooming in your vision for <style=cIsUtility>1</style> second.";

        public override string ItemLore => "Jesus fucking christ what drugs were we on when we came up with FOV crits god this is such a dogshit mechanic I hate this.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.AIBlacklist, GOTCETags.Crit, GOTCETags.FovRelated };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/ZoomLenses.png");

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.CharacterBody.Start += Start;
        }

        public void Start(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self)
        {
            if (self.isPlayerControlled)
            {
                self.gameObject.AddComponent<GOTCE_FovComponent>();
            }
            orig(self);
        }
    }

    public class GOTCE_FovComponent : MonoBehaviour
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

                if (!Util.CheckRoll(stats.fovCritChance, body.master) && critting) {
                    critting = false;

                    gameObject.GetComponent<CameraTargetParams>().RemoveParamsOverride(handle, 0.5f);
                }
                else if (Util.CheckRoll(stats.fovCritChance, body.master) && !critting) {
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