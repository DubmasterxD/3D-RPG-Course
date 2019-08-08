using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        NavMeshAgent navMeshAgent;
        Animator anim;

        void Start()
        {
            anim = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            float forwardSpeed = Vector3.Distance(Vector3.zero, navMeshAgent.velocity);
            anim.SetFloat("forwardSpeed", forwardSpeed);
        }

        public void StartMovementAction(Vector3 destination)
        {
            MoveTo(destination);
            GetComponent<ActionScheduler>().StartAction(this);
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            if (navMeshAgent.isStopped)
                navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            if (!navMeshAgent.isStopped)
                navMeshAgent.isStopped = true;
        }
    }
}
