using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;

namespace GOTCE.Items.Lunar
{
    //TODO:

    // cache movement keybinds at the start of every run, and set them in fixedupdate to prevent cheese

    public class MemoriesOfMisery : ItemBase<MemoriesOfMisery>
    {
        public override string ConfigName => "Memories of Misery";

        public override string ItemName => "Memories of Misery";

        public override string ItemLangTokenName => "GOTCE_MemoriesOfMisery";

        public override string ItemPickupDesc => "Increase your move speed... <color=#FF7F7F>BUT invert your controls.</color>\n";

        public override string ItemFullDescription => "Increase your <style=cIsUtility>move speed</style> by <style=cIsUtility>50%</style> <style=cStack>(+30% per stack)</style>. Invert your controls. Reduce <style=cIsHealing>maximum health</style> by <style=cIsHealing>0%</style> <style=cStack>(+15% per stack)</style>.";

        public override string ItemLore => "Look out for yourself\r\nI wake up to the sounds of the silence that allows\r\nFor my mind to run around with my ear up to the ground\r\nI'm searching to behold the stories that are told\r\nWhen my back is to the world that was smiling when I turned\r\nTell you, you're the greatest\r\nBut once you turn, they hate us\r\nOh, the misery\r\nEverybody wants to be my enemy\r\nSpare the sympathy\r\nEverybody wants to be my enemy\r\nLook out for yourself\r\nMy enemy\r\nLook out for yourself\r\nBut I'm ready\r\nYour words up on the wall as you're praying for my fall\r\nAnd the laughter in the halls\r\nAnd the names that I've been called\r\nI stack it in my mind and I'm waiting for the time\r\nWhen I show you what it's like to be words spit in a mic\r\nTell you, you're the greatest\r\nBut once you turn, they hate us (ha)\r\nOh, the misery\r\nEverybody wants to be my enemy\r\nSpare the sympathy\r\nEverybody wants to be my enemy\r\nLook out for yourself\r\nMy enemy (yeah)\r\nLook out for yourself\r\nUh, look, okay\r\nI'm hoping that somebody pray for me\r\nI'm praying that somebody hope for me\r\nI'm staying where nobody 'posed to be\r\nP-p-posted, being a wreck of emotions\r\nReady to go whenever, just let me know\r\nThe road is long, so put the pedal into the floor\r\nThe enemy on my trail, my energy unavailable\r\nI'ma tell 'em, \"Hasta luego\"\r\nThey wanna plot on my trot to the top\r\nI've been outta shape, thinkin' out the box\r\nI'm an astronaut, I blasted off the planet rock\r\nTo cause catastrophe and it matters more because I had it\r\nAnd I had a thought about wreaking havoc on an opposition, kinda shockin'\r\nThey want a static with precision, I'm automatic quarterback\r\nI ain't talking sacking pack it, pack it up, I don't panic, batter-batter up\r\nWho the baddest? It don't matter 'cause we at ya throat\r\nEverybody wants to be my enemy\r\nSpare the sympathy\r\nEverybody wants to be my enemy\r\nOh, the misery\r\nEverybody wants to be my enemy\r\nSpare the sympathy\r\nEverybody wants to be my enemy (I swear)\r\nPray it away, I swear, I never be a saint, no way\r\nMy enemy\r\nPray it away, I swear, I never be a saint\r\nLook out for yourself\r\n";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/memoriesofmisery.png");

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
            On.RoR2.PlayerCharacterMasterController.FixedUpdate += PlayerCharacterMasterController_FixedUpdate;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    args.moveSpeedMultAdd += 0.5f + 0.3f * (stack - 1);
                    args.healthMultAdd += 0f - 0.15f * (stack - 1);
                }
            }
        }

        private void PlayerCharacterMasterController_FixedUpdate(On.RoR2.PlayerCharacterMasterController.orig_FixedUpdate orig, PlayerCharacterMasterController self)
        {
            var body = self.gameObject.GetComponent<CharacterMaster>().GetBody();
            if (body && body.inventory && body.inventory.GetItemCount(Instance.ItemDef) > 0)
            {
                self.bodyInputs.moveVector = -self.bodyInputs.moveVector;
            }
            orig(self);
        }
    }
}