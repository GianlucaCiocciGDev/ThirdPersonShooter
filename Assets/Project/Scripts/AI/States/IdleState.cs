using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public class IdleState : BaseState
    {
        public PursueTargetState pursueTargetState;
        public PatrolState patrolState;

        private float stateTimer = 0;
        [SerializeField] private float waitTime = 7;

        public override void OnEnter(EnemyManager enemyManager)
        {
            stateTimer = 0;
            enemyManager.agent.isStopped = true;
            enemyManager.agent.SetDestination(enemyManager.transform.position);
            enemyManager.agent.stoppingDistance = .1f;
            enemyManager.aimTarget.localPosition = enemyManager.aimTargetLookAtStartPosition;
        }
        public override void OnExit(EnemyManager enemyManager)
        {
            stateTimer = 0;
        }
        public override void OnTick(EnemyManager enemyManager)
        {
            CheckForNextState(enemyManager);
            WaitForTime(enemyManager);
            UpdateAnimatorValue(enemyManager);
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
        private void UpdateAnimatorValue(EnemyManager enemyManager) => enemyManager.animator.SetFloat("Vertical", Mathf.Clamp01(enemyManager.agent.velocity.magnitude));
        private void WaitForTime(EnemyManager enemyManager)
        {
            stateTimer += Time.deltaTime;
            enemyManager.transform.rotation = Quaternion.Euler(0.0f, Mathf.PingPong(Time.time * 10, 90.0f), 0.0f);
            if (stateTimer > waitTime)
                enemyManager.SwitchNextState(patrolState);
        }
        #endregion
    }
}
