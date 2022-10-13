using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using RoR2.Skills;
using UnityEngine.Networking;
using GOTCE.Skills;

namespace GOTCE.Enemies.Minibosses
{
    public class CrackedPest
    {
        public static GameObject CrackedPestObj;
        public static GameObject CrackedPestMaster;

        public static void Create()
        {
            CrackedPestObj = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/FlyingVermin/FlyingVerminBody.prefab").WaitForCompletion(), "CrackedPest");
            CrackedPestMaster = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/FlyingVermin/FlyingVerminMaster.prefab").WaitForCompletion(), "CrackedPestMaster");
            // R2API.ContentAddition.AddBody(CrackedPestObj);
            CharacterBody body = CrackedPestObj.GetComponent<CharacterBody>();
            body.baseDamage = 30f;
            body.levelDamage = 6f;

            body.baseMaxHealth = 150f;
            body.levelMaxHealth = 45f;

            body.baseMoveSpeed = 45f;

            body.baseAttackSpeed = 1f;
            body.levelAttackSpeed = 1f;

            body.hasOneShotProtection = true;

            body.oneShotProtectionFraction = 0.5f;

            body.baseNameToken = "CRACKED_PEST_NAME";
            // body.inventory.GiveItem(RoR2Content.Items.Behemoth, 3);

            body.name = "CrackedPestBody";

            // CharacterModel model = CrackedPestObj.GetComponentInChildren<CharacterModel>();
            /* model.baseRendererInfos[0].defaultMaterial = Main.MainAssets.LoadAsset<Material>("Assets/Materials/Enemies/crackedPestMaterial.mat");
            model.baseRendererInfos[1].defaultMaterial = Main.MainAssets.LoadAsset<Material>("Assets/Materials/Enemies/crackedPestMaterial.mat");
            model.baseRendererInfos[2].defaultMaterial = Main.MainAssets.LoadAsset<Material>("Assets/Materials/Enemies/crackedPestMaterial.mat"); */

            SkillLocator locator = CrackedPestObj.GetComponent<SkillLocator>();
            SkillFamily family = ScriptableObject.CreateInstance<SkillFamily>();
            ((ScriptableObject)family).name = "cracked";
            // family.variants = new SkillFamily.Variant[1];
            locator.primary._skillFamily = family;
            locator.primary._skillFamily.variants = new SkillFamily.Variant[1];
            SkillDef cracked = Skills.CrackedVerminSpit.Instance.SkillDef;

            locator.primary._skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = cracked
            };


            CharacterMaster master = CrackedPestMaster.GetComponent<CharacterMaster>();
            master.bodyPrefab = CrackedPestObj;
            master.name = "CrackedPestMaster";

            LanguageAPI.Add("CRACKED_PEST_NAME", "Cracked Pest");
            LanguageAPI.Add("CRACKED_PEST_SUBTITLE", "Emoji Crack");

            ContentAddition.AddBody(CrackedPestObj);
            ContentAddition.AddMaster(CrackedPestMaster);

            On.RoR2.GlobalEventManager.OnHitAll += vineboom;
        }

        public static void vineboom(On.RoR2.GlobalEventManager.orig_OnHitAll orig, GlobalEventManager self, DamageInfo info, GameObject target) {
            orig(self, info, target);
            if (NetworkServer.active) {
                if (info.procChainMask.HasProc(ProcType.AACannon)) {
                    float damage = info.damage * 0.15f;
                    float num = 8f;

                    EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXQuick"), new EffectData
                    {
                        origin = info.position,
                        scale = num,
                        rotation = Util.QuaternionSafeLookRotation(info.force)
                    }, transmit: true);

                    BlastAttack blast = new BlastAttack();
                    blast.attacker = info.attacker;
                    blast.baseDamage = damage;
                    blast.radius = num;
                    blast.inflictor = null;
                    blast.falloffModel = BlastAttack.FalloffModel.None;
                    blast.crit = info.crit;
                    blast.position = info.position;
                    blast.procCoefficient = 0f;
                    blast.damageColorIndex = DamageColorIndex.Item;
                    blast.teamIndex = TeamComponent.GetObjectTeam(info.attacker);
                    blast.damageType = info.damageType;
                    blast.Fire();
                }
            }
        }
    }
}