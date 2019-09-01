using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();
            float[] levels = lookupTable[characterClass][stat];
            if (levels.Length >= level)
            {
                return levels[level - 1];
            }
            return 0;
        }

        private void BuildLookup()
        {
            if(lookupTable!=null)
            {
                return;
            }
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach (ProgressionCharacterClass progressionCharacterClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();
                foreach (ProgressionStats progressionStat in progressionCharacterClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }
                lookupTable[progressionCharacterClass.characterClass] = statLookupTable;
            }
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();
            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
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
