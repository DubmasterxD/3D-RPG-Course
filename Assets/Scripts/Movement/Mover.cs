using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        NavMeshAgent navMeshAgent;
        Animator anim;
        Health character;
        [SerializeField] float maxSpeed = 6f; 

        void Start()
        {
            anim = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            character = GetComponent<Health>();
        }

        void Update()
        {
            navMeshAgent.enabled = !character.IsDead;
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            float forwardSpeed = Vector3.Distance(Vector3.zero, navMeshAgent.velocity);
            anim.SetFloat("forwardSpeed", forwardSpeed);
        }

        public void StartMovementAction(Vector3 destination, float speedFraction)
        {
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            if (navMeshAgent.isStopped)
            {
                navMeshAgent.isStopped = false;
            }
        }

        public void Cancel()
        {
            if (!navMeshAgent.isStopped)
            {
                navMeshAgent.isStopped = true;
            }
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            //GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
