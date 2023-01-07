using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.SaveSystem.Serializables;
using UnityEngine.UI;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.SaveSystem
{
    /// <summary>
    /// Saves settings and other relevant things.
    /// 
    /// Extended by ISaveable
    /// </summary>
    public class SettingSaving : MonoBehaviour, ISaveable
    {
        // Sliders
        [Header("Audio UI")]
        public Slider master;
        public Slider music;
        public Slider sfx;
        public Slider dialogue;

        /// <summary>
        /// Implemented class. Called when SavingLoading SAVES to disk.
        /// </summary>
        /// <returns></returns>
        public object CaptureState()
        {
            return new SaveData
            {
                master = master.value,
                music = music.value,
                sfx = sfx.value,
                dialogue = dialogue.value
            };
        }

        /// <summary>
        /// Implemented class. Called when SavingLoading LOADS from disk.
        /// </summary>
        /// <returns></returns>
        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;

            master.value = saveData.master;
            music.value = saveData.music;
            sfx.value = saveData.sfx;
            dialogue.value = saveData.dialogue;
        }

        /// <summary>
        /// Savedata data structure
        /// </summary>
        [System.Serializable]
        private struct SaveData
        {
            public float master;
            public float music;
            public float sfx;
            public float dialogue;
        }
    }
}
