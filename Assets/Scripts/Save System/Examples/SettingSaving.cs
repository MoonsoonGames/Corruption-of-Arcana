using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

/// <summary>
/// Authored & Written by @mattordev
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

        [Space]
        [Header("Graphics UI")]
        public Toggle fullScreenToggle;

        [Space]
        [Header("Other")]
        [SerializeField] private string startId = string.Empty;
        public Stopwatch stopwatch;

        /// <summary>
        /// Checks to see whether the game has been started before
        /// </summary>
        private void GenerateStartId()
        {
            if (CheckForFirstRun())
            {
                // LoadSliders();
                return;
            }

            if (CheckForFirstRun() == false)
            {
                UnityEngine.Debug.Log("created slider start ID");
                startId = Guid.NewGuid().ToString();
                PlayerPrefs.SetString("StartID", startId);
                PlayerPrefs.Save();

                // Set the sliders for first usage
                master.value = 1;
                music.value = 1;
                sfx.value = 1;
                dialogue.value = 1;
            }
        }

        private void Start()
        {
            GenerateStartId();
            LoadGraphics();
        }

        #region Audio saving
        public void SaveSliders()
        {
            UnityEngine.Debug.Log("Saved");
            PlayerPrefs.SetFloat("master", master.value);
            PlayerPrefs.SetFloat("music", music.value);
            PlayerPrefs.SetFloat("sfx", sfx.value);
            PlayerPrefs.SetFloat("dialogue", dialogue.value);
            SaveSettings();
        }

        public void LoadSliders()
        {
            UnityEngine.Debug.Log("Loaded Audio");
            master.value = PlayerPrefs.GetFloat("master");
            music.value = PlayerPrefs.GetFloat("music");
            sfx.value = PlayerPrefs.GetFloat("sfx");
            dialogue.value = PlayerPrefs.GetFloat("dialogue");
        }
        #endregion

        #region Graphics saving
        public void SaveGraphics()
        {
            // Convert the int to a saveable bool
            PlayerPrefs.SetInt("isFullScreen", BoolToInt(fullScreenToggle.isOn));
            SaveSettings();
        }

        public void FullscreenButton()
        {
            Screen.fullScreen = fullScreenToggle.isOn;
            SaveGraphics();
        }

        public void LoadGraphics()
        {
            fullScreenToggle.isOn = IntToBool(PlayerPrefs.GetInt("isFullScreen"));
            // Apply the saved options
            Screen.fullScreen = fullScreenToggle.isOn;
        }
        #endregion

        public void SaveSettings()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
            PlayerPrefs.Save();
            stopwatch.Stop();
            UnityEngine.Debug.Log($"Settings save completed in {stopwatch.Elapsed}");
        }

        /// <summary>
        /// Checks to see whether the game has ran before based on if an ID string is found in the player prefs
        /// </summary>
        /// <returns>returns true if the game has ran, false if it hasn't</returns>
        public bool CheckForFirstRun()
        {
            if (PlayerPrefs.HasKey("StartID"))
            {
                startId = PlayerPrefs.GetString("StartID");
                return true;
            }
            else
            {
                // If there's no ID found, return false
                return false;
            }
        }

        [ContextMenu("Delete data")]
        public void DeletePlayerPrefsData()
        {
            PlayerPrefs.DeleteAll();
        }

        bool IntToBool(int val)
        {
            if (val != 0)
                return true;
            else
                return false;
        }

        int BoolToInt(bool val)
        {
            if (val)
                return 1;
            else
                return 0;
        }
    }
}
