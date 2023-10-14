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

        public override string ItemLore => "\"Hello and welcome to SpafDonalds, how may I help you?\"\n\"Yes hello I would like to get the 15-piece chicken SpafNuggets with a side of large fries and a medium Eclipse 16 with no ice.\"\n\"Certainly. That will be 20 bobux please.\"\n\"Yeah here you go.\"\n\"Thank you very much. Enjoy your meal.\"\n\"Yeah you too.\"\n\n\n\n...\n\"Wait fuck.\"";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, GOTCETags.Bullshit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/spafnarsfries.png");

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