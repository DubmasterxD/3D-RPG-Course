using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;

        public bool IsDead { get; private set; } = false;

        public void TakeDamage(float damage)
        {
            if (IsDead)
            {
                return;
            }
            healthPoints -= damage;
            if (healthPoints <= 0)
            {
                healthPoints = 0;
                GetComponent<Animator>().SetTrigger("die");
                IsDead = true;
            }
        }
    }
}
