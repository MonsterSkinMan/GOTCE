using RoR2;
using R2API;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class Retrobauble : ItemBase<Retrobauble>
    {
        public override string ConfigName => "Retrobauble";

        public override string ItemName => "Retrobauble";

        public override string ItemLangTokenName => "GOTCE_Retrobauble";

        public override string ItemPickupDesc => "Gain a small chance to slightly slow enemies on hit.";

        public override string ItemFullDescription => "Gain a <style=cIsUtility>30%</style> <style=cStack>(+5% per stack)</style> chance to <style=cIsUtility>slow</style> enemies on hit for <style=cIsUtility>-10% movement speed</style> for <style=cIsUtility>1s</style>.";

        public override string ItemLore => "Delivery Timeth: Chronobauble\nTracking Numerical: 99**\nDelivery By Cart: 03/03/2066\nShipping Method: Of Utter Importance\nShipping Address: 9042 Pvt.Drive, Yustik Plaza, Mercury\nBoxing Details:\n\nWeren't you but a small lad? The time of the Summer Solstice during which the small whippersnappers would often horse around outside feels as if it was not a long period of time since - but it has, in truth, been of many suns and moons, making a decade's worth of times the earth has rotated around the sun, am I not wrong? Our distinguishing factor between us and the dirt and stone beneath our feet truthfully does feel as if it moves faster than how we experience it as we spend more time as mortals - there are less moments to remember that can keep you grounded to the ticking hands of the clock. I do not, quite frankly, remember the moments when I was here for between twenty-five to twenty-six rotations around the sun. How did those specific memories leave?\n\nBack on manners, I located this antique from a shop located in Mercury. We refer to this object as a \"Chronobauble\" - The seller of these trinkets said it's about Einstein's theory of special relativity, the bloke, its supposed effect is to create distortions, which I do not fully comprehend my good friend. To put it simply, its specified effect is to slow down the time around us. I find that questionable.\n\nTo conclude, I will be ordering this object to arrive back within my hands in ten years. I interpret it as a gift towards myself from mine self. It will serve as a reminder to make my own memories and to slow down my interactions. Those ten rotations, as we specified, will pass at an incredibly uncomprehendable rate, and I implore you to try and recollect those memories! Life is not worth passing by, you must put effort into that difference between you and the ground beneath you!";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, GOTCETags.Bullshit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/Retrobauble.png");

        public static BuffDef oldChrono;

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
            oldChrono = ScriptableObject.CreateInstance<BuffDef>();
            oldChrono.isHidden = false;
            oldChrono.isDebuff = true;
            oldChrono.canStack = false;
            oldChrono.isCooldown = false;
            oldChrono.buffColor = new Color32(173, 156, 105, 255);
            oldChrono.iconSprite = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/SlowOnHit/bdSlow60.asset").WaitForCompletion().iconSprite;

            ContentAddition.AddBuffDef(oldChrono);

            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
        }

        private void GlobalEventManager_onServerDamageDealt(DamageReport report)
        {
            var attackerBody = report.attackerBody;
            if (!attackerBody)
                return;

            var master = attackerBody.master;
            if (!master)
                return;

            var victimBody = report.victimBody;
            if (!victimBody)
                return;

            var stack = GetCount(attackerBody);
            if (stack > 0 && Util.CheckRoll(30f + 5f * (stack - 1), master))
            {
                victimBody.AddTimedBuffAuthority(oldChrono.buffIndex, 1f);
            }
        }
    }
}