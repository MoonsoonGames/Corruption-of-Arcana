using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.SaveSystem
{
    /// <summary>
    /// Saves settings and other relevant things using the built in Player Prefs system
    /// </summary>
    public class SettingSaving : MonoBehaviour
    {
        // Sliders
        [Header("Audio UI")]
        public Slider master;
        public Slider music;
        public Slider sfx;
        public Slider dialogue;

        [Header("Other")]
        public Stopwatch stopwatch;

        public void SaveSliders()
        {
            PlayerPrefs.SetFloat("master", master.value);
            PlayerPrefs.SetFloat("music", music.value);
            PlayerPrefs.SetFloat("sfx", sfx.value);
            PlayerPrefs.SetFloat("dialogue", dialogue.value);
        }

        public void LoadSliders()
        {
            master.value = PlayerPrefs.GetFloat("master");
            music.value = PlayerPrefs.GetFloat("music");
            sfx.value = PlayerPrefs.GetFloat("sfx");
            dialogue.value = PlayerPrefs.GetFloat("dialogue");
        }

        public void SaveSettings()
        {
            stopwatch.Start();
            PlayerPrefs.Save();
            stopwatch.Stop();
            UnityEngine.Debug.Log($"Settings save completed in {stopwatch.Elapsed}");
        }
    }
}
