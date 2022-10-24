/*using EntityStates;
using RoR2;
using RoR2.VoidRaidCrab;
using UnityEngine;

namespace GOTCE.Enemies.EntityStatesCustom
{
    public class SpawnState : BaseState
    {
        // everything except duration taken from rip

        public bool doLegs = false;

        public string animationStateName = "Spawn";

        public string animationPlaybackRateParam = "Spawn.playbackRate";
        public string animationLayerName = "Body";

        public float duration = 1.5f;
        // vanilla is 10

        public string spawnSoundString = "Play_voidRaid_spawn";

        public string spawnMuzzleName = "SpawnEffect";

        public GameObject spawnEffectPrefab = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidRaidCrab/VoidRaidCrabSpawnEffect.prefab").WaitForCompletion());

        public CharacterSpawnCard jointSpawnCard = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/DLC1/VoidRaidCrab/cscVoidRaidCrabJoint.asset").WaitForCompletion();

        public string leg1Name = "FrontLegL";

        public string leg2Name = "FrontLegR";

        public string leg3Name = "MidLegL";

        public string leg4Name = "MidLegR";

        public string leg5Name = "BackLegL";

        public string leg6Name = "BackLegR";

        public override void OnEnter()
        {
            OnEnter();
            spawnEffectPrefab.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            // vanilla is 5
            PlayAnimation(animationLayerName, animationStateName, animationPlaybackRateParam, duration);
            Util.PlaySound(spawnSoundString, gameObject);
            if (spawnEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(spawnEffectPrefab, gameObject, spawnMuzzleName, false);
            }
            if (doLegs && NetworkServer.active)
            {
                ChildLocator modelChildLocator = GetModelChildLocator();
                if (jointSpawnCard && modelChildLocator)
                {
                    DirectorPlacementRule directorPlacementRule = new()
                    {
                        placementMode = DirectorPlacementRule.PlacementMode.Direct,
                        spawnOnTarget = transform
                    };
                    SpawnJointBodyForLegServer(leg1Name, modelChildLocator, directorPlacementRule);
                    SpawnJointBodyForLegServer(leg2Name, modelChildLocator, directorPlacementRule);
                    SpawnJointBodyForLegServer(leg3Name, modelChildLocator, directorPlacementRule);
                    SpawnJointBodyForLegServer(leg4Name, modelChildLocator, directorPlacementRule);
                    SpawnJointBodyForLegServer(leg5Name, modelChildLocator, directorPlacementRule);
                    SpawnJointBodyForLegServer(leg6Name, modelChildLocator, directorPlacementRule);
                }
            }
        }

        private void SpawnJointBodyForLegServer(string legName, ChildLocator childLocator, DirectorPlacementRule placementRule)
        {
            DirectorSpawnRequest directorSpawnRequest = new(jointSpawnCard, placementRule, Run.instance.stageRng)
            {
                summonerBodyObject = base.gameObject
            };
            DirectorCore instance = DirectorCore.instance;
            GameObject gameObject = (instance != null) ? instance.TrySpawnObject(directorSpawnRequest) : null;
            Transform transform = childLocator.FindChild(legName);
            if (gameObject && transform)
            {
                CharacterMaster component = gameObject.GetComponent<CharacterMaster>();
                if (component)
                {
                    LegController component2 = transform.GetComponent<LegController>();
                    if (component2)
                    {
                        component2.SetJointMaster(component, transform.GetComponent<ChildLocator>());
                    }
                }
            }
        }

        public override void FixedUpdate()
        {
            FixedUpdate();
            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}*/