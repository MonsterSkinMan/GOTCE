/*using System;
using UnityEngine;

namespace GOTCE.Survivors.Cracktain {
    public class JeroneManager : MonoBehaviour, IOnTakeDamageServerReceiver {
        public CracktainPassiveBehaviour ownerBehaviour;
        public CharacterBody owner;
        //
        public Vector3 currentTargetPosition = Vector3.zero;
        public GameObject pingTarget;
        public bool shouldTarget => currentTargetPosition != Vector3.zero && (!pingTarget || !pingTarget.GetComponent<CharacterBody>());
        //
        public BaseAI ai;
        private bool hasAdded = false;

        public void Start() {
            ai = GetComponent<BaseAI>();
            ai.master.inventory.onInventoryChanged += OnChanged;
            HealthComponent.onCharacterHealServer += OnHeal;
        }

        public void OnChanged() {
            if (owner && owner.inventory) {
                Inventory inv = owner.inventory;
                inv.CopyItemsFrom(ai.master.inventory);
            }
        }

        public void OnHeal(HealthComponent hc, float heal, ProcChainMask mask) {
            if (ai.body && hc == ai.body.healthComponent) {
                owner.healthComponent.Heal(heal, new());
            }
        }

        public void FixedUpdate() {
            if (ai.body && ai.body.healthComponent && !hasAdded) {
                hasAdded = true;
                List<IOnTakeDamageServerReceiver> receivers = ai.body.healthComponent.onTakeDamageReceivers.ToList();
                receivers.Add(this);
                ai.body.healthComponent.onTakeDamageReceivers = receivers.ToArray();
            }

            if (ai.master.money != owner.master.money) {
                ai.master.money = owner.master.money;
            }

            if (ownerBehaviour && ownerBehaviour.pcmc) {
                Vector3 oldPingTarget = currentTargetPosition;

                PlayerCharacterMasterController pcmc = ownerBehaviour.pcmc;
                bool isActive = pcmc.pingerController && pcmc.pingerController.currentPing.active;

                if (!isActive) {
                    currentTargetPosition = Vector3.zero;
                    ai.broadNavigationAgent.InvalidatePath();
                    ai.EvaluateSkillDrivers();
                    ai.customTarget.gameObject = pingTarget;
                    return;
                }

                currentTargetPosition = pcmc.pingerController.currentPing.origin;
                pingTarget = pcmc.pingerController.currentPing.targetGameObject;

                if (oldPingTarget != currentTargetPosition) {
                    ai.broadNavigationAgent.InvalidatePath();
                    ai.EvaluateSkillDrivers();
                }

                if (pingTarget && pingTarget.GetComponent<CharacterBody>()) {
                    if (pingTarget.GetComponent<CharacterBody>().teamComponent.teamIndex == TeamIndex.Player) {
                        pingTarget = null;
                        currentTargetPosition = Vector3.zero;
                        return;
                    }
                    
                    ai.currentEnemy.gameObject = pingTarget;
                }
                else if (pingTarget) {
                    ai.customTarget.gameObject = pingTarget;
                }
            }
        }

        public void OnTakeDamageServer(DamageReport damageReport)
        {
            if (owner) {
                owner.healthComponent.TakeDamage(damageReport.damageInfo);
            }
        }
    }
}*/