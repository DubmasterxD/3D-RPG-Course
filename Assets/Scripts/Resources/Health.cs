using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        float healthPoints = -1f;

        public bool IsDead { get; private set; } = false;

        GameObject instigator = null;
        BaseStats baseStats = null;

        private void Start()
        {
            baseStats = GetComponent<BaseStats>();
            if(gameObject.CompareTag("Player"))
            {
                baseStats.onLevelUp += UpdateHealth;
            }
            if (healthPoints < 0)
            {
                healthPoints = baseStats.GetStat(Stat.Health);
            }
        }

        public float GetPercentage()
        {
            return 100 * healthPoints / baseStats.GetStat(Stat.Health);
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

        private void UpdateHealth()
        {
            healthPoints = baseStats.GetStat(Stat.Health);
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
                float experienceReward = baseStats.GetStat(Stat.ExperienceReward);
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
