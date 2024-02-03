using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items.White
{
    public class OSPGenerator : ItemBase<OSPGenerator>
    {
        public override string ConfigName => "Personal OSP Generator";

        public override string ItemName => "Personal OSP Generator";

        public override string ItemLangTokenName => "GOTCE_OSPGenerator";

        public override string ItemPickupDesc => "Increases One Shot Protection threshold and invincibility frames";

        public override string ItemFullDescription => "Increases OSP threshold by 7% (+7% per stack) and increases OSP invincibility time by 0.1 (+0.1 per stack) seconds.";

        public override string ItemLore => "If you didn't know, there is a hidden mechanic (although it's not that hidden anymore with the health bar update in 1.0) called \"one shot protection\"; If you are at or above 90% of your combined max hp, which is your regular health plus any shields (not barrier) that you have, you cannot die to one instance of damage. There are a few more things to one shot protection, such as its lingering duration but in regards to the shield gens the problem is not that shields remove or disable your one shot protection but rather significantly alter how it's kept up. See, your shields cannot be leeched or regened normally, the only way to restore missing shields is to take absolutely NO damage for seven seconds and then allow them to fill up back to their full amount which takes no longer than two seconds. This is the issue with shields and one shot protection; if you take heavy damage and lose your one shot protection you do not want to be stuck around waiting for that close to 10 seconds until it is back up. Everyone who's gotten past the first loop knows that there are plenty (and I mean PLENTY) of things that can one shot you in your run, so why would you want to intentionally gimp yourself by not having one shot protection up as much as possible. Now, are shield gens bad? No, not necessarily, extra hp is pretty nice to have; but is the tiny amount of hp the shield gens actually provide you worth the trade-off of being wary of a one shot around every single corner? Not to mention your overall reduction to healing and regen capabilities. To me, no it's not which is why the shield gen receives a C.";

        public override ItemTier Tier => ItemTier.NoTier;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Healing };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/NEAPlaceholder.png");

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
            On.RoR2.CharacterBody.RecalculateStats += OSP;
            On.RoR2.HealthComponent.TriggerOneShotProtection += Timer;
        }

        public void OSP(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody body)
        {
            orig(body);
            int c = GetCount(body);
            if (c > 0) {
                body.oneShotProtectionFraction += (0.07f * c);
            }
        }

        public void Timer(On.RoR2.HealthComponent.orig_TriggerOneShotProtection orig, HealthComponent self) {
            orig(self);
            int c = GetCount(self.body);
            if (c > 0) {
                self.ospTimer += (0.1f * c);
            }
        }
    }
}