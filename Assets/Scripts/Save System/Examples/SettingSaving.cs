using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.SaveSystem.Serializables;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.SaveSystem
{
    public class SettingSaving : MonoBehaviour, ISaveable
    {
        [Header("Audio Sliders")]
        [SerializeField] private float master;
        [SerializeField] private float music;
        [SerializeField] private float sfx;
        [SerializeField] private float dialogue;


        public object CaptureState()
        {
            return new SaveData
            {
                master = master,
                music = music,
                sfx = sfx,
                dialogue = dialogue
            };
        }

        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;

            master = saveData.master;
            music = saveData.music;
            sfx = saveData.sfx;
            dialogue = saveData.sfx;
        }

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
