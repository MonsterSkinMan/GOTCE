using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using HarmonyLib;

namespace GOTCE.Items.Red
{
    public class ImposterBlade : ItemBase<ImposterBlade>
    {
        public override string ItemName => "Imposter's Blade";

        public override string ConfigName => "Imposters Blade";

        public override string ItemLangTokenName => "GOTCE_ImposterBlade";

        public override string ItemPickupDesc => "Teleport forward after killing an elite.";

        public override string ItemFullDescription => "On elite kill, teleport forward 10 (+5 per stack) meters.";

        public override string ItemLore => "Please repost\nThere's an imposter\nEverywhere you go\nPlease repost\nLooking the most\nAmogous sussous\nPlease repost\nThere's an imposter\nNowhere to report\nCould have lost the game that they host\nThey're in you and they're in me";

        public override ItemTier Tier => ItemTier.NoTier;

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