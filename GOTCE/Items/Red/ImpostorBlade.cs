using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using HarmonyLib;

namespace GOTCE.Items.Red
{
    public class ImpostorBlade : ItemBase<ImpostorBlade>
    {
        public override string ItemName => "Impostor's Blade";

        public override string ConfigName => "Impostors Blade";

        public override string ItemLangTokenName => "GOTCE_ImpostorBlade";

        public override string ItemPickupDesc => "Teleport forward after killing an elite.";

        public override string ItemFullDescription => "On elite kill, teleport forward 10 (+5 per stack) meters.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            GlobalEventManager.onCharacterDeathGlobal += TP;
        }

        public void TP(DamageReport report) {
            if (NetworkServer.active && report.attackerBody && report.victimIsElite) {
                int c = GetCount(report.attackerBody);

                if (c > 0) {
                    float dist = 10 + (5 * (c - 1));
                    Vector3 pos = report.attackerBody.corePosition + (report.attackerBody.inputBank.GetAimRay().direction * dist);
                    EffectManager.SpawnEffect(Utils.Paths.GameObject.ParentTeleportEffect.Load<GameObject>(), new EffectData {
                        origin = pos,
                        scale = 1f
                    }, true);
                    TeleportHelper.TeleportBody(report.attackerBody, pos);
                    AkSoundEngine.PostEvent(Events.Play_parent_teleport, report.attacker);
                }
            }
        }
    }
}