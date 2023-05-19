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
    /// <summary>
    /// Defines whether an entity is saveable or not.
    /// Holds one public variable, the item ID which NEEDS to be generated
    /// </summary>
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string id = string.Empty;

        public string Id => id;

        // Generatable ID through the context menu.
        [ContextMenu("Generate Id")]
        private void GenerateId() => id = Guid.NewGuid().ToString();

        /// <summary>
        /// Finds each saveable
        /// </summary>
        /// <returns></returns>
        public object CaptureState()
        {
            var state = new Dictionary<string, object>();

            foreach (var saveable in GetComponents<ISaveable>())
            {
                // set the dict, call the capture state on that item.
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }

            return state;
        }

        /// <summary>
        /// Finds each loadable
        /// </summary>
        /// <param name="state"></param>
        public void RestoreState(object state)
        {
            var stateDictionary = (Dictionary<string, object>)state;

            foreach (var saveable in GetComponents<ISaveable>())
            {
                string typeName = saveable.GetType().ToString();

                if (stateDictionary.TryGetValue(typeName, out object value))
                {
                    // set the dict, call the restore state on that item.
                    saveable.RestoreState(value);
                }
            }
        }

        public void ResetState()
        {
            foreach (var saveable in GetComponents<ISaveable>())
            {
                saveable.ResetState();
            }
        }
    }
}
