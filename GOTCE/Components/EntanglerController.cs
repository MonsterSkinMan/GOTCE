using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using static GOTCE.Main;
using RoR2.CharacterAI;
using RoR2.Skills;
using R2API.Networking.Interfaces;

namespace GOTCE.Components
{
    public class EntanglerController : MonoBehaviour
    {
        public EntanglerControllerLeader leaderController;
        private BaseAI ai;
        private LineRenderer renderer;
        private InputBankTest inputBank;
        private SkillLocator skillLocator;
        private CharacterMaster master => gameObject.GetComponent<CharacterMaster>();
        private float aimVelocity;

        private void Start()
        {
            ai = gameObject.GetComponent<BaseAI>();
            inputBank = gameObject.GetComponent<CharacterMaster>().GetBody().inputBank;
            skillLocator = gameObject.GetComponent<CharacterMaster>().GetBody().skillLocator;
        }

        public void FixedUpdate()
        {
            if (leaderController && leaderController.isControlling && NetworkServer.active)
            {
                foreach (GenericSkill skill in skillLocator.allSkills)
                {
                    skill.ExecuteIfReady();
                }

                ai.skillDriverUpdateTimer = 5f;

                if (leaderController.lastPingLocation != Vector3.zero) {
                    Vector3 targetPosition = leaderController.lastPingLocation + (ai.bodyTransform.position - ai.body.footPosition);
                    ai.SetGoalPosition(targetPosition);
                    ai.localNavigator.targetPosition = targetPosition;
                    ai.localNavigator.allowWalkOffCliff = true;
                    ai.localNavigator.Update(Time.fixedDeltaTime);
                    ai.bodyInputs.moveVector = ai.localNavigator.moveVector;
                    ai.bodyInputBank.moveVector = ai.localNavigator.moveVector;
                }

                if (!master.GetBody().HasBuff(Buffs.ValveBalance.instance.BuffDef)) {
                    master.GetBody().AddBuff(Buffs.ValveBalance.instance.BuffDef);
                }
            }
            else
            {
                if (master.GetBody().HasBuff(Buffs.ValveBalance.instance.BuffDef)) {
                    master.GetBody().RemoveBuff(Buffs.ValveBalance.instance.BuffDef);
                }
            }
        }
    }

    public class EntanglerControllerLeader : MonoBehaviour
    {
        public bool isControlling = false;
        public CharacterBody leaderBody => gameObject.GetComponent<CharacterBody>();
        public Vector3 aimDirection;
        public Vector3 lastPingLocation = new Vector3(0, 0, 0);
        private LineRenderer renderer;
        private LineRenderer renderer2;
        public InputBankTest inputBank;
        private Transform muzzleL;
        private Transform muzzleR;
        private CharacterMaster master;
        private GameObject laserPrefab1;
        private GameObject laserPrefab2;

        private void Start()
        {
            inputBank = leaderBody.inputBank;
            laserPrefab1 = GameObject.Instantiate(EntityStates.GolemMonster.ChargeLaser.laserPrefab, gameObject.transform.position, gameObject.transform.rotation);
            laserPrefab2 = GameObject.Instantiate(EntityStates.GolemMonster.ChargeLaser.laserPrefab, gameObject.transform.position, gameObject.transform.rotation);
            renderer = laserPrefab1.GetComponent<LineRenderer>();
            renderer2 = laserPrefab2.GetComponent<LineRenderer>();
            renderer.startWidth = 0.1f;
            renderer.endWidth = 0.1f;
            renderer.enabled = false;
            renderer.startColor = Color.red;
            renderer.endColor = Color.red;

            renderer2.startWidth = 0.1f;
            renderer2.endWidth = 0.5f;
            renderer2.enabled = false;
            renderer2.startColor = Color.red;
            renderer2.endColor = Color.red;

            muzzleL = leaderBody.modelLocator.modelTransform.GetComponent<ChildLocator>().FindChild("MuzzleLeft");
            muzzleR = leaderBody.modelLocator.modelTransform.GetComponent<ChildLocator>().FindChild("MuzzleRight");

            master = leaderBody.master;
        }

        private void FixedUpdate()
        {
            if (leaderBody && leaderBody.equipmentSlot)
            {
                Ray ray = leaderBody.equipmentSlot.GetAimRay();

                bool cast = Util.CharacterRaycast(gameObject, ray, out RaycastHit info, int.MaxValue, ~0, QueryTriggerInteraction.Ignore);

                if (cast)
                {
                    aimDirection = info.point;
                }
            }

            if (isControlling)
            {
                renderer.enabled = true;
                renderer2.enabled = true;
                Vector3 end = inputBank.GetAimRay().GetPoint(300f);
                renderer.SetPosition(0, muzzleL.position);
                renderer.SetPosition(1, end);
                renderer2.SetPosition(0, muzzleR.position);
                renderer2.SetPosition(1, end);

                SetPos(master.playerCharacterMasterController.pingerController.currentPing.origin);
                // Debug.Log(lastPingLocation);

                if (!leaderBody.HasBuff(Buffs.ValveBalance.instance.BuffDef)) {
                    leaderBody.AddBuff(Buffs.ValveBalance.instance.BuffDef);
                }
            }
            else
            {
                renderer.enabled = false;
                renderer2.enabled = false;
                lastPingLocation = new(0f, 0f, 0f);

                if (leaderBody.HasBuff(Buffs.ValveBalance.instance.BuffDef)) {
                    leaderBody.RemoveBuff(Buffs.ValveBalance.instance.BuffDef);
                }
            }
        }

