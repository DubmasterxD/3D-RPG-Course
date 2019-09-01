using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                yield return SceneManager.LoadSceneAsync((int)state["lastSceneBuildIndex"]);
            }
            RestoreState(state);
        }

        public void Delete(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            File.Delete(path);
        }

        private void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile);
            Debug.Log("saving to :" + path);
            using (FileStream fs = File.Open(path, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, state);
            }
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            using (FileStream fs = File.Open(path, FileMode.OpenOrCreate))
            {
                BinaryFormatter bf = new BinaryFormatter();
                Dictionary<string, object> state = new Dictionary<string, object>();
                if (fs.Length > 0)
                {
                    state = (Dictionary<string, object>)bf.Deserialize(fs);
                }
                return state;
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueidentifier()] = saveable.CaptureState();
            }
            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                if (state.ContainsKey(saveable.GetUniqueidentifier()))
                {
                    saveable.RestoreState(state[saveable.GetUniqueidentifier()]);
                }
            }
        }

        private byte[] SerializeVector(Vector3 vector)
        {
            byte[] vectorBytes = new byte[3 * 4];
            BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0);
            BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
            BitConverter.GetBytes(vector.z).CopyTo(vectorBytes, 8);
            return vectorBytes;
        }

        private Vector3 DeserializeVector(byte[] buffer)
        {
            Vector3 result = new Vector3();
            result.x = BitConverter.ToSingle(buffer, 0);
            result.y = BitConverter.ToSingle(buffer, 4);
            result.z = BitConverter.ToSingle(buffer, 8);
            return result;
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath,saveFile+".sav");
        }
    }
}
