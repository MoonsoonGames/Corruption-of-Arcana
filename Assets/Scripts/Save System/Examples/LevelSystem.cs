using System.Collections;
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
    /// Extended by ISaveable
    /// </summary>
    public class LevelSystem : MonoBehaviour, ISaveable
    {

        [SerializeField] private int level = 1;
        [SerializeField] private int xp = 100;
        public object CaptureState()
        {
            return new SaveData
            {
                level = level,
                xp = xp
            };
        }

        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;

            level = saveData.level;
            xp = saveData.xp;
        }

        public void ResetState()
        {
            //TODO: Reset all values to default and then save them
        }

        [System.Serializable]
        private struct SaveData
        {
            public int level;
            public int xp;
        }
    }
}
