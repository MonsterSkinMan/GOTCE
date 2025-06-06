﻿using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using MonoMod.Cil;
using Mono.Cecil.Cil;

namespace GOTCE.Items.Green
{
    public class TrickleDownBleedonomics : ItemBase<TrickleDownBleedonomics>
    {
        public override string ConfigName => "Trickle Down Bleedonomics";

        public override string ItemName => "Trickle-down Bleedonomics";

        public override string ItemLangTokenName => "GOTCE_TrickleDownBleedonomics";

        public override string ItemPickupDesc => "Your bleed gains proc coefficient.";

        public override string ItemFullDescription => "Your bleed gains <style=cIsDamage>0.3</style> <style=cStack>(+0.3 per stack)</style> proc coefficient.";

        public override string ItemLore => "I promise you, it'll work! All of you will get your blood soon! If we give our blood to the... more fortunate, and as long as they don't have to donate as much, then they will have to bleed at some point and give back to us. Once that blood bleeds, it should trickle down to us, and we all will get more blood since the higher-up should be giving that blood away, but in greater mass! It'll work, I tell you!\n\nSo give those tri-tips and shatterspleens to me! I'll get you your blood procs later once I have enough!";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/TrickledownBleedonomics.png");

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
            // IL.RoR2.DotController.EvaluateDotStacksForType += DotController_EvaluateDotStacksForType;
            // On.RoR2.DamageInfo.ModifyDamageInfo += Proc;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            var stack = Util.GetItemCountGlobal(Instance.ItemDef.itemIndex, true);
            if (stack > 0 && (damageInfo.dotIndex == DotController.DotIndex.Bleed || damageInfo.dotIndex == DotController.DotIndex.SuperBleed))
            {
                if (!damageInfo.procChainMask.HasProc(ProcType.Behemoth))
                {
                    damageInfo.procCoefficient = 0.3f * stack;
                    GlobalEventManager.instance.OnHitEnemy(damageInfo, self.gameObject);
                }
            }
            orig(self, damageInfo);
        }

        private void DotController_EvaluateDotStacksForType(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdcR4(0f),
                x => x.MatchStfld("RoR2.DamageInfo", "procCoefficient")))
            {
                c.Index++;
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<float, DamageInfo, float>>((orig, self) =>
                {
                    var stack = Util.GetItemCountGlobal(Instance.ItemDef.itemIndex, true);
                    if (stack > 0 && (self.dotIndex == DotController.DotIndex.Bleed || self.dotIndex == DotController.DotIndex.SuperBleed))
                    {
                        // GlobalEventManager.instance.OnHitEnemy(self, self.vcitm cant fuckigdfbsnigouidfg);
                        return 0.3f * stack;
                    }
                    return orig;
                });
            }
            else
            {
                Main.ModLogger.LogError("Failed to apply Trickle-down Bleedonomics Proc Coefficient hook");
            }
        }

        public void Proc(On.RoR2.DamageInfo.orig_ModifyDamageInfo orig, DamageInfo self, HurtBox.DamageModifier mod)
        {
            var stack = Util.GetItemCountGlobal(Instance.ItemDef.itemIndex, true);
            if (stack > 0 && (self.dotIndex == DotController.DotIndex.Bleed || self.dotIndex == DotController.DotIndex.SuperBleed))
            {
                if (!self.procChainMask.HasProc(ProcType.Behemoth))
                {
                    self.procCoefficient += 0.3f * stack;
                    // GlobalEventManager.instance.OnHitEnemy(self, self.victim ASDJSDUITGJDFUIDH)
                }
            }

            orig(self, mod);
        }
    }
}