/*using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using System;
using R2API;

namespace GOTCE.Items.Green
{
    public class UnseasonedPatty : ItemBase<UnseasonedPatty>
    {
        public override string ConfigName => "Unseasoned Patty";

        public override string ItemName => "Unseasoned Patty";

        public override string ItemLangTokenName => "GOTCE_UnseasonedPatty";

        public override string ItemPickupDesc => "Gain stacks of seasoning on kill if you meet very specific conditions that spawn ethereal bubbles that give immunity to fruiting (scales with attack speed)";

        public override string ItemFullDescription => "Upon killing an enemy, if you have no stacks of Seasoning, if the enemy was on fire and not a Blazing elite, or if the enemy was a Glacial elite with at most 2 debuffs, or if the enemy had at least 400% (-50% per stack, hyperbolic) more max HP than you, or if the enemy was within 20m (+4m per stack) of the teleporter and within 2m of a boss with at most 50% (+25% per stack, hyperbolic) health, while you have at least 5 (-1 per stack) other green items in your inventory, excluding Regenerating Scrap, Fuel Cells, Bandoliers, Predatory Instincts, unless there are at least 3 Predatory Instincts in your inventory, and Ghor's Tomes, gain 10 (-10% per stack, hyperbolic) stacks of Seasoning, and cause the enemy to drop a spherical Ethereal Bubble with a base volume of 14,137m³ (+6,442m³ per stack). All enemies standing within the Ethereal Bubble will receive 1 stack of Seasoning every 0.00555556 minutes, unless the enemies have more than 23 stacks of Seasoning. Each stack of Seasoning active on the stage increases the base volume of the Ethereal Bubble by 200m³ per stack. Killing enemies within the bubble grants you an instant 20 stacks of Seasoning, unless the enemy's health decreases from 100% to 0% in under 1.3 seconds (-10% per stack, hyperbolic.) All allies standing within the Ethereal Bubble will lose 1 stack of Seasoning every 0.000277778 seconds, and will receive +3% attack speed. Activating a Shrine of Combat, as long as you interact with no other objects afterward, will cause the next barrel you open to have exactly 75 (does not scale) gold coins (scales with attack speed). Increases max HP by 5 (+2 per stack, scales with luck unless your equipment is Remote Caffeinator or Ifrit's Distinction). Grants immunity to Fruiting (scales with attack speed).";

        public override string ItemLore => "why did i think this was a good idea";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.AIBlacklist, ItemTag.Damage, GOTCETags.Bullshit };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => null;

        public override void Init(ConfigFile config)
        {
            base.Init(config);
            Seasoning.Awake();
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            On.RoR2.GlobalEventManager.OnCharacterDeath += Kill;
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += BlockFruiting;
            RecalculateStatsAPI.GetStatCoefficients += (body, args) => {
                if (NetworkServer.active && GetCount(body) > 0) {
                    bool cantScale = body.equipmentSlot.equipmentIndex == RoR2Content.Equipment.AffixRed.equipmentIndex || body.equipmentSlot.equipmentIndex == DLC1Content.Equipment.VendingMachine.equipmentIndex;
                    float increase = 5 + (2 * (GetCount(body) - 1));
                    if (!cantScale) {
                        increase *= body.master.luck;
                    }
                    args.baseHealthAdd += increase;
                }
            };
            // On.RoR2.Interactor.PerformInteraction += scaleswithattackspeed;
        }

        public void scaleswithattackspeed(On.RoR2.Interactor.orig_PerformInteraction orig, Interactor self, UnityEngine.GameObject interactableObject) {
            // todo: write this
        }

        // TOOD: network this it probably doesnt work in multiplayer

        public void Kill(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport report) {
            if (report.victimBody && report.attackerBody && NetworkServer.active && GetCount(report.attackerBody) > 0 && report.attackerBody.isPlayerControlled) {
                // total greens
                int total = 0;
                foreach(ItemIndex item in report.attackerBody.inventory.itemAcquisitionOrder) {
                    if (ItemCatalog.GetItemDef(item).tier == ItemTier.Tier2 || ItemCatalog.GetItemDef(item).deprecatedTier == ItemTier.Tier2) {
                        if (GetCountSpecific(report.attackerBody, RoR2Content.Items.AttackSpeedOnCrit) >= 3) {
                            total += GetCountSpecific(report.attackerBody, ItemCatalog.GetItemDef(item));
                        }
                        else if (item != RoR2Content.Items.Bandolier.itemIndex && item != RoR2Content.Items.EquipmentMagazine.itemIndex && item != DLC1Content.Items.RegeneratingScrap.itemIndex) {
                            total += GetCountSpecific(report.attackerBody, ItemCatalog.GetItemDef(item));
                        }
                    }
                }

                // total ghors tomes
                int totalTomes = GetCountSpecific(report.attackerBody, RoR2Content.Items.BonusGoldPackOnKill);
                
                // bools
                bool wasOnFire = report.victimBody.HasBuff(RoR2Content.Buffs.OnFire) && !report.victimBody.HasBuff(RoR2Content.Buffs.AffixRed);
                bool glacialWithDebuffs = report.victimBody.HasBuff(RoR2Content.Buffs.AffixBlue) && report.victimBody.buffs.Length >= 2;
                bool moreHealth = report.victimBody.maxHealth > (report.attackerBody.maxHealth * (4 / GetCount(report.attackerBody)));
                bool closeToTele = Vector3.Distance(report.victimBody.transform.position, TeleporterInteraction.instance.transform.position) <= (20 + (4 * (GetCount(report.victimBody) - 1)));
                bool noSeasoning = !report.attackerBody.HasBuff(Seasoning.buff);
                bool withinBoss = false;

                /* Debug.Log("wasOnFire: " + wasOnFire);
                Debug.Log("glacialWithDebuffs: " + glacialWithDebuffs);
                Debug.Log("moreHealth: " + moreHealth);
                Debug.Log("closeToTele: " + closeToTele);
                Debug.Log("noSeasoning: " + noSeasoning);
                Debug.Log("totalGreens: " + total);
                Debug.Log("totalTomes: " + totalTomes); */

            /*    SphereSearch search = new();
                List<HurtBox> hurtboxBuffer = new();
                search.radius = 2;
                search.origin = report.victimBody.corePosition;
                search.mask = LayerIndex.entityPrecise.mask;
                search.RefreshCandidates();
                search.FilterCandidatesByDistinctHurtBoxEntities();
                search.OrderCandidatesByDistance();
                search.GetHurtBoxes(hurtboxBuffer);
                search.ClearCandidates();

                foreach (HurtBox box in hurtboxBuffer) {
                    if (box && box.healthComponent && box.healthComponent.body && box.healthComponent.body.isChampion) {
                        if (box.healthComponent.health >= (0.5 * box.healthComponent.body.maxHealth)) {
                            withinBoss = true;
                        }
                    }
                }

                // ints
                int reqGreens = 5 - (1 * (GetCount(report.attackerBody) - 1));
                int reqTomes = 5;

                if (total >= reqGreens && totalTomes >= reqTomes) {
                    if (wasOnFire || glacialWithDebuffs || moreHealth || (withinBoss && closeToTele) && noSeasoning) {
                        int stacks = Mathf.CeilToInt(Utils.MathHelpers.InverseHyperbolicScaling(10, 1, 1, GetCount(report.attackerBody)));
                        float radius = (14.137f + (6.442f * (GetCount(report.attackerBody) - 1))) * 0.5f;
                        // float radius = 5;

                        // guh why doesnt addbuff have a stacks param why do i have to do this
                        for (int i = 0; i < stacks; i++) {
                            report.attackerBody.AddBuff(Seasoning.buff);
                        }
                    
                        // do the bubble thing
                        GameObject bubble = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        bubble.GetComponent<SphereCollider>().isTrigger = true; // trigger so it doesnt have actual collision
                        bubble.AddComponent<BubbleController>();
                        bubble.GetComponent<BubbleController>().component = report.attackerBody.masterObject.GetComponent<Components.GOTCE_StatsComponent>();
                        bubble.transform.position = report.victimBody.corePosition;
                        bubble.GetComponentInChildren<MeshRenderer>().material = Main.SecondaryAssets.LoadAsset<Material>("Assets/Materials/matBubble.mat");
                        // bubble.transform.localScale = new Vector3(radius, radius, radius); // .normalized
                        bubble.layer = LayerIndex.projectile.intVal;
                        bubble.GetComponent<BubbleController>().radiusBase = radius;
                        Debug.Log(bubble.transform.position);
                        report.attackerBody.masterObject.GetComponent<Components.GOTCE_StatsComponent>().bubbles.Add(bubble);
                    }
                }

                // died within bubble 
                // TODO: check if enemy died within 1.3 seconds
                Components.GOTCE_StatsComponent stats = report.attackerBody.masterObject.GetComponent<Components.GOTCE_StatsComponent>();
                foreach (GameObject bubble in stats.bubbles) {
                    if (Vector3.Distance(report.victimBody.transform.position, bubble.transform.position) <= bubble.GetComponent<BubbleController>().radius) {
                        for (int i = 0; i < 20; i++) {
                            report.attackerBody.AddBuff(Seasoning.buff);
                        }
                        break;
                    }
                }
                
            }
            orig(self, report);
        }

        public void BlockFruiting(On.RoR2.CharacterBody.orig_AddTimedBuff_BuffDef_float orig, CharacterBody self, BuffDef buff, float duration) {
            if (self.inventory) {
                if (buff == RoR2Content.Buffs.Fruiting && GetCountSpecific(self, ItemDef) > 0) {
                    float chance = self.attackSpeed * 0.1f;
                    if (Util.CheckRoll(chance, self.master)) {
                        duration = 0f;
                    }
                } 
            }
            orig(self, buff, duration);
        }
    }

    public class Seasoning {
        public static BuffDef buff;

        public static void Awake() {
            buff = ScriptableObject.CreateInstance<BuffDef>();
            buff.canStack = true;
            buff.isDebuff = false;
            buff.name = "Seasoning";
            buff.isHidden = false;

            R2API.ContentAddition.AddBuffDef(buff);
        }
    }

    /*public class BubbleController : MonoBehaviour {
        float stopwatch = 0f;
        float lifetime = 1000f;

        float stopwatchSearch = 0f;
        float stopwatchDelay = 1f;

        float seasoningDelayEnemies = 0.3333336f;
        float seasoningCleanseDelay = 0.000277778f;

        float stopwatchEnemies = 0f;
        float stopwatchClenase = 0f;
        public float radiusBase;
        public float radiusInc;
        public float radius;

        List<HurtBox> hurtboxBuffer;
        SphereSearch search;
        public Components.GOTCE_StatsComponent component;

        public void Start() {
            search = new();
            hurtboxBuffer = new();
        }
        public void FixedUpdate() { // TODO: this partially works
            if (radius != null) {
                radius = radiusBase + radiusInc;
                // float vel = 6;
                // float num = Mathf.SmoothDamp(gameObject.transform.localScale.x, radius * 2, ref vel, 0.6f);
                gameObject.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
                // gameObject.transform.localScale = new Vector3(num, num, num);
            }
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= lifetime) {
                Destroy(gameObject);
            }

            stopwatchSearch += Time.fixedDeltaTime;
            if (stopwatchSearch >= stopwatchDelay) {
                hurtboxBuffer.Clear();
                stopwatchSearch = 0;
                search.radius = radiusBase + radiusInc;
                search.origin = gameObject.transform.position;
                search.mask = LayerIndex.entityPrecise.mask;
                search.RefreshCandidates();
                search.FilterCandidatesByDistinctHurtBoxEntities();
                search.OrderCandidatesByDistance();
                search.GetHurtBoxes(hurtboxBuffer);
                search.ClearCandidates();

                radiusInc = 0;
                // TODO: this doesn't work
                /* foreach (TeamComponent com in TeamComponent.GetTeamMembers(TeamIndex.Monster)) {
                    if (com.body) {
                        radiusInc += 0.2f * com.body.GetBuffCount(Seasoning.buff);
                    }
                }
                foreach (TeamComponent com in TeamComponent.GetTeamMembers(TeamIndex.Player)) {
                    if (com.body) {
                        radiusInc += 0.2f * com.body.GetBuffCount(Seasoning.buff);
                    }
                }
                foreach (TeamComponent com in TeamComponent.GetTeamMembers(TeamIndex.Void)) {
                    if (com.body) {
                        radiusInc += 0.2f * com.body.GetBuffCount(Seasoning.buff);
                    }
                } */
         /*   }

           /* stopwatchClenase += Time.fixedDeltaTime;
            if (stopwatchClenase >= seasoningCleanseDelay) {
                stopwatchClenase = 0f;
                foreach (HurtBox box in hurtboxBuffer) {
                    if (box && box.teamIndex == TeamIndex.Player) {
                        box.healthComponent.body.SetBuffCount(Seasoning.buff.buffIndex, box.healthComponent.body.GetBuffCount(Seasoning.buff) - 1);
                        if (box.healthComponent.body.masterObject && box.healthComponent.body.masterObject.GetComponent<Components.GOTCE_StatsComponent>()) {
                            box.healthComponent.body.masterObject.GetComponent<Components.GOTCE_StatsComponent>().withinBubble = true;
                            Debug.Log("Distance from body to bubble: " + Vector3.Distance(box.healthComponent.transform.position, gameObject.transform.position));
                        }
                    }
                }
            }

            stopwatchEnemies += Time.fixedDeltaTime;
            if (stopwatchEnemies >= seasoningDelayEnemies) {
                stopwatchEnemies = 0f;
                foreach (HurtBox box in hurtboxBuffer) {
                    if (box && box.teamIndex == TeamIndex.Monster) {
                        box.healthComponent.body.AddBuff(Seasoning.buff);
                    }
                }
            }
        }
        public void OnDestroy() {
            // check for component here in the case that the player dies before a bubble expires
            if (component) {
                component.bubbles.Remove(gameObject);
            }
        }
    }
} */