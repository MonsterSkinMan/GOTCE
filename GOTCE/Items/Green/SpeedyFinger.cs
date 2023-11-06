using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;

namespace GOTCE.Items.Green
{
    public class SpeedyFinger : ItemBase<SpeedyFinger>
    {
        public override string ConfigName => "Speedy Finger";

        public override string ItemName => "Speedy Finger";

        public override string ItemLangTokenName => "GOTCE_SpeedyFinger";

        public override string ItemPickupDesc => "Gain bonus attack speed for each secondary skill charge.";

        public override string ItemFullDescription => "Increase <style=cIsDamage>attack speed</style> by <style=cIsDamage>20%</style> <style=cStack>(+10% per stack)</style> for each <style=cIsUtility>secondary skill charge</style> you have.";

        public override string ItemLore => "The Gunslinger was a modern legend amongst Mercurian folks. They appeared after the plague that withered many of the farms. The gunslinger had no name, had no face, just a mask, a hat, a cloak, and their trusty revolver. The only people who feared them were the bandits who robbed and pillaged whoever they could find. The bandits were ruthless, stealing from the poor farmers who barely survived the plague, to even the banks of the most prominent towns. It was only the crooks who feared the Gunslinger.\n\nWhat gave the Gunslinger their name, though, was their ability. For anyone who saw the gunslinger, they would know the anomalous dexterity the Gunslinger had, for once the gunslinger started firing, the bullets would fly at a faster rate than before. They once killed an entire gang when they walked in, leaving a building full of bodies, and bullets still smoking.\n\nWhat ended it all though? Why is the Gunslinger only a myth now? Well, I'm not telling you. Get fucked.";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.BackupMagSynergy, GOTCETags.Bullshit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/SpeedyFinger.png");

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
            if (sender)
            {
                var skillLocator = sender.skillLocator;
                if (skillLocator)
                {
                    var stack = GetCount(sender);
                    if (stack > 0)
                    {
                        args.baseAttackSpeedAdd += (0.2f + 0.1f * (stack - 1)) * sender.skillLocator.secondary.maxStock;
                    }
                }
            }
        }
    }
}