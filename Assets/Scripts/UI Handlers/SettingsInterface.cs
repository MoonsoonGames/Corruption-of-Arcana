using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Necropanda.Interfaces
{
    public class SettingsInterface : MonoBehaviour
    {
        public GameObject Pausemenu;
        public GameObject SettingsScreen;
        public Slider MasterVolume;
        public Slider MusicVolume;
        public Slider SEVolume;
        public Slider DialogueVolume; //This might be scraped due to timeframe

        public void Close()
        {
            Debug.Log("Close Settings");
            SettingsScreen.SetActive(false);
            Pausemenu.SetActive(true);
        }

        public void SplashClose()
        {
            SettingsScreen.SetActive(false);
        }
    }
}