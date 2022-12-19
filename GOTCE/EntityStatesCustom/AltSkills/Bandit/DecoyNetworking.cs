using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using static GOTCE.Main;
using RoR2.CharacterAI;
using RoR2.Skills;
using R2API.Networking.Interfaces;
using R2API.Networking;

namespace GOTCE.EntityStatesCustom.AltSkills.Bandit {
    public class DecoyNetworking {
        [RunMethod(RunAfter.Start)]
        public static void RegisterNetworkMessages() {
            NetworkingAPI.RegisterMessageType<DecoySync>();
        }
    }

    public class DecoySync : INetMessage {
        GameObject owner;
        public void Deserialize(NetworkReader reader) {
            owner = reader.ReadGameObject();
        }
        public void Serialize(NetworkWriter writer) {
            writer.Write(owner);
        }
        public void OnReceived() {
            CharacterBody characterBody = owner.GetComponent<CharacterBody>();
            try
                {
                    MasterSummon masterSummon2 = new()
                    {
                        position = characterBody.corePosition,
                        ignoreTeamMemberLimit = true,
                        masterPrefab = Enemies.Standard.ExplodingDecoy.Instance.prefabMaster,
                        summonerBodyObject = characterBody.gameObject,
                        rotation = Quaternion.LookRotation(characterBody.transform.forward)
                    };
                    masterSummon2.Perform();
                }
            catch
            {
                Main.ModLogger.LogDebug("Failed to spawn Exploding Decoy used by player: " + characterBody.GetUserName());
            }
        }
        public DecoySync() {

        }
        public DecoySync(GameObject _owner) {
            owner = _owner;
        }
    }
}