using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace GOTCE.Items.Lunar
{
    public class CrownPrince : ItemBase<CrownPrince>
    {
        public override string ConfigName => "Crown Prince";

        public override string ItemFullDescription => "<style=cIsUtility>Cheat death</style>. Taking ANY damage instantly kills you. <style=cIsUtility>Breaks</style> after <style=cIsUtility>50</style> <style=cStack>(+50 per stack)</style> deaths. <style=cIsUtility>0%</style> <style=cStack>(+5% per stack)</style> chance for a death to be a true kill.";

        public override string ItemLore => "Hell is not bad.\nHell is not bad.\nHell is not bad.\nHell is not bad.\nHell is not bad.\nHell is not bad.\nHell is not bad.\nHell is not bad.\nHell is not bad.\nHell is not bad.\nHell is not bad.\nHell is not bad.\nHell is not bad.\nHell is not bad.\nHell is not bad.\nHell is not bad.\nHell is not bad.\nHell is not bad.\nHell is not bad.\nHell is not bad.";

        public override ItemTier Tier => ItemTier.Lunar;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Healing, ItemTag.AIBlacklist, ItemTag.BrotherBlacklist, GOTCETags.Masochist };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/CrownPrince.png");

        public override string ItemLangTokenName => "GOTCE_CrownPrince";

        public override string ItemName => "Crown Prince";

        public override string ItemPickupDesc => "Cheat death... <color=#FF7F7F>BUT you die in a single hit.</color> Breaks after 50 deaths.";

        public void AddUses(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex def, int count)
        {
            /* Main.ModLogger.LogDebug("--------------" + self.gameObject.ToString() + "-----------------");
            Main.ModLogger.LogDebug("Stats: " + (bool)self.gameObject.GetComponent<Components.GOTCE_StatsComponent>());
            Main.ModLogger.LogDebug("Def: " + (def == ItemDef.itemIndex));
            Main.ModLogger.LogDebug("NetworkServer: " + NetworkServer.active); */
            if (self.gameObject.GetComponent<Components.GOTCE_StatsComponent>() && def == ItemDef.itemIndex && NetworkServer.active)
            {
                Components.GOTCE_StatsComponent stats = self.gameObject.GetComponent<Components.GOTCE_StatsComponent>();
                stats.crownPrinceUses += 50 * count;
                //Debug.Log(stats.crownPrinceUses);
                stats.crownPrinceTrueKillChance += 5f * (count - 1);
            }
            orig(self, def, count);
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            // On.RoR2.GlobalEventManager.OnPlayerCharacterDeath += Revive;
            On.RoR2.Inventory.GiveItem_ItemIndex_int += AddUses;
            On.RoR2.Inventory.RemoveItem_ItemIndex_int += RemoveUses;
            On.RoR2.HealthComponent.TakeDamage += Instakill;
            On.RoR2.CharacterMaster.OnBodyDeath += Revive;
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public void Instakill(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo info)
        {
            orig(self, info);
            if (self.body && NetworkServer.active)
            {
                // CharacterBody attacker = info.attacker.GetComponent<CharacterBody>();
                if (self.body.inventory && self.body.inventory.GetItemCount(ItemDef) > 0)
                {
                    self.Suicide();
                }
            }
        }

        public void RemoveUses(On.RoR2.Inventory.orig_RemoveItem_ItemIndex_int orig, Inventory self, ItemIndex def, int count)
        {
            /* Main.ModLogger.LogDebug("--------------" + self.gameObject.ToString() + "-----------------");
            Main.ModLogger.LogDebug("Stats: " + (bool)self.gameObject.GetComponent<Components.GOTCE_StatsComponent>());
            Main.ModLogger.LogDebug("Def: " + (def == ItemDef.itemIndex));
            Main.ModLogger.LogDebug("NetworkServer: " + NetworkServer.active); */
            if (self.gameObject.GetComponent<Components.GOTCE_StatsComponent>() && def == ItemDef.itemIndex && NetworkServer.active)
            {
                Components.GOTCE_StatsComponent stats = self.gameObject.GetComponent<Components.GOTCE_StatsComponent>();
                stats.crownPrinceUses -= 50 * count;
                // Debug.Log(stats.crownPrinceUses);
                stats.crownPrinceTrueKillChance -= 5f * (count - 1);
                if (stats.crownPrinceUses < 0) stats.crownPrinceUses = 0;
            }
            orig(self, def, count);
        }

        /*public void Revive(On.RoR2.GlobalEventManager.orig_OnPlayerCharacterDeath orig, GlobalEventManager self, DamageReport report, NetworkUser netuser) {
            if (report.victim && report.victimBody && NetworkServer.active) {
                Components.GOTCE_StatsComponent stats = report.victimMaster.GetComponent<Components.GOTCE_StatsComponent>();
                if (stats.crownPrinceUses > 0) {
                    stats.crownPrinceUses--;
                    report.victimMaster.RespawnExtraLife();
                    if (Util.CheckRoll(stats.crownPrinceTrueKillChance, 0)) {
                        report.victimMaster.TrueKill();
                    }
                    if (stats.crownPrinceUses <= 0) {
                        int countp = report.victimMaster.inventory.GetItemCount(ItemDef);
                        report.victimMaster.inventory.RemoveItem(ItemDef, countp);
                    }
                }
            }
            orig(self, report, netuser);
        } */

        public void Revive(On.RoR2.CharacterMaster.orig_OnBodyDeath orig, CharacterMaster self, CharacterBody body)
        {
            orig(self, body);
            if (NetworkServer.active)
            {
                if (self.GetComponent<Components.GOTCE_StatsComponent>())
                {
                    Components.GOTCE_StatsComponent stats = self.GetComponent<Components.GOTCE_StatsComponent>();
                    if (stats.crownPrinceUses <= 0)
                    {
                        self.inventory.RemoveItem(ItemDef, self.inventory.GetItemCount(ItemDef));
                        self.inventory.GiveItem(NoTier.CrownPrinceEXE.Instance.ItemDef, self.inventory.GetItemCount(ItemDef));
                    }
                    if (stats.crownPrinceUses > 0)
                    {
                        if (!Util.CheckRoll(stats.crownPrinceTrueKillChance, 0))
                        {
                            stats.crownPrinceUses--;
                            self.preventGameOver = true;
                            stats.Invoke(nameof(stats.RespawnExtraLife), 1f);
                            // Debug.Log(stats.crownPrinceTrueKillChance);
                        }
                    }
                }
            }
        }
    }
}