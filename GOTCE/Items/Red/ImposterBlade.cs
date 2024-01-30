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

        public override string ItemLore => "Incident #493 of the UES Safe Travels: Tharson, Furthen, and Section 005 of Scout Team\n\nAfter Tharson and Furthen's vital signs went dark, we sent a scout team to attempt to gather their bodies and research. We could not risk their losses being in vain; I promised myself that. The scout team found their way to Rallypoint Charlie, where they found a small group of survivors and... Tharson and Furthen. The team reported that upon inspecting Tharson and Furthen, it was just their transmitters that were damaged; they were back online after a few minutes of contact. The scout team requested to take some time on the planet to rest. This... was a mistake.\n\nThe team woke up to one of the survivors, dead, a knife lodged into their back. There was a murderer among us. Nobody was awake to see it. Even worse, the crew still had to prepare the Rallypoint for travel and eventual disposal; we can't have another ship pick up on its signal and come here to uncharted space. I requested they continue their work and to keep a close eye on each other, and that I would attempt to deduce who the killer was\n\nBig mistake. The bodies kept dropping, some with knife punctures, some with brutal punches, one even seemingly had the wildlife breach containment and kill them. Near the end, there were five left from the initial ten people: Tharson, Furthen, Liam, Brian, and Senna. I realized that Tharson, Furthen, and Senna were the remaining survivors whilst Brian and Liam were from our expedition crew. I decided to keep a close eye on Senna, and realized they only had a final task to do: decommission the power source, a geyser power plant. They dismissed, and ran to the power plant. Senna and Furthen went in to shut down the geyser, while Tharson stayed behind with Brian and Liam. Then it cracked. While Senna was waiting, I noticed Tharson charging straight at her. I screamed at her to get out of the way, and Tharson slammed into the electric wires, decommissioning him. I asked Senna to remove his helmet. It wasn't Tharson. It was... a monstrosity. A yellow, cat-like creature with thousands of teeth and no eyes, but a human-shaped mouth.\n\nSenna ran out, screaming at them that they found the murderer. They all ran in, and threw Tharson into the lava below. The impostor was dealt with... at least, that is what we thought, because when Liam turned up with a knife in him, we realized another impostor was still among us. Furthen inspected the body, and pocketed a note. Brian pointed it out, and had to wrestle the note from Furthen: \"F\" was all that was on it. Furthen then pulled out his knife, and stabbed Brian several times. Senna ran, and ran she did, and Furthen chased. As he did, whatever creature Furthen was revealed itself, losing the outfit, and letting out a sound I could only describe as... terrorizing. After exhausting the creature, Senna stole his knife, and killed the creature. She picked up Brian, and continued on. She never came back to investigate the bodies, but I don't blame her. The only thing from that place she took was the knife as it still had some of \"Furthen's\" DNA on it, and Brian, who thankfully survived the incident.\n\nIncident report: 6 dead, 2... dead again? I'm going down there from now on to ensure deaths like this do not repeat.";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/ImpostersBlade.png");

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