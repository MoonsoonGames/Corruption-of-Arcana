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
        public E_Scenes initialScene;

        public void NewGame()
        {
            //Reset loadsettings/progress
            SceneManager.LoadScene(initialScene.ToString());
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
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}