using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using static GOTCE.Main;
using UnityEngine.AddressableAssets;

namespace GOTCE.Components
{
    public class GOTCE_CrackedComponent : MonoBehaviour
    {
        private float timer = 0f;
        private bool swapped = false;
        private CharacterBody body;
        public void Start() {
            body = gameObject.GetComponent<CharacterBody>();
        }

        public void FixedUpdate() {
            if (!swapped) {
                timer += Time.fixedDeltaTime;
                if (timer > 1.5f) {
                    swapped = true;
                    CharacterModel model = body.modelLocator.modelTransform.GetComponent<CharacterModel>();
                    Material mat = MainAssets.LoadAsset<Material>("Assets/Materials/Enemies/crackedPestMaterial.mat");
                    Material mat2 = MainAssets.LoadAsset<Material>("Assets/Materials/Enemies/tongueMaterial.mat");
                    // CharacterModel model = body.gameObject.GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>();
                    model.baseRendererInfos[0].defaultMaterial = mat;
                    model.baseRendererInfos[1].defaultMaterial = mat;
                    model.baseRendererInfos[2].defaultMaterial = mat2;
                }
            }
        }
    }
}