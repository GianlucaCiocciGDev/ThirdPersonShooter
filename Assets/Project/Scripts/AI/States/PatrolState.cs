using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Gdev
{
    public class PatrolState : BaseState
    {
        public PursueTargetState pursueTargetState;
        public IdleState idleState;

        [SerializeField]private Transform[] checkpoints;
        private int currentCheckPointIndex = 0;

        public override void OnEnter(EnemyManager enemyManager)
        {
            enemyManager.agent.isStopped = false;
            enemyManager.agent.stoppingDistance = .1f;
            enemyManager.aimTarget.localPosition = enemyManager.aimTargetLookAtStartPosition;
            NextCheckpoint();
        }
        public override void OnExit(EnemyManager enemyManager)
        {
        }
        public override void OnTick(EnemyManager enemyManager)
        {
            UpdateAnimatorValue(enemyManager);
            CheckForNextState(enemyManager);
            MoveToCheckpoint(enemyManager);
            CheckForCheckpoint(enemyManager);
        }
        protected override void CheckForNextState(EnemyManager enemyManager)
        {
            (Vector3 targetDirection, float distanceFromTarget) = base.DetectionInfo(enemyManager);

            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
            if (distanceFromTarget <= enemyManager.detectionRadius)
            {
                if (viewableAngle > enemyManager.minDetectionAngle && viewableAngle < enemyManager.maxDetectionAngle)
                {
                    if (!Physics.Linecast(enemyManager.transform.position, enemyManager.currentTarget.transform.position))
                    {
                        enemyManager.SwitchNextState(pursueTargetState);
                    }
                }
            }
        }

        #region Decision Group
        private void MoveToCheckpoint(EnemyManager enemyManager) => enemyManager.agent.destination = checkpoints[currentCheckPointIndex].transform.position;
        private void NextCheckpoint()
        {
            currentCheckPointIndex++;
            if (currentCheckPointIndex >= checkpoints.Length)
                currentCheckPointIndex = 0;
        }
        private bool IsDestinationReached(EnemyManager enemyManager) => enemyManager.agent.remainingDistance < enemyManager.agent.stoppingDistance && !enemyManager.agent.pathPending;
        private void UpdateAnimatorValue(EnemyManager enemyManager) => enemyManager.animator.SetFloat("Vertical", Mathf.Clamp01(enemyManager.agent.velocity.magnitude));
        private void CheckForCheckpoint(EnemyManager enemyManager)
        {
            if (IsDestinationReached(enemyManager))
                enemyManager.SwitchNextState(idleState);
        }
        #endregion
    }
}
