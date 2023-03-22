using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Gdev
{
    public class EnemyManager : MonoBehaviour
    {
        [Tooltip("Set initial state desidered ( Idle, ecc )")]
        [SerializeField]private BaseState currentState;
        public float detectionRadius;
        public float attackRadius;
        public float minDetectionAngle;
        public float maxDetectionAngle;

        [HideInInspector] public PlayerManager currentTarget;
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Animator animator;

        public Transform aimTarget;
        [HideInInspector] public Transform aimTargetLookAt;
        [HideInInspector] public Vector3 aimTargetLookAtStartPosition;

        [HideInInspector]public bool canChangeState = true;

        private void Start()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            aimTargetLookAtStartPosition = aimTarget.localPosition;
            currentTarget = FindObjectOfType<PlayerManager>();
            detectionRadius = Mathf.Pow(detectionRadius, 2);
            attackRadius = Mathf.Pow(attackRadius, 2);
        }
        private void Update()
        {
            HandleStateMachine();
        }

        #region State Machines
        private void HandleStateMachine()
        {
            if (currentState != null)
                currentState.OnTick(this);
        }
        public void SwitchNextState(BaseState state)
        {
            if (canChangeState)
            {
                currentState.OnExit(this);
                state.OnEnter(this);
                currentState = state;
            }
        }
        #endregion

        public void Respawn()
        {
            //currentState = transform.Find("Patrol State").GetComponent<PatrolState>();
            GetComponent<EnemyStastManager>().OnRestore(100);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.DrawRay(transform.position + new Vector3(0,1,0), transform.forward * 5f);
        }
    }
}
