using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;

        public bool IsDead { get; private set; } = false;

        public void TakeDamage(float damage)
        {
            if (!IsDead)
            {
                healthPoints -= damage;
                CheckIfDead();
            }
        }

        private void CheckIfDead()
        {
            if (healthPoints <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            healthPoints = 0;
            GetComponent<Animator>().SetTrigger("die");
            IsDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            CheckIfDead();
        }
    }
}
