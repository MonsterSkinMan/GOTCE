using BepInEx.Configuration;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOTCE.Equipment
{
    public class BottledMetamorphEquip : EquipmentBase<BottledMetamorphEquip>
    {
        public override string EquipmentName => "Bottled Metamorphosis";

        public override string EquipmentLangTokenName => "GOTCE_BottledMetamorphosis";

        public override string EquipmentPickupDesc => "Transform into another random survivor.";

        public override string EquipmentFullDescription => "Transform into a <style=cIsUtility>random survivor</style> aside from the one you are currently playing as or Heretic.";

        public override string EquipmentLore => "The world inhabited by life is a nonsensical place. Imparting any sort of rules towards nature or general logic on the way the world behaves can only confuse you. The best way to integrate yourself into the animalistic side of our world is to embrace it. Let the chaos of life itself flow around you, rather than being destroyed by its torrential force. Many benefits can be absorbed from the disorder of life.";

        public override GameObject EquipmentModel => null;

        public override Sprite EquipmentIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/BottledMetamorphosis.png");

        public override float Cooldown => 120f;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }
        private static readonly System.Random random = new System.Random();
        private static GameObject heretic;
        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateEquipment();
            Hooks();
            heretic = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Heretic/HereticBody.prefab").WaitForCompletion();
        }
        
        public static GameObject GetRandomSurvivorBodyPrefab()
        {
            List<GameObject> bodies = new();
            foreach (SurvivorDef def in SurvivorCatalog.allSurvivorDefs)
            {
                if (def.bodyPrefab.name != heretic.name)
                {
                    bodies.Add(def.bodyPrefab);
                }
            }
            return bodies[random.Next(0, bodies.Count)];
        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            if (slot.characterBody)
            {
                slot.characterBody.master.bodyPrefab = BottledMetamorphEquip.GetRandomSurvivorBodyPrefab();
                slot.characterBody.master.Respawn(slot.characterBody.master.GetBody().transform.position + new Vector3(0f, 5f, 0f), slot.characterBody.master.GetBody().transform.rotation);
            }
            return true;
        }
    }
}
