using RoR2;
using Unity;
using System;
using UnityEngine;
using RoR2.Projectile;

namespace GOTCE.Components
{
    public class ViendPearlManager : MonoBehaviour
    {
        public GameObject mostRecentPearl;

        public void Swap()
        {
            if (!mostRecentPearl)
            {
                Main.ModLogger.LogError("Tried to use Return with no active pearl.");
                return;
            }
            // Vector3 position = TeleportHelper.FindSafeTeleportDestination(mostRecentPearl.transform.position, gameObject.GetComponent<CharacterBody>(), RoR2Application.rng) ?? mostRecentPearl.transform.position;
            Vector3 position = mostRecentPearl.transform.position;
            TeleportHelper.TeleportBody(gameObject.GetComponent<CharacterBody>(), position);
            Destroy(mostRecentPearl);
        }
    }

    public class ViendPearlBehavior : MonoBehaviour
    {
        public GameObject owner;
        public Transform ownerTransform;
        public float forwardSpeed = 95;
        public float downwardSpeed = 0;
        private bool hasEnabled = false;

        public void OnEnable()
        {
            // owner = gameObject.GetComponent<ProjectileController>().owner;
            if (!owner)
            {
                Destroy(gameObject);
            }
            else
            {
                owner.GetComponent<ViendPearlManager>().mostRecentPearl = gameObject;
            }
            // Debug.Log(gameObject.GetComponent<ProjectileSimple>().lifetime);
        }

        public void OnDestroy()
        {
            // Main.ModLogger.LogFatal(System.Environment.StackTrace);
            GameObject voidVFX = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/CritGlassesVoid/CritGlassesVoidExecuteEffect.prefab").WaitForCompletion();
            EffectManager.SpawnEffect(voidVFX, new EffectData
            {
                origin = gameObject.transform.position,
                scale = 1f
            }, true);
        }

        public void FixedUpdate()
        {
            if (NetworkServer.active)
            {
                downwardSpeed += Time.fixedDeltaTime;
                if (downwardSpeed >= 0.15f && !hasEnabled)
                {
                    gameObject.GetComponent<ProjectileStickOnImpact>().enabled = true;
                    hasEnabled = true;
                }
                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                /// rb.velocity = new Vector3(rb.velocity.x, downwardSpeed * -2, rb.velocity.z);
                rb.AddForce(Vector3.down * downwardSpeed * 5, ForceMode.Impulse);
            }
        }
    }

    public class ViendPearlUpgradeBehavior : MonoBehaviour
    {
        public GameObject owner;

        public void OnDestroy()
        {
            if (owner)
            {
                if (NetworkServer.active && owner.GetComponent<CharacterBody>() && gameObject.name == "pearlupgradeimpacted")
                {
                    TeleportHelper.TeleportBody(owner.GetComponent<CharacterBody>(), gameObject.transform.position);
                }
            }
        }
    }
}