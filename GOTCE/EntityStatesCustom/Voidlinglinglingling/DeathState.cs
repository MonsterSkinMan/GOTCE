/*using System;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace GOTCE.Enemies.EntityStatesCustom
{
    public class DeathState : GenericCharacterDeath
    {
        public bool addPrintController = true;
        public string animationStateName = "TrueDeath";
        public string animationPlaybackRateParam = "TrueDeath.playbackRate";
        public string animationLayerName = "Body";

        public float startingPrintBias = -10f;

        public Vector3 ragdollForce = new(0f, 0f, 0f);
        public float printDuration = 7f;
        public float maxPrintBias = 10f;

        public GameObject initialEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidRaidCrab/VoidRaidCrabDeathPending.prefab").WaitForCompletion();

        public string initialEffectMuzzle = "Head";

        public GameObject explosionEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidRaidCrab/VoidRaidCrabDeath.prefab").WaitForCompletion();

        public string explosionEffectMuzzle = "Head";

        public float explosionForce = 600f;
        // vanilla is 6000

        public float duration = 1f;
        // vanilla is 5

        private Transform modelTransform;
        public override bool shouldAutoDestroy => false;

        public override void PlayDeathAnimation(float crossfadeDuration)
        {
            PlayCrossfade(animationLayerName, animationStateName, animationPlaybackRateParam, duration, crossfadeDuration);
        }

        public override void OnEnter()
        {
            OnEnter();
            initialEffectPrefab.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            // vanilla is 1
            explosionEffectPrefab.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            // vanilla is 8
            if (VoidRaidGauntletController.instance)
            {
                VoidRaidGauntletController.instance.SetCurrentDonutCombatDirectorEnabled(false);
            }
            modelTransform = GetModelTransform();
            Transform transform = FindModelChild("StandableSurface");
            if (transform)
            {
                transform.gameObject.SetActive(false);
            }
            if (explosionEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(initialEffectPrefab, gameObject, initialEffectMuzzle, false);
            }
            if (addPrintController)
            {
                PrintController printController = modelTransform.gameObject.AddComponent<PrintController>();
                printController.printTime = printDuration;
                printController.enabled = true;
                printController.startingPrintHeight = 99f;
                printController.maxPrintHeight = 99f;
                printController.startingPrintBias = startingPrintBias;
                printController.maxPrintBias = maxPrintBias;
                printController.disableWhenFinished = false;
                printController.printCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
            }
            if (rigidbodyMotor)
            {
                rigidbodyMotor.moveVector = Vector3.zero;
            }
        }

        public override void FixedUpdate()
        {
            FixedUpdate();
            if (NetworkServer.active && fixedAge >= duration)
            {
                if (explosionEffectPrefab)
                {
                    EffectManager.SimpleMuzzleFlash(explosionEffectPrefab, gameObject, explosionEffectMuzzle, true);
                }
                DestroyBodyAsapServer();
            }
        }

        public override void OnExit()
        {
            if (modelTransform)
            {
                RagdollController component = modelTransform.GetComponent<RagdollController>();
                Rigidbody component2 = GetComponent<Rigidbody>();
                if (component && component2)
                {
                    component.BeginRagdoll(ragdollForce);
                }
                ExplodeRigidbodiesOnStart component3 = modelTransform.GetComponent<ExplodeRigidbodiesOnStart>();
                if (component3)
                {
                    component3.force = explosionForce;
                    component3.enabled = true;
                }
            }
        }
    }
}*/