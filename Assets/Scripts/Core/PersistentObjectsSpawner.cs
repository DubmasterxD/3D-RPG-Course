using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectsSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectsPrefab = null;
        static bool hasSpawned = false;

        private void Awake()
        {
            if(!hasSpawned)
            {
                SpawnPersistenObjects();
                hasSpawned = true;
            }
        }

        private void SpawnPersistenObjects()
        {
            GameObject persistentObject = Instantiate(persistentObjectsPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}
