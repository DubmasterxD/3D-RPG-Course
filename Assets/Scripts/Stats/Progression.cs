using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            foreach (ProgressionCharacterClass progressionCharacterClass in characterClasses)
            {
                if (characterClass != progressionCharacterClass.characterClass)
                {
                    continue;
                }
                foreach (ProgressionStats progressionStat in progressionCharacterClass.stats)
                {
                    if (progressionStat.stat == stat && progressionStat.levels.Length >= level)
                    {
                        return progressionStat.levels[level - 1];
                    }
                }
            }
            return 0;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass = default;
            public ProgressionStats[] stats = null;
        }

        [System.Serializable]
        class ProgressionStats
        {
            public Stat stat;
            public float[] levels;
        }
    }
}
