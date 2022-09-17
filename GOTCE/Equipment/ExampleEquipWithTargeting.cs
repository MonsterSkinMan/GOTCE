using BepInEx.Configuration;
using R2API;
using RoR2;
using System.Linq;
using UnityEngine;
using static GOTCE.Main;

namespace GOTCE.Equipment
{
    class ExampleEquipWithTargeting : EquipmentBase<ExampleEquipWithTargeting>
    {
        public override string EquipmentName => "Deprecate Me Equipment Targeting Edition";

        public override string EquipmentLangTokenName => "DEPRECATE_ME_EQUIPMENT_TARGETING";

        public override string EquipmentPickupDesc => "";

        public override string EquipmentFullDescription => "";

        public override string EquipmentLore => "";

        public override GameObject EquipmentModel => MainAssets.LoadAsset<GameObject>("ExampleEquipmentPrefab.prefab");

        public override Sprite EquipmentIcon => MainAssets.LoadAsset<Sprite>("ExampleEquipmentIcon.png");

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateTargetingIndicator();
            CreateEquipment();
            Hooks();
        }

        protected override void CreateConfig(ConfigFile config)
        {

        }

        /// <summary>
        /// An example targeting indicator implementation. We clone the woodsprite's indicator, but we edit it to our liking. We'll use this in our activate equipment.
        /// We shouldn't need to network this as this only shows for the player with the Equipment.
        /// </summary>
        private void CreateTargetingIndicator()
        {
            TargetingIndicatorPrefabBase = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/WoodSpriteIndicator"), "ExampleIndicator", false);
            TargetingIndicatorPrefabBase.GetComponentInChildren<SpriteRenderer>().sprite = MainAssets.LoadAsset<Sprite>("ExampleReticuleIcon.png");
            TargetingIndicatorPrefabBase.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            TargetingIndicatorPrefabBase.GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.identity;
            TargetingIndicatorPrefabBase.GetComponentInChildren<TMPro.TextMeshPro>().color = new Color(0.423f, 1, 0.749f);
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            //We check for the characterbody, and if that has an inputbank that we'll be getting our aimray from. If we don't have it, we don't continue.
            if (!slot.characterBody || !slot.characterBody.inputBank) { return false; }

            //Check for our targeting controller that we attach to the object if we have "Use Targeting" enabled.
            var targetComponent = slot.GetComponent<TargetingControllerComponent>();

            //Ensure we have a target component, and that component is reporting that we have an object targeted.
            if (targetComponent && targetComponent.TargetObject)
            {
                var chosenHurtbox = targetComponent.TargetFinder.GetResults().First();

                //Here we would use said hurtbox for something. Could be anything from firing a projectile towards it, applying a buff/debuff to it. Anything you can think of.
                if (chosenHurtbox)
                {
                    //stuff
                }

                return true;
            }
            return false;
        }
    }
}
