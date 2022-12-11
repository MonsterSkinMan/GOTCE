using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using BepInEx.Configuration;
using System.Linq;

namespace GOTCE.Items.Red
{
    public class UltraInstinctAegis : ItemBase<UltraInstinctAegis>
    {
        public override string ConfigName => "Ultra Instinct Aegis";

        public override string ItemName => "Ultra Instinct Aegis";

        public override string ItemLangTokenName => "GOTCE_UltraAegis";

        public override string ItemPickupDesc => "Landing a ‘Critical Strike’ shortly after taking heavy damage grants temporary barrier";

        public override string ItemFullDescription => "On '<style=cIsDamage>critical strike</style>', less than <style=cIsUtility>3</style> seconds after taking damage greater than or equal to <style=cIsHealing>85% maximum health</style>, gain a <style=cIsUtility>buff</style> that gives you <style=cIsHealing>50% barrier</style> a second for <style=cIsUtility>5</style> <style=cStack>(+2 per stack)</style> seconds.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override Enum[] ItemTags => new Enum[] { ItemTag.Healing, ItemTag.AIBlacklist, GOTCETags.BarrierRelated };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Items/UltraInstinctAegis.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Init(ConfigFile config)
        {
            base.Init(config);
            TimerBuff.Awake();
        }

        public override void Hooks()
        {
            On.RoR2.HealthComponent.TakeDamage += Timer;
            On.RoR2.GlobalEventManager.OnCrit += Crit;
        }

        public void Timer(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo info)
        {
            orig(self, info);
            if (NetworkServer.active && self.body && self.body.inventory && self.body.inventory.GetItemCount(ItemDef) > 0)
            {
                if (info.damage >= (self.body.maxHealth * 0.85f))
                {
                    self.body.AddTimedBuff(TimerBuff.buff, 3f + 2f * self.body.inventory.GetItemCount(ItemDef));
                }
            }
        }

        public void Crit(On.RoR2.GlobalEventManager.orig_OnCrit orig, GlobalEventManager self, CharacterBody body, DamageInfo info, CharacterMaster master, float procCoefficient, ProcChainMask mask)
        {
            orig(self, body, info, master, procCoefficient, mask);
            if (NetworkServer.active)
            {
                if (body.HasBuff(TimerBuff.buff))
                {
                    body.RemoveBuff(TimerBuff.buff);
                    body.gameObject.AddComponent<Aegis>();
                }
            }
        }
    }

    public class TimerBuff
    {
        public static BuffDef buff;

        public static void Awake()
        {
            buff = ScriptableObject.CreateInstance<BuffDef>();
            buff.canStack = false;
            buff.isDebuff = false;
            buff.name = "guh";
            buff.isHidden = true;

            R2API.ContentAddition.AddBuffDef(buff);
        }
    }

    public class Aegis : MonoBehaviour
    {
        private float stopwatch = 0f;
        private float delay = 1f;
        private int times = 0;

        public void FixedUpdate()
        {
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= delay)
            {
                gameObject.GetComponent<CharacterBody>().healthComponent.AddBarrier(gameObject.GetComponent<CharacterBody>().maxHealth * 0.5f);
                stopwatch = 0f;
                times++;
            }
            if (times >= 5)
            {
                DestroyImmediate(this);
            }
        }
    }
}