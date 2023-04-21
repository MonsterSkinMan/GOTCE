/*using System;
using UnityEngine;

namespace GOTCE.Survivors.Cracktain {
    public class CracktainPassiveBehaviour : MonoBehaviour {
        private GameObject JeroneMaster => CrackedCaptain.JeroneMaster;
        public CharacterMaster CurrentJerone;
        public CharacterBody body;
        public PlayerCharacterMasterController pcmc;
        public CharacterMaster master;
        private CharacterDirection direction;
        private InputBankTest bank;
        public void Start() {
            body = GetComponent<CharacterBody>();
            master = body.master;
            pcmc = master.playerCharacterMasterController;

            direction = GetComponent<CharacterDirection>();
            bank = GetComponent<InputBankTest>();

            if (NetworkServer.active) {
                Invoke(nameof(SpawnJerone), 2f);
            }
        }

        public void SpawnJerone() {
            MasterSummon summon = new();
            summon.masterPrefab = JeroneMaster;
            summon.summonerBodyObject = base.gameObject;
            summon.ignoreTeamMemberLimit = true;
            summon.position = PickJeronePosition();
            summon.inventoryToCopy = master.inventory;

            CurrentJerone = summon.Perform();
            JeroneManager jeroneManager = CurrentJerone.GetComponent<JeroneManager>();
            jeroneManager.owner = body;
            jeroneManager.ownerBehaviour = this;
        }

        public void FixedUpdate() {
            direction.targetTransform.forward = bank.GetAimRay().direction;
        }

        public Vector3 PickJeronePosition() {
            NodeGraph nodeGraph = SceneInfo.instance.groundNodes;
            NodeGraph.NodeIndex[] nodes = nodeGraph.FindNodesInRange(base.transform.position, 0, 40, HullMask.Human).ToArray();
            NodeGraph.NodeIndex index = nodes[Random.Range(0, nodes.Length)];
            nodeGraph.GetNodePosition(index, out Vector3 pos);
            return pos;
        }
    }
}*/