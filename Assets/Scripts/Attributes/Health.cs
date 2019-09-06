using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] TakeDamageEvent takeDamage = null;
        [SerializeField] UnityEvent onDie = null;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }
        

        float healthPoints = -1f;

        public bool IsDead { get; private set; } = false;

        GameObject instigator = null;
        BaseStats baseStats = null;

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
        }

        private void Start()
        {
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
                takeDamage.Invoke(damage);
                healthPoints -= damage;
                this.instigator = instigator;
                if(CheckIfDead())
                {
                    AwardExperience();
                    onDie.Invoke();
                }
            }
        }

        private void UpdateHealth()
        {
            healthPoints = baseStats.GetStat(Stat.Health);
        }

        private bool CheckIfDead()
        {
            if (healthPoints <= 0)
            {
                Die();
                return true;
            }
            return false;
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
