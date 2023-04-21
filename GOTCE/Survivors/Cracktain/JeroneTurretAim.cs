using System;
using UnityEngine;

namespace GOTCE.Survivors.Cracktain {
    public class JeroneTurretAim : MonoBehaviour {
        public InputBankTest bank;
        public ModelLocator locator;
        public ChildLocator clocator;
        public Transform turret;

        public void Start() {
            bank = GetComponent<InputBankTest>();
            locator = GetComponent<ModelLocator>();
            clocator = locator.modelTransform.GetComponent<ChildLocator>();
            turret = clocator.FindChild("Gun");
        }

        public void FixedUpdate() {
            Vector3 rotation = bank.GetAimRay().direction;
            rotation.y = 0;
            turret.forward = rotation;
        }
    }
}