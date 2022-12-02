using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written by @mattordev using Dino Dappers tutorial - https://www.youtube.com/watch?v=f5GvfZfy3yk&t=2s
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.SaveSystem
{
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string id = string.Empty;

        public string Id => id;

        [ContextMenu("Generate Id")]
        private void GenerateId() => id = Guid.NewGuid().ToString();

        public object CaptureState()
        {
            var state = new Dictionary<string, object>();

            foreach (var saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }

            return state;
        }

        public void RestoreState(object state)
        {
            var stateDictionary = (Dictionary<string, object>)state;

            foreach (var saveable in GetComponents<ISaveable>())
            {
                string typeName = saveable.GetType().ToString();

                if (stateDictionary.TryGetValue(typeName, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }
    }
}
