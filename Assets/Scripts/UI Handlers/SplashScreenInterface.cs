using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Necropanda.Interfaces
{
    public class SplashScreenInterface : MonoBehaviour
    {
        public GameObject SettingsMenu;
        public E_Scenes initialScene;

        public void NewGame()
        {
            SaveManager.instance.ResetAllData();

            //Reset loadsettings/progress
            if (initialScene == E_Scenes.Null)
            {
                Debug.LogWarning("no initial scene");
            }

            if (LoadingScene.instance == null)
                Debug.LogWarning("Loading instance is null");

            LoadingScene.instance.LoadScene(initialScene, E_Scenes.Null, 0);
            //load game
        }

        public void ArenaMode()
        {
            LoadingScene.instance.LoadScene(E_Scenes.ArenaMode, E_Scenes.Null, 0);
        }

        public void LoadGame()
        {
            //locate save file
            //set load settings/progress
            //load game

            SaveManager.instance.LoadAllData();
            //Reset loadsettings/progress

            if (LoadingScene.instance.loadScene != E_Scenes.Null)
                LoadingScene.instance.LoadScene(LoadingScene.instance.loadScene, E_Scenes.Null, -1);
            else
                NewGame();
        }

        public void Settings()
        {
            SettingsMenu.SetActive(true);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void Hover(TextMeshProUGUI text)
        {
            Debug.Log("Hover");
            text.color = new Color(1, 0.2f, 0.8f, 1);
        }

        public void StopHover(TextMeshProUGUI text)
        {
            Debug.Log("Stop Hover");
            text.color = Color.white;
        }
    }
}