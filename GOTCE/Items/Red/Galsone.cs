using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace GOTCE.Items.Red
{
    public class Galsone : ItemBase<Galsone>
    {
        public override string ConfigName => "Galsone";

        public override string ItemName => "Galsone";

        public override string ItemLangTokenName => "GOTCE_Galsone";

        public override string ItemPickupDesc => "I'm galsone!";

        public override string ItemFullDescription => "Killing an enemy <style=cIsDamage>ignites</style> all enemies within <style=cIsDamage>12m</style> <style=cStack>(+4m per stack)</style> for <style=cIsDamage>200%</style> base damage. Additionally, enemies burn for <style=cIsDamage>200%</style> <style=cStack>(+100% per stack)</style> base damage and get <style=cIsDamage>poisoned</style> and <style=cIsDamage>blighted</style> for <style=cIsDamage>2</style> seconds.";

        public override string ItemLore => "<style=cMono>\r\n Audio t-twanscwiption c-compwete fwom signyaw echoes. Assignying genyewic tokens. </style>\r\n\r\n [Fiwe cwackwing] \r\n\r\n MAN 1: D-D-do you t-think t-they'we gonnya come fow us? \r\n\r\n MAN 2: They'ww twy. It's going t-t-t-to be a vewy wong whiwe. \r\n\r\n MAN 1: W-W-What? Why? \r\n\r\n MAN 2: A wong whiwe. Even if they knyow whewe t-to wook we'd be months out fwom the n-nyeawest powt. And that's if they even have a-any ships as f-fast as ouws \u2013 FTW s-ships awe vewy wawe nyowadays. \r\n\r\n MAN 1: Months...?! And w-w-what do you mean if they k-knyow whewe? What a-about the othew ships on o-ouw shipping woutes? \r\n\r\n MAN 2: We wewen't on the woute. \r\n\r\n [Fiwe p-pops] \r\n\r\n MAN 1: W-What?! \r\n\r\n MAN 2: We shouwd've b-been h-hawfway wawfway to P-Pwocyon by the time we c-cwashed... but we wewen \u2019 t. The s-s-ship nyevew annyounced it was swowing down e-eithew, so that \u2019 ww make twianguwating ouw positions even hawdew. \r\n\r\n MAN 1: I-I-I-I don't get it. Who wouwd t-take a UES t-t-twain off couwse? That's compwetewy wompwetewy i-insanye! \r\n\r\n MAN 2: I don \u2019 t k-knyow \u2013 onwy the Captain d-does. Thewe \u2019 s nyo w-weason t-t-to swow down in this staw system - thewe's nyot e-even supposed t-t-to be a habitabwe pwanyet out hewe. \r\n\r\n [Sizzwing] \r\n\r\n MAN 2: This wooks cooked to me. Can't v-vouch fow how i-it'ww taste - but we have to eat. \r\n\r\n MAN 1: I... I can't e-e-even t-think w-wight nyow. I \u2019 m nyot hungwy wungwy. \r\n\r\n MAN 2: Eat. We've g-got a wot of twavewing to do t-tomowwow and we'ww nyeed to keep ouw stwength. \r\n\r\n MAN 1: Suwe. Okay. O-Okay. Um \u2026 do you t-think it's poisonyous? \r\n\r\n MAN 2: Eat. \r\n\r\n <style=cMono>End of wequested twanscwipt. </style> \r\n";
        // owoified gasoline string
        // i couldnt find a good multline to single line tool that didnt add spaces

        //public override string ItemLore => "<style=cMono>\r\nAudio transcription complete from signal echoes. Assigning generic tokens.</style>\r\n\r\n[Fire crackling]\r\n\r\nMAN 1: D-do you think they're gonna come for us?\r\n\r\nMAN 2: They'll try. It's going to be a very long while.\r\n\r\nMAN 1: What? Why?\r\n\r\nMAN 2: A long while. Even if they know where to look we'd be months out from the nearest port. And that's if they even have any ships as fast as ours \u2013 FTL ships are very rare nowadays.\r\n\r\nMAN 1: Months...?! And what do you mean if they know where? What about the other ships on our shipping routes?\r\n\r\nMAN 2: We weren't on the route.\r\n\r\n[Fire pops]\r\n\r\nMAN 1: What?!\r\n\r\nMAN 2: We should've been halfway to Procyon by the time we crashed... but we weren\u2019t. The ship never announced it was slowing down either, so that\u2019ll make triangulating our positions even harder.\r\n\r\nMAN 1: I-I don't get it. Who would take a UES train off course? That's completely insane!\r\n\r\nMAN 2: I don\u2019t know \u2013 only the Captain does. There\u2019s no reason to slow down in this star system - there's not even supposed to be a habitable planet out here.\r\n\r\n[Sizzling]\r\n\r\nMAN 2: This looks cooked to me. Can't vouch for how it'll taste - but we have to eat.\r\n\r\nMAN 1: I... I can't even think right now. I\u2019m not hungry.\r\n\r\nMAN 2: Eat. We've got a lot of traveling to do tomorrow and we'll need to keep our strength.\r\n\r\nMAN 1: Sure. Okay. Okay. Um\u2026 do you think it's poisonous?\r\n\r\nMAN 2: Eat.\r\n\r\n<style=cMono>End of requested transcript. </style>\r\n";
        // white gasoline string
        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, ItemTag.AIBlacklist, ItemTag.OnKillEffect };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/galsone.png");

        private static readonly SphereSearch galsoneSphereSearch = new();
        private static readonly List<HurtBox> galsoneHurtBoxBuffer = new();
        private GameObject explosionVFX;

        public override void Init(ConfigFile config)
        {
            base.Init(config);
            explosionVFX = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/IgniteOnKill/IgniteExplosionVFX.prefab").WaitForCompletion();
            // change particlesystem later, waiting for mystic to send screenshots
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        private void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
        {
            if (damageReport.attackerBody && damageReport.attackerBody.inventory)
            {
                var stack = damageReport.attackerBody.inventory.GetItemCount(Instance.ItemDef);
                if (stack > 0)
                {
                    var attackerTeamIndex = damageReport.attackerBody.teamComponent.teamIndex;

                    var radius = 12f + 4f * (stack - 1);
                    var bodyRadius = damageReport.victimBody.radius;
                    var finalRadius = radius + bodyRadius;

                    var explosionDamage = 2f;
                    float finalDamage = damageReport.attackerBody.damage * explosionDamage;

                    Vector3 corePosition = damageReport.victimBody.corePosition;
                    galsoneSphereSearch.origin = corePosition;
                    galsoneSphereSearch.mask = LayerIndex.entityPrecise.mask;
                    galsoneSphereSearch.radius = finalRadius;
                    galsoneSphereSearch.RefreshCandidates();
                    galsoneSphereSearch.FilterCandidatesByHurtBoxTeam(TeamMask.GetUnprotectedTeams(attackerTeamIndex));
                    galsoneSphereSearch.FilterCandidatesByDistinctHurtBoxEntities();
                    galsoneSphereSearch.OrderCandidatesByDistance();
                    galsoneSphereSearch.GetHurtBoxes(galsoneHurtBoxBuffer);
                    galsoneSphereSearch.ClearCandidates();

                    float num5 = (float)(1 + stack) * 2f * damageReport.attackerBody.damage;
                    for (int i = 0; i < galsoneHurtBoxBuffer.Count; i++)
                    {
                        HurtBox hurtBox = galsoneHurtBoxBuffer[i];

                        if (hurtBox.healthComponent)
                        {
                            InflictDotInfo inflictDotInfo = new()
                            {
                                victimObject = hurtBox.healthComponent.gameObject,
                                attackerObject = damageReport.attacker,
                                totalDamage = new float?(num5),
                                dotIndex = DotController.DotIndex.Burn,
                                damageMultiplier = 1f
                            };

                            InflictDotInfo poisonDot = new()
                            {
                                victimObject = hurtBox.healthComponent.gameObject,
                                attackerObject = damageReport.attacker,
                                dotIndex = DotController.DotIndex.Poison,
                                duration = 5f
                            };

                            InflictDotInfo blightDot = new()
                            {
                                victimObject = hurtBox.healthComponent.gameObject,
                                attackerObject = damageReport.attacker,
                                dotIndex = DotController.DotIndex.Blight,
                                duration = 5f
                            };
                            UnityEngine.Object @object;
                            if (damageReport == null)
                            {
                                @object = null;
                            }
                            else
                            {
                                CharacterMaster attackerMaster = damageReport.attackerMaster;
                                @object = ((attackerMaster != null) ? attackerMaster.inventory : null);
                            }
                            if (@object)
                            {
                                StrengthenBurnUtils.CheckDotForUpgrade(damageReport.attackerMaster.inventory, ref inflictDotInfo);
                            }
                            DotController.InflictDot(ref inflictDotInfo);
                            DotController.InflictDot(ref poisonDot);
                            DotController.InflictDot(ref blightDot);
                        }
                    }

                    galsoneHurtBoxBuffer.Clear();

                    new BlastAttack
                    {
                        radius = finalRadius,
                        baseDamage = finalDamage,
                        procCoefficient = 0f,
                        crit = Util.CheckRoll(damageReport.attackerBody.crit, damageReport.attackerMaster),
                        damageColorIndex = DamageColorIndex.Item,
                        attackerFiltering = AttackerFiltering.Default,
                        falloffModel = BlastAttack.FalloffModel.None,
                        attacker = damageReport.attacker,
                        teamIndex = attackerTeamIndex,
                        position = corePosition
                    }.Fire();

                    EffectManager.SpawnEffect(GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab, new EffectData
                    {
                        origin = corePosition,
                        scale = finalRadius,
                        rotation = Util.QuaternionSafeLookRotation(damageReport.damageInfo.force)
                    }, true);
                }
            }

            orig(self, damageReport);
        }
    }
}