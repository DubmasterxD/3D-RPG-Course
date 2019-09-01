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

        public int currentLevel { get; private set; } = 1;

        public event Action onLevelUp;

        private void Start()
        {
            Experience experience = GetComponent<Experience>();
            if(experience!=null)
            {
                experience.onExperienceGained += UpdateLevel;
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
            return progression.GetStat(stat, characterClass, currentLevel);
        }

        public int CalculateLevel()
        {
            float currentXP = GetComponent<Experience>().ExperiencePoints;
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
