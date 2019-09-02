using System;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass = default;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffect = null;
        [SerializeField] bool shouldUseModifiers = false;

        public int currentLevel { get; private set; } = 1;

        public event Action onLevelUp;

        Experience experience;

        private void Awake()
        {
            experience = GetComponent<Experience>();
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void Start()
        {
            if(experience!=null)
            {
                currentLevel = CalculateLevel();
            }
            else
            {
                currentLevel = startingLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel <= currentLevel)
            {
                return;
            }
            currentLevel = newLevel;
            onLevelUp();
            if (levelUpEffect != null)
            {
                Instantiate(levelUpEffect, transform);
            }
        }

        public float GetStat(Stat stat)
        {
            if (shouldUseModifiers)
            {
                return (GetBaseStat(stat) + GetAdditiveModifiers(stat)) * (1 + GetPercentageModifiers(stat) / 100);
            }
            else
            {
                return GetBaseStat(stat);
            }
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, currentLevel);
        }

        private float GetAdditiveModifiers(Stat stat)
        {
            float sum = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    sum += modifier;
                }
            }
            return sum;
        }

        private float GetPercentageModifiers(Stat stat)
        {
            float sum = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    sum += modifier;
                }
            }
            return sum;
        }

        private int CalculateLevel()
        {
            float currentXP = experience.ExperiencePoints;
            int maxLvl = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass) + 1;
            for (int level = 1; level < maxLvl; level++)
            {
                if(progression.GetStat(Stat.ExperienceToLevelUp,characterClass,level)>currentXP)
                {
                    return level;
                }
            }
            return maxLvl;
        }
    }
}
