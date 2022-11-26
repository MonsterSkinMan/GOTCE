using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using GOTCE.Misc;
using System.IO;
using Unity;
using Unity.Audio;
using UnityEngine.Video;

namespace GOTCE.Items.Yellow
{
    public class Wooler : ItemBase<Wooler>
    {
        public override string ConfigName => "Wool Blanket";

        public override string ItemName => "Wool Blanket";

        public override string ItemLangTokenName => "GOTCE_Wooler";

        public override string ItemPickupDesc => "Your damage scales off the quality of your items...";

        public override string ItemFullDescription => "Gain a <style=cIsUtility>+5%</style> <style=cStack>(+5% per stack)</style> <style=cIsDamage>damage increase</style> for each item <style=cLunarObjective>above B tier</style> you have... BUT <style=cDeath>lose -5%</style> <style=cStack>(-5% per stack)</style> <style=cDeath>damage</style> for each item <style=cLunarObjective>below B tier</style> you have. <style=cHumanObjective>Based off Woolie's all item tier list.</style>";

        public override string ItemLore => Main.SecondaryAssets.LoadAsset<TextAsset>("Assets/Prefabs/WoolerLore.txt").text;

        public override ItemTier Tier => ItemTier.Boss;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Damage, GOTCETags.NonLunarLunar };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

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
            RecalculateStatsAPI.GetStatCoefficients += (body, args) =>
            {
                if (NetworkServer.active)
                {
                    if (GetCount(body) > 0)
                    {
                        float increase = GetCount(body) * 0.05f;

                        float totalInc = 0f;
                        float totalDec = 0f;

                        foreach (ItemIndex index in body.inventory.itemAcquisitionOrder)
                        {
                            if (Woolie.TierMap.TryGetValue(ItemCatalog.GetItemDef(index), out Tier tier))
                            {
                                if ((int)tier >= (int)Misc.Tier.B)
                                {
                                    totalInc += increase * GetCountSpecific(body, ItemCatalog.GetItemDef(index));
                                }
                                else
                                {
                                    totalDec += increase * GetCountSpecific(body, ItemCatalog.GetItemDef(index));
                                }
                                // Debug.Log($"Item {ItemCatalog.GetItemDef(index).nameToken} is {(int)tier} tier");
                            }
                        }

                        args.damageMultAdd += totalDec * (-1);
                        args.damageMultAdd += totalInc;
                    }
                }
            };
            /* GameObject model = ItemDef.pickupModelPrefab;
            VideoPlayer player = model.AddComponent<VideoPlayer>();
            player.url = "https://cdn.discordapp.com/attachments/567832879879553037/1039699109113778176/8mb.video-yzD-Rpct5RSV.mp4";
            player.renderMode = VideoRenderMode.RenderTexture;
            player.targetTexture = (RenderTexture)model.GetComponent<MeshRenderer>().material.mainTexture;
            ItemDef.pickupModelPrefab = model; */
        }
    }
}