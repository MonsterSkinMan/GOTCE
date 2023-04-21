/*using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using System;
using System.Collections.Generic;
using EntityStates;
using GOTCE.Skills;
using RoR2.ExpansionManagement;
using GOTCE.EntityStatesCustom;
using GOTCE.Survivors.Cracktain;

namespace GOTCE.Survivors {
    public class CrackedCaptain : SurvivorBase<CrackedCaptain>
    {
        public static GameObject CracktainBody;
        public static SurvivorDef sdCracktain;
        public static GameObject JeroneBody;
        public static GameObject JeroneMaster;
        //
        public static SkillDef sdLaser;
        public static SkillDef sdBarrage;
        public static SkillDef sdBeacons;
        public static SkillDef sdPortal;
        public static SkillDef sdJeronePrimary;
        public static SkillDef sdJeroneSecondary;
        public static SkillDef sdJeroneUtility;
        public static SkillDef sdJeroneSpecial;

        public override void Create()
        {
            CracktainBody = Main.SecondaryAssets.LoadAsset<GameObject>("CracktainBody.prefab");
            JeroneBody = Main.SecondaryAssets.LoadAsset<GameObject>("JeroneBody.prefab");
            JeroneMaster = Main.SecondaryAssets.LoadAsset<GameObject>("JeroneMaster.prefab");
            sdCracktain = Main.SecondaryAssets.LoadAsset<SurvivorDef>("sdCracktain.asset");

            JeroneBody.AddComponent<JeroneTurretAim>();
            JeroneMaster.AddComponent<JeroneManager>();
            CracktainBody.AddComponent<CracktainPassiveBehaviour>();

            BaseAI ai = JeroneMaster.GetComponent<BaseAI>();
            EntityStateMachine esm = JeroneMaster.GetComponent<EntityStateMachine>();

            SerializableEntityStateType type = new(typeof(EntityStatesCustom.Jerone.AI.JeroneAI));

            ai.scanState = type;
            esm.initialStateType = type;
            esm.mainStateType = type;

            LoadSkills();
            sdLaser.activationState = new(typeof(EntityStatesCustom.Cracktain.Laser));
            //
            sdJeroneSecondary.activationState = new(typeof(EntityStatesCustom.Jerone.Shotgun));
            sdJeroneSpecial.activationState = new(typeof(EntityStatesCustom.Jerone.Berserk));

            CharacterBody body = CracktainBody.GetComponent<CharacterBody>();
            body._defaultCrosshairPrefab = Utils.Paths.GameObject.StandardCrosshair.Load<GameObject>();

            ContentAddition.AddBody(JeroneBody);
            ContentAddition.AddBody(CracktainBody);
            ContentAddition.AddMaster(JeroneMaster);
            ContentAddition.AddSurvivorDef(sdCracktain);

            SetupLanguage();

            On.RoR2.GenericPickupController.BodyHasPickupPermission += LetJeronePickup;
        }

        public bool LetJeronePickup(On.RoR2.GenericPickupController.orig_BodyHasPickupPermission orig, CharacterBody body) {
            if (body.bodyIndex == BodyCatalog.FindBodyIndex("JeroneBody")) {
                return true;
            }
            return orig(body);
        }

        public void LoadSkills() {
            sdLaser = Main.SecondaryAssets.LoadAsset<SkillDef>("sdLaser.asset");
            sdBarrage = Main.SecondaryAssets.LoadAsset<SkillDef>("sdBarrage.asset");
            sdBeacons = Main.SecondaryAssets.LoadAsset<SkillDef>("sdBeacon.asset");
            sdPortal = Main.SecondaryAssets.LoadAsset<SkillDef>("sdPortal.asset");
            sdJeronePrimary = Main.SecondaryAssets.LoadAsset<SkillDef>("sdJeronePrimary.asset");
            sdJeroneSecondary = Main.SecondaryAssets.LoadAsset<SkillDef>("sdJeroneSecondary.asset");
            sdJeroneUtility = Main.SecondaryAssets.LoadAsset<SkillDef>("sdJeroneUtility.asset");
            sdJeroneSpecial = Main.SecondaryAssets.LoadAsset<SkillDef>("sdJeroneSpecial.asset");
        }

        public void SetupLanguage() {
            LanguageAPI.Add("GOTCE_CRACKTAIN_BODY_NAME", "Cracked Captain");
            LanguageAPI.Add("GOTCE_CRACKTAIN_BODY_SUBTITLE", "The Aviator");
            LanguageAPI.Add("GOTCE_JERONE_NAME", "Jerone");
            //
            LanguageAPI.Add("GOTCE_CRACKTAIN_PRIMARY_NAME", "CRK Laser System");
            LanguageAPI.Add("GOTCE_CRACKTAIN_PRIMARY_DESC", "<style=cIsDamage>Ignite.</style> Fire twin lasers for <style=cIsDamage>2x200% damage</style>.");
            //
            LanguageAPI.Add("GOTCE_CRACKTAIN_SECONDARY_NAME", "Ion Barrage");
            LanguageAPI.Add("GOTCE_CRACKTAIN_SECONDARY_DESC", "<style=cIsDamage>Stunning</style>. Launch a barrage of <style=cIsUtility>ion mortars</style> for <style=cIsDamage>3x700% damage</style>.");
            //
            LanguageAPI.Add("GOTCE_CRACKTAIN_UTILITY_NAME", "Bolstering Beacon Bundle");
            LanguageAPI.Add("GOTCE_CRACKTAIN_UTILITY_DESC", "Deploy a <style=cIsUtility>temporary support beacon</style> from 10 available types.");
            //
            LanguageAPI.Add("GOTCE_CRACKTAIN_SPECIAL_NAME", "Warp Beacons");
            LanguageAPI.Add("GOTCE_CRACKTAIN_SPECIAL_DESC", "Deploy 2 <style=cIsUtility>Warp Beacons</style>. Interacting with a Warp Beacon teleports the interactor to the opposite beacon.");
            //
            LanguageAPI.Add("GOTCE_CRACKTAIN_PASSIVE_NAME", "Tactical Support");
            LanguageAPI.Add("GOTCE_CRACKTAIN_PASSIVE_DESC", "Cracked Captain commands a robot, <style=cIsUtility>Jerone</style>. Jerone relies on your pings for instruction. Both you and Jerone share items collected, and <style=cDeath>damage taken</style>.");
            //
        }
    }
}*/