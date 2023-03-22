using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public class AttackState : BaseState
    {
        [SerializeField] PursueTargetState pursueTargetState;
        [SerializeField] PatrolState patrolState;

        PlayerStatsManager playerStatsManager;
        [SerializeField] private WeaponFX weaponFX;
        private float timer;
        [SerializeField] private float timeBeforeShoot = 1.5f;
        [SerializeField] private float damageOnShoot = 15;

        public override void OnEnter(EnemyManager enemyManager)
        {
            enemyManager.agent.stoppingDistance = 2;
            if (playerStatsManager == null)
                enemyManager.currentTarget.TryGetComponent<PlayerStatsManager>(out playerStatsManager);
        }
        public override void OnExit(EnemyManager enemyManager)
        {
            enemyManager.agent.stoppingDistance = .1f;
        }
        public override void OnTick(EnemyManager enemyManager)
        {
            UpdateAnimatorValue(enemyManager);
            SetIKTarget(enemyManager);
            Shoot(enemyManager);
            SetDestination(enemyManager);
            CheckForNextState(enemyManager);
        }
        protected override void CheckForNextState(EnemyManager enemyManager)
        {
            (Vector3 targetDirection, float distanceFromTarget) = base.DetectionInfo(enemyManager);

            HandleRotateTowardsTarget(enemyManager, targetDirection);

            if (distanceFromTarget >= enemyManager.attackRadius)
            {
                enemyManager.SwitchNextState(pursueTargetState);
            }

            if (playerStatsManager.playerIsDead)
            {
                enemyManager.SwitchNextState(patrolState);
                enemyManager.canChangeState = false;
            }
        }

        #region Decision Group
        private void HandleRotateTowardsTarget(EnemyManager enemyManager, Vector3 targetDirection)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }
        private void SetIKTarget(EnemyManager enemyManager) => enemyManager.aimTarget.position = enemyManager.aimTargetLookAt.position;
        private void Shoot(EnemyManager enemyManager)
        {
            timer += Time.deltaTime;
            if (timer > timeBeforeShoot)
            {
                if(!Physics.Linecast(enemyManager.transform.position, enemyManager.currentTarget.transform.position))
                {
                    weaponFX.PlayMuzzle();
                    weaponFX.PlayLaser(enemyManager.aimTargetLookAt.position);
                    if(Physics.Raycast(enemyManager.transform.position + new Vector3(0, 1, 0), enemyManager.transform.forward,out RaycastHit hit,5f))
                    {
                        if (hit.transform.CompareTag("Player"))
                            playerStatsManager.OnDamage(damageOnShoot);
                        //Debug.DrawRay(enemyManager.transform.position, enemyManager.transform.forward * 5f,Color.red);
                    }
                       
                }
                timer = 0;
            }
        }
        private void SetDestination(EnemyManager enemyManager) => enemyManager.agent.SetDestination(enemyManager.currentTarget.transform.position);
        private void UpdateAnimatorValue(EnemyManager enemyManager) => enemyManager.animator.SetFloat("Vertical", Mathf.Clamp01(enemyManager.agent.velocity.magnitude));
        #endregion
    }
}
