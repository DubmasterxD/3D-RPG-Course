﻿using UnityEditor;
using UnityEngine;
//tmp
using UnityEngine.AI;
using RPG.Core;
using System.Collections.Generic;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";

        public string GetUniqueidentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                if (stateDict.ContainsKey(saveable.GetType().ToString()))
                {
                    saveable.RestoreState(stateDict[saveable.GetType().ToString()]);
                }
            }
        }

        private void Update()
        {
            if(Application.IsPlaying(gameObject))
            {
                return;
            }
            if(string.IsNullOrEmpty(gameObject.scene.path))
            {
                return;
            }
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");
            if(string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = GUID.Generate().ToString();
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
