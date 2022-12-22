using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using GOTCE.Components;

namespace GOTCE.Items.White
{
    public class gd2 : ItemBase<gd2>
    {
        public override bool CanRemove => true;
        public override string ConfigName => ItemName;
        public override string ItemFullDescription => "Gain <style=cIsUtility>5% sprint crit chance</style>. On '<style=cIsUtility>critical sprint</style>', your attacks inflict <style=cIsDamage>blight</style> for the next <style=cIsUtility>4</style> <style=cStack>(+2 per stack)</style> seconds.";
        public override Sprite ItemIcon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Items/RoR2Discussion.png");
        public override string ItemLangTokenName => "GOTCE_gd2";
        public override string ItemLore => "The tar took over garbage once. The hivemind of humans touched by tar over the course of the missions worshipped the garbage, but soon split into groups. The tar was testing them, testing the strength it had given to see if they were worthy. What the tar found was the idiocy that mankind could bring. Their ramblings were insane.\n\n\"Ion Surge gives moblity!\"\n\"No, the DPS of Flamethrower is worth sacrificing the mobility!\"\n\"Suppressive Fire offers so much more damage than the grenades ever could! Even if frags did 2000% damage and had no falloff I'd stil use suppressive because I value survivability and consistency more than anything!\"\n\"Suppressive Fire users when Stun Grenade:\"\n\"Blight is so much better than you idiots who use Poison.\"\n\"You won't make it past stage four with Blight. No scale bro.\"\n\"image0.png\"\n\"User has been banned from RoR2Cord\"\n\"I fucking hate Gearbox!\"\n\"They're gonna ruin this fucking game.\"\n\"Never cared for it anyways.\"\n\"Tharson is not dead bro\"\n\nBut was it truly their fault? The tar told them to argue, and so they did. In the end, neither were right, but neither were wrong, for all the tar wanted was to test their strength. And so they did. Some people say that they can still hear the arguments spewed out from the garbage can.";
        public override GameObject ItemModel => null;
        public override string ItemName => "#ror2-discussion";
        public override string ItemPickupDesc => "On 'critical sprint', your attacks inflict blight for a short duration.";
        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.Crit };
        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            RoR2BlightBuff.Awake();
            CriticalTypes.OnSprintCrit += Blight;
            On.RoR2.DamageInfo.ModifyDamageInfo += Inflict;
            StatsCompEvent.StatsCompRecalc += (object sender, StatsCompRecalcArgs args) =>
            {
                if (args.Stats && NetworkServer.active)
                {
                    if (args.Stats.inventory)
                    {
                        args.Stats.SprintCritChanceAdd += GetCount(args.Stats.body) > 0 ? 5 : 0;
                    }
                }
            };
        }

        public void Blight(object sender, SprintCritEventArgs args)
        {
            if (args.Body && NetworkServer.active && GetCount(args.Body) > 0)
            {
                args.Body.AddTimedBuff(RoR2BlightBuff.buff, 4f + (2f * (GetCount(args.Body) - 1)));
            }
        }

        public void Inflict(On.RoR2.DamageInfo.orig_ModifyDamageInfo orig, DamageInfo self, HurtBox.DamageModifier mod)
        {
            if (self.attacker && self.attacker.GetComponent<CharacterBody>() && NetworkServer.active)
            {
                if (self.attacker.GetComponent<CharacterBody>().HasBuff(RoR2BlightBuff.buff))
                {
                    self.damageType |= DamageType.BlightOnHit;
                }
            }
            orig(self, mod);
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }
    }

    public class RoR2BlightBuff
    {
        public static BuffDef buff;

        public static void Awake()
        {
            buff = ScriptableObject.CreateInstance<BuffDef>();
            buff.canStack = false;
            buff.isDebuff = false;
            buff.name = "spcrit";
            buff.isHidden = true;

            R2API.ContentAddition.AddBuffDef(buff);
        }
    }
}