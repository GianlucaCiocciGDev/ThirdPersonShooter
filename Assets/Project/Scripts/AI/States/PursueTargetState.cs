using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

namespace Gdev
{
    public class PursueTargetState : BaseState
    {
        [SerializeField] PatrolState patrolState;
        [SerializeField] AttackState attackState;

        public override void OnEnter(EnemyManager enemyManager)
        {
            enemyManager.agent.enabled = true;
            enemyManager.agent.isStopped = false;
            enemyManager.agent.stoppingDistance = .1f;
            enemyManager.aimTargetLookAt = enemyManager.currentTarget.transform.GetChild(1).transform;
        }
        public override void OnExit(EnemyManager enemyManager)
        {
            print("Idle exit");
        }
        public override void OnTick(EnemyManager enemyManager)
        {
            UpdateAnimatorValue(enemyManager);
            SetIKTarget(enemyManager);
            SetDestination(enemyManager);
            CheckForNextState(enemyManager);
        }
        protected override void CheckForNextState(EnemyManager enemyManager)
        {
            (Vector3 targetDirection, float distanceFromTarget) = base.DetectionInfo(enemyManager);

            HandleRotateTowardsTarget(enemyManager, targetDirection);

            if (distanceFromTarget <= enemyManager.attackRadius)
                enemyManager.SwitchNextState(attackState);

            if (distanceFromTarget >= enemyManager.detectionRadius)
                enemyManager.SwitchNextState(patrolState);

        }

        #region Decision Group
        private void UpdateAnimatorValue(EnemyManager enemyManager) => enemyManager.animator.SetFloat("Vertical", Mathf.Clamp01(enemyManager.agent.velocity.magnitude));
        private void HandleRotateTowardsTarget(EnemyManager enemyManager,Vector3 targetDirection)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }
        private void SetIKTarget(EnemyManager enemyManager) => enemyManager.aimTarget.position = enemyManager.aimTargetLookAt.position;
        private void SetDestination(EnemyManager enemyManager) => enemyManager.agent.SetDestination(enemyManager.currentTarget.transform.position);
        #endregion
    }
}
