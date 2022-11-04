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
        public bool SettingsOpen = false;

        void Start()
        {
            SettingsOpen = false;
            SettingsScreen.SetActive(false);
        }

        void Update()
        {
            //opening settings
            if(SettingsOpen == false)
            {
                Debug.Log("Open Settings");
                SettingsScreen.SetActive(true);
                SettingsOpen = true;
                Pausemenu.SetActive(false);
            }

            //closing settings
            else if (SettingsOpen == true)
            {
                Close();
            }
        }

        public void Close()
        {
            Debug.Log("Close Settings");
            SettingsScreen.SetActive(false);
            SettingsOpen = false;
            Pausemenu.SetActive(true);
        }
    }
}