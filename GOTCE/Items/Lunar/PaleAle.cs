using BepInEx.Configuration;
using IL.RoR2.Projectile;
using MonoMod.Cil;
using R2API;
using Rewired;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace GOTCE.Items.Lunar
{
    public class PaleAle : ItemBase<PaleAle>
    {
        public override string ConfigName => "Pale Ale";

        public override string ItemName => "Pale Ale";

        public override string ItemLangTokenName => "GOTCE_PaleAle";

        public override string ItemPickupDesc => "Increase your damage... <color=#FF7F7F>BUT majorly fuck up everyone's vision.</color> Currently host only :(\n";

        public override string ItemFullDescription => "Increase your <style=cIsDamage>damage</style> by <style=cIsDamage>60%</style> <style=cStack>(+40% per stack)</style>. Majorly fuck up everyone's vision. Increase <style=cIsUtility>cooldowns</style> by <style=cIsUtility>0%</style> <style=cStack>(+50% per stack)</style>. Currently host only :(";

        public override string ItemLore => "Holy shit brother! I figured out how to turn these worthless moon rocks into booze! Hot damn!\r\nSo basically I [REDACTED] and then I [REDACTED] and then after a little [REDACTED] I [REDACTED] Commando’s mother. Hot damn indeed! Anyways, it's time to get wasted. Glug glug glug…\r\nHoooly shit this stuff is strong, brother. I feel so much more powerful, but I also feel like there's a really horrible screen filter on my vision. Ohhhhh god…\r\nFuck you, brother. I hate you so fucking much, and I hate you even more because I’m drunk as shit. That gift should've been for ME! Not... YOU! It reminds me of a little ditty I once heard on my smartphone. It went a little something like this: There once was a man named Joe, and my tooth fell out on his head, and he put it under his pillow. And then, the Tooth Fairy took it and gave him a one dollar bill! That money should have been mine, not his! And so, I punched his guts in and poured acid all over his butt! And then I threw bananas at him like a wild monkey! After that, Joe ran away to the bottom of the Atlantic Ocean, with his glasses and moustache. But when he was running away, he dropped the one dollar bill, and it was finally mine!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\nSo what did you think of the song, brother? W-what? You thought it was shit? Well, fuck you, you insignificant WHORE!!!\r\n";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/PaleAle.png");
        // private static readonly string[] blacklistedScenes = { "artifactworld", "crystalworld", "eclipseworld", "infinitetowerworld", "intro", "loadingbasic", "lobby", "logbook", "mysteryspace", "outro", "PromoRailGunner", "PromoVoidSurvivor", "splash", "title", "voidoutro" };

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
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.damageMultAdd += 0.6f + 0.4f * (stack - 1);
                    args.cooldownMultAdd += 0f + 0.5f * (stack - 1);
                }

                sender.AddItemBehavior<PaleAleBehavior>(stack);
            }
        }
    }

    public class PaleAleBehavior : CharacterBody.ItemBehavior
    {
        public PostProcessVolume vol;
        public PostProcessProfile ppProfile;
        public GameObject ppHolder;

        public void Start()
        {
            if (body.hasAuthority)
            {
                ppHolder = new GameObject("ppHolder");
                ppHolder.transform.SetParent(gameObject.transform);
                ppHolder.layer = LayerIndex.postProcess.intVal;
                vol = ppHolder.AddComponent<PostProcessVolume>();
                vol.priority = int.MaxValue;
                vol.weight = int.MaxValue;

                vol.isGlobal = true;
                // vol.gameObject.layer = LayerIndex.postProcess.intVal;

                ppProfile = ScriptableObject.CreateInstance<PostProcessProfile>();
                ppProfile.name = "PaleAlePP";

                AmbientOcclusion ao = ppProfile.AddSettings<AmbientOcclusion>();
                ao.SetAllOverridesTo(true);
                ao.intensity.value = 0.71f;
                ao.thicknessModifier.value = 3.07f;
                ao.ambientOnly.value = false;

                Bloom bloom = ppProfile.AddSettings<Bloom>();
                bloom.SetAllOverridesTo(true);
                bloom.intensity.value = 1.2f;
                bloom.softKnee.value = 4.4f;
                bloom.threshold.value = -0.2f;

                ChromaticAberration chromab = ppProfile.AddSettings<ChromaticAberration>();
                chromab.SetAllOverridesTo(true);
                chromab.intensity.value = 20f;

                ColorGrading grading = ppProfile.AddSettings<ColorGrading>();
                grading.SetAllOverridesTo(true);
                grading.hueShift.value = 147f;
                grading.saturation.value = 45f;

                DepthOfField dof = ppProfile.AddSettings<DepthOfField>();
                dof.SetAllOverridesTo(true);
                dof.aperture.value = 15f;
                dof.focalLength.value = 124.56f;
                dof.focusDistance.value = 5f;

                Grain grain = ppProfile.AddSettings<Grain>();
                grain.SetAllOverridesTo(true);
                grain.intensity.value = 0.02f;
                grain.size.value = 11.18f;
                grain.lumContrib.value = 7.17f;
                grain.colored.value = true;

                MotionBlur mblur = ppProfile.AddSettings<MotionBlur>();
                mblur.SetAllOverridesTo(true);
                mblur.shutterAngle.value = 270f;
                mblur.sampleCount.value = 10;

                vol.sharedProfile = ppProfile;
            }
        }

        public void OnDestroy()
        {
            if (body.hasAuthority)
            {
                DestroyImmediate(vol);
                DestroyImmediate(ppProfile);
                DestroyImmediate(ppHolder);
            }
        }
    }
}