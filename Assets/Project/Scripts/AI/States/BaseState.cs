using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public abstract class BaseState : MonoBehaviour
    {
        public abstract void OnEnter(EnemyManager enemyManager);
        public abstract void OnTick(EnemyManager enemyManager);
        public abstract void OnExit(EnemyManager enemyManager);
        protected abstract void CheckForNextState(EnemyManager enemyManager);
        protected virtual (Vector3, float) DetectionInfo(EnemyManager enemyManager)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            targetDirection.y = 0;
            float distanceFromTarget = targetDirection.sqrMagnitude;
            targetDirection.Normalize();

            if (targetDirection == Vector3.zero)
            {
                targetDirection = transform.forward;
            }

            return(targetDirection, distanceFromTarget);
        }
    }
}
