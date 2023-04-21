using System;
using UnityEngine;

namespace GOTCE.EntityStatesCustom.Jerone {
    public class Berserk : BaseSkillState {
        public float duration = 5f;
        public override void OnEnter()
        {
            base.OnEnter();
            base.characterBody.AddTimedBuff(RoR2Content.Buffs.WarCryBuff, duration);
            AkSoundEngine.PostEvent(Events.Play_teamWarCry_activate, base.gameObject);
            outer.SetNextStateToMain();
        }
    }
}