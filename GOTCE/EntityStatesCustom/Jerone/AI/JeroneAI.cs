/*using System;
using UnityEngine;
using GOTCE.Survivors.Cracktain;
using EntityStates.AI;

namespace GOTCE.EntityStatesCustom.Jerone.AI {
    public class JeroneAI : BaseAIState {
        private float aiUpdateTimer;
        private Vector3 footPosition;
        private JeroneManager manager;
        private CracktainPassiveBehaviour owner;
        private AISkillDriver dominantDriver;
        private SkillSlot currentSlot;
        private bool meetsConditions;
        private float strafeTimer;
        private float strafeDirection;
        private float lastPathUpdate;
        private float fallbackNodeAge = float.NegativeInfinity;
        private float fallbackNodeDuration = 4f;
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!base.ai || !base.body) {
                return;
            }

            if (!manager) {
                manager = base.transform.GetComponent<JeroneManager>();
            }

            if (manager && !owner) {
                owner = manager.ownerBehaviour;
            }

            aiUpdateTimer -= Time.fixedDeltaTime;
            strafeTimer -= Time.fixedDeltaTime;
            
            UpdateFootPosition();

            if (aiUpdateTimer <= 0f) {
                aiUpdateTimer = BaseAIState.cvAIUpdateInterval.value;
                UpdateAI(aiUpdateTimer);
            }
        }

        public void UpdateFootPosition() {
            footPosition = base.body.footPosition;
            BroadNavigationSystem.Agent broadNavigationAgent = base.ai.broadNavigationAgent;
            broadNavigationAgent.currentPosition = footPosition;
        }

        public void UpdateAI(float deltaTime) {
            BaseAI.SkillDriverEvaluation eval = ai.skillDriverEvaluation;

            dominantDriver = eval.dominantSkillDriver;
            currentSlot = SkillSlot.None;
            meetsConditions = true;
            bodyInputs.moveVector = Vector3.zero;

            AISkillDriver.MovementType movementType = AISkillDriver.MovementType.Stop;
            float moveScale = 1f;
            bool requiresLos = false;
            bool requiresAimLos = false;
            bool requiresAimConf = false;

            if (dominantDriver) {
                moveScale = dominantDriver.moveInputScale;
                requiresAimConf = dominantDriver.activationRequiresAimConfirmation;
                requiresLos = dominantDriver.activationRequiresTargetLoS;
                requiresAimLos = dominantDriver.activationRequiresAimTargetLoS;
                movementType = dominantDriver.movementType;
                currentSlot = dominantDriver.skillSlot;
            }

            Vector3 position = base.body.transform.position;
            BroadNavigationSystem.Agent agent = base.ai.broadNavigationAgent;
            BroadNavigationSystem.AgentOutput output = agent.output;
            BaseAI.Target target = base.ai.currentEnemy;
            BaseAI.Target custom = base.ai.customTarget;

            Vector3 moveTargetPosition = Vector3.zero;

            if (manager.shouldTarget) {
                moveTargetPosition = manager.currentTargetPosition;
            }
            else {
                if (target.gameObject) {
                    target.GetBullseyePosition(out Vector3 bpos);
                    moveTargetPosition = bpos;
                }
            }

            if (moveTargetPosition == Vector3.zero) {
                moveTargetPosition = PickRandomNearbyReachablePosition() ?? footPosition;
            }

            base.ai.SetGoalPosition(moveTargetPosition);

            GameObject current = custom.gameObject ?? target.gameObject;

            if (true) { // too lazy to unindent everything
                Vector3 targetPosition = position;

                if (manager.shouldTarget) {
                    targetPosition = (output.nextPosition ?? footPosition) + (position - footPosition);
                }
                else {
                    Vector3 vec1 = ((!dominantDriver || !dominantDriver.ignoreNodeGraph) ? (output.nextPosition ?? footPosition) : moveTargetPosition);
                    Vector3 vec2 = (vec1 - footPosition).normalized * 10f;
                    Vector3 vec3 = Vector3.Cross(Vector3.up, vec2);

                    switch (movementType) {
                        case AISkillDriver.MovementType.ChaseMoveTarget:
                            targetPosition = vec1 + (position - footPosition);
                            break;
                        case AISkillDriver.MovementType.FleeMoveTarget:
                            targetPosition -= vec2;
                            break;
                        case AISkillDriver.MovementType.StrafeMovetarget:
                            if (strafeTimer <= 0) {
                                if (strafeDirection == 0f) {
                                    strafeDirection = ((Random.Range(0, 1) == 0) ? (-1f) : 1f);
                                    strafeTimer = 0.25f;
                                }

                                targetPosition += vec3 * strafeDirection;
                            }
                            break;
                    }
                }

                base.ai.localNavigator.targetPosition = targetPosition;
                base.ai.localNavigator.allowWalkOffCliff = false;
                base.ai.localNavigator.Update(deltaTime);

                if (base.ai.localNavigator.wasObstructedLastUpdate) {
                    strafeDirection *= -1f;
                }

                bodyInputs.moveVector = base.ai.localNavigator.moveVector;
                bodyInputs.moveVector *= moveScale;
            }

            if (output.lastPathUpdate > lastPathUpdate && !output.targetReachable) {
                agent.goalPosition = PickRandomNearbyReachablePosition();
                agent.InvalidatePath();
            }

            lastPathUpdate = output.lastPathUpdate;
        }

        public override BaseAI.BodyInputs GenerateBodyInputs(in BaseAI.BodyInputs previousBodyInputs)
        {
            bool s1 = false;
            bool s2 = false;
            bool s3 = false;
            bool s4 = false;
            bool wasPressed = false;

            if (base.ai.bodyInputBank) {
                AISkillDriver.ButtonPressType buttonPressType = dominantDriver ? dominantDriver.buttonPressType : AISkillDriver.ButtonPressType.Abstain;

                switch (currentSlot) {
                    case SkillSlot.Primary:
                        wasPressed = previousBodyInputs.pressSkill1;
                        break;
                    case SkillSlot.Secondary:
                        wasPressed = previousBodyInputs.pressSkill2;
                        break;
                    case SkillSlot.Utility:
                        wasPressed = previousBodyInputs.pressSkill3;
                        break;
                    case SkillSlot.Special:
                        wasPressed = previousBodyInputs.pressSkill4;
                        break;
                }
                
                bool shouldPress = true;

                switch (buttonPressType) {
                    case AISkillDriver.ButtonPressType.Abstain:
                        shouldPress = false;
                        break;
                    case AISkillDriver.ButtonPressType.TapContinuous:
                        shouldPress = !wasPressed;
                        break;
                }

                switch (currentSlot) {
                    case SkillSlot.Primary:
                        s1 = shouldPress;
                        break;
                    case SkillSlot.Secondary:
                        s2 = shouldPress;
                        break;
                    case SkillSlot.Utility:
                        s3 = shouldPress;
                        break;
                    case SkillSlot.Special:
                        s4 = shouldPress;
                        break;
                }

                bodyInputs.pressSkill1 = s1;
                bodyInputs.pressSkill2 = s2;
                bodyInputs.pressSkill3 = s3;
                bodyInputs.pressSkill4 = s4;
                bodyInputs.pressSprint = false;
                bodyInputs.pressActivateEquipment = false;
                bodyInputs.desiredAimDirection = Vector3.zero;

                if (dominantDriver) {
                    bodyInputs.pressSprint = dominantDriver.shouldSprint;
                    bodyInputs.pressActivateEquipment = dominantDriver.shouldFireEquipment && !previousBodyInputs.pressActivateEquipment;

                    AISkillDriver.AimType aimType = dominantDriver.aimType;
                    BaseAI.Target aimTarget = base.ai.skillDriverEvaluation.aimTarget;

                    if (aimType == AISkillDriver.AimType.MoveDirection)
                    {
                        AimInDirection(ref bodyInputs, bodyInputs.moveVector);
                    }
                    if (aimTarget != null)
                    {
                        bodyInputBank.aimDirection = (aimTarget.lastKnownBullseyePosition.Value - base.bodyTransform.position).normalized;
                    }
                }

                if (base.ai.customTarget.gameObject && base.ai.customTarget.gameObject.GetComponent<IInteractable>() != null) {
                    base.ai.bodyInputBank.interact.PushState(true);
                    // Debug.Log("pushing interact");
                }
            }
            ModifyInputsForJumpIfNeccessary(ref bodyInputs);
            return base.GenerateBodyInputs(previousBodyInputs);
        }
    }
}*/