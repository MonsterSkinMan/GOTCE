using BepInEx.Configuration;
using MonoMod.Cil;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GOTCE.Items.Lunar
{
    public class BackupHammer : ItemBase<BackupHammer>
    {
        public override string ConfigName => "Backup Hammer";

        public override string ItemName => "Backup Hammer";

        public override string ItemLangTokenName => "GOTCE_BackupHammer";

        public override string ItemPickupDesc => "Gain extra secondary charges... <color=#FF7F7F>BUT you can only use your secondary skill.</color>\n";

        public override string ItemFullDescription => "Gain <style=cIsUtility>10</style> <style=cStack>(+10 per stack)</style> <style=cIsUtility>secondary charges</style>. You can only use your <style=cIsUtility>secondary skill</style>. Increase <style=cIsUtility>secondary skill cooldown</style> by <style=cIsUtility>0%</style> <style=cStack>(+50% per stack)</style>.";

        public override string ItemLore => "Okay I don't feel like writing a backup hammer log so it doesn't get one. It's a dogshit item anyways I don't think there are going to be any backup hammer fans out there who are disappointed over a lack of a backup hammer log.";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.BackupMagSynergy };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/backuphammer.png");

        private SkillDef lockedDef;

        public override void Init(ConfigFile config)
        {
            base.Init(config);
            lockedDef = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Captain/CaptainSkillDisconnected.asset").WaitForCompletion();
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            IL.RoR2.GenericSkill.CalculateFinalRechargeInterval += GenericSkill_CalculateFinalRechargeInterval;
        }

        private void GenericSkill_CalculateFinalRechargeInterval(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(
                x => x.MatchLdarg(0),
                x => x.MatchCallOrCallvirt<RoR2.GenericSkill>("get_baseRechargeInterval")
                ))
            {
                c.Remove();
                c.Remove();
            }
            else
            {
                Main.ModLogger.LogError("Failed to apply Cooldown Increase IL Hook #1");
            }

            if (c.TryGotoNext(
                x => x.MatchCallOrCallvirt<UnityEngine.Mathf>("Min")
                ))
            {
                c.Remove();
                //c.EmitDelegate<Func<float, float, float>>((v1, v2) => v2);
                //c.Emit(OpCodes.Ret);
            }
            else
            {
                Main.ModLogger.LogError("Failed to apply Cooldown Increase IL Hook #2");
            }

            // asked from Zenithrium, I know it's not great cause of c.Remove(), ye
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.cooldownMultAdd += 0.5f * (stack - 1);
                }
            }
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            if (self && self.inventory)
            {
                var stack = self.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0 && self.skillLocator)
                {
                    var sl = self.skillLocator;
                    if (sl.primary)
                    {
                        sl.primary.SetSkillOverride(self.masterObject, lockedDef, GenericSkill.SkillOverridePriority.Replacement);
                    }
                    if (sl.utility)
                    {
                        sl.utility.SetSkillOverride(self.masterObject, lockedDef, GenericSkill.SkillOverridePriority.Replacement);
                    }
                    if (sl.special)
                    {
                        sl.special.SetSkillOverride(self.masterObject, lockedDef, GenericSkill.SkillOverridePriority.Replacement);
                    }
                    if (sl.secondary)
                    {
                        sl.secondary.SetBonusStockFromBody(sl.secondary.bonusStockFromBody + 10 * stack);
                    }
                }

                if (stack <= 0 && self.skillLocator)
                {
                    var sl = self.skillLocator;
                    if (sl.primary)
                    {
                        sl.primary.UnsetSkillOverride(self.masterObject, lockedDef, GenericSkill.SkillOverridePriority.Replacement);
                    }
                    if (sl.utility)
                    {
                        sl.utility.UnsetSkillOverride(self.masterObject, lockedDef, GenericSkill.SkillOverridePriority.Replacement);
                    }
                    if (sl.special)
                    {
                        sl.special.UnsetSkillOverride(self.masterObject, lockedDef, GenericSkill.SkillOverridePriority.Replacement);
                    }
                }
            }
            orig(self);
        }
    }
}