using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public class DeadState : BaseState
    {
        public override void OnEnter(EnemyManager enemyManager)
        {
            enemyManager.agent.isStopped = true;
            enemyManager.agent.SetDestination(enemyManager.transform.position);
            enemyManager.agent.stoppingDistance = .1f;
            enemyManager.agent.enabled = false;
            enemyManager.animator.CrossFade("Death", .2f);
            enemyManager.animator.SetFloat("Vertical", 0);
        }

        public override void OnExit(EnemyManager enemyManager)
        {
            
        }

        public override void OnTick(EnemyManager enemyManager)
        {
        }

        #region Decision Group
        protected override void CheckForNextState(EnemyManager enemyManager)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
