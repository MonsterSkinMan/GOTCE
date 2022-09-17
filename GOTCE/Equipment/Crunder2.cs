/*using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Equipment
{
    public class Crunder2 : EquipmentBase<Crunder2>
    {
        public override string EquipmentName => "The Crowdfunder 2";

        public override string EquipmentLangTokenName => "GOTCE_LunarGat";

        public override string EquipmentPickupDesc => "Toggle to fire at a ludicrous rate... <color=#FF7F7F>BUT it costs Lunar Coins to fire.</color>\n";

        public override string EquipmentFullDescription => "Fires a continuous onslaught that deals <style=cIsDamage>5000% damage per bullet</style>, each with a 50.0 proc coefficient. Costs 1 Lunar Coin per bullet. Cost doesn't increase over time.";

        public override string EquipmentLore => "This- this- this stupid fuck is <i>COMPLETELY</i> wrong about everything! I don't understand how wrong one can be at one time, over one thing. Brother- brother, you seeing this shit? You seeing it? You have a phone, right, brother? He thinks it's <i>bad</i>. I have no clue why.  Sure, the bullets are weak, but they fire at an extraordinary speed, and speed is war. Sure, it may cost gold to fire, but it also has no cooldown, and the economy is fucked anyways.\nYou know what? Fine. Fuck him. I'll make one myself. They can use my coins instead of gold; everyone cheats them in anyway. He can't possibly think it's weak after this. I'll show him.";

        public override GameObject EquipmentModel => null;

        public override Sprite EquipmentIcon => null;

        public override float Cooldown => 0;

        public override bool EnigmaCompatible => false;

        public override bool IsLunar => true;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateEquipment();
            Hooks();
        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            throw new NotImplementedException();
        }
    }
}
*/