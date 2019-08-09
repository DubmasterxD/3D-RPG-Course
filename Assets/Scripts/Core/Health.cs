using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;

        public bool IsDead { get; private set; } = false;

        public void TakeDamage(float damage)
        {
            if (!IsDead)
            {
                healthPoints -= damage;
                if (healthPoints <= 0)
                {
                    Die();
                }
            }
        }

        private void Die()
        {
            healthPoints = 0;
            GetComponent<Animator>().SetTrigger("die");
            IsDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
