using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;

        public bool IsDead { get; private set; } = false;

        GameObject instigator = null;

        private void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetHealth();
        }

        public float GetPercentage()
        {
            return 100 * healthPoints / GetComponent<BaseStats>().GetHealth();
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            if (!IsDead)
            {
                healthPoints -= damage;
                this.instigator = instigator;
                CheckIfDead();
            }
        }

        private void CheckIfDead()
        {
            if (healthPoints <= 0)
            {
                Die();
                AwardExperience();
            }
        }

        private void Die()
        {
            healthPoints = 0;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Animator>().SetTrigger("die");
            IsDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience()
        {
            if (instigator != null && instigator.GetComponent<Experience>() != null)
            {
                float experienceReward = GetComponent<BaseStats>().GetExperienceReward();
                instigator.GetComponent<Experience>().GainExperience(experienceReward);
            }
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
