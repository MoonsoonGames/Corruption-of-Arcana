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
    public class SettingSaving : MonoBehaviour, ISaveable
    {

        [Header("Audio UI")]
        public Slider master;
        public Slider music;
        public Slider sfx;
        public Slider dialogue;

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

        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;

            master.value = saveData.master;
            music.value = saveData.music;
            sfx.value = saveData.sfx;
            dialogue.value = saveData.sfx;
        }

        // /// <summary>
        // /// Call this to set any of the settings to the saved values
        // /// </summary>
        // public void SetSettings()
        // {
        //     Debug.Log("test");
        //     master.value = masterVol;
        //     music.value = musicVol;
        //     sfx.value = sfxVol;
        //     dialogue.value = dialogueVol;
        // }

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