        private void SetPos(Vector3 position)
        {
            lastPingLocation = position;
        }
    }

    public class EntanglerHooks
    {
        public static void Hook()
        {
            On.RoR2.CharacterAI.BaseAI.UpdateBodyAim += (orig, self, delta) =>
            {
                if (self.body && self.leader != null)
                {
                    if (self.master && self.master.GetComponent<EntanglerController>())
                    {
                        EntanglerController controller = self.master.GetComponent<EntanglerController>();
                        if (self.leader.characterBody.GetComponent<EntanglerControllerLeader>())
                        {
                            EntanglerControllerLeader leader = self.leader.characterBody.GetComponent<EntanglerControllerLeader>();

                            if (leader.isControlling)
                            {
                                self.bodyInputs.desiredAimDirection = Vector3.Normalize(leader.aimDirection - self.bodyInputBank.aimOrigin);
                            }
                        }
                    }
                }
                orig(self, delta);
            };

            On.RoR2.CharacterAI.BaseAI.UpdateBodyInputs += (orig, self) =>
            {
                orig(self);
                if (self.body && self.leader != null)
                {
                    if (self.master && self.master.GetComponent<EntanglerController>())
                    {
                        EntanglerController controller = self.master.GetComponent<EntanglerController>();
                        if (self.leader != null && self.leader.characterBody != null && self.leader.characterBody.GetComponent<EntanglerControllerLeader>())
                        {
                            EntanglerControllerLeader leader = self.leader.characterBody.GetComponent<EntanglerControllerLeader>();

                            if (leader.isControlling)
                            {
                                self.bodyInputBank.skill1.PushState(true);
                                self.bodyInputBank.skill2.PushState(true);
                                self.bodyInputBank.skill3.PushState(true);
                                self.bodyInputBank.skill4.PushState(true);
                                self.bodyInputBank.sprint.PushState(true);
                                self.bodyInputBank.jump.PushState(leader.inputBank.jump.down);
                                self.bodyInputBank.activateEquipment.PushState(true);
                            }
                        }
                    }
                }
            };

            On.EntityStates.AI.Walker.Combat.UpdateAI += (orig, self, delta) =>
            {
                if (self.ai && self.ai.leader.gameObject)
                {
                    if (self.ai.leader.gameObject.GetComponent<EntanglerControllerLeader>())
                    {
                        if (self.ai.leader.gameObject.GetComponent<EntanglerControllerLeader>().isControlling)
                        {
                            // under the entangler effect, don't perform basic ai stuff
                        }
                        else
                        {
                            orig(self, delta);
                            return;
                        }
                    }
                    else
                    {
                        orig(self, delta);
                        return;
                    }
                }
                else
                {
                    orig(self, delta);
                    return;
                }
                orig(self, delta);
            };

            R2API.Networking.NetworkingAPI.RegisterMessageType<EntanglerSync>();
            R2API.Networking.NetworkingAPI.RegisterMessageType<EntanglerControlSync>();

            ContentAddition.AddEntityState<EntityStatesCustom.AltSkills.Engineer.Entangler>(out bool _);
        }
    }

    public class EntanglerSync : INetMessage {
        GameObject owner;
        GameObject minion;
        public void OnReceived() {
            if (owner && minion) {
                if (!owner.GetComponent<EntanglerControllerLeader>()) {
                    owner.AddComponent<EntanglerControllerLeader>();
                }

                EntanglerControllerLeader leader = owner.GetComponent<EntanglerControllerLeader>();

                if (!minion.GetComponent<EntanglerController>()) {
                    EntanglerController controller = minion.AddComponent<EntanglerController>();
                    controller.leaderController = leader;
                }
            }
        }  
        public EntanglerSync() {

        }
        public EntanglerSync(GameObject _owner, GameObject _minion) {
            owner = _owner;
            minion = _minion;
        }
        public void Serialize(NetworkWriter writer) {
            writer.Write(owner);
            writer.Write(minion);
        }
        public void Deserialize(NetworkReader reader) {
            owner = reader.ReadGameObject();
            minion = reader.ReadGameObject();
        }
    }

    public class EntanglerControlSync : INetMessage {
        GameObject owner;
        bool value;
        public void OnReceived() {
            if (owner) {
                if (!owner.GetComponent<EntanglerControllerLeader>()) {
                    owner.AddComponent<EntanglerControllerLeader>();
                }

                EntanglerControllerLeader leader = owner.GetComponent<EntanglerControllerLeader>();

                leader.isControlling = value;
            }
        }
        public void Serialize(NetworkWriter writer) {
            writer.Write(owner);
            writer.Write(value);
        }
        public void Deserialize(NetworkReader reader) {
            owner = reader.ReadGameObject();
            value = reader.ReadBoolean();
        }
        public EntanglerControlSync() {

        }
        public EntanglerControlSync(GameObject _owner, bool _value) {
            owner = _owner;
            value = _value;
        }
    }
}