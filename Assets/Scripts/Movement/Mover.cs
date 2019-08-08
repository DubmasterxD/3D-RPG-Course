using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
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

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            if (navMeshAgent.isStopped)
                navMeshAgent.isStopped = false;
        }

        public void StopMoving()
        {
            if (!navMeshAgent.isStopped)
                navMeshAgent.isStopped = true;
        }
    }
}
