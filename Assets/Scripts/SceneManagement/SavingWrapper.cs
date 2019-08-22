using UnityEngine;
using RPG.Saving;
using System.Collections;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        [SerializeField] float fadeInTime = 1f;

        IEnumerator Start()
        {
            yield return LoadLastScene();
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                StartCoroutine(LoadLastScene());
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImidiate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeInTime);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
    }
}
