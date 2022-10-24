/*using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using BepInEx.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace GOTCE.Items.Red
{
    public class GabesShank : ItemBase<GabesShank>
    {
        public override string ConfigName => "Gabes Shank";

        public override string ItemName => "Gabe's Shank";

        public override string ItemLangTokenName => "GOTCE_GabesShank";

        public override string ItemPickupDesc => "Gain bonus damage for every game you have installed on Steam.";

        public override string ItemFullDescription => "Gain <style=cIsDamage>+2%</style> <style=cStack>(+1% per stack)</style> <style=cIsDamage>damage</style> for <style=cIsUtility>every game you have installed on Steam</style>. <style=cUserSetting>This item only functions if your Steam profile visibility is set to public</style>";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage, ItemTag.Healing, ItemTag.Utility, ItemTag.AIBlacklist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/GabesShank.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += Increase;
            CharacterBody.onBodyStartGlobal += Steam;
        }

        public void Increase(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args) {
            if (NetworkServer.active && body.isPlayerControlled && body.inventory.GetItemCount(ItemDef) > 0) {
                if (body.masterObject && body.masterObject.GetComponent<Components.GOTCE_StatsComponent>()) {
                    Components.GOTCE_StatsComponent stats = body.masterObject.GetComponent<Components.GOTCE_StatsComponent>();
                    args.damageMultAdd += (0.02f + (0.01f*(body.inventory.GetItemCount(ItemDef) - 1)))*stats.game_count;
                }
            }
        }

        public async void Steam(CharacterBody body) {
            if (body.isPlayerControlled) {
                HttpClient http = new();
                string id = TranslateSteamID(Util.LookUpBodyNetworkUser(body).id.steamId.ToSteamID()).ToString();
                string url = $"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=C937967136919706C42E0DCBC8C1DDF6&steamid={id}&format=json&include_played_free_games=true";
                try {
                    using HttpResponseMessage response = await http.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string res = await response.Content.ReadAsStringAsync();
                    var game = JsonConvert.DeserializeObject<GabeJson>(res).response;
                    string game2 = JsonConvert.SerializeObject(game);
                    int game_count = JsonConvert.DeserializeObject<GabeJson2>(game2).game_count;

                    if (body.masterObject && body.masterObject.GetComponent<Components.GOTCE_StatsComponent>()) {
                        body.masterObject.GetComponent<Components.GOTCE_StatsComponent>().game_count = game_count;
                    }
                } catch {
                    Main.ModLogger.LogError("Could not get the game count of the player for Gabe's Shank. Maybe your Steam profile isn't on public visibility?");
                }
            }
        }

        public static Int64 TranslateSteamID(string steamID)
        {
            Int64 result = 0;

            var template = new Regex(@"STEAM_(\d):([0-1]):(\d+)");
            var matches = template.Matches(steamID);
            if (matches.Count <= 0) return 0;
            var parts = matches[0].Groups;
            if (parts.Count != 4) return 0;

            Int64 x = Int64.Parse(parts[1].Value) << 24;
            Int64 y = Int64.Parse(parts[2].Value);
            Int64 z = Int64.Parse(parts[3].Value) << 1;

            result =  ((1 + (1 << 20) + x) << 32) | (y + z);        
            return result;
        }

        
    }

    public class GabeJson {
        public System.Object response {get; set;}
    }

    public class GabeJson2 {
        public int game_count {get; set;}
    }
}*/