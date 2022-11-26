using RoR2;
using RoR2.Achievements;
using System;
using Unity;
using UnityEngine;

namespace GOTCE.Achievements.CrackedCommando
{
    [RegisterAchievement("CONSISTENCY", "SuppressiveBarrageUnlockable", null, typeof(Server))]
    public class SuppressiveBarrage : BaseAchievement
    {
        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            SetServerTracked(true);
        }

        public override void OnBodyRequirementBroken()
        {
            base.OnBodyRequirementBroken();
            SetServerTracked(false);
        }

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(Survivors.CrackedMando.Instance.prefab);
        }

        private class Server : BaseServerAchievement
        {
            public int required = 25; // 55.6% chance

            public override void OnInstall()
            {
                base.OnInstall();
                CharacterBody.onBodyInventoryChangedGlobal += Check;
            }

            public override void OnUninstall()
            {
                base.OnUninstall();
                CharacterBody.onBodyInventoryChangedGlobal -= Check;
            }

            public void Check(CharacterBody body)
            {
                CharacterBody current = serverAchievementTracker.networkUser.GetCurrentBody();
                if (current && current.inventory && current.inventory.GetItemCount(RoR2Content.Items.StunChanceOnHit) >= required)
                {
                    Grant();
                }
            }
        }
    }

    public class SuppressiveBarrageUnlock : AchievementBase<SuppressiveBarrageUnlock>
    {
        public override string Name => "Cracked Commando: Consistency";
        public override string Description => "As Cracked Commando, reach 55.6% stun chance";
        public override string UnlockName => "SuppressiveBarrageUnlockable";
        public override string TokenName => "CONSISTENCY";
        public override Sprite Icon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/NEA.png");
    }
}