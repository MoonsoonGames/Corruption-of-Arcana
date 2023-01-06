using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Necropanda.Interfaces
{
    public class SplashScreenInterface : MonoBehaviour
    {
        public GameObject SettingsMenu;
        public GameObject[] Buttons;

        public void NewGame()
        {
            //Reset loadsettings/progress
            SceneManager.LoadScene("DevRoom");
            //load game
        }

        public void LoadGame()
        {
            //locate save file
            //set load settings/progress
            //load game
        }

        public void Settings()
        {
            SettingsMenu.SetActive(true);
            ToggleButtons(Buttons, false);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void SettingsClose()
        {
            SettingsMenu.SetActive(false);
            ToggleButtons(Buttons, true);
        }

        public void FullscreenBTN()
        {
            if (Screen.fullScreen == true)
            {
                Screen.SetResolution(1600, 900, false);
            }
            else 
            {
                Screen.SetResolution(1600, 900, true);
            }
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(SettingsMenu.activeSelf == true)
                {
                    SettingsMenu.SetActive(false);
                }
            }
        }
        void ToggleButtons(GameObject[] Buttons, bool state)
        {
            foreach(GameObject obj in Buttons)
            {
                obj.SetActive(state);
            }
        }
    }
}