using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.Navigation;

namespace GOTCE.Items.White
{
    public class SkullKey : ItemBase<SkullKey>
    {
        public override string ConfigName => "Skull Key";

        public override string ItemName => ":Skull: Key";

        public override string ItemLangTokenName => "GOTCE_SkullKey";

        public override string ItemPickupDesc => "'Critical Stage Transitions' give you 5 powerful items. Not consumed on use.";

        public override string ItemFullDescription => "Gain <style=cIsUtility>5%</style> Stage Transition Crit chance. On '<style=cIsUtility>Stage Transition Crit</style>', gain <style=cIsUtility>5</style> <style=cStack>(+5 per stack)</style> items (<style=cIsHealth>50%</style>/<style=cShrine>50%</style>).";

        public override string ItemLore => "Perhaps the crack itself has treasures to be found. So far, its purpose has been as a slingshot along the borders of our reality, but this new artifact is a direct wedge into another dimension, uprooting its treasures and bringing them here. A parallel space carrying boundless treasures- imagine that! Everything I've pulled from within the crack has been incredibly useful; I've gotten rocket launchers, detritivore desk plants, and even a spaceship part. I can't wait to see what all I can gather.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Utility, ItemTag.AIBlacklist, ItemTag.OnStageBeginEffect, GOTCETags.Crit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/skullkey.png");

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
            CriticalTypes.OnStageCrit += MeWhenWithor;
        }

        public void MeWhenWithor(object sender, StageCritEventArgs args)
        {
            if (NetworkServer.active)
            {
                var instances = PlayerCharacterMasterController.instances;
                foreach (PlayerCharacterMasterController playerCharacterMaster in instances)
                {
                    if (playerCharacterMaster.master.inventory.GetItemCount(ItemDef) > 0)
                    {
                        int maxLockboxes = playerCharacterMaster.master.inventory.GetItemCount(ItemDef) * 5;
                        for (int i = 0; i < maxLockboxes; i++)
                        {
                            CharacterMaster master = playerCharacterMaster.master;
                            // NodeGraph nodes = SceneInfo.instance.GetNodeGraph(RoR2.Navigation.MapNodeGroup.GraphType.Ground);
                            // Vector3 pos;
                            // NodeGraph.NodeIndex node = nodes.FindClosestNodeWithFlagConditions(master.GetBody().transform.position += new Vector3(r1, -2, r2), HullClassification.Human, NodeFlags.None, NodeFlags.None, false);
                            // nodes.GetNodePosition(node, out pos);
                            DirectorCore.instance.TrySpawnObject(new DirectorSpawnRequest(Interactables.SkullLockbox.Instance.isc, new DirectorPlacementRule
                            {
                                minDistance = 2f,
                                maxDistance = 5f,
                                placementMode = DirectorPlacementRule.PlacementMode.NearestNode,
                                preventOverhead = false,
                                position = master.GetBody().transform.position,
                            }, Run.instance.treasureRng));
                        }
                    }
                }
            }
        }
    }
}